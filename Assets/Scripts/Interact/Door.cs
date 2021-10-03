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

        protected override void Start()
        {
            base.Start();
            _myColList = transform.GetComponentsInChildren<Collider2D>();
        }
        
        private void SetAllCollidersStatus(bool active)
        {
            foreach (Collider2D colliders in _myColList) colliders.enabled = active;
        }

        public override void Interact()
        {
            base.Interact();
            photonView.RPC("RpcUnlockDoorForAll", RpcTarget.All);
        }

        [PunRPC]
        private void RpcUnlockDoorForAll()
        {
            SetAllCollidersStatus(false);
            canvas.gameObject.SetActive(false);
            Unlock();
            // Call the new area method.
        }
    }
}
