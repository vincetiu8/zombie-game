using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class WindowController : Health
    {
        [SerializeField] private GameObject barricadesGraphics;
        [SerializeField] private GameObject windowCollider; //Will prevent the enemy from going forward.

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
            /*if (_health + change > initialHealth)
            {
                _health = initialHealth;
                Debug.Log("Window already fully fixed");
                return;
            }
            else if (_health + change <= 0) //all barricades broken, enemy can go forward
            {
                _health = 0;
                windowCollider.SetActive(false); //stopped at 0 so window can be repaired back to state
                //can't return early here otherwise the last barricade being broken won't get rendered
            }
            else
            {
                _health += change;
                windowCollider.SetActive(true);
            }*/

            _health = Mathf.Clamp(_health + change, 0, initialHealth);
            Debug.Log(_health);
            if (_health == 0)
            {
                windowCollider.SetActive(false);
            }
            else
            {
                windowCollider.SetActive(true);
            }


            float _checkingRepairState = _health;
            for (int i = 0; i < initialHealth; i++) //renders the amount of barricades to display
            {
                if (_checkingRepairState > 0) barricadesGraphics.transform.GetChild(i).gameObject.SetActive(true);
                else barricadesGraphics.transform.GetChild(i).gameObject.SetActive(false);
                _checkingRepairState -= 1;
            }
        }


    }
}
