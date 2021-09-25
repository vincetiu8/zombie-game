using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Lobby
{
	public class RoomListItem : MonoBehaviour
	{
		#region Variables

		[SerializeField] private TMP_Text text;

		private RoomInfo _info;

		#endregion

		#region Public Methods

		public void Setup(RoomInfo info)
		{
			_info = info;
			text.text = info.Name;
		}

		public void OnClick()
		{
			Launcher.instance.JoinRoom(_info);
		}

		#endregion
	}
}