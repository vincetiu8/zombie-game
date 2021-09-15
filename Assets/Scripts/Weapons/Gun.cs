using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Weapons
{
    public class Gun : MonoBehaviourPun, IWeapon
    {
        [SerializeField] private string weaponName;
        [SerializeField] private Transform firepoint;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private WeaponAttributes[] weaponLevels;
        [SerializeField] private AmmoType ammoType;

        private int _currentLevel;
        private int _bulletsInMagazine;
        private float _fireCooldown;
        private WeaponAttributes _currentAttributes;
        private Coroutine _reloadCoroutine;
        private AmmoInventory _ammoInventory;

        private void Start()
        {
            _currentAttributes = weaponLevels[0];
            _bulletsInMagazine = _currentAttributes.magazineSize;
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            
            if (_fireCooldown > 0)
            {
                _fireCooldown -= Time.deltaTime;
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

        public void Fire()
        {
            if (_fireCooldown > 0 || _bulletsInMagazine < 1) return;

            if (_reloadCoroutine != null)
            {
                StopCoroutine(_reloadCoroutine);
            }
            
            GameObject bulletClone = PhotonNetwork.Instantiate(bulletPrefab.name, firepoint.position, firepoint.rotation);
            Vector2 bulletVelocity = firepoint.right * _currentAttributes.bulletVelocity;
            
            bulletClone.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
            bulletClone.GetComponent<Bullet>().damage = _currentAttributes.damage;
            

            _bulletsInMagazine--;
        }

        public void Reload()
        {
            if (_reloadCoroutine != null || _ammoInventory.GetAmmo(ammoType) == 0) return;

            _reloadCoroutine = StartCoroutine(ReloadCoroutine());
        }
        
        private IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(_currentAttributes.reloadTime);

            _bulletsInMagazine = _ammoInventory.WithdrawAmmo(ammoType, _currentAttributes.magazineSize);
        }

        public override string ToString()
        {
            return $"{weaponName} Level {_currentLevel} Stats:\n{_currentAttributes}";
        }
    }
}