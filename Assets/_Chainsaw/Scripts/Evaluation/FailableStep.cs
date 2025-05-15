using System;
using System.Collections.Generic;
using _Chainsaw.Scripts.Tutorial;
using UnityEngine;
using Object = UnityEngine.Object;

public class FailableStep : MonoBehaviour, ITutorialStep
{
    //Asignables en inspector
    [SerializeField] private MonoBehaviour successCheckObj;
    [SerializeField] private List<MonoBehaviour> failChecksObj;
    [SerializeField] private bool endOnFailure;
    
    //Variables que se van a usar, a partir de lo que se asigna en el inspector
    private TutorialDisplayUtilities displayUtilities;
    private ITutorialStep successStep;
    private List<ITutorialStep> failedSteps;

    //Variables privadas
    private bool isFinished;
    private bool failed = false;
    
    //Que solo se puedan asignar objectos correctos en el inspector
    private void OnValidate()
    {
        if (successCheckObj != null && successCheckObj.GetComponent<ITutorialStep>() == null)
        {
            successCheckObj = null;
        }

        for (int i = 0; i < failChecksObj.Count; i++)
        {
            if(failChecksObj[i].GetComponent<ITutorialStep>() == null)
                failChecksObj[i] = null;
        }
    }
    
    private void Start()
    {
        successStep = successCheckObj as ITutorialStep;
        failedSteps = new  List<ITutorialStep>();
        for(int i = 0; i < failChecksObj.Count; i++)
            failedSteps.Add(failChecksObj[i] as ITutorialStep);
    }

    public void Initialize(TutorialDisplayUtilities _displayUtilities)
    {
        displayUtilities = _displayUtilities;
        
        displayUtilities.dialogueDisplayer.ShowDialogue(gameObject.name);
        displayUtilities.button.gameObject.SetActive(false);
        
        failed = false;
        
        var mockDisplay = new TutorialDisplayUtilities();

        successStep.Finished += SuccessfulFinish;
        successStep.Initialize(mockDisplay);
        successStep.IsRunning = true;
        
        foreach (var t in failedSteps)
        {
            t.Finished += FailedFinish;
            t.Initialize(mockDisplay);
            t.IsRunning = true;
        }

        Started?.Invoke();
    }

    private void Finish()
    {
        isFinished = true;
        
        displayUtilities.button.onClick.RemoveAllListeners();
        
        Finished?.Invoke();
    }

    private void SuccessfulFinish()
    {
        if (isFinished) return;

        EndCheck();
    }

    private void FailedFinish()
    {
        if (isFinished) return;

        failed = true;
        
        if (endOnFailure)
        {
            EndCheck();
        }
    }

    private void EndCheck()
    {
        successStep.Finished -= SuccessfulFinish;
        foreach (var t in failedSteps)
            t.Finished -= FailedFinish;
        
        displayUtilities.button.gameObject.SetActive(true);
        displayUtilities.button.onClick.RemoveAllListeners();
        displayUtilities.button.onClick.AddListener(Finish);
        
        ShowResult();
    }
    private void ShowResult()
    {
        displayUtilities.dialogueDisplayer.ShowDialogue(failed ? "Failed" : "Success"); //TODO expand
    }

    
    #region rest of ITutorialStep implementation
    public void Cancel() { }

    public void Tick(float deltaTime)
    {
        successStep.Tick(deltaTime);
        
        for(int i = 0; i < failedSteps.Count; i++)
            failedSteps[i].Tick(deltaTime);
    }

    public void ShouldGoBackCheck(float deltaTime) { }

    public void BackStepCheckStarted() { }

    public void BackStepCheckFinished() { }

    public Action Started { get; set; }
    public Action Finished { get; set; }
    public Action GoBack { get; set; }
    public bool IsRunning { get; set; }
    #endregion
}
