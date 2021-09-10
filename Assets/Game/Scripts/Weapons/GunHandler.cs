using System;
using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;


namespace Game.Scripts.Weapons
{
    [RequireComponent(typeof(GunFunctions))]
    public class GunHandler : NetworkBehaviour
    {

        [SerializeField] private Camera camera;
        [SerializeField] private Transform playerSprite;
        [SerializeField] private Transform firepoint;
        [SerializeField] private GameObject bulletPrefab;

        [HideInInspector] public bool interruptAnimations;
        
        public struct Gun
        {
            [HideInInspector] public string gunName; // What name shows up when you see it in game and hold it
            [HideInInspector] public string gunDescription;// Description you read before/after you buy the gun
            [HideInInspector] public float totalAmmoLeft;
            [HideInInspector] public float leftInMag;
        }
        [SerializeField] private float bulletSpeed;

        Gun currentGunAttributes = new Gun();  

        //public string[] effects; Saved for a later PR

        public List<GunFunctions.WeaponAttributes> WeaponTiers = new List<GunFunctions.WeaponAttributes>();

        private int currentTier = 0;

        protected virtual void Start()
        {
            WeaponTiers[0].Display();
            currentGunAttributes.leftInMag = WeaponTiers[currentTier].magazineSize;
            currentGunAttributes.totalAmmoLeft = WeaponTiers[currentTier].maxAmmo;

            //UpgrageWeapon();

            AddWeaponTier(1, 10, 5, 1, 3); // This will be the base weapon attribute
            AddWeaponTier(2, 20, 10, 0.5f, 2); // This will be ultimate weapon I
            AddWeaponTier(3, 30, 15, 0.25f, 1);// This will be ultimate weapon II

            Debug.Log("Total amount of tiers: " + WeaponTiers.Count);
        }



        void AddWeaponTier(float damage, float ammo, float magazineSize, float fireRate, float reload)
        {
            WeaponTiers.Add(new GunFunctions.WeaponAttributes(damage, ammo, magazineSize, fireRate, reload));
        }


        public void UpgrageWeapon()
        {
            if (currentTier >= (WeaponTiers.Count - 1))
            {
                Debug.Log("Already at max weapon level");
                return;
            }
            currentTier = currentTier + 1;

            MaxAmmo();

            WeaponTiers[currentTier].Display();

        }

        //Perks activation section
        public void MaxAmmo()
        {
            currentGunAttributes.totalAmmoLeft = WeaponTiers[currentTier].maxAmmo;
            currentGunAttributes.leftInMag = WeaponTiers[currentTier].magazineSize;
        }




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
        public void Reload(InputAction.CallbackContext context)
        {
            if (context.started) return;

            StartCoroutine(CmdReload());
        }

        [Command]
        private void CmdFire()
        {
            if (_cooldown > 0) return;
            if (currentGunAttributes.leftInMag == 0) { Debug.Log("No more Ammo!"); return; }
            GameObject bulletClone = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
            bulletClone.GetComponent<Rigidbody2D>().velocity = firepoint.right * bulletSpeed;
            //CollisionDamager _collisionDamager = bulletClone.GetComponent<CollisionDamager>();
            //_collisionDamager.TransferAttributes(); Haven't written these functions yet, wanted to pass this PR first before anything
            NetworkServer.Spawn(bulletClone);
            _cooldown = WeaponTiers[currentTier].fireCooldown;
            currentGunAttributes.leftInMag = currentGunAttributes.leftInMag - 1;
            Debug.Log("Left in mag: " + currentGunAttributes.leftInMag);

        }

        private IEnumerator CmdReload()
        {
            if (currentGunAttributes.leftInMag == WeaponTiers[currentTier].magazineSize) { Debug.Log("Magazine already full!"); yield break; }
            if (currentGunAttributes.totalAmmoLeft == 0) { Debug.Log("No more spare rounds left for reload!"); yield break
                    ; }
            Debug.Log("Reloading");
            _cooldown = WeaponTiers[currentTier].reload;
            //play animation
            while (_cooldown > 0) 
             // if you think this is still not good enough, ill move this to update, but i feel that this accomplishes the same thing.
            {
                if (interruptAnimations) yield break;
                yield return new WaitForSeconds(0.01f);//Done to prevent lag
            }
            //yield return new WaitUntil(() => _cooldown <= 0);
            Debug.Log("Current magazine size: " + WeaponTiers[currentTier].magazineSize);
            if (currentGunAttributes.totalAmmoLeft > WeaponTiers[currentTier].magazineSize)
            {
                currentGunAttributes.totalAmmoLeft = currentGunAttributes.totalAmmoLeft - (WeaponTiers[currentTier].magazineSize - currentGunAttributes.leftInMag);
                currentGunAttributes.leftInMag = WeaponTiers[currentTier].magazineSize;
            }
            else
            {
                currentGunAttributes.leftInMag = currentGunAttributes.totalAmmoLeft;
                currentGunAttributes.totalAmmoLeft = 0;
            }
            Debug.Log("Total ammo left: " + currentGunAttributes.totalAmmoLeft);
            
        }

        public void FaceMouse(InputAction.CallbackContext context)
        {
            Vector2 mousePos = camera.ScreenToWorldPoint(context.ReadValue<Vector2>());

            Vector2 direction = mousePos - (Vector2) playerSprite.position;
            
            playerSprite.rotation = Quaternion.Euler(0, 0, Utils.Vector2ToDeg(direction));
        }
    }
}
