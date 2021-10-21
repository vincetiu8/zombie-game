using Interact;
using Networking;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Objects
{
	/// <summary>
	///     Movable object represents an object the player can pickup and move.
	/// </summary>
	public class MovableObject : Interactable
	{
		private Collider2D[] _colList;
		private bool         _isHolding;

		private void Awake()
		{
			_colList = transform.GetComponentsInChildren<Collider2D>();
		}

		public override void StartInteraction()
		{
			_isHolding = !_isHolding;
			startInteraction.Invoke();
			photonView.RPC("RPCInteract", RpcTarget.All, _isHolding);

			GameObject player = GameManager.Instance.localPlayerInstance;

			// When the colliders are disabled, it removes this from the interactable list
			// We need to add it back so the local player can drop the item and vice versa
			// This also means closer objects will be interacted with instead of dropping this objects
			// Therefore, it is suggested to make the interactable trigger as small as possible
			if (!_isHolding)
			{
				finishInteraction.Invoke();
				player.GetComponent<PlayerInteract>().RemoveInteractable(gameObject);
				return;
			}

			MiscUtils.ToggleAction(player.GetComponent<PlayerInput>(), "Movement", true);
			player.GetComponent<PlayerInteract>().AddInteractable(gameObject);

			if (photonView.IsMine) return;

			photonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
		}

		public override float GetProgress()
		{
			return _isHolding ? 1 : 0;
		}

		[PunRPC]
		private void RPCInteract(bool isHolding, PhotonMessageInfo info)
		{
			_isHolding = isHolding;
			SetAllCollidersStatus(!_isHolding);
			if (_isHolding)
			{
				GameObject player = GameManager.Instance.PlayerInstances[info.Sender.GetPlayerNumber()];
				transform.SetParent(player.transform.Find("PlayerObject"));
				return;
			}

			transform.SetParent(null);
		}

		private void SetAllCollidersStatus(bool active)
		{
			gameObject.layer = LayerMask.NameToLayer(active ? "MovingObstacle" : "Obstacles");
			// foreach (Collider2D colliders in _colList) colliders.enabled = active;
		}
	}
}