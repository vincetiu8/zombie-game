using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputManager : MonoBehaviour
    {
        private const string PlayerNamePrefKey = "PlayerName";

        private void Start()
        {
            string defaultName = string.Empty;
            InputField inputField = GetComponent<InputField>();
            if (inputField)
            {
                if (PlayerPrefs.HasKey(PlayerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
                    inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            PhotonNetwork.NickName = value;
            PlayerPrefs.SetString(PlayerNamePrefKey, value);
        }
    }
}
