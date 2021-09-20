using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

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

        public override void Interact(GameObject player)
        {
            if (!_isHolding)
            {

                player.transform.Find("PlayerObject").Find("WeaponPivot").gameObject.SetActive(false);
                player.GetComponent<WeaponsHandler>().enabled = false; // Hmmm this doens't seem to work for some reason
                Debug.Log("Taking box");
                //collier.SetActive(false);
                gameObject.transform.SetParent (player.transform.Find("PlayerObject").gameObject.transform);
            }
            else
            {
                player.transform.Find("PlayerObject").Find("WeaponPivot").gameObject.SetActive(true);
                player.GetComponent<WeaponsHandler>().enabled = true;
                Debug.Log("Putting down box");
                collier.SetActive(true);
                gameObject.transform.SetParent (null);
            }

            _isHolding = !_isHolding;
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (_isHolding) return;
            base.OnTriggerExit2D(collision);
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
            //Debug.Log(_player.transform.rotation);
        }
        

    }

}