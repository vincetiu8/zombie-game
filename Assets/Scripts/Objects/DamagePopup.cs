using TMPro;
using UnityEngine;

namespace Objects
{
	public class DamagePopup : MonoBehaviour
	{
		[SerializeField] private float moveYSpeed;
		[SerializeField] private float popupLifeSpan;
		[SerializeField] private float disappearSpeed;
		private                  Color _color;

		private TextMeshPro _textMeshPro;

		private void Awake()
		{
			_textMeshPro = transform.GetComponent<TextMeshPro>();
			_color = _textMeshPro.color;
		}

		private void Update()
		{
			transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

			popupLifeSpan -= Time.deltaTime;

			if (popupLifeSpan < 0)
			{
				_color.a -= disappearSpeed * Time.deltaTime;
				_textMeshPro.color = _color;
			}

			if (_color.a < 0) Destroy(gameObject);
		}

		public void Setup(int damageAmount)
		{
			if (damageAmount > 0)
			{
				_color = Color.green;
				_textMeshPro.SetText("+" + damageAmount);
			}
			else
			{
				_color = Color.red;
				_textMeshPro.SetText(damageAmount.ToString());
			}

			_textMeshPro.color = _color;
		}
	}
}