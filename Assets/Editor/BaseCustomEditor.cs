using System.Collections.Generic;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(plzDeleteScript))]
    public class BaseCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            plzDeleteScript script = (plzDeleteScript)target;

            base.OnInspectorGUI();
            ShowVariablesOnBool("showStuff", new string[] {"number", "text"});

            //script.showStuff = EditorGUILayout.Toggle("yee", script.showStuff);
            //ShowIfBool(script.showStuff, new string[] {"number", "text"});

        }

        private void ShowVariablesOnBool(string boolName, IEnumerable<string> listOfVariablesToShow)
        {
            SerializableVariable(boolName);
            bool showList = serializedObject.FindProperty(boolName).boolValue;
            ShowIfBool(showList, listOfVariablesToShow);

            //EditorGUILayout.LabelField(serializedObject.FindProperty(variable).boolValue.ToString());
        }

        private void ShowIfBool(bool show, IEnumerable<string> variableList)
        {
            if (show)
            {
                
                SerializableVariable(variableList);
            }
        }
        
        private void SerializableVariable(string variableName)
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(variableName), true);
            serializedObject.ApplyModifiedProperties();
        }
        private void SerializableVariable(IEnumerable<string> variableNames)
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