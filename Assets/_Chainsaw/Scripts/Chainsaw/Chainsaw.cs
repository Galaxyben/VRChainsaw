using System;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;

public class Chainsaw : MonoBehaviour
{
    public enum ChainsawParts
    {
        NONE,
        SAFETY_TRIGGER,
        FRONT_GUARD,
        START,
        FRONT_HANDLE,
        REAR_HANDLE,
        FUEL_TANK,
        OIL_TANK,
        ENGINE,
        THROTTLE,
        CHAIN,
        GUIDE_BAR,
        BUMPER_SPIKES,
        PURGE_BULB,
        CHOKE,
    }

    public enum State
    {
        RUNNING,
        FUELED,
        GROUNDED,
        LUBRICATED,
        TENSIONED,
        CHOKE_SWITCH,
        PURGED,
        CHOKED,
        COVERED,
        TENSION_CHECKED
    }

    public struct GrabData
    {
        public bool holding;
        public IXRSelectInteractor interactor;

        public InteractorHandedness GetHandedness()
        {
            return interactor.handedness;
        }
    }

    public MeshRenderer highlightsMesh;

    public float dmgCooldown;
    public float dmg;

    public Cap fuelCap, oilCap;//, sawCover;
    public Transform tensionPivot;
    public SawCollider sawCollider;

    public ToggleSwitch chokeSwitch;

    public float groundedHeightLimit;
    public LayerMask groundMask;

    private bool m_spilled;
    public LayerMask spillMask;
    public Transform fuelSpillPivot;
    public GameObject fuelSpillRag;
    public GameObject fuelSpill;

    public GrabData frontHandle;
    public GrabData backHandle;

    private XRGrabInteractable m_grabInteractable;

    private float rollAngle;
    private float pitchAngle;

    #region tempAudio

    [Header("Audio")]
    public AudioSource audioSource;

    public AudioClip chainsawRunningIdle_sfx;
    public AudioClip chainsawStart_sfx;

    public AudioClip chainsawStartRev_sfx;
    public AudioClip chainsawLoopRev_sfx;
    public AudioClip chainsawEndRev_sfx;

    #endregion

    public UnityEvent chainsawRunning;
    public UnityEvent frontGrabbed, backGrabbed;
    public UnityEvent frontDropped, backDropped;

    private MaterialPropertyBlock highlightsPropBlock;

    private Vector3 tensionPivotInitialPosition;

    private float m_lastTime;

    private bool m_running = false;
    private bool m_fueled = false;
    private bool m_lubricated = false;
    private bool m_tensioned = false;
    private bool m_grounded = false;
    private bool m_cutting = false;
    private bool m_purged = false;
    private bool m_covered = false;
    private bool m_tensionCheck = false;

    private int m_chokeAttempts = 0;
    private int m_startupAttempts = 0;

    private const int chokeAttemptsForChoke = 3;
    
    private void Start()
    {
        m_grabInteractable = GetComponent<XRGrabInteractable>();
        highlightsPropBlock = new MaterialPropertyBlock();
        highlightsMesh.GetPropertyBlock(highlightsPropBlock);
        tensionPivotInitialPosition = tensionPivot.localPosition;
        ToggleCoverTension();
    }

    private void Update()
    {
        m_grounded = Physics.Raycast(transform.position, Vector3.down, groundedHeightLimit, groundMask);

        rollAngle = transform.localEulerAngles.z;
        pitchAngle = transform.localEulerAngles.x;

        // If cutting and dmg cooldown finished, resolve damage to cuttable object
        if (m_cutting && Time.time > (m_lastTime + dmgCooldown))
        {
            Cuttable cuttableObj;
            if (sawCollider.AcceptableAngle(out cuttableObj))
            {
                cuttableObj.ReceiveDamage(dmg);
            }

            m_lastTime = Time.time;
        }
    }

    public void HandleOnSelectEntered(SelectEnterEventArgs _args)
    {
        Debug.Log("Interactor: " + _args.interactorObject + ": " + _args.interactorObject.handedness);

        float distanceToBack = Vector3.Distance(_args.interactorObject.transform.position, m_grabInteractable.colliders[0].transform.position);
        float distanceToFront = Vector3.Distance(_args.interactorObject.transform.position, m_grabInteractable.colliders[1].transform.position);

        if(distanceToBack > distanceToFront)
        {
            frontHandle.holding = true;
            frontHandle.interactor = _args.interactorObject;
            frontGrabbed.Invoke();
        } else
        {
            backHandle.holding = true;
            backHandle.interactor = _args.interactorObject;
            backGrabbed.Invoke();
        }
    }

    public void HandleOnSelectExit(SelectExitEventArgs _args)
    {
        Debug.Log("Interactor: " + _args.interactorObject + ": " + _args.interactorObject.handedness);
        if(_args.interactorObject == frontHandle.interactor)
        {
            frontHandle.holding = false;
            frontHandle.interactor = null;
            frontDropped.Invoke();
        } else
        {
            backHandle.holding = false;
            backHandle.interactor = null;
            backDropped.Invoke();
        }
    }

    public void RevUp()
    {
        if (m_running)
        {
            if (!m_cutting)
            {
                StartCoroutine(RevStart());
                m_cutting = true;
            }
        }
    }

    public void RevDown()
    {
        if (m_running)
        {
            if (m_cutting)
            {
                StartCoroutine(RevEnd());
                m_cutting = false;
            }
        }
    }

    public bool Startup()
    {
        if (!m_purged)
        {
            return false;
        }

        if(m_chokeAttempts < chokeAttemptsForChoke)
        {
            if (chokeSwitch.IsSwitchOn())
            {
                audioSource.clip = chainsawStart_sfx;
                audioSource.loop = false;
                audioSource.Play();
                m_chokeAttempts++;
            }

            return false;
        }

        if(m_startupAttempts < 2)
        {
            audioSource.clip = chainsawStart_sfx;
            audioSource.loop = false;
            audioSource.Play();
            m_startupAttempts++;
            return false;
        }

        if (!m_running)
        {
            audioSource.clip = chainsawStart_sfx;
            audioSource.loop = false;

            if (m_fueled && m_lubricated && m_tensioned && !m_covered && m_grounded)
            {
                audioSource.clip = chainsawRunningIdle_sfx;
                audioSource.loop = true;
                m_running = true;
                chainsawRunning.Invoke();
            }

            audioSource.Play();

        }

        return m_running;
    }

    public void OffHot()
    {
        m_chokeAttempts = 3;
        m_fueled = true;
        m_lubricated = true;
        m_purged = true;
        m_covered = false;
        AdjustTension();

        m_startupAttempts = 0;
        m_running = false;
        audioSource.Stop();
    }

    public void OffHotButton()
    {
        if (!m_running)
            return;

        m_chokeAttempts = 3;
        m_fueled = true;
        m_lubricated = true;
        m_purged = true;
        m_covered = false;
        AdjustTension();

        m_startupAttempts = 0;
        m_running = false;
        audioSource.Stop();
    }

    public void AddFuel()
    {
        if (fuelCap.IsOpen())
        {
            m_fueled = true;

            if (!m_spilled)
            {
                RaycastHit hit;
                if (Physics.Raycast(fuelSpillPivot.position, Vector3.down, out hit, 5f, spillMask))
                {
                    fuelSpill.transform.position = hit.point + (Vector3.up * 0.001f);
                    fuelSpill.SetActive(true);
                    fuelSpillRag.SetActive(true);
                }
            }
        }
    }

    public void CleanFuelSpill()
    {
        fuelSpill.SetActive(false);
    }

    public void AddLubricant()
    {
        if (oilCap.IsOpen())
            m_lubricated = true;
    }

    public void AdjustTension()
    {
        if (!m_covered && !m_tensioned)
        {
            tensionPivot.DOLocalMoveY(tensionPivotInitialPosition.y, 0.5f).SetEase(Ease.Linear);
            m_tensioned = true;
        }
    }

    public void ToggleCover(bool _toggle)
    {
        m_covered = _toggle;
        ToggleCoverTension();
    }

    public void CheckTension()
    {
        m_tensionCheck = true;
    }

    public bool IsTensionChecked()
    {
        return m_tensionCheck;
    }

    public bool IsCovered()
    {
        return m_covered;
    }

    public void Purge()
    {
        if(!m_purged)
            m_purged = true;
    }

    public void ToggleCoverTension()
    {
        if (!m_covered)
        {
            if(!m_tensioned)
                tensionPivot.localPosition = tensionPivotInitialPosition - new Vector3(0f, 0.0001f, 0f);
        } else
        {
            tensionPivot.localPosition = tensionPivotInitialPosition;
        }
    }

    public void HighlightPart(ChainsawParts _part)
    {
        highlightsPropBlock.SetInt("_step", (int)_part);
        highlightsMesh.SetPropertyBlock(highlightsPropBlock);
    }

    public void HidePartHighlight()
    {
        highlightsPropBlock.SetInt("_step", 0);
        highlightsMesh.SetPropertyBlock(highlightsPropBlock);
    }

    public void HighlightPart(int _part) => HighlightPart((ChainsawParts)_part);
    
    public bool IsCutting() { return m_cutting; }
    public bool IsRunning() {  return m_running; }
    public bool IsFueled() {  return m_fueled; }
    public bool IsLubricated() {  return m_lubricated; }
    public bool IsTensioned() {  return m_tensioned; }
    public bool IsGrounded() {  return m_grounded; }
    public bool IsPurged() { return m_purged; }
    public bool IsChokeSwitchOn() { return chokeSwitch.IsSwitchOn(); }
    public bool IsChocked() { return m_chokeAttempts >= chokeAttemptsForChoke; }

    public bool GetState(State state) => state switch
    {
        State.RUNNING => m_running,
        State.FUELED => m_fueled,
        State.GROUNDED => m_grounded,
        State.TENSIONED => m_tensioned,
        State.LUBRICATED => m_lubricated,
        State.CHOKE_SWITCH => chokeSwitch.IsSwitchOn(),
        State.PURGED => m_purged,
        State.CHOKED => m_chokeAttempts >= chokeAttemptsForChoke,
        State.COVERED => m_covered,
        State.TENSION_CHECKED => IsTensionChecked(),
        _ => throw new NotImplementedException()
    };
    
    public IEnumerator RevStart()
    {
        audioSource.clip = chainsawStartRev_sfx;
        audioSource.Play();
        yield return new WaitForSeconds(chainsawStartRev_sfx.length);
        audioSource.clip = chainsawLoopRev_sfx;
        audioSource.Play();
    }

    public IEnumerator RevEnd()
    {
        audioSource.clip = chainsawEndRev_sfx;
        audioSource.Play();
        yield return new WaitForSeconds(chainsawEndRev_sfx.length);
        audioSource.clip = chainsawRunningIdle_sfx;
        audioSource.Play();
    }
}
