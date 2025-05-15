using System;
using _Chainsaw.Scripts.Tutorial.Steps.StepExtras;
using UnityEngine;
using UnityEngine.UI;

namespace _Chainsaw.Scripts.Tutorial
{
    public class GazeRequirementForStep : MonoBehaviour
    {
        [SerializeField] GazeableManager gazeableManager;
        [SerializeField] private Chainsaw.ChainsawParts part;
        [SerializeField] private GazeableCollection gazeables;
        [SerializeField] private Button button;

        ITutorialStep tutorialStep;
        
        private void OnValidate()
        {
            if(gazeables == null)
                gazeables = FindFirstObjectByType<GazeableCollection>();

            var highlighter = GetComponent<InStepChainsawHighlighter>();
            if (highlighter)
            {
                part = highlighter.PartToHighlight;
            }
        }

        private void OnEnable()
        {
            if (tutorialStep == null)
                tutorialStep = GetComponent<ITutorialStep>();

            tutorialStep.Started += OnStepStart;
        }
        
        private void OnDisable()
        {
            tutorialStep.Started -= OnStepStart;
        }
        
        private void OnStepStart()
        {
            gazeableManager.gazingEnabled = true;
            var target = gazeables.GetGazeable(part);
            if(target)
                target.gameObject.SetActive(true);

            button.interactable = false;
        
            gazeableManager.OnGazeCompleted.AddListener(Finish);
        }

        private void Finish()
        {
            var target = gazeables.GetGazeable(part);
            if(target)
                target.gameObject.SetActive(false);
            button.interactable = true;
            gazeableManager.OnGazeCompleted.RemoveListener(Finish);
        }
    }
}
