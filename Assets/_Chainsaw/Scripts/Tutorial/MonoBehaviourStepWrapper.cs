using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Chainsaw.Scripts.Tutorial
{
    public class MonoBehaviourStepWrapper<T>: MonoBehaviour, ITutorialStep where T : ITutorialStep
    {
        [Header("Step settings")]
        [SerializeField] protected T step;
        
        [Space]
        [Header("Events")]
        public UnityEvent startedUnityEvent;
        public UnityEvent finishedUnityEvent;
        public UnityEvent cancelledUnityEvent;

        public void Initialize(TutorialDisplayUtilities displayUtilities)
        {
            Debug.Log($"[Step] <color=blue>Initialized</color> step: <b>{gameObject.name}</b>");
            step.Initialize(displayUtilities);
            startedUnityEvent.Invoke();
            Finished += OnFinished;
        }

        public void Cancel()
        {
            Debug.Log($"[Step] <color=red>Canceled</color> step: <b>{gameObject.name}</b>");
            step.Cancel();
            cancelledUnityEvent?.Invoke();
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

        private void OnFinished()
        {
            finishedUnityEvent.Invoke();
            Finished -= OnFinished;
        }
        
        public Action Started { get => step.Started; set => step.Started = value; }
        public Action Finished { get => step.Finished; set => step.Finished = value; }
        public Action GoBack { get => step.GoBack; set => step.GoBack = value; }
        public bool IsRunning {get => step.IsRunning; set => step.IsRunning = value; }
    }
}
