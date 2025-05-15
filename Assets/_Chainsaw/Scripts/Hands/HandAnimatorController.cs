using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Chainsaw.Scripts.Hands
{
    public class HandAnimatorController : MonoBehaviour
    {
        private static readonly int Trigger = Animator.StringToHash("Trigger");
        private static readonly int Grip = Animator.StringToHash("Grip");
        
        [SerializeField] private InputActionProperty triggerAction;
        [SerializeField] private InputActionProperty gripAction;

        [SerializeField] Animator anim;

        private void OnValidate()
        {
            if(anim == null)
                anim = GetComponent<Animator>();
        }

        private void Update()
        {
            float triggerValue = triggerAction.action.ReadValue<float>();
            float gripValue = gripAction.action.ReadValue<float>();
            
            anim.SetFloat(Trigger, triggerValue);
            anim.SetFloat(Grip, gripValue);
        }
    }
}
