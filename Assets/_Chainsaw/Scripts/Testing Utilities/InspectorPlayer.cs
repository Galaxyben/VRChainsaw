using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _Chainsaw.Scripts.Testing_Utilities
{
    public class InspectorPlayer : MonoBehaviour
    {
        [SerializeField] UnityEvent m_Event;
        [SerializeField] Key m_Key;

        private void Update()
        {
            if(m_Key != Key.None && Keyboard.current[m_Key].wasPressedThisFrame)
            {
                Play();
            }
        }

        public void Play()
        {
            m_Event?.Invoke();
        }

        public void CopyButton()
        {
            var btn = GetComponent<Button>();
            if(btn)
            {
                m_Event = btn.onClick;
            }
        }
    }
}