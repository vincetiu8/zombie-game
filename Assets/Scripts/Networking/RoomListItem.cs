using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Networking
{
	public class RoomListItem : MonoBehaviour
	{
		#region Variables
		[SerializeField] TMP_Text text;

		public RoomInfo info;
		#endregion

		#region Public Methods
		public void Setup(RoomInfo _info)
		{
			info = _info;
			text.text = _info.Name;
		}

		public void OnClick()
		{
			Launcher.Instance.JoinRoom(info);
		}
		#endregion
	}
}