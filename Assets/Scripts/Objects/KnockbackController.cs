using System.ComponentModel;
using Enemy;
using Networking;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class KnockbackController : MonoBehaviour
{
	[Description("Is multiplied with the amount of knockback target takes")] [SerializeField] [Range(0, 200)]
	protected float knockBackMultiplier;

	private float _cooldown;

	private bool _isStunned;

	private PlayerInput _playerInput;

	private Rigidbody2D _rigidbody2D;


	private void Start()
	{
		_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
		_playerInput = GameManager.Instance.localPlayerInstance.GetComponent<PlayerInput>();
	}

	private void Update()
	{
		if (_cooldown > 0)
		{
			_cooldown -= Time.deltaTime;
			return;
		}

		BeStunned(false);
	}

	public void TakeKnockBack(float angle, float amount)
	{
		float finalMultiplier = amount * knockBackMultiplier;
		_rigidbody2D.AddForce(TransformUtils.DegToVector2(angle) * finalMultiplier, (ForceMode2D)ForceMode.Impulse);
	}

	public void TakeStun(int duration)
	{
		// Stuns overwrites previous stun if it's duration is higher, otherwise, it is ignored (Prevents stuns from stacking)
		if (_cooldown > duration) return;
		_cooldown = duration;
		BeStunned(true);
	}

	// make method, in stun, out stun controleld by bool, same bool return

	private void BeStunned(bool stunned)
	{
		if (stunned == _isStunned) return;

		_isStunned = stunned;

		switch (transform.tag)
		{
			case "Enemy":
				transform.GetComponent<ChaserAI>().DisableMovement(stunned);
				break;
			case "Player":
				if (_isStunned)
				{
					_playerInput.currentActionMap.Disable();
					break;
				}

				_playerInput.currentActionMap.Enable();
				break;
		}
	}
    
    public void ScaleKnockback(float scale)
    {
        knockBackMultiplier *= scale;
    }}