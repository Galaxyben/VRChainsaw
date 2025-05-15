using System;
using UnityEngine;
using UnityEngine.Events;

public class KeyLock : MonoBehaviour
{
    [InspectorName("Key")]
    public Transform keyObject;
    [InspectorName("Lock")]
    public Transform lockObject;

    public float unlockRange = 0.05f;

    public GameObject[] keyLockIndicators;

    public UnityEvent unlockEvent;
    public Cap[] unlockBlockers;

    public bool keylockStartEnabled = false;
    
    private bool m_interacting;
    private bool m_locked = true;

    private bool m_keylockEnabled;

    private void Start()
    {
        foreach (GameObject indicator in keyLockIndicators)
        {
            indicator.SetActive(false);
        }

        m_keylockEnabled = keylockStartEnabled;
        
        ConditionsCheck();
    }

    private void Update()
    {
        if (!m_keylockEnabled)
            return;

        if (m_interacting && m_locked && ConditionsCheck())
        {
            if(Vector3.Distance(keyObject.position, lockObject.position) <= unlockRange)
            {
                Unlock();
            }
        }
    }

    public void ToggleKeylockEnabled(bool _toggle)
    {
        Debug.Log($"Set keylock enabled to {{_toggle}} for {gameObject.name}");
        m_keylockEnabled = _toggle;
    }

    private bool ConditionsCheck()
    {
        bool result = true;

        foreach(Cap blocker in unlockBlockers)
        {
            if (!blocker.IsOpen())
                result = false;
        }

        return result;
    }

    public void ToggleInteracting(bool _toggle)
    {
        m_interacting = _toggle;
        if (m_locked)
        {
            foreach (GameObject indicator in keyLockIndicators)
            {
                indicator.SetActive(_toggle);
            }
        }
    }

    public void Unlock()
    {
        foreach(GameObject indicator in keyLockIndicators)
        {
            indicator.SetActive(false);
        }

        unlockEvent.Invoke();

        m_locked = false;
    }

    public void ResetKeyLock()
    {
        m_locked = true;
    }
}
