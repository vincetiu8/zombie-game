using Interact;
using Photon.Pun;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerRevive : TimedInteractable
    {
        private PlayerHealth _playerHealth;
        // Start is called before the first frame update
        void Start()
        {
            _playerHealth = GetComponent<PlayerHealth>();
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        /*public void PlayerIsDown()
        {
            photonView.RPC("RPCSetAvailableForInteract", RpcTarget.All, !toggle);
        }*/

        protected override void OnSuccessfulInteraction()
        {
            _playerHealth.ReviveSucessful();
        }
    }
}
