using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class WindowController : MonoBehaviour
    {
        public GameObject barricadesGraphics;
        public GameObject windowCollider; //Will prevent the enemy from going forward.

        [SerializeField] private int maxRepairState; //Coding it in case there will be many differnt window sizes.
        [HideInInspector] public int repairState;

        private float nextActionTime = 0.0f;
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
                if (Time.time > nextActionTime)
                {
                    //Everything in here will be called every barricadeBreakTime interval
                    nextActionTime = Time.time + barricadeBreakTime;
                    ChangeWindowState(-1);
                }

            }
        }


        public void ChangeWindowState(int stateChange)
        {
            if (repairState + stateChange <= 0) //all barricades broken, enemy can go forward
            {
                repairState = 0;
                windowCollider.SetActive(false); //stopped at 0 so window can be repaired back to state 1
            }
            else if (repairState + stateChange > maxRepairState)
            {
                repairState = maxRepairState;
                Debug.Log("Window fully fixed");
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
