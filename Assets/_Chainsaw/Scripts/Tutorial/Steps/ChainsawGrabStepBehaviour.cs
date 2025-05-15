using System;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace _Chainsaw.Scripts.Tutorial.Steps
{
    public class ChainsawGrabStepBehaviour : MonoBehaviourStepWrapper<ChainsawGrabStep>
    {
        private void OnValidate()
        {
            if (step.chainsaw == null)
                step.chainsaw = FindFirstObjectByType<Chainsaw>();
        }
    }

    [Serializable]
    public class ChainsawGrabStep : BasicStep, ITutorialStep
    {
        public Chainsaw chainsaw;
        public bool front;
        public InteractorHandedness handedness;

        public override void Initialize(TutorialDisplayUtilities displayUtilities)
        {
            base.Initialize(displayUtilities);
            
            if(front)
                chainsaw.frontGrabbed.AddListener(Finish);
            else
                chainsaw.backGrabbed.AddListener(Finish);
            
            Started?.Invoke();
        }

        public override void Finish()
        {
            if (front && handedness == chainsaw.frontHandle.GetHandedness() || handedness == InteractorHandedness.None)
            {
                base.Finish();
                chainsaw.frontGrabbed.RemoveListener(Finish);
                Finished?.Invoke();
            }
            else if (!front && handedness == chainsaw.backHandle.GetHandedness() || handedness == InteractorHandedness.None)
            {
                base.Finish();
                chainsaw.backGrabbed.RemoveListener(Finish);
                Finished?.Invoke();
            }
        }


        public void Cancel()
        {
        }

        public void Tick(float deltaTime)
        {
            
        }

        public void ShouldGoBackCheck(float deltaTime)
        {
            
        }

        public void BackStepCheckStarted()
        {
            if(front)
                chainsaw.frontDropped.AddListener(DoGoBack);
            else
                chainsaw.backDropped.AddListener(DoGoBack);
        }

        public void BackStepCheckFinished()
        {
            if(front)
                chainsaw.frontDropped.RemoveListener(DoGoBack);
            else
                chainsaw.backDropped.RemoveListener(DoGoBack);
        }

        void DoGoBack()
        {
            GoBack?.Invoke();
        }

        public Action Started { get; set; }
        public Action Finished { get; set; }
        public Action GoBack { get; set; }
        public bool IsRunning { get; set; }
    }
}