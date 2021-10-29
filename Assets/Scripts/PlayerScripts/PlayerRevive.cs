using System;
using Interact;
using Networking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class PlayerRevive : TimedInteractable
    {
        private PlayerHealth _playerHealth;
        // Start is called before the first frame update
        protected override void Start()
        {
            _playerHealth = GetComponentInParent<PlayerHealth>();
            //_playerHealth = GameManager.Instance.localPlayerInstance.GetComponent<PlayerHealth>();
            base.Start();
            
        }
        
        /*public void PlayerIsDown()
        {
            photonView.RPC("RPCSetAvailableForInteract", RpcTarget.All, !toggle);
        }*/
        
        protected override void OnSuccessfulInteraction()
        {
            Debug.Log("revive sucessful");
            _playerHealth.ReviveSuccessful();
            FinishInteraction();
        }
    }
}
