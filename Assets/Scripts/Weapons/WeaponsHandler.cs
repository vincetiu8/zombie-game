using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class WeaponsHandler : MonoBehaviourPun
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform playerSprite;

        private IWeapon _currentWeapon;

        private void Start()
        {
            _currentWeapon = GetComponentInChildren<Gun>();
        }

        public void FireAction(InputAction.CallbackContext context)
        {
            if (!photonView.IsMine) return;
            
            if (!context.started) return;
            
            _currentWeapon.Fire();
        }

        public void ReloadAction(InputAction.CallbackContext context)
        {
            if (!photonView.IsMine) return;
            
            if (!context.performed) return;

            CmdReload();
        }
        
        private void CmdReload()
        {
            _currentWeapon.Reload();
        }

        public void FaceMouse(InputAction.CallbackContext context)
        {
            if (!photonView.IsMine) return;
            
            Vector2 mousePos = playerCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());

            Vector2 direction = mousePos - (Vector2) playerSprite.position;

            playerSprite.rotation = Quaternion.Euler(0, 0, Utils.Vector2ToDeg(direction));
        }
    }
}
