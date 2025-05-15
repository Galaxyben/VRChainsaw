using System;
using UnityEngine;
using UnityEngine.Localization;

namespace _Chainsaw.Scripts.Tutorial
{
    [CreateAssetMenu(fileName = "WaitStep", menuName = "Scriptable Objects/WaitStep")]
    public class WaitStepScriptable : ScriptableObjectStepWrapper<WaitStep>
    {
        public float waitTime => step.waitTime;
    }
    
    [System.Serializable]
    public class WaitStep : BasicStep, ITutorialStep
    {
        public float waitTime = 1f;
        
        private float waitStart;

        public void BackStepCheckFinished()
        {
            
        }

        public Action Started { get; set; }
        public Action Finished { get; set; }
        public Action GoBack { get; set; }
        public bool IsRunning { get; set; }

        public override void Initialize(TutorialDisplayUtilities displayUtilities)
        {
            base.Initialize(displayUtilities);
            
            Debug.Log($"Starting tutorial wait step for {waitTime} seconds");
            waitStart = Time.time;
        }

        public void Cancel()
        {
            base.Finish();
        }

        public void Tick(float deltaTime)
        {
            if (IsRunning && waitStart + waitTime < Time.time)
                Finish();
        }

        public void BackStepCheckStarted()
        {
            //Can't go back here
        }
        
        public void ShouldGoBackCheck(float deltaTime)
        {
            
        }

        public override void Finish()
        {
            base.Finish();
            
            Debug.Log("Finished tutorial wait step");
            Finished?.Invoke();
        }
    }
}
