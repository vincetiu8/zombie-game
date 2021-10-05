using System;
using System.ComponentModel;
using Photon.Pun;
using UnityEngine;
using Utils;

namespace Networking
{
	/// <summary>
	///     An optimized script to sync the movement of 2D objects.
	///     Use this instead of PhotonTransformView.
	///		Note this doesn't support negative distances, so make sure the object can't go anywhere negative.
	/// </summary>
	[RequireComponent(typeof(PhotonView))]
	public class OptimizedTransformView : MonoBehaviourPun, INetworkSerializeView
	{
		private const int PositionLength = 12;
		private const int RotationLength = 8;

		[Header("Sync Settings")] [Description("Whether to sync the object's position or not")] [SerializeField]
		private bool syncPosition;

		[Description("The precision the position should be synced to, in decimal places")]
		[SerializeField]
		[Range(0, 10)]
		private int positionPrecision = 2;

		[Description("Whether to sync the object's rotation or not")] [SerializeField]
		private bool syncRotation;

		[Header("Smoothing Settings")] [Description("Whether to smooth the movement or not")] [SerializeField]
		private bool smooth;

		[Description("How fast the movement should be smoothed")] [SerializeField] [Range(1, 20)]
		private float smoothingSpeed = 10;

		private Vector2 _correctPos;
		private float   _precisionCorrection;

		private void Awake()
		{
			_precisionCorrection = (int)Math.Pow(10, positionPrecision);
		}

		private void Update()
		{
			if (photonView.IsMine) return;

			if (syncPosition) SyncPosition();
		}

		public bool Serialize(byte[] data, ref int offset)
		{
			if (syncPosition)
			{
				float xPos = transform.position.x;
				float yPos = transform.position.y;
				BitUtils.WriteFloat(data, xPos, _precisionCorrection, PositionLength, ref offset);
				BitUtils.WriteFloat(data, yPos, _precisionCorrection, PositionLength, ref offset);
			}

			if (!syncRotation) return true;

			float zRot = transform.rotation.eulerAngles.z;
			BitUtils.WriteFloat(data, zRot, BitUtils.Deg2Byte, RotationLength, ref offset);
			return true;
		}

		public void Deserialize(byte[] data, ref int offset)
		{
			if (syncPosition)
			{
				_correctPos.x = BitUtils.ReadFloat(data, _precisionCorrection, PositionLength, ref offset);
				_correctPos.y = BitUtils.ReadFloat(data, _precisionCorrection, PositionLength, ref offset);
			}

			if (!syncRotation) return;

			float zRot = BitUtils.ReadFloat(data, BitUtils.Deg2Byte, RotationLength, ref offset);
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