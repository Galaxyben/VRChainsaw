using UnityEngine;
using DG.Tweening;

public class ToggleSwitch : MonoBehaviour
{
    public Transform switchPivot;
    public float maxAngle;

    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    public RotationAxis rotAxis;
    public bool invertAxis;

    private Vector3 m_initialRotation;
    private Vector3 m_targetRotation;
    private Tween m_switchTween;
    private bool m_switchState = false;

    private void Start()
    {
        m_initialRotation = switchPivot.localEulerAngles;

        m_targetRotation = new Vector3(
            maxAngle * (rotAxis == RotationAxis.X ? invertAxis ? -1 : 1 : 0),
            maxAngle * (rotAxis == RotationAxis.Y ? invertAxis ? -1 : 1 : 0),
            maxAngle * (rotAxis == RotationAxis.Z ? invertAxis ? -1 : 1 : 0));
    }

    public void Toggle()
    {
        m_switchState = !m_switchState;

        if (m_switchTween != null)
            if (m_switchTween.IsActive())
                m_switchTween.Complete();

        m_switchTween = switchPivot.DOLocalRotate(m_switchState ? m_targetRotation : m_initialRotation, 0.1f).SetEase(Ease.Linear);
    }

    public bool IsSwitchOn()
    {
        return m_switchState == true;
    }
}
