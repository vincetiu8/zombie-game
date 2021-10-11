using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        }
/// <summary>
///  <para>Are all strings because there is no script to be inheriting from in the base editor.</para>
///  <para>Any variables called here MUST be serialized.</para>
///  <example><code> ShowVariablesOnBool("boolToShowStuff", new string[] {"number", "text"}); </code>></example>
/// </summary>
/// <param name="boolName"> Name of the bool you want to tie the variables to</param>
/// <param name="listOfVariablesToShow"> List of all the variables you want to display, datatype does not matter</param>
protected void ShowVariablesOnBool(string boolName, IEnumerable<string> listOfVariablesToShow)
        {
            // Displays bool in custom inspector
            SerializableVariable(boolName);
            
            // Show elements depending on the value of the bool
            ShowIfBool(serializedObject.FindProperty(boolName).boolValue, listOfVariablesToShow);

            //EditorGUILayout.LabelField(serializedObject.FindProperty(variable).boolValue.ToString());
        }

        protected void ShowIfBool(bool show, IEnumerable<string> variableList)
        {
            if (!show) return;
            EditorGUI.indentLevel++;
            SerializableVariable(variableList);
            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
        
        protected void SerializableVariable(string variableName)
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(variableName), true);
            serializedObject.ApplyModifiedProperties();
        }
        protected void SerializableVariable(IEnumerable<string> variableNames)
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