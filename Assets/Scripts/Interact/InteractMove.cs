using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interact
{
    public class InteractMove : Interactable
    {
        private bool _isHolding = false;
        [SerializeField] private GameObject collier;
        
        Transform stuckTo;
        Quaternion offset;
        Quaternion look;
        float distance;

        private GameObject _player;
        public override void Interact(GameObject player)
        {
            if (!_isHolding)
            {
                player.transform.Find("PlayerObject").Find("WeaponPivot").Find("Weapon").gameObject.SetActive(false); // This doesn't work, why??
                Debug.Log("Taking box");
                collier.SetActive(false);
                
                
                gameObject.transform.SetParent (player.transform.Find("PlayerObject").gameObject.transform);

                
                //player.transform.Find("PlayerObject").Find("WeaponPivot").Find("Weapon").gameObject.SetActive(false);

                transform.parent.Find("WeaponPivot").Find("Weapon").gameObject.SetActive(false);
                
                Debug.Log(player.transform.Find("PlayerObject").Find("WeaponPivot").Find("Weapon").gameObject);

                
                 
                /*_player = player.transform.Find("PlayerObject").gameObject;
                Vector3 diff = player.gameObject.transform.position - transform.position;
                offset = Quaternion.FromToRotation (player.gameObject.transform.forward, diff.normalized);
                look = Quaternion.FromToRotation (player.gameObject.transform.forward, transform.forward);
                distance = diff.magnitude;
                stuckTo = player.gameObject.transform;*/
            }
            else
            {
                player.transform.Find("PlayerObject").Find("WeaponPivot").GetChild(0).gameObject.SetActive(true);
                Debug.Log("Putting down box");
                collier.SetActive(true);
            }

            _isHolding = !_isHolding;
        }

        // Making this a child of the player causes quite a lot of bugs so i'm doing this instead

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (stuckTo != null)
            {
                /*Vector3 dir = offset * stuckTo.forward;
                transform.position = stuckTo.position - (dir * distance);
                //transform.rotation = stuckTo.rotation * 
                transform.parent.rotation = _player.transform.localRotation;*/

            }

            //transform.localRotation = Quaternion.Euler(0, 0, 40);
            Debug.Log(_player.transform.rotation);
        }
        

    }

}