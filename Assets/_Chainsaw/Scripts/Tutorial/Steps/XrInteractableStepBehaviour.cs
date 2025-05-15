using System;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

namespace _Chainsaw.Scripts.Tutorial.Steps
{
    public class XrInteractableStepBehaviour : MonoBehaviourStepWrapper<XrInteractableStep>
    {
        
    }

    /// <summary>
    /// For now only checks for grabbed state by any of the interactors but can be expanded eventually
    /// </summary>
    [Serializable]
    public class XrInteractableStep : BasicStep, ITutorialStep
    {
        public XRBaseInteractable interactable;
        public XRBaseInputInteractor[] interactors;
        
        public override void Initialize(TutorialDisplayUtilities displayUtilities)
        {
            base.Initialize(displayUtilities);
            Started?.Invoke();

            interactable.selectEntered.AddListener(SelectEntered);
            
            //Check if it's being grabbed already
            foreach (var interactor in interactors)
            {
                if (interactor.hasSelection)
                {
                    IXRSelectInteractable grabbed = interactor.interactablesSelected[0];
                    if (grabbed.transform == interactable.transform)
                    {
                        Finish(); //Could make a delay so the step is running for a bit at least
                        return;
                    }
                }
            }

        }

        public void Cancel()
        {
            base.Finish();
        }

        public override void Finish()
        {
            base.Finish();
            interactable.selectEntered.RemoveListener(SelectEntered);
            Finished?.Invoke();
        }

        public void Tick(float deltaTime) { }
        public void ShouldGoBackCheck(float deltaTime) { }

        public void BackStepCheckStarted()
        {
            interactable.selectExited.AddListener(SelectExited);
        }

        public void BackStepCheckFinished()
        {
            interactable.selectExited.RemoveListener(SelectExited);
        }

        void SelectEntered(SelectEnterEventArgs args)
        {
            foreach (var interactor in interactors)
            {
                if (args.interactorObject.transform == interactor.transform)
                {
                    Finish();
                    return;
                }
            }
        }

        void SelectExited(SelectExitEventArgs args)
        {
            foreach (var interactor in interactors)
            {
                if (args.interactorObject.transform == interactor.transform)
                {
                    interactable.selectExited.RemoveListener(SelectExited);
                    GoBack?.Invoke();
                    return;
                }
            }
        }

        public Action Started { get; set; }
        public Action Finished { get; set; }
        public Action GoBack { get; set; }
        public bool IsRunning { get; set; }
    }
}