using UnityEngine;

namespace Levels
{
    public class WindowController : Health
    {
        [SerializeField] private GameObject barricadesGraphics;
        [SerializeField] private GameObject windowCollider; // Will prevent the enemy from going forward.
        
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
            float previousHealth = _health;
            _health = Mathf.Clamp(_health + change, 0, initialHealth);
            if (Mathf.Abs(previousHealth - _health) < 0.01f) return; // If zombies hitting an already destroyed window or player fixing an already fixed window

            windowCollider.SetActive(_health != 0);

            if (_health - previousHealth < 0 && Mathf.CeilToInt(_health) != 6)
            {
                barricadesGraphics.transform.GetChild(Mathf.CeilToInt(_health)).gameObject.SetActive(false);
            }
            else if (_health - previousHealth > 0)
            {
                barricadesGraphics.transform.GetChild((int)_health-1).gameObject.SetActive(true);
            }
        }


    }
}
