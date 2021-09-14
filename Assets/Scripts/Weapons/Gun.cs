using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Weapons
{
    public abstract class Gun : MonoBehaviourPun, IWeapon
    {
        [SerializeField] private string weaponName;
        [SerializeField] private Transform firepoint;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private WeaponAttributes[] weaponLevels;
        [SerializeField] private AmmoType ammoType;

        
        protected int bulletsInMagazine;
        protected float fireCooldown;
        protected Coroutine reloadCoroutine;

        private int _currentLevel;
        private WeaponAttributes _currentAttributes;
        private Coroutine _reloadCoroutine;
        private AmmoInventory _ammoInventory;

        private void Start()
        {
            _currentAttributes = weaponLevels[0];
            bulletsInMagazine = _currentAttributes.magazineSize;
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            
            if (fireCooldown > 0)
            {
                fireCooldown -= Time.deltaTime;
            }
        }

        public void Setup(AmmoInventory ammoInventory)
        {
            _ammoInventory = ammoInventory;
        }

        public void Upgrade()
        {
            if (_currentLevel + 1 <= weaponLevels.Length) return;

            _currentLevel += 1;
            _currentAttributes = weaponLevels[_currentLevel];
        }

        public abstract void ToggleFire(bool isFiring);

        protected void Fire()
        {
            GameObject bulletClone = PhotonNetwork.Instantiate(bulletPrefab.name, firepoint.position, firepoint.rotation);
            Vector2 bulletVelocity = firepoint.right * _currentAttributes.bulletVelocity;
            
            bulletClone.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
            bulletClone.GetComponent<Bullet>().damage = _currentAttributes.damage;
            

            bulletsInMagazine--;
        }

        public void Reload()
        {
            if (_reloadCoroutine != null || _ammoInventory.GetAmmo(ammoType) == 0) return;

            reloadCoroutine = StartCoroutine(ReloadCoroutine());
        }
        
        private IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(_currentAttributes.reloadTime);

            bulletsInMagazine = _ammoInventory.WithdrawAmmo(ammoType, _currentAttributes.magazineSize);
        }

        public override string ToString()
        {
            return $"{weaponName} Level {_currentLevel} Stats:\n{_currentAttributes}";
        }
    }
}