using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class BaseCustomEditor : UnityEditor.Editor
    {
        /// <summary>
        ///  <para>Are all strings because there is no script to be inheriting from in the base editor.</para>
        /// </summary>
        /// <param name="boolName"> Name of the bool you want to tie the variables to</param>
        /// <param name="listOfVariablesToShow"> List of all the variables you want to display, datatype does not matter</param>
        /// <remarks><para>Any variables called here MUST already be serialized in the script it is called from.</para></remarks>
        /// <example><code> ShowVariablesOnBool("boolToShowStuff", new string[] {"number", "text"}); </code>></example>
        protected void ShowVariablesOnBool(string boolName, IEnumerable<string> listOfVariablesToShow)
        {
            // Displays bool in custom inspector
            SerializableVariable(boolName);
            
            // Show elements depending on the value of the bool
            if (!serializedObject.FindProperty(boolName).boolValue) return;
            EditorGUI.indentLevel++;
            SerializableVariable(listOfVariablesToShow);
            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            //EditorGUILayout.LabelField(serializedObject.FindProperty(variable).boolValue.ToString());
        }

        /// <summary>
        /// Serializes a variable in the custom inspector
        /// </summary>
        /// <remarks> Think of this like a [SerializeField] but for custom inspector, meaning any <c>Attributes</c> 
        /// given to the variable (ie. <c>Tooltip</c> , <c>Range</c>) will carry over as well</remarks>
        /// <param name="variableName"> Name of the variable you want to serialize (MUST already be serialized in the script as well)</param>
        protected void SerializableVariable(string variableName)
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(variableName), true);
            serializedObject.ApplyModifiedProperties();
        }
        
        /// <summary>
        /// Serializes a list variable in the custom inspector
        /// </summary>
        /// <param name="variableNames"> List of names of the variables you want to serialize (all of them MUST be serialized in the script as well) </param>
        protected void SerializableVariable(IEnumerable<string> variableNames)
        {
            foreach (string varName in variableNames)
            {
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedObject.FindProperty(varName), true);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}