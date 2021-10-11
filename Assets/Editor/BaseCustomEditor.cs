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
            TieShowToBool(script.showStuff, new string[] {"number", "text"});
            
           
        }

        void TieShowToBool(bool variable, IEnumerable<string> variableList)
        {
            //bool show = EditorGUILayout.PropertyField(serializedObject.FindProperty(variable), true);
            
            System.Reflection.PropertyInfo propName = script.GetType().GetProperty( "name" );
            if( propName != null )
            {
                propName.SetValue(     
                    this, // So we specify who owns the object
                    "blorp", // A C# object as the value, will be casted (if possible)
                    null
                );
 
                // And GetValue can be used in a similar fassion.
                // Equivalent of Debug.log( "..." + this.name )
                //Debug.Log( "The name is " + propName.GetValue( this, null ) );
            }
            
            ShowIfBool(show, variableList);
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