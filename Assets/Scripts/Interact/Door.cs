using System;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using Networking;
using Photon.Pun;
using UnityEngine;

namespace Interact
{
    /// <summary>
    ///  Used for things like Unlockable areas and the power switch
    /// </summary>
    public class Door : Unlockable
    {
        private WaveSpawner _waveSpawner;
        
        // Should be the enemySpawnpoint of the room being opened
        [SerializeField] private Transform enemySpawnpoint;
        private bool _noEnemySpawnpoint;

        private Collider2D[] _allColList;
        private Canvas[] _allCanvas;

        protected override void Start()
        {
            base.Start();
            Transform parent = transform.parent;
            _allColList = parent.GetComponentsInChildren<Collider2D>();
            _allCanvas = parent.GetComponentsInChildren<Canvas>();
            _waveSpawner = GameManager.instance.GetComponent<WaveSpawner>();
            if (enemySpawnpoint == null) _noEnemySpawnpoint = true;
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
            
            if (_noEnemySpawnpoint) return;
            // Take all the transforms of the children found in "Enemy" into a list (Enemy should already be active by default)
            List<Transform> enemySpawnPoints = enemySpawnpoint.Cast<Transform>().ToList();
            
            _waveSpawner.AddSpawnPoints(enemySpawnPoints);
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
    }
}
