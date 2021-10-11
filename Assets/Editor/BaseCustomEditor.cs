using System.Collections.Generic;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(plzDeleteScript))]
    public class BaseCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            TieShowToBool();
        }

        void TieShowToBool(bool variable, IEnumerable<string> variableList)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(variable)), true);
            ShowIfBool(variable, variableList);
        }

        void ShowIfBool(bool show, IEnumerable<string> variableList)
        {
            if (show)
            {
                SeralizableVariable(variableList);
            }
        }

        void SeralizableVariable(IEnumerable<string> variableNames)
        {
            serializedObject.Update();
            foreach (string varName in variableNames)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(varName), true);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}