using System;
using UnityEngine;
using Weapons;

namespace Interact
{
    public class MovableObject : Interactable
    {
        private bool _isHolding;
        [SerializeField] private GameObject collier;

        private void Awake()
        {
            //foreach (var VARIABLE in COLLECTION)
            
                Collider[] colList = transform.GetComponentsInChildren<Collider>(); //this doint work
            Debug.Log(colList.Length);
        }

        public override void Interact(GameObject player)
        {
            if (!_isHolding)
            {

                player.transform.Find("PlayerObject").Find("WeaponPivot").gameObject.SetActive(false);
                
                Debug.Log("Taking box");
                collier.SetActive(false); 
                // Prevents colliders getting buggy and also prevents other players from taking your box while you're holding it
                gameObject.transform.SetParent (player.transform.Find("PlayerObject").gameObject.transform); 
                // Make box become part of the player, so it takes in rotation, etc
                player.GetComponent<WeaponsHandler>().currentWeapon.canAttack = false;
                // Disable player's ability to shoot (just disabling the player's weapon doesn't actually stop them from shooting)
            }
            else
            {
                player.transform.Find("PlayerObject").Find("WeaponPivot").gameObject.SetActive(true);
                Debug.Log("Putting down box");
                collier.SetActive(true);
                gameObject.transform.SetParent (null);
                player.GetComponent<WeaponsHandler>().currentWeapon.canAttack = true;
            }
            _isHolding = !_isHolding;
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (_isHolding) return;
            collision.GetComponent<PlayerInteract>().RemoveInteractableObject(gameObject);
            // Needs to remove object from list twice as 2 of the box will be added to the list since we ignored it when we picked it up
            base.OnTriggerExit2D(collision);
        }
    }

}