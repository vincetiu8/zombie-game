using Photon.Pun;
using UnityEngine;

namespace Interact
{
    /// <summary>
    ///  Used for things like Unlockable areas and the power switch
    /// </summary>
    public class Door : Unlockable
    {
        private Collider2D[] _myColList;
        private UnlockableAreas unlockableArea;

        
        protected override void Start()
        {
            base.Start();
            _myColList = transform.GetComponentsInChildren<Collider2D>();
            unlockableArea = GameObject.FindObjectOfType<UnlockableAreas>();
        }
        
        private void SetAllCollidersStatus(bool active)
        {
            foreach (Collider2D colliders in _myColList) colliders.enabled = active;
        }

        public override void Interact()
        {
            base.Interact();
            photonView.RPC("RpcUnlockDoor", RpcTarget.All);
        }

        [PunRPC]
        private void RpcUnlockDoor()
        {
            SetAllCollidersStatus(false);
            canvas.gameObject.SetActive(false);
            Unlock();
            unlockableArea.UnlockArea(itemName);
        }
    }
}
