using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class SawCollider : MonoBehaviour
{
    private float m_angleDelta;
    private float m_distanceDelta;

    public float maxDistanceDelta;
    public float maxAngleDelta;

    public Chainsaw chainsawLogic;
    public GameObject chainsawVisuals;
    public Transform kickbackZone;
    public GameObject bladeCollider;

    private Cuttable m_cuttableTarget = null;

    public UnityEvent kickbackTriggered;

    private void OnCollisionEnter(Collision _col)
    {
        if (_col.gameObject.CompareTag("Tree"))
        {
            if (chainsawLogic.IsCutting())
            {
                if (Vector3.Distance(_col.contacts[0].point, kickbackZone.position) <= 0.1f)
                {
                    Vector3 kickBackDir = (_col.contacts[0].point - kickbackZone.position).normalized;

                    float topAngleComp = Vector3.Angle(kickBackDir, transform.up);
                    float botAngleComp = Vector3.Angle(kickBackDir, -transform.up);

                    bladeCollider.SetActive(false);
                    chainsawVisuals.transform.DOLocalRotate(new Vector3(topAngleComp < botAngleComp ? -40f : 40f, 0f, 0f), 0.2f).SetEase(Ease.OutBounce).OnComplete(() =>
                    {
                        chainsawVisuals.transform.DOLocalRotate(Vector3.zero, 0.8f).OnComplete(() =>
                        {
                            bladeCollider.SetActive(true);
                        });
                    });

                    kickbackTriggered.Invoke();
                }
            }
        }
    }

    private void OnCollisionStay(Collision _col)
    {
        if (_col.collider.CompareTag("Tree"))
        {
            m_cuttableTarget = _col.gameObject.GetComponent<Cuttable>();
            m_angleDelta = Vector3.Angle(transform.up, m_cuttableTarget.cutPoint.forward);
            m_distanceDelta = Vector3.Distance(m_cuttableTarget.cutPoint.position, _col.contacts[0].point);
        }
    }

    private void OnCollisionExit(Collision _col)
    {
        if (_col.collider.CompareTag("Tree"))
        {
            m_cuttableTarget = null;
        }
    }

    public bool AcceptableAngle(out Cuttable _cuttable)
    {
        bool result = false;

        if (m_cuttableTarget != null)
        {
            

            if (m_distanceDelta <= maxDistanceDelta)
            {
                if (m_angleDelta <= maxAngleDelta)
                {
                    result = true;
                }
            }
        }

        _cuttable = m_cuttableTarget;

        return result;
    }
}
