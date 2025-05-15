using System;
using UnityEngine;

namespace _Chainsaw.Scripts.Tutorial
{
    public abstract class ScriptableObjectStepWrapper<T> : ScriptableObject, ITutorialStep where T : ITutorialStep
    {
        [SerializeField] protected T step;
        
        public void Initialize(TutorialDisplayUtilities displayUtilities)
        {
            step.Initialize(displayUtilities);
        }

        public void Cancel()
        {
            
        }

        public void Tick(float deltaTime)
        {
            step.Tick(deltaTime);
        }

        public void ShouldGoBackCheck(float deltaTime)
        {
            step.ShouldGoBackCheck(deltaTime);
        }

        public void BackStepCheckStarted()
        {
            step.BackStepCheckStarted();
        }

        public void BackStepCheckFinished()
        {
            step.BackStepCheckStarted();
        }

        public Action Started { get => step.Started; set => step.Started = value; }

        public Action Finished
        {
            get => step.Finished; set => step.Finished = value; 
        }

        public Action GoBack { get; set; }

        public bool IsRunning
        {
            get => step.IsRunning; set => step.IsRunning = value; 
        }
    }
}
