using System.ComponentModel;
using Photon.Pun;
using UnityEngine;

namespace Networking
{
	/// <summary>
	///     An optimized script to sync the movement of 2D objects.
	///     Use this instead of PhotonTransformView
	/// </summary>
	[RequireComponent(typeof(PhotonView))]
	public class OptimizedTransformView : MonoBehaviourPun, IPunObservable
	{
		[Header("Sync Settings")] [Description("Whether to sync the object's position or not")] [SerializeField]
		private bool syncPosition;

		[Description("Whether to sync the object's rotation or not")] [SerializeField]
		private bool syncRotation;

		[Header("Smoothing Settings")] [Description("Whether to smooth the movement or not")] [SerializeField]
		private bool smooth;

		[Description("How fast the movement should be smoothed")] [SerializeField] [Range(1, 20)]
		private float smoothingSpeed = 10;


		private Vector2 _correctPos;

		private void Update()
		{
			if (photonView.IsMine) return;

			if (syncPosition) SyncPosition();
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			float zRot;
			byte byteZRot;

			if (stream.IsWriting)
			{
				if (syncPosition)
				{
					stream.SendNext(transform.position.x);
					stream.SendNext(transform.position.y);
				}

				if (!syncRotation) return;

				zRot = transform.rotation.eulerAngles.z;
				byteZRot = (byte)(zRot / 360 * 255);
				stream.SendNext(byteZRot);

				return;
			}

			if (syncPosition)
			{
				_correctPos.x = (float)stream.ReceiveNext();
				_correctPos.y = (float)stream.ReceiveNext();
			}


			if (!syncRotation) return;

			byteZRot = (byte)stream.ReceiveNext();
			zRot = byteZRot / 255f * 360;
			transform.rotation = Quaternion.AngleAxis(zRot, Vector3.forward);
		}

		private void SyncPosition()
		{
			if (!smooth || transform.position == Vector3.zero)
			{
				transform.position = _correctPos;
				return;
			}

			transform.position = Vector2.Lerp(transform.position,
			                                  _correctPos,
			                                  Time.deltaTime * smoothingSpeed
			                                 );
		}
	}
}