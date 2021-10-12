using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(plzDeleteScript))]
    public class plzDeleteScriptEditor : BaseCustomEditor
    {
        public override void OnInspectorGUI()
        {
            plzDeleteScript script = (plzDeleteScript)target;

            SerializableVariable("alwaysShow");
            
            ShowVariablesOnBool("showStuff", new string[] {"number", "text"});
            
            ShowVariablesOnBool("showMoreStuff", new string[] {"gameObject", "monoBehaviour", "transform", "vector2"});
        }
    }

}