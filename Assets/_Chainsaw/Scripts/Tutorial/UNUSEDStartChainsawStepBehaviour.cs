using System;
using UnityEngine;

namespace _Chainsaw.Scripts.Tutorial
{
    public class UNUSEDStartChainsawStepBehaviour : MonoBehaviourStepWrapper<StartchainsawStep>
    {
        private void OnValidate()
        {
            if(step.chainsaw == null)
                step.chainsaw = FindFirstObjectByType<Chainsaw>();
        }
    }

    public class StartchainsawStep : BasicStep, ITutorialStep
    {
        [Tooltip("Leave at 0 to rev until chainsaw is running")]
        public int revsToComplete = 0;
        public Chainsaw chainsaw;
        
        public override void Initialize(TutorialDisplayUtilities displayUtilities)
        {
            base.Initialize(displayUtilities);
        
            Started?.Invoke();
        }

        public void Tick(float deltaTime)
        {
            if (revsToComplete == 0)
            {
                if(chainsaw.IsRunning())
                    Finish();
            }
            else if (chainsaw.IsChocked())
            {
                
            }
        }

        public override void Finish()
        {
            base.Finish();
        
            Finished?.Invoke();
        }

        public void Cancel()
        {
            
        }
    
        public void ShouldGoBackCheck(float deltaTime)
        {
            //can't go back here
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