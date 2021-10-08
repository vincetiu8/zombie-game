using System;
using Enemy;
using UnityEditor;

namespace Editor
{
    /// <summary>
    /// Custom editor for WaveSpawner
    /// </summary>
    [CustomEditor(typeof(WaveSpawner))]
    public class WaveSpawnerEditor : UnityEditor.Editor 
    {
        // Code layout is a bit weird because order matters when displaying fields in the inspector
        public override void OnInspectorGUI()
        {
            WaveSpawner waveSpawner = target as WaveSpawner;

            waveSpawner.useRandomWaves = EditorGUILayout.Toggle("Random Waves", waveSpawner.useRandomWaves);
            waveSpawner.fixedMultiplier = EditorGUILayout.Toggle("Fixed Multiplier", waveSpawner.fixedMultiplier);
            
            using (var randomWaveGroup = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(waveSpawner.useRandomWaves)))
            {
                if (randomWaveGroup.visible)
                {
                    serializedObject.Update();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("randomWaves"), true);
                    serializedObject.ApplyModifiedProperties();
                }
            }
            using (var fixedWaveGroup = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(!waveSpawner.useRandomWaves)))
            {
                if (fixedWaveGroup.visible)
                {
                    serializedObject.Update();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("waves"), true);
                    serializedObject.ApplyModifiedProperties();
                }
            }
            
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnpoints"), true);
            serializedObject.ApplyModifiedProperties();
            
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("waveDelay"), true);
            serializedObject.ApplyModifiedProperties();

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("searchIntervalAmount"), true);
            serializedObject.ApplyModifiedProperties();
        
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("increaseStats"), true);
            serializedObject.ApplyModifiedProperties();

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("resetStatIncrease"), true);
            serializedObject.ApplyModifiedProperties();
            
            using (var randomStatGroup =                            
                new EditorGUILayout.FadeGroupScope(Convert.ToSingle(!waveSpawner.fixedMultiplier)))
            {   
                // fixedMultiplier isn't inverted here because doing so makes it display weirdly
                // Means I have to create a new group for fixed and random vars
                if (randomStatGroup.visible)
                {
                    serializedObject.Update();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("randomStatMin"), true);
                    serializedObject.ApplyModifiedProperties();
                
                    serializedObject.Update();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("randomStatMax"), true);
                    serializedObject.ApplyModifiedProperties();
                
                    serializedObject.Update();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("randomMinIncrement"), true);
                    serializedObject.ApplyModifiedProperties();
                    
                    serializedObject.Update();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("randomMaxIncrement"), true);
                    serializedObject.ApplyModifiedProperties();
                }
            }

            using (var fixedStatGroup =
                new EditorGUILayout.FadeGroupScope(Convert.ToSingle(waveSpawner.fixedMultiplier)))
            {
                if (fixedStatGroup.visible)
                {
                    serializedObject.Update();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("fixedStatMultiplier"), true);
                    serializedObject.ApplyModifiedProperties();
                    
                    serializedObject.Update();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("fixedStatDivider"), true);
                    serializedObject.ApplyModifiedProperties();

                    serializedObject.Update();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("fixedStatIncrement"), true);
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
