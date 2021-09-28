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
		private float   _correctRot;

		private void Update()
		{
			if (photonView.IsMine) return;

			if (syncPosition) SyncPosition();

			if (syncRotation) SyncRotation();
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.IsWriting)
			{
				if (syncPosition)
				{
					stream.SendNext(transform.position.x);
					stream.SendNext(transform.position.y);
				}

				if (syncRotation) stream.SendNext(transform.rotation.eulerAngles.z);

				return;
			}

			if (syncPosition)
			{
				_correctPos.x = (float)stream.ReceiveNext();
				_correctPos.y = (float)stream.ReceiveNext();
			}


			if (syncRotation) _correctRot = (float)stream.ReceiveNext();
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

		private void SyncRotation()
		{
			if (!smooth || transform.rotation.z == 0)
			{
				transform.rotation = Quaternion.Euler(0, 0, _correctRot);
				return;
			}

			transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(
			                                                       transform.rotation.eulerAngles.z,
			                                                       _correctRot,
			                                                       Time.deltaTime * smoothingSpeed
			                                                      ));
		}
	}
}