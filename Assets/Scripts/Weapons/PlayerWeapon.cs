using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform playerSprite;
        [SerializeField] private Transform firepoint;
        [SerializeField] private GameObject bulletPrefab;
        
        public Weapon currentWeapon;
        
        private void Update()
        {
            if (currentWeapon.fireCooldown > 0)
            {
                currentWeapon.fireCooldown -= Time.deltaTime;
            }
        }
        
        public void FireAction(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            CmdFire();
        }
        
        private void CmdFire()
        {
            if (currentWeapon.fireCooldown > 0 || currentWeapon.bulletsInMagazine < 1) return;

            WeaponAttributes weaponAttributes = currentWeapon.GetAttributes();
            
            GameObject bulletClone = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
            Vector2 bulletVelocity = firepoint.right * weaponAttributes.bulletVelocity;
            
            bulletClone.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
            bulletClone.GetComponent<Bullet>().damage = weaponAttributes.damage;
            currentWeapon.Fire();
        }
        
        public void ReloadAction(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            CmdReload();
        }
        
        private void CmdReload()
        {
            StartCoroutine(Reload());
        }
        
        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(currentWeapon.GetAttributes().reloadTime);

            currentWeapon.bulletsInMagazine = currentWeapon.GetAttributes().magazineSize;
        }
        
        public void FaceMouse(InputAction.CallbackContext context)
        {
            Vector2 mousePos = playerCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());

            Vector2 direction = mousePos - (Vector2) playerSprite.position;
            
            playerSprite.rotation = Quaternion.Euler(0, 0, Utils.Vector2ToDeg(direction));
        }
    }
}
