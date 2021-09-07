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
        [SerializeField] private Camera camera;
        [SerializeField] private Transform playerSprite;
        [SerializeField] private Transform firepoint;
        [SerializeField] private GameObject bulletPrefab;
        
        [SerializeField] private float bulletSpeed;

        //[SerializeField] private float fireCooldown;
        [HideInInspector] public float currentDamage;
        [HideInInspector] public float currentMaxAmmo;
        [HideInInspector] public float currentMagSize;
        [HideInInspector] public float currentFireRate;
        [HideInInspector] public float currentReload;
        [HideInInspector] public float totalAmmoLeft;
        [HideInInspector] public float leftInMag;
        public string[] effects;

        [SerializeField] protected string gunName; // What name shows up when you see it in game and hold it
        [SerializeField] protected string gunDescription;// Description you read before/after you buy the gun

        private int currentTier = 0;

        protected virtual void GunStats() // use this to define your weapon's base and upgraded attributes, override when writing your own weapon scripts
        {
            AddWeaponTier(1, 10, 5, 1, 3); // This will be the base weapon attribute
            AddWeaponTier(2, 20, 10, 0.5f, 2); // This will be ultimate weapon I
            AddWeaponTier(3, 30, 15, 0.25f, 1);// This will be ultimate weapon II

            currentDamage = WeaponTiers[0].damage;
            currentMaxAmmo = WeaponTiers[0].ammo;
            currentMagSize = WeaponTiers[0].magazineSize;
            currentFireRate = WeaponTiers[0].fireRate;
            currentReload = WeaponTiers[0].reload;

            Debug.Log("Total amount of tiers: " + WeaponTiers.Count);
        }

        public class WeaponAttributes
        {
            public float damage;
            public float ammo; // the TOTAL amount of ammo weapon has, ie. if it's 0 your gun can shoot no more bullets
            public float magazineSize; //how many rounds weapon has after a reload
            public float fireRate;
            public float reload; //how many seconds it takes to reload weapon

            public WeaponAttributes(float dmg, float amo, float magsiz, float frate, float re)
            {
                damage = dmg;
                ammo = amo;
                magazineSize = magsiz;
                fireRate = frate;
                reload = re;
            }

            public void Display() //Just for easy debugging
            {
                string result = "Damage: " + damage.ToString() + ", Ammo: " + ammo.ToString() + ", Mag Size: " + ammo.ToString() + ", Fire Rate: " + fireRate.ToString() + ", Reload Time: " + reload.ToString();
                Debug.Log(result);
            }
        }
        public List<WeaponAttributes> WeaponTiers = new List<WeaponAttributes>();
        void Start()
        {
            GunStats();
            WeaponTiers[0].Display();
            leftInMag = currentMagSize;
            totalAmmoLeft = currentMaxAmmo;

            //UpgrageWeapon();
        }

        void AddWeaponTier(float damage, float ammo, float magazineSize, float fireRate, float reload)
        {
            WeaponTiers.Add(new WeaponAttributes(damage, ammo, magazineSize, fireRate, reload));
        }
        public void UpgrageWeapon()
        {
            if (currentTier >= (WeaponTiers.Count - 1))
            {
                Debug.Log("Already at max weapon level");
                return;
            }
            currentTier = currentTier + 1;

            gunName = gunName + " I";
            currentDamage = WeaponTiers[currentTier].damage;
            currentMaxAmmo = WeaponTiers[currentTier].ammo;
            currentMagSize = WeaponTiers[currentTier].magazineSize;
            currentFireRate = WeaponTiers[currentTier].fireRate;
            currentReload = WeaponTiers[currentTier].reload;

            MaxAmmo();

            WeaponTiers[currentTier].Display();

        }

        //Perks activation section
        public void MaxAmmo()
        {
            totalAmmoLeft = currentMaxAmmo;
            leftInMag = currentMagSize;
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
            if (leftInMag == 0) { Debug.Log("No more Ammo!"); return; }
            if (context.started) return;

            CmdFire();
        }
        public void Reload(InputAction.CallbackContext context)
        {
            if (leftInMag == currentMagSize) {Debug.Log("Magazine already full!"); return; }
            if (totalAmmoLeft == 0) { Debug.Log("No more spare rounds left for reload!"); return; }
            if (context.started) return;

            StartCoroutine(CmdReload());
        }

        [Command]
        private void CmdFire()
        {
            if (_cooldown > 0) return;
            GameObject bulletClone = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
            bulletClone.GetComponent<Rigidbody2D>().velocity = firepoint.right * bulletSpeed;
            //CollisionDamager _collisionDamager = bulletClone.GetComponent<CollisionDamager>();
            //_collisionDamager.TransferAttributes(); Haven't written these functions yet, wanted to pass this PR first before anything
            NetworkServer.Spawn(bulletClone);
            _cooldown = currentFireRate;
            leftInMag = leftInMag - 1;
            Debug.Log("Left in mag: " + leftInMag);

        }

        private IEnumerator CmdReload()
        {
            Debug.Log("Reloading");
            _cooldown = currentReload;
            //play animation
            yield return new WaitUntil(() => _cooldown <= 0);
            Debug.Log("Current magazine size: " + currentMagSize);
            if (totalAmmoLeft > currentMagSize)
            {
                totalAmmoLeft = totalAmmoLeft - (currentMagSize - leftInMag);
                leftInMag = currentMagSize;
            }
            else
            {
                leftInMag = totalAmmoLeft;
                totalAmmoLeft = 0;
            }
            Debug.Log("Total ammo left: " + totalAmmoLeft);
            
        }

        public void FaceMouse(InputAction.CallbackContext context)
        {
            Vector2 mousePos = camera.ScreenToWorldPoint(context.ReadValue<Vector2>());

            Vector2 direction = mousePos - (Vector2) playerSprite.position;
            
            playerSprite.rotation = Quaternion.Euler(0, 0, Utils.Vector2ToDeg(direction));
        }
    }
}
