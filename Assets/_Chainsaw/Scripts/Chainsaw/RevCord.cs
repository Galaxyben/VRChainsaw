using UnityEngine;
using DG.Tweening;

public class RevCord : MonoBehaviour
{
    public Chainsaw chainsaw;
    public float revDistance;

    private Vector3 m_originalPosition;
    private Quaternion m_originalRotation;
    private bool m_revCheck = false;
    private bool m_revDistReached = false;

    private void Start()
    {
        DOTween.Init();

        m_originalPosition = transform.localPosition;
        m_originalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (m_revCheck && !m_revDistReached)
        {
            if (Vector3.Distance(m_originalPosition, transform.localPosition) >= revDistance)
            {
                chainsaw.Startup();
                m_revDistReached = true;
            }
        }
    }

    public void HandleSelectedEnter()
    {
        m_revCheck = true;
        m_revDistReached = false;
    }

    public void HandleSelectedExit()
    {
        m_revCheck = false;
        transform.DOLocalMove(m_originalPosition, 0.5f).SetEase(Ease.OutBounce);
        transform.DOLocalRotateQuaternion(m_originalRotation, 0.5f).SetEase(Ease.Linear);
    }
}
