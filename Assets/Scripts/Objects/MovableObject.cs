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
	public class MovableObject : Interactable
	{
		[SerializeField] private LayerMask    preventPlace = LayerMask.NameToLayer("Obstacles");
		private                  Collider2D[] _colList;
		private                  int          _contacts;
		private                  bool         _isHolding;

		private NavMeshObstacle _navMeshObstacle;
		private SpriteRenderer  _spriteRenderer;

		private void Awake()
		{
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

			if (!_isHolding) startInteraction.Invoke();

			photonView.RPC("RPCInteract", RpcTarget.All, !_isHolding);

			GameObject player = GameManager.Instance.localPlayerInstance;

			// When the colliders are disabled, it removes this from the interactable list
			// We need to add it back so the local player can drop the item and vice versa
			// This also means closer objects will be interacted with instead of dropping this objects
			// Therefore, it is suggested to make the interactable trigger as small as possible
			if (!_isHolding)
			{
				finishInteraction.Invoke();
				return;
			}

			MiscUtils.ToggleAction(player.GetComponent<PlayerInput>(), "Movement", true);

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
			_navMeshObstacle.enabled = active;
			_spriteRenderer.sortingLayerID = SortingLayer.NameToID(active ? "Objects" : "Enemies");
			_spriteRenderer.sortingOrder = active ? 2 : 5;
			gameObject.layer = LayerMask.NameToLayer(active ? "Obstacles" : "MovingObstacles");
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