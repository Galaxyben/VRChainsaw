using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace _Chainsaw.Scripts.Testing_Utilities
{
    /// <summary>
    /// Helps test with joystick walk on editor without having to worry about leaving some setting wrong for a build.
    /// </summary>
    public class TestWithJoystickWalk : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private bool useJoystickMovement;

        private void Awake()
        {
            if (useJoystickMovement)
            {
                var movementProvider = GetComponentInChildren<DynamicMoveProvider>(true);
                if(movementProvider != null)
                    movementProvider.gameObject.SetActive(true);
                else
                    Debug.LogError("Didn't find DynamicMoveProvider component, can't enable joystick movement");
            }
        }
#endif
    }
}
