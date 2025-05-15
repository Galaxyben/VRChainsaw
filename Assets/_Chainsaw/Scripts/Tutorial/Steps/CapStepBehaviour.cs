using System;

namespace _Chainsaw.Scripts.Tutorial.Steps
{
    public class CapStepBehaviour : MonoBehaviourStepWrapper<CapStep> { }

    [Serializable]
    public class CapStep : BasicStep, ITutorialStep
    {
        public Cap cap;
        public bool checkForOpen;

        public override void Initialize(TutorialDisplayUtilities displayUtilities)
        {
            base.Initialize(displayUtilities);
            Started?.Invoke();
        }

        public void Cancel()
        {
            base.Finish();
        }

        public override void Finish()
        {
            base.Finish();
            Finished?.Invoke();
        }

        public void Tick(float deltaTime)
        {
            if (cap.IsOpen() == checkForOpen)
                Finish();
        }

        public void ShouldGoBackCheck(float deltaTime)
        {
            if(cap.IsOpen() != checkForOpen)
                GoBack?.Invoke();
        }

        public void BackStepCheckStarted()
        {
            
        }

        public void BackStepCheckFinished()
        {
            
        }

        public Action Started { get; set; }
        public Action Finished { get; set; }
        public Action GoBack { get; set; }
        public bool IsRunning { get; set; }
    }
}


