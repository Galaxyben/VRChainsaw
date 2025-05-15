using System;
using _Chainsaw.Scripts.Tutorial;
using UnityEngine;

public class KickbackTriggerStepBehaviour : MonoBehaviourStepWrapper<KickbackTriggerStep>
{
    private void OnValidate()
    {
        if (step != null && step.sawCollider == null)
            step.sawCollider = FindFirstObjectByType<SawCollider>();
    }
}

[Serializable]
public class KickbackTriggerStep : BasicStep, ITutorialStep
{
    public SawCollider sawCollider;
    
    public override void Initialize(TutorialDisplayUtilities displayUtilities)
    {
        base.Initialize(displayUtilities);

        sawCollider.kickbackTriggered.AddListener(Finish);
        
        Started?.Invoke();
    }

    public override void Finish()
    {
        base.Finish();
        
        sawCollider.kickbackTriggered.RemoveListener(Finish);
        
        Finished?.Invoke();
    }

    public void Cancel()
    {
        sawCollider.kickbackTriggered.RemoveListener(Finish);
    }

    public void Tick(float deltaTime)
    {
        
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
