using Photon.Pun;
using UnityEngine;

namespace Networking
{
    public class ObjectSetup : MonoBehaviour
    {
        [SerializeField] private Behaviour[] componentsToDisableIfNotMasterClient;

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                return;
            }
            
            foreach (Behaviour behaviour in componentsToDisableIfNotMasterClient)
            {
                behaviour.enabled = false;
            }
        }
    }
}