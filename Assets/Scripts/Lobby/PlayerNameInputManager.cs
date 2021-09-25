using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
	[RequireComponent(typeof(InputField))]
	public class PlayerNameInputManager : MonoBehaviour
	{
		#region Variables

		private const string PlayerNamePrefKey = "PlayerName";

		#endregion

		#region Unity Methods

		private void Start()
		{
			string defaultName = string.Empty;
			InputField inputField = GetComponent<InputField>();
			if (inputField)
				if (PlayerPrefs.HasKey(PlayerNamePrefKey))
				{
					defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
					inputField.text = defaultName;
				}

			PhotonNetwork.NickName = defaultName;
		}

		#endregion

		#region Public Methods

		public void SetPlayerName(string value)
		{
			if (string.IsNullOrEmpty(value)) return;

			PhotonNetwork.NickName = value;
			PlayerPrefs.SetString(PlayerNamePrefKey, value);
		}

		#endregion
	}
}