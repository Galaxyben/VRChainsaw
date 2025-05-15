using System;
using TMPro;
using UnityEngine;

namespace _Chainsaw.Scripts.Tutorial.Steps
{
    public class ChainsawStateStepBehaviour : MonoBehaviourStepWrapper<ChainsawStateStep>
    {
        private void OnValidate()
        {
            if (!step.chainsaw) step.chainsaw = FindAnyObjectByType<Chainsaw>();
        }
    }

    [Serializable]
    public class ChainsawStateStep : BasicStep, ITutorialStep
    {
        public Chainsaw chainsaw;
        public Chainsaw.State state;
        public bool status;
        
        public override void Initialize(TutorialDisplayUtilities displayUtilities)
        {
            base.Initialize(displayUtilities);
            Started?.Invoke();
        }

        

        public void Tick(float deltaTime)
        {
            if (IsRunning)
            {
                if (chainsaw.GetState(state) == status)
                    Finish();
            }
            else
            {
                if(chainsaw.GetState(state) != status)
                    GoBack?.Invoke();
            }
        }

        public override void Finish()
        {
            base.Finish();
            Finished?.Invoke();
        }
        
        public void Cancel()
        {
            base.Finish();
        }
        public void ShouldGoBackCheck(float deltaTime)
        {
            if(chainsaw.GetState(state) != status)
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
        public bool IsRunning { get; set; }
        public Action GoBack { get; set; }
    }
}
