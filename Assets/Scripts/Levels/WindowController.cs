using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class WindowController : Health
    {
        [SerializeField] private GameObject barricadesGraphics;
        [SerializeField] private GameObject windowCollider; //Will prevent the enemy from going forward.
        private float _previousHealth;

        //[SerializeField] private int maxRepairState; //Coding it in case there will be many differnt window sizes initial health
        //private int repairState; //Make this public if it needs to be accessed health

        [SerializeField] private float barricadeBreakRate;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                ChangeHealth(-barricadeBreakRate * Time.deltaTime);
                //Debug.Log(_health);

            }
        }


        public override void ChangeHealth(float change)
        {
            _previousHealth = _health;
            _health = Mathf.Clamp(_health + change, 0, initialHealth);
            if (_previousHealth == _health) return;
            if (_health == 0)
            {
                windowCollider.SetActive(false);
                barricadesGraphics.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                windowCollider.SetActive(true);
            }

            if ((_health - _previousHealth < 0))
            {
                barricadesGraphics.transform.GetChild((int)_previousHealth+1).gameObject.SetActive(false);
            }
            else if ((_health - _previousHealth > 0))
            {
                barricadesGraphics.transform.GetChild((int)_health-1).gameObject.SetActive(true);
            }
            _previousHealth = _health;
        }


    }
}
