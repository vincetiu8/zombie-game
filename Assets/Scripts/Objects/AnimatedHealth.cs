using Objects;
using Photon.Pun;
using UnityEngine;

/// <summary>
///     A wrapper around the HealthController that updates the sprite based on the amount of health left.
///     Use this to display damage on sprites.
/// </summary>
public class AnimatedHealth : HealthController
{
	[Header("Sprite Settings")] [SerializeField]
	private Sprite[] sprites;

	private SpriteRenderer _spriteRenderer;

	protected virtual void Start()
	{
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	[PunRPC]
	protected override void RPCChangeHealth(int newHealth)
	{
		base.RPCChangeHealth(newHealth);
		SetSprite();
	}

	private void SetSprite()
	{
		int spriteIndex = Health * sprites.Length / initialHealth;
		_spriteRenderer.sprite = sprites[spriteIndex];
	}
}