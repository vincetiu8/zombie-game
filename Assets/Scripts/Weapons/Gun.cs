using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Weapons
{
    public class Gun : Weapon
    {
        [SerializeField] protected GunAttributes[] weaponLevels;
        [SerializeField] private Transform firepoint;
        [SerializeField] private GameObject bulletPrefab;

        private GunAttributes _currentGunAttributes;
        private int _bulletsInMagazine;
        private Coroutine _reloadCoroutine;

        protected override void Start()
        {
            base.Start();
            maxLevel = weaponLevels.Length;
            _currentGunAttributes = weaponLevels[currentLevel];
            currentAttributes = _currentGunAttributes;
            _bulletsInMagazine = _currentGunAttributes.magazineSize;
        }

        protected override void Fire()
        {
            if (_bulletsInMagazine < 1) return;

            if (_reloadCoroutine != null)
            {
                StopCoroutine(_reloadCoroutine);
            }
            
            GameObject bulletClone = PhotonNetwork.Instantiate(bulletPrefab.name, firepoint.position, firepoint.rotation);
            Vector2 bulletVelocity = firepoint.right * _currentGunAttributes.bulletVelocity;
            
            bulletClone.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
            bulletClone.GetComponent<Bullet>().damage = currentAttributes.damage;
            

            _bulletsInMagazine--;
            base.Fire();
        }

        public override void Reload()
        {
            if (_reloadCoroutine != null) return;
            
            _reloadCoroutine = StartCoroutine(ReloadCoroutine());
        }
        
        private IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(_currentGunAttributes.reloadTime);
            
            _bulletsInMagazine = _currentGunAttributes.magazineSize;
            _reloadCoroutine = null;
        }

        public override void Upgrade()
        {
            _currentGunAttributes = weaponLevels[currentLevel];
            currentAttributes = _currentGunAttributes;
        }
        
        public override string ToString()
        {
            return $"{weaponName} Level {currentLevel} Stats:\n{currentAttributes}";
        }
    }
}