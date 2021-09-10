using System;
using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;


namespace Game.Scripts.Weapons
{
    public class GunHandler : NetworkBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform playerSprite;
        [SerializeField] private Transform firepoint;
        [SerializeField] private GameObject bulletPrefab;
        
        public WeaponHandler currentWeapon;

        [Server]
        private void Update()
        {
            if (currentWeapon.fireCooldown > 0)
            {
                currentWeapon.fireCooldown -= Time.deltaTime;
            }
        }

        [Client]
        public void FireAction(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            CmdFire();
        }

        [Command]
        private void CmdFire()
        {
            if (currentWeapon.fireCooldown > 0 || currentWeapon.bulletsInMagazine < 1) return;

            GameObject bulletClone = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
            Vector2 bulletVelocity = firepoint.right * currentWeapon.GetAttributes().bulletVelocity;
            
            bulletClone.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
            NetworkServer.Spawn(bulletClone);
            currentWeapon.Fire();
        }
        
        [Client]
        public void ReloadAction(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            CmdReload();
        }

        [Command]
        private void CmdReload()
        {
            StartCoroutine(Reload());
        }
        
        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(currentWeapon.GetAttributes().reloadTime);

            currentWeapon.bulletsInMagazine = currentWeapon.GetAttributes().magazineSize;
        }

        [Client]
        public void FaceMouse(InputAction.CallbackContext context)
        {
            Vector2 mousePos = playerCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());

            Vector2 direction = mousePos - (Vector2) playerSprite.position;
            
            playerSprite.rotation = Quaternion.Euler(0, 0, Utils.Vector2ToDeg(direction));
        }
    }
}
