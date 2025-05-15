using Unity.XR.CoreUtils;
using UnityEngine;

namespace _Chainsaw.Scripts.Posing
{
    public class PoseSetupUI : MonoBehaviour
    {
        [SerializeField] private XROrigin xrOrigin;
        [SerializeField] private PoseMatcher poseMatcher;

        public void SetHmdHeight()
        {
            poseMatcher.hmdHeight = xrOrigin.Camera.transform.position.y;
        }
    }
}
