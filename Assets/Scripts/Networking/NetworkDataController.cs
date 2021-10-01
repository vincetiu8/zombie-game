using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Networking
{
	public class NetworkDataController : MonoBehaviour, IPunObservable
	{
		private const            int         MaxComponents = 4;
		[SerializeField] private Component[] networkedComponents;

		private readonly byte[] _data = new byte[32];
		private          int    _offset;

		void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			Debug.Assert(networkedComponents.Length <= MaxComponents, "Too many networked components");

			ClearData();

			if (stream.IsWriting)
			{
				for (int i = 0; i < networkedComponents.Length; ++i)
				{
					if (networkedComponents[i] is INetworkSerializeView component &&
					    component.Serialize(_data, ref _offset)) SetDataComponentIndex(i);
				}

				if (_offset <= MaxComponents) return;

				int dataLength = (int)Mathf.Ceil(_offset / 8f);
				byte[] dataToSend = new byte[dataLength];

				// To avoid this, calculate required _data array size and remove this copying
				Array.Copy(_data, dataToSend, dataLength);

				stream.SendNext(dataToSend);

				return;
			}

			byte[] newData = (byte[])stream.ReceiveNext();

			for (int i = 0; i < networkedComponents.Length; ++i)
			{
				INetworkSerializeView component = networkedComponents[i] as INetworkSerializeView;

				if (!CheckDataComponentIndex(newData, i)) continue;

				component?.Deserialize(newData, ref _offset);
			}
		}

		private void ClearData()
		{
			for (int i = 0; i < _data.Length; ++i) _data[i] = 0;

			_offset = MaxComponents;
		}

		private void SetDataComponentIndex(int index)
		{
			_data[0] |= (byte)(1 << (8 - MaxComponents + index));
		}

		private static bool CheckDataComponentIndex(IReadOnlyList<byte> data, int index)
		{
			return (data[0] & (byte)(1 << (8 - MaxComponents + index))) > 0;
		}
	}
}