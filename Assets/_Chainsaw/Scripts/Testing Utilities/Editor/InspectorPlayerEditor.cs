using UnityEditor;
using UnityEngine;

namespace _Chainsaw.Scripts.Testing_Utilities.Editor
{
    [CustomEditor(typeof(InspectorPlayer))]
    public class InspectorPlayerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            InspectorPlayer s = (InspectorPlayer)target;

            if(GUILayout.Button("Play"))
            {
                s.Play();
            }

            if (GUILayout.Button("Copy button component events"))
            {
                s.CopyButton();
            }
        }
    }
}