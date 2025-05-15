using System;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace _Chainsaw.Scripts.Tutorial.Steps.Editor
{
    public class StepBaseEditor<T> : UnityEditor.Editor where T : ITutorialStep
    {
        private Button button;
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            button = new Button(FinishStep) { text = "Finish Step"};
            root.Add(button);
            
            var s = (MonoBehaviourStepWrapper<T>)target;
            
            button.SetEnabled(s.IsRunning);
            
            return root;
        }

        private void FinishStep()
        {
            var s = (MonoBehaviourStepWrapper<T>)target;
            s.Finished.Invoke();
            button.SetEnabled(s.IsRunning);
        }
        
        void OnFinished()
        {
            var s = (MonoBehaviourStepWrapper<T>)target;
            s.Finished -= OnFinished;

            button.SetEnabled(false);
        }
    }

    [CustomEditor(typeof(ChainsawStateStepBehaviour))] 
    public class ChainsawEventStepBehaviourEditor : StepBaseEditor<ChainsawStateStep> { }
    
    //[CustomEditor(typeof(ButtonClickStepBehaviour))] 
    //public class ButtonClickStepBehaviourEditor : StepBaseEditor<ButtonClickStep> { }
    
    [CustomEditor(typeof(WaitStepScriptable))]
    public class WaitStepScriptableEditor : StepBaseEditor<WaitStep> { }
    
    [CustomEditor(typeof(CuttableStepBehaviour))]
    public class CuttableStepBehaviourEditor : StepBaseEditor<CuttableStep> { }
    
    [CustomEditor(typeof(CapStepBehaviour))]
    public class CapStepBehaviourEditor : StepBaseEditor<CapStep> { }
    
    [CustomEditor(typeof(XrInteractableStepBehaviour))]
    public class XrInteractableStepBehaviourEditor : StepBaseEditor<XrInteractableStep> { }
    
    [CustomEditor(typeof(KeyUnlockStepBehaviour))]
    public class KeyUnlockStepBehaviourEditor : StepBaseEditor<KeyUnlockStep> { }
    
    [CustomEditor(typeof(GazeStepBehaviour))]
    public class GazeStepBehaviourEditor : StepBaseEditor<GazeStep> { }
}
