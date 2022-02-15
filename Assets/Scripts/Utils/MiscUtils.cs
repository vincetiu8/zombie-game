using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
	public static class MiscUtils
	{
		public delegate bool ToggleFunction(InputAction action);

		public static bool IsInLayerMask(LayerMask layerMask, int layer)
		{
			return (layerMask.value & (1 << layer)) > 0;
		}

		public static void ToggleActions(PlayerInput playerInput, string[] exceptions, bool enabled)
		{
			ToggleActions(playerInput, action =>
			                           {
				                           if (Array.Exists(exceptions, s => s == action.name)) return true;

				                           ToggleAction(action, enabled);
				                           return true;
			                           });
		}

		public static void ToggleActions(PlayerInput playerInput, ToggleFunction toggleFunction)
		{
			foreach (InputAction action in playerInput.currentActionMap.actions)
			{
				if (!toggleFunction(action)) break;
			}
		}

		public static void ToggleAction(PlayerInput playerInput, string name, bool enabled)
		{
			ToggleActions(playerInput, action =>
			                           {
				                           if (action.name != name) return true;
				                           ToggleAction(action, enabled);
				                           return false;
			                           });
		}

		private static void ToggleAction(InputAction action, bool enabled)
		{
			if (enabled)
			{
				action.Enable();
				return;
			}

			action.Disable();
		}
        
        /// <summary>
        /// Get objects around caller and orders it in order of closest to farthest
        /// </summary>
        /// <param name="searchRadius"></param>
        /// <param name="layerToSearch"></param>
        /// <param name="removeTargetsBehindObstacles"> If an object is behind a wall, should it still be included in the list</param>
        /// <returns></returns>
        public static  Collider2D[] ListNearbyObjects(float searchRadius, string layerToSearch, bool 
        removeTargetsBehindObstacles, GameObject referenceObject)
        {
            LayerMask mask = LayerMask.GetMask(layerToSearch);
            List<Collider2D> targetsArray = Physics2D.OverlapCircleAll(referenceObject.transform.position, searchRadius, 
                mask).ToList();
            
            // Removes boss from list if present
            Collider2D myCollider = referenceObject.GetComponent<Collider2D>();
            if (targetsArray.Contains(myCollider)) targetsArray.Remove(myCollider);

            if (removeTargetsBehindObstacles)
                // Loops through all the objects found by overlapCircleAll
                foreach (Collider2D target in from target in targetsArray.ToList() 
                                                  
                                              // Create a Raycast between the boss and the target in question (Ray has ends on the 2 objects in question and makes a list of all objects in between)
                                              let hits = Physics2D.RaycastAll(
                                                  referenceObject.transform.position, target.transform.position - referenceObject.transform.position, 
                                                  Vector2.Distance(referenceObject.transform.position, target.transform.position)) 
                
                                              // Check if a wall is included in the array create by the Raycast (hits)
                                              where hits.Any(hit => hit.transform.gameObject.layer == LayerMask.NameToLayer("Obstacles")) select target) 
                
                    // If there is a wall found, remove object from the target list
                    targetsArray.Remove(target);
            
            // Order list by how close players are to object
            return targetsArray.OrderBy(
                    individualTarget => Vector2.Distance(referenceObject.transform.position, individualTarget.transform.position))
                .ToArray();
        }

        public static bool CheckForObjectsInRadius(float searchRadius, string layerToSearch, bool 
            removeTargetsBehindObstacles, GameObject referenceObject)
        {
            Collider2D[] targets = ListNearbyObjects(searchRadius, layerToSearch, removeTargetsBehindObstacles, 
            referenceObject);
            return targets.Length != 0;
        }
	}
}