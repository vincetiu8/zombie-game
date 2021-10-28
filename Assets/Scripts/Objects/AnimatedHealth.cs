using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Objects
{
	/// <summary>
	///     A wrapper around the HealthController that updates the sprite based on the amount of health left.
	///     Use this to display damage on sprites.
	/// </summary>
	public class AnimatedHealth : HealthController
	{
		[Header("Sprite Settings")] [SerializeField]
		private Sprite[] sprites;

		[SerializeField] private SpriteRenderer spriteRenderer;

		[Header("Death Settings")] [SerializeField] [Range(0.5f, 5f)]
		private float deathTime;

		[SerializeField] private Light2D[] lights;

		[Tooltip("All colliders in children are disabled automatically. Add additional components here.")]
		[SerializeField]
		private Behaviour[] componentsToDisableOnDeath;

		protected virtual void Start()
		{
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		}

		[PunRPC]
		protected override void RPCChangeHealth(int newHealth, int change)
		{
			base.RPCChangeHealth(newHealth, change);
			SetSprite();
		}

		private void SetSprite()
		{
			int spriteIndex = (Health * sprites.Length - 1) / initialHealth;
//			spriteRenderer.sprite = sprites[spriteIndex];
		}

		protected override void OnDeath()
		{
			photonView.RPC("RPCInitialOnDeath", RpcTarget.All);
		}

		[PunRPC]
		protected virtual void RPCInitialOnDeath(PhotonMessageInfo info)
		{
			Health = 0;
			Collider2D[] collider2Ds = GetComponentsInChildren<Collider2D>();
			foreach (Collider2D col in collider2Ds) col.enabled = false;

			foreach (Behaviour behaviour in componentsToDisableOnDeath) behaviour.enabled = false;

			//StartCoroutine(DeathFade());
		}

		/*private IEnumerator DeathFade()
		{
			float timeRemaining = deathTime;
			Color color = spriteRenderer.color;
			spriteRenderer.sortingLayerID = SortingLayer.NameToID("Objects");
			spriteRenderer.sortingOrder = 3;
			while (timeRemaining > 0)
			{
				float intensity = timeRemaining / deathTime;
				timeRemaining -= Time.deltaTime;
				color.a = intensity;
				spriteRenderer.color = color;

				foreach (Light2D light in lights) light.intensity = intensity;
				yield return null;
			}

			RPCOnDeath();
		}*/
	}
}