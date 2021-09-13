using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class WindowController : MonoBehaviour
    {
        [SerializeField] private GameObject barricadesGraphics;
        [SerializeField] private GameObject windowCollider; //Will prevent the enemy from going forward.

        [SerializeField] private int maxRepairState; //Coding it in case there will be many differnt window sizes.
        private int repairState; //Make this public if it needs to be accessed

        private float timer = 0.0f;
        [SerializeField] private float barricadeBreakTime;

        void Start()
        {
            repairState = maxRepairState;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //Debug.Log("Enemy in window");
                timer += Time.deltaTime;
                if (timer > barricadeBreakTime)
                {
                    timer = timer - barricadeBreakTime;
                    ChangeWindowState(-1);
                    
                }

            }
        }


        public void ChangeWindowState(int stateChange)
        {
            if (repairState + stateChange > maxRepairState)
            {
                repairState = maxRepairState;
                Debug.Log("Window already fully fixed");
                return;
            }
            else if (repairState + stateChange <= 0) //all barricades broken, enemy can go forward
            {
                repairState = 0;
                windowCollider.SetActive(false); //stopped at 0 so window can be repaired back to state
                //can't return early here otherwise the last barricade being broken won't get rendered
            }
            else
            {
                repairState += stateChange;
                windowCollider.SetActive(true);
            }

            //Debug.Log("repairState: " + repairState);

            float _checkingRepairState = repairState;
            for (int i = 0; i < maxRepairState; i++) //renders the amount of barricades to display
            {

                if (_checkingRepairState > 0)
                {
                    barricadesGraphics.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    barricadesGraphics.transform.GetChild(i).gameObject.SetActive(false);
                }
                
                _checkingRepairState -= 1;
            }
        }


    }
}
