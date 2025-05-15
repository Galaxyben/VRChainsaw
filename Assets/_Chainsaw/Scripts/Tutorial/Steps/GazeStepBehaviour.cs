using System;
using UnityEngine;

namespace _Chainsaw.Scripts.Tutorial
{
    public class GazeStepBehaviour : MonoBehaviourStepWrapper<GazeStep>
    {
        public void OnValidate()
        {
            if(step.gazeableManager == null)
                step.gazeableManager = FindFirstObjectByType<GazeableManager>();
        }
    }

    [Serializable]
    public class GazeStep : BasicStep, ITutorialStep
    {
        public GazeableManager gazeableManager;
        public Gazeable target;

        private bool wasGazing;
        
        public override void Initialize(TutorialDisplayUtilities displayUtilities)
        {
            base.Initialize(displayUtilities);

            wasGazing = gazeableManager.gazingEnabled;
            gazeableManager.gazingEnabled = true;
            target.gameObject.SetActive(true);

            gazeableManager.OnGazeCompleted.AddListener(Finish);
            
            Started?.Invoke();
        }

        public void Cancel()
        {
            base.Finish();
        }

        public override void Finish()
        {
            base.Finish();
            
            gazeableManager.gazingEnabled = wasGazing;
            target.gameObject.SetActive(false);
            
            gazeableManager.OnGazeCompleted.RemoveListener(Finish);
            
            Finished?.Invoke();
        }

        public void Tick(float deltaTime) { }
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