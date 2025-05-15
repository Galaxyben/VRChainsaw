using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace _Chainsaw.Scripts.Tutorial.Steps.StepExtras
{
    public class PerimeterTapeProgressPoint : MonoBehaviour
    {
        public PerimeterTapeProgressPoint nextPoint;
        public PerimeterTapeProgressPoint previousPoint;
        [SerializeField] new Collider collider;
        [SerializeField] private XRSimpleInteractable interactable;
        [SerializeField] private PerimeterTape perimeterTape;
        [SerializeField] private float progressAtThisPoint;
        
        private bool isPressDown;
        public bool IsPressDown
        {
            get => isPressDown;
            set
            {
                isPressDown = value;
                if (isPressDown && interactable.isHovered)
                {
                    Complete();
                }
                else if (isPressDown)
                {
                    Debug.Log($"Pressed trigger but {gameObject.name} was not hovered");
                }
            }
        }

        public GameObject visualIndicator;

        [HideInInspector] public bool completeOnlyOnSelect = false;
        public UnityEvent CompletedEvent;

        private void OnValidate()
        {
            if (interactable)
            {
                collider = interactable.GetComponent<Collider>();
            }
            else if(collider)
            {
                interactable = collider.GetComponent<XRSimpleInteractable>();
            }
        }

        private void OnEnable()
        {
            interactable.hoverEntered.AddListener(HoverEnterEvent);
        }

        private void OnDisable()
        {
            interactable.hoverEntered.RemoveListener(HoverEnterEvent);
            collider.enabled = false;
        }

        private void HoverEnterEvent(HoverEnterEventArgs arg0)
        {
            if(IsPressDown)
                Complete();
            else
            {
                Debug.Log($"Hovered {gameObject.name} but trigger was not pressed");
            }
        }
        
        public void Complete()
        {
            if(nextPoint != null)
                nextPoint.gameObject.SetActive(true);
            
            perimeterTape.SetTapeProgress(progressAtThisPoint);
            gameObject.SetActive(false);
            
            CompletedEvent?.Invoke();
        }

        public void Cancel()
        {
            IsPressDown = false;
            if (previousPoint != null)
            {
                previousPoint.gameObject.SetActive(true);
                previousPoint.Complete();
                gameObject.SetActive(false);
            }
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            collider.enabled = true;
            visualIndicator.gameObject.SetActive(true);
            //todo: add some visuals here
        }

        public void Deactivate()
        {
            IsPressDown = false;
            gameObject.SetActive(false);
            visualIndicator.gameObject.SetActive(false);
            collider.enabled = false;
        }
    }
}
