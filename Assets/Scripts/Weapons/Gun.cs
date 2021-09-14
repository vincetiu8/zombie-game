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
        //test comment
        private int _currentLevel;
        private int _bulletsInMagazine;
        private float _fireCooldown;
        private WeaponAttributes _currentAttributes;
        private AmmoInventory ammo;
        private Coroutine _reloadCoroutine;

        private void Start()
        {
            ammo = GetComponent<AmmoInventory>();

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
            if (_reloadCoroutine != null) return;

            _reloadCoroutine = StartCoroutine(ReloadCoroutine());
        }
        
        private IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(_currentAttributes.reloadTime);

            _bulletsInMagazine = _currentAttributes.magazineSize;
            int ammoStock = ammo.GetAmmoStock(ammoType);
            ammoStock -= _currentAttributes.magazineSize;
            ammo.SetAmmoStock(ammoType, ammoStock);
        }

        public override string ToString()
        {
            return $"{weaponName} Level {_currentLevel} Stats:\n{_currentAttributes}";
        }
    }
}