using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

namespace Networking {
    public class PlayerSetup : MonoBehaviour {
        private PhotonView view;

        [SerializeField] private Behaviour[] componentsToDisableIfMine;
        [SerializeField] private Behaviour[] componentsToDisableIfNotMine;

        private void Start() {
            view = GetComponent<PhotonView>();
            if (!view.IsMine) {
                for (int i = 0; i < componentsToDisableIfNotMine.Length; i++) {
                    componentsToDisableIfNotMine[i].enabled = false;
                }
            }
            if (view.IsMine) {
                for (int i = 0; i < componentsToDisableIfMine.Length; i++) {
                    componentsToDisableIfMine[i].enabled = false;
                }
            }
        }
    }   
}
