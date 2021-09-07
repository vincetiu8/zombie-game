using System;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Weapons
{
    public class GunHandler : NetworkBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private Transform playerSprite;
        [SerializeField] private Transform firepoint;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float fireCooldown;

        [SyncVar] private float _cooldown;

        // Should only be run on the server
        private void Update()
        {
            if (_cooldown > 0)
            {
                _cooldown -= Time.deltaTime;
            }
        }

        [Client]
        public void Fire(InputAction.CallbackContext context)
        {
            if (context.started) return;
            
            CmdFire();
        }

        [Command]
        private void CmdFire()
        {
            if (_cooldown > 0) return;
            GameObject bulletClone = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
            bulletClone.GetComponent<Rigidbody2D>().velocity = firepoint.right * bulletSpeed;
            NetworkServer.Spawn(bulletClone);
            _cooldown = fireCooldown;
        }

        public void FaceMouse(InputAction.CallbackContext context)
        {
            Vector2 mousePos = camera.ScreenToWorldPoint(context.ReadValue<Vector2>());

            Vector2 direction = mousePos - (Vector2) playerSprite.position;
            
            playerSprite.rotation = Quaternion.Euler(0, 0, Utils.Vector2ToDeg(direction));
        }
    }
}
