using UnityEngine;

namespace Interact
{
    /// <summary>
    ///  Used for things like Unlockable areas and the power switch
    /// </summary>
    public class Door : Unlockable
    {
        private Collider2D[] _myColList;

        protected override void Start()
        {
            base.Start();
            _myColList = transform.GetComponentsInChildren<Collider2D>();
        }
        
        private void SetAllCollidersStatus(bool active)
        {
            foreach (Collider2D colliders in _myColList) colliders.enabled = active;
        }

        protected override void Unlock()
        {
            base.Unlock();
            SetAllCollidersStatus(false);
            canvas.gameObject.SetActive(false);
            // Call the new area method.
        }
    }
}
