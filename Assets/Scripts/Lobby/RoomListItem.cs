using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Lobby
{
	public class RoomListItem : MonoBehaviour
	{
		[SerializeField]  private TMP_Text text;
		[HideInInspector] public  RoomInfo RoomInfo;

		public void Setup(RoomInfo info)
		{
			RoomInfo = info;
			text.text = info.Name;
		}

		public void OnClick()
		{
			Launcher.Instance.JoinRoom(RoomInfo);
		}
	}
}