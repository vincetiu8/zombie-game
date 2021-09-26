using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Lobby
{
	public class RoomListItem : MonoBehaviour
	{
		[SerializeField] private TMP_Text text;

		private RoomInfo _info;

		public void Setup(RoomInfo info)
		{
			_info = info;
			text.text = info.Name;
		}

		public void OnClick()
		{
			Launcher.instance.JoinRoom(_info);
		}
	}
}