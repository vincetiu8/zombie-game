using Interact;
using Networking;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Utils;

namespace Objects
{
	/// <summary>
	///     Movable object represents an object the player can pickup and move.
	/// </summary>
	public class MovableObject : TimedInteractable
	{
		[Header("Movable object Settings")] [SerializeField]
		private LayerMask preventPlace;

		private readonly string[] _ignoredActions = { "Interact", "Movement", "Mouse" };

		private Collider2D[] _colList;

		private int             _contacts;
		private bool            _isHolding;
		private PlayerInput     _localPlayerInput;
		private NavMeshObstacle _navMeshObstacle;
		private SpriteRenderer  _spriteRenderer;

		protected override void Awake()
		{
			base.Awake();
			_navMeshObstacle = GetComponent<NavMeshObstacle>();
			_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			_colList = GetComponentsInChildren<Collider2D>();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!_isHolding || !MiscUtils.IsInLayerMask(preventPlace, other.gameObject.layer)) return;

			_contacts++;
			_spriteRenderer.color = Color.red;
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!_isHolding || !MiscUtils.IsInLayerMask(preventPlace, other.gameObject.layer)) return;

			_contacts--;

			if (_contacts == 0) _spriteRenderer.color = Color.white;
		}

		public override void StartInteraction()
		{
			if (_isHolding && _contacts > 0) return;

			base.StartInteraction();
		}

		protected override void OnSuccessfulInteraction()
		{
			photonView.RPC("RPCInteract", RpcTarget.All, !_isHolding);

			FinishInteraction();

			if (!_isHolding) return;

			DisableRotation();

			if (photonView.IsMine) return;

			photonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
		}

		private void DisableRotation()
		{
			if (_localPlayerInput == null)
				_localPlayerInput = GameManager.Instance.localPlayerInstance.GetComponent<PlayerInput>();
			MiscUtils.ToggleActions(_localPlayerInput, _ignoredActions, false);
		}

		[PunRPC]
		private void RPCInteract(bool isHolding, PhotonMessageInfo info)
		{
			SetAllCollidersStatus(_isHolding);
			_isHolding = isHolding;
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
			gameObject.tag = active ? "Interactable" : "Untagged";
			_navMeshObstacle.enabled = active;
			_spriteRenderer.sortingLayerID = SortingLayer.NameToID(active ? "Objects" : "Enemies");
			_spriteRenderer.sortingOrder = active ? 2 : 5;
			foreach (Collider2D collider in _colList)
			{
				if (active)
				{
					if (!collider.enabled)
					{
						collider.enabled = true;
						continue;
					}

					collider.isTrigger = false;
					continue;
				}

				if (collider.isTrigger)
				{
					collider.enabled = false;
					continue;
				}

				collider.isTrigger = true;
			}
		}
	}
}