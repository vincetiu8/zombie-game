using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

namespace Networking
{
    public class PlayerSetup : MonoBehaviour
    {
        [SerializeField] private Behaviour[] componentsToDisableIfNotMine;

        private void Start()
        {
            PhotonView view = GetComponent<PhotonView>();
            if (view.IsMine) return;

            foreach (Behaviour behaviour in componentsToDisableIfNotMine)
            {
                behaviour.enabled = false;
            }
        }
    }
}