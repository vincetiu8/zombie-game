using Enemy;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(WaveSpawner))]
    public class WaveSpawnerEditor : BaseCustomEditor
    {
        public override void OnInspectorGUI()
        {
            WaveSpawner spawner = (WaveSpawner) target;

            DrawDefaultInspector();
            
            if (GUILayout.Button("Add Fixed Wave"))
            {
                spawner.waveList.Add(new FixedWave());
            }
            if (GUILayout.Button("Add Random Wave")) 
            {
                spawner.waveList.Add(new RandomWave());
            }
            if (GUILayout.Button("Add Chance Wave")) 
            {
                spawner.waveList.Add(new ChanceWave());
            }
            
            EditorUtility.SetDirty(spawner);
        }
    }
}
