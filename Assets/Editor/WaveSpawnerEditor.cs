using System;
using System.Collections.Generic;
using Enemy;
using UnityEditor;
using UnityEngine;

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

            waveSpawner.useRandomWaves = EditorGUILayout.Toggle("Use Random Waves", waveSpawner.useRandomWaves);
            waveSpawner.fixedMultiplier = EditorGUILayout.Toggle("Use Fixed Multiplier", waveSpawner.fixedMultiplier);
            
            using (var randomWaveGroup = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(waveSpawner.useRandomWaves)))
            {
                if (randomWaveGroup.visible)
                {
                    SerializeVariable(new string[] {"randomWaves"});
                }
            }
            using (var fixedWaveGroup = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(!waveSpawner.useRandomWaves)))
            {
                if (fixedWaveGroup.visible)
                {
                    SerializeVariable(new string[] {"waves"});
                }
            }
            
            SerializeVariable(new string[] {"spawnpoints", "waveDelay", "searchIntervalAmount", "increaseStats", "resetStatIncrease"});

            using (var randomStatGroup =                            
                new EditorGUILayout.FadeGroupScope(Convert.ToSingle(!waveSpawner.fixedMultiplier)))
            {   
                // fixedMultiplier isn't inverted here because doing so makes it display weirdly
                // Means I have to create a new group for fixed and random vars
                if (randomStatGroup.visible)
                {
                    SerializeVariable(new string[] {"randomStatMin", "randomStatMax", "randomMinIncrement", "randomMaxIncrement"});
                }
            }

            using (var fixedStatGroup =
                new EditorGUILayout.FadeGroupScope(Convert.ToSingle(waveSpawner.fixedMultiplier)))
            {
                if (fixedStatGroup.visible)
                {
                    SerializeVariable(new string[] {"fixedStatMultiplier", "fixedStatDivider", "fixedStatIncrement"});
                }
            }
            
            void SerializeVariable(IEnumerable<string> variableNames)
            {
                foreach (string name in variableNames)
                {
                    serializedObject.Update();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(name), true);
                    serializedObject.ApplyModifiedProperties();
                }
            }
            
            EditorUtility.SetDirty(waveSpawner);
            
        }
    }
}
