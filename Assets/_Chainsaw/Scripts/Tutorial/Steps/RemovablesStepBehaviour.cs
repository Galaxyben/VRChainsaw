using System;

namespace _Chainsaw.Scripts.Tutorial.Steps
{
    public class RemovablesStepBehaviour : MonoBehaviourStepWrapper<RemovablesStep>
    { 
        
    }

    [Serializable]
    public class RemovablesStep : BasicStep, ITutorialStep
    {
        public Removable[] removables;
    
        public override void Initialize(TutorialDisplayUtilities displayUtilities)
        {
            base.Initialize(displayUtilities);
        
            Started?.Invoke();
        }

        public override void Finish()
        {
            base.Finish();
        
            Finished?.Invoke();
        }

        public void Cancel()
        {
        
        }

        public void Tick(float deltaTime)
        {
            foreach (var removable in removables)
            {
                if (removable != null && removable.gameObject.activeSelf)
                    return;
            }
        
            Finish();
        }

        public void ShouldGoBackCheck(float deltaTime)
        {
        
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