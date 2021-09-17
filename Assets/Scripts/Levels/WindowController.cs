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
            float previousHealth = health;
            health = Mathf.Clamp(health + change, 0, initialHealth);
            if (Mathf.Abs(previousHealth - health) < 0.01f) return; // If zombies hitting an already destroyed window or player fixing an already fixed window

            windowCollider.SetActive(health != 0);

            if (health - previousHealth < 0 && Mathf.CeilToInt(health) != 6)
            {
                barricadesGraphics.transform.GetChild(Mathf.CeilToInt(health)).gameObject.SetActive(false);
            }
            else if (health - previousHealth > 0)
            {
                barricadesGraphics.transform.GetChild((int)health-1).gameObject.SetActive(true);
            }
        }


    }
}
