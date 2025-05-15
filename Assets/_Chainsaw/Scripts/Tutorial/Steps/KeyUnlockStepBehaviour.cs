using System;
using _Chainsaw.Scripts.Tutorial;
using UnityEngine;

public class KeyUnlockStepBehaviour : MonoBehaviourStepWrapper<KeyUnlockStep>
{
    
}

[Serializable]
public class KeyUnlockStep : BasicStep, ITutorialStep
{
    public KeyLock keyLock;

    public override void Initialize(TutorialDisplayUtilities displayUtilities)
    {
        base.Initialize(displayUtilities);
        
        keyLock.unlockEvent.AddListener(Finish);
        
        Started?.Invoke();
    }

    public void Cancel()
    {
        base.Finish();
    }

    public void Tick(float deltaTime)
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

    public override void Finish()
    {
        base.Finish();
        
        keyLock.unlockEvent.RemoveListener(Finish);
        
        Finished?.Invoke();
    }

    public Action Started { get; set; }
    public Action Finished { get; set; }
    public Action GoBack { get; set; }
    public bool IsRunning { get; set; }
}
