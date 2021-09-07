using Mirror;
using UnityEngine;

namespace Game.Scripts
{
    public class NetworkSetup : NetworkBehaviour
    {
        [SerializeField] private Behaviour[] behavioursToDisable;

        private void Start()
        {
            if (isLocalPlayer) return;
        
            foreach (Behaviour behaviour in behavioursToDisable)
            {
                behaviour.enabled = false;
            }
        }
    }
}
