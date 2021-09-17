using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class WindowController : Health
    {
        [SerializeField] private GameObject barricadesGraphics;
        [SerializeField] private GameObject windowCollider; // Will prevent the enemy from going forward.
        private float _previousHealth;
        [HideInInspector] public bool zombieAtWindow;
        [SerializeField] private float barricadeBreakRate;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                ChangeHealth(-barricadeBreakRate * Time.deltaTime);
                zombieAtWindow = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                zombieAtWindow = false;
            }
        }

        public override void ChangeHealth(float change)
        {
            _health = Mathf.Clamp(_health + change, 0, initialHealth);
            if (_previousHealth == _health) return; // If zombies hitting an already destroyed window or player fixing an already fixed window

            if (_health == 0) windowCollider.SetActive(false);
            else windowCollider.SetActive(true);
            
            if ((_health - _previousHealth < 0 && (Mathf.CeilToInt(_health) != 6)))
            {
                barricadesGraphics.transform.GetChild(Mathf.CeilToInt(_health)).gameObject.SetActive(false);
            }
            else if ((_health - _previousHealth > 0))
            {
                barricadesGraphics.transform.GetChild((int)_health-1).gameObject.SetActive(true);
            }
            _previousHealth = _health;
        }


    }
}
