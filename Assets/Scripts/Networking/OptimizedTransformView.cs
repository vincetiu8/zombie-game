using Photon.Pun;
using UnityEngine;

namespace Networking
{
	[RequireComponent(typeof(PhotonView))]
	public class OptimizedTransformView : MonoBehaviourPun, IPunObservable
	{
		[SerializeField] private float smoothingDelay = 5;
		[SerializeField] private bool  syncPosition;
		[SerializeField] private bool  syncRotation;

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
			if (transform.position == Vector3.zero)
			{
				transform.position = _correctPos;
				return;
			}

			transform.position = Vector2.Lerp(transform.position,
			                                  _correctPos,
			                                  Time.deltaTime * smoothingDelay
			                                 );
		}

		private void SyncRotation()
		{
			if (transform.rotation.z == 0)
			{
				transform.rotation = Quaternion.Euler(0, 0, _correctRot);
				return;
			}

			transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(
			                                                       transform.rotation.eulerAngles.z,
			                                                       _correctRot,
			                                                       Time.deltaTime * smoothingDelay
			                                                      ));
		}
	}
}