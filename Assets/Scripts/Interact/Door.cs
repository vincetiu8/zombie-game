using System;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using Networking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/*
namespace Interact
{
    /// <summary>
    ///  Used for things like Unlockable areas and the power switch
    /// </summary>
    public class Door : Unlockable
    {
        #region Script
        
        //private WaveSpawner _waveSpawner;
        
        // Should be the enemySpawnpoint of the room being opened
        [SerializeField] private bool _enemySpawnpoint;
        //[SerializeField] private Transform enemySpawnpoint;
        [SerializeField] private Room roomToUnlock;

        private Collider2D[] _allColList;
        private Canvas[] _allCanvas;

        protected override void Start()
        {
            base.Start();
            Transform parent = transform.parent;
            _allColList = parent.GetComponentsInChildren<Collider2D>();
            _allCanvas = parent.GetComponentsInChildren<Canvas>();
            //_waveSpawner = GameManager.instance.GetComponent<WaveSpawner>();
        }
        
        private void SetAllCollidersStatus(bool active)
        {
            foreach (Collider2D colliders in _allColList) colliders.enabled = active;
        }

        protected override void EnoughMoneyInteract()
        {
            base.Interact();
            photonView.RPC("RpcUnlockDoor", RpcTarget.All);
        }

        [PunRPC]
        private void RpcUnlockDoor()
        {
            SetAllCollidersStatus(false);
            foreach (Canvas canvases in _allCanvas)
            {
                canvases.gameObject.SetActive(false);
            }
            Unlock();
            
            Debug.Log("adding spawnpoints");
            
            if (!_enemySpawnpoint) return;
            // Take all the transforms of the children found in "Enemy" into a list (Enemy should already be active by default)
            //List<Transform> enemySpawnPoints = enemySpawnpoint.Cast<Transform>().ToList();
            roomToUnlock.UnlockRoom();
            //_waveSpawner.AddSpawnPoints(enemySpawnPoints);
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.gameObject.layer == LayerMask.NameToLayer("Players"))
                DisplayNamePrice(true);
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
            if (collision.gameObject.layer == LayerMask.NameToLayer("Players")) 
                DisplayNamePrice(false);
        }

        private void DisplayNamePrice(bool doDisplay)
        {
            itemNameUI.gameObject.SetActive(doDisplay);
            itemPriceUI.gameObject.SetActive(doDisplay);
            Debug.Log("setting active: " + doDisplay);
        }
        
        #endregion

        #region Editor
    #if UNITY_EDITOR
        
        [CustomEditor(typeof(Door))]
        public class DoorEditor : Editor
        {
            ///*
            public override void OnInspectorGUI()
            {
                //base.OnInspectorGUI();
                
                EditorGUIUtility.labelWidth = 220;

                // Add reference to script
                Door door = (Door)target;
                
                EditorGUILayout.LabelField("Info to display", EditorStyles.boldLabel);
                
                EditorGUILayout.BeginHorizontal();
                
                    EditorGUILayout.LabelField("Name of room", GUILayout.MaxWidth(120));
                    door.itemName = EditorGUILayout.TextField(door.itemName, GUILayout.MaxWidth(100));
                
                    EditorGUILayout.LabelField("Cost to open door", GUILayout.MaxWidth(120));
                    door.purchasePrice = EditorGUILayout.IntField(door.purchasePrice, GUILayout.MaxWidth(100));
                
                EditorGUILayout.EndHorizontal();
                // Add separator
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Begin sprite area
                door._hasSprite = EditorGUILayout.Toggle("Has Sprite?", door._hasSprite, GUILayout.MaxWidth(150));
                if (door._hasSprite)
                {
                    EditorGUI.indentLevel++;
                    
                    door.spriteRenderer = (SpriteRenderer) EditorGUILayout.ObjectField("Sprite Renderer", door
                    .spriteRenderer, typeof(SpriteRenderer));
                    
                    door.beforeUnlock = (Sprite) EditorGUILayout.ObjectField("Sprite before unlock", door
                        .beforeUnlock, typeof(Sprite), GUILayout.Height(200));
                    
                    door.afterUnlock = (Sprite) EditorGUILayout.ObjectField("Sprite before unlock", door
                        .afterUnlock, typeof(Sprite), GUILayout.Height(200));
                    
                    EditorGUI.indentLevel--;

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }

                // Begin Canvas area
                door._hasCanvas = EditorGUILayout.Toggle("Has Canvas?", door._hasCanvas, GUILayout.MaxWidth(150));
                if (door._hasCanvas)
                {
                    EditorGUI.indentLevel++;

                    door.myCanvas = (Canvas) EditorGUILayout.ObjectField("This side of the door's Canvas", door
                        .myCanvas, typeof(Canvas));
                    
                    door.itemNameUI = (Text) EditorGUILayout.ObjectField("Text object to display area name", door
                        .itemNameUI, typeof(Text));
                    
                    door.itemPriceUI = (Text) EditorGUILayout.ObjectField("Text object to display area price", door
                        .itemPriceUI, typeof(Text));

                    EditorGUI.indentLevel--;
                    
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }
                
                // Begin enemy spawnpoint area
                door._enemySpawnpoint = EditorGUILayout.Toggle("Enemy Spawnpoint in new area?", door._enemySpawnpoint, GUILayout.MaxWidth(150));
                if (door._enemySpawnpoint)
                {
                    EditorGUI.indentLevel++;

                    door.roomToUnlock =
                        (Room) EditorGUILayout.ObjectField("Room to unlock", door.roomToUnlock, typeof(Room));

                    //door.enemySpawnpoint = (Transform) EditorGUILayout.ObjectField("Enemy Spawn Points Gameobject", door.enemySpawnpoint, typeof
                    //(Transform));
                    
                    EditorGUI.indentLevel--;
                    
                }
                // Saves all settings to script (VERY IMPORTANT)
                EditorUtility.SetDirty(door);
            }//
        }
        
    #endif
        #endregion
        
    }
}*/
