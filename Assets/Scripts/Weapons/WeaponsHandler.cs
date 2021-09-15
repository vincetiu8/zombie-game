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

        private Weapon _currentWeapon;

        private void Start()
        {
            _currentWeapon = GetComponentInChildren<Gun>();
        }

        public void FireAction(InputAction.CallbackContext context)
        {
            if (!photonView.IsMine) return;
            
            // When the mouse is pressed down, two actions are sent: started and performed
            // We'll use performed here to check for the press
            if (context.performed)
            {
                _currentWeapon.ToggleFire(true);
                return;
            }

            // Cancelled indicates the mouse was released
            // This is mainly to cancel
            if (context.canceled)
            {
                _currentWeapon.ToggleFire(false);
            }
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

            float angle = Utils.Vector2ToDeg(direction);

            playerSprite.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            _currentWeapon.FaceMouse(direction.magnitude);
        }
    }
}
