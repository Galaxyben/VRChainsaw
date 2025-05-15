using System;
using UnityEngine;

namespace _Chainsaw.Scripts.Tutorial.Steps
{
    public class CuttableStepBehaviour : MonoBehaviourStepWrapper<CuttableStep> { }

    [Serializable]
    public class CuttableStep : BasicStep, ITutorialStep
    {
        [SerializeField] private Cuttable cuttable;

        public void Cancel()
        {
            base.Finish();
        }

        public void Tick(float deltaTime)
        {
            if (cuttable.IsDead)
                Finish();
        }

        public void ShouldGoBackCheck(float deltaTime) { }

        public void BackStepCheckStarted()
        {
            if(cuttable.IsDead == false) //Don't know how this would happen but oh well
                GoBack?.Invoke();
        }

        public void BackStepCheckFinished()
        {
            
        }

        public override void Finish()
        {
            base.Finish();
            
            Finished?.Invoke();
        }

        public Action Started { get; set; }
        public Action Finished { get; set; }
        public Action GoBack { get; set; }
        public bool IsRunning { get; set; }
    }
}
