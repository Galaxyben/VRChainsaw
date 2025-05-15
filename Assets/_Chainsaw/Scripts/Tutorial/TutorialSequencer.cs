using System;
using System.Collections.Generic;
using _Chainsaw.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace _Chainsaw.Scripts.Tutorial
{
    public class TutorialSequencer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] TutorialDisplayUtilities displayUtilities;

        [Header("Settings")] 
        [SerializeField] private bool getStepsAutomatically;
        [SerializeField] List<Object> steps;
        
        private readonly Stack<ITutorialStep> sequence = new Stack<ITutorialStep>();
        private Stack<ITutorialStep> backSequence = new Stack<ITutorialStep>();
        private ITutorialStep current { get { return sequence.Count > 0 ? sequence.Peek() : null; } }
        private ITutorialStep currentBack { get { return backSequence.Count > 0 ? backSequence.Peek() : null; } }
        private bool isStarted = false;
        public delegate void TutorialEvent(ITutorialStep step); 
        public TutorialEvent StepStartedEvent;
        public TutorialEvent StepFinishedEvent;
        public TutorialEvent StepCanceledEvent;
        public TutorialEvent TutorialCompletedEvent;
        public TutorialEvent TutorialStartEvent;

        private void OnValidate()
        {
            //Check that only ITutorialSteps can be added to the list
            if (!getStepsAutomatically)
            {
                for (int i = 0; i < steps.Count; i++)
                {
                    if (steps[i] is ITutorialStep) continue;

                    if (steps[i] is GameObject go)
                    {
                        steps[i] = go.GetComponent<ITutorialStep>() as Component;
                    }
                    else
                    {
                        steps[i] = null;
                    }
                }
            }
            else
            {
                var stepObjects = GetComponentsInChildren<Transform>();
                steps.Clear();
                foreach (var stepObject in stepObjects)
                {
                    var stepComp = stepObject.GetComponent<ITutorialStep>();
                    if(stepComp != null)
                        steps.Add(stepComp as Component);
                }
            }
        }

        /// <summary>
        /// Prepare the step sequence and start tutorial
        /// </summary>
        [ContextMenu("Start Tutorial")]
        public void StartTutorial()
        {
            Debug.Log("Starting Tutorial");

            
            for (int i = steps.Count - 1; i >= 0; i--)
            {
                var step = (ITutorialStep)steps[i];
                sequence.Push(step);
            }

            isStarted = true;
            TutorialStartEvent?.Invoke(current);
            Next();
        }
        
        private void Update()
        {
            if (!isStarted) return;
            if(current != null) current.Tick(Time.deltaTime);
            if(currentBack != null) currentBack.ShouldGoBackCheck(Time.deltaTime);
        }

        /// <summary>
        /// Move to the next step in the tutorial sequence.
        /// </summary>
        private void Next()
        {
            if (sequence.Count == 0) //No more steps, complete
            {
                CompleteTutorial();
                return;
            }
            
            //Know how to go back to previous step if needed
            
            if (currentBack != null)
            {
                currentBack.GoBack += GoBackStepHandler;
                currentBack.BackStepCheckStarted();
            }

            //Get new step, start it and wait for it to finish
            if (current != null)
            {
                current.Finished += StepFinishedHandler;

                current.Initialize(displayUtilities);
                current.IsRunning = true;
                StepStartedEvent?.Invoke(current);
            }
        }

        /// <summary>
        /// Method called when steps are finished, goes to the next step.
        /// </summary>
        private void StepFinishedHandler()
        {
            current.Finished -= StepFinishedHandler; //Clean prev current
            
            current.IsRunning = false;
            StepFinishedEvent?.Invoke(current);
            
            if (currentBack != null) currentBack.GoBack -= GoBackStepHandler;
            backSequence.Push(sequence.Pop()); //Moving first step in line to first backstep in line
            
            Next();
        }

        private void GoBackStepHandler()
        {
            if (current != null) current.Finished -= StepFinishedHandler; //Clean prev current
            if (currentBack != null) currentBack.GoBack -= GoBackStepHandler;
            
            // -- End previous current steps
            
            //Stop current step
            current.Cancel();
            current.IsRunning = false;
            StepCanceledEvent?.Invoke(current);
            
            sequence.Push(backSequence.Pop()); //Move step from undo stack to do stack
            
            // -- Start new current step and backstep
            if (currentBack != null)
            {
                currentBack.GoBack += GoBackStepHandler;
                currentBack.BackStepCheckStarted();
            }            
            
            var step = current;
            step.Finished += StepFinishedHandler;
            
            step.Initialize(displayUtilities);
            step.IsRunning = true;
            StepStartedEvent?.Invoke(step);
        }

        private void CompleteTutorial()
        {
            if (current != null)
            {
                current.IsRunning = false;
                current.Finished -= StepFinishedHandler;
            }

            if (currentBack != null)
            {
                currentBack.BackStepCheckFinished();
                currentBack.GoBack -= GoBackStepHandler;
            }

            sequence.Clear();
            backSequence.Clear();
            
            displayUtilities.dialogueDisplayer.HideHeader();
            
            TutorialCompletedEvent(current);
        }
    }
    
    public interface ITutorialStep
    {
        void Initialize(TutorialDisplayUtilities displayUtilities);
        void Cancel();
        void Tick(float deltaTime);
        void ShouldGoBackCheck(float deltaTime);
        void BackStepCheckStarted();
        void BackStepCheckFinished();
        Action Started { get; set; }
        Action Finished { get; set; }
        Action GoBack { get; set; }
        bool IsRunning { get; set; }
    }

    [Serializable]
    public struct TutorialDisplayUtilities
    {
        public DialogueTextDisplayer dialogueDisplayer;
        public Button button;
        public TMP_Text buttonTmp;
    }
}