using System;
using _Chainsaw.Scripts.Tutorial;
using UnityEngine;
using UnityEngine.Events;

public class EmptyStepBehaviour : MonoBehaviour, ITutorialStep
{
    [SerializeField] ConcreteStep basicStep;

    [Space]
    [Header("Events")]
    public UnityEvent startedUnityEvent;
    public UnityEvent finishedUnityEvent;
    public UnityEvent cancelledUnityEvent;
    
    public void Initialize(TutorialDisplayUtilities displayUtilities)
    {
        basicStep.Initialize(displayUtilities);
        Started?.Invoke();
        startedUnityEvent?.Invoke();
    }

    public void Finish()
    {
        basicStep.Finish();
        
        Finished?.Invoke();
        finishedUnityEvent?.Invoke();
    }

    public void DoGoBack()
    {
        GoBack?.Invoke();
    }

    public void Cancel()
    {
        cancelledUnityEvent?.Invoke();
    }

    public void Tick(float deltaTime) { }

    public void ShouldGoBackCheck(float deltaTime) { }

    public void BackStepCheckStarted() { }

    public void BackStepCheckFinished() { }

    public Action Started { get; set; }
    public Action Finished { get; set; }
    public Action GoBack { get; set; }
    public bool IsRunning { get; set; }

    [Serializable]
    private class ConcreteStep : BasicStep
    {
        
    }
}
