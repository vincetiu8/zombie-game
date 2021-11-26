using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects {
	public class DamagePopup : MonoBehaviour {
		[SerializeField] private float moveYSpeed;
		[SerializeField] private float popupLifeSpan;
		[SerializeField] private float disappearSpeed;

		private TextMeshPro _textMeshPro;
		private Color       _color;

		public static DamagePopup Create(Transform pfDamagePopup, Vector3 position, int damageAmount) {
			Transform damagePopupTransform = Instantiate(pfDamagePopup, position, Quaternion.identity);

			DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
			damagePopup.Setup(damageAmount);

			return damagePopup;
		}

		private void Awake() {
			_textMeshPro = transform.GetComponent<TextMeshPro>();
			_color = _textMeshPro.color;
		}

		public void Setup(int damageAmount) {
			
			if (damageAmount > 0) {
				_color=Color.green;
				_textMeshPro.SetText("+"+damageAmount.ToString());
			}
			else {
				_color=Color.red;
				_textMeshPro.SetText(damageAmount.ToString());
			}



			_textMeshPro.color = _color;
		}

		private void Update() {
			transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

			popupLifeSpan -= Time.deltaTime;

			if (popupLifeSpan < 0) {
				_color.a -= disappearSpeed * Time.deltaTime;
				_textMeshPro.color = _color;
			}

			if (_color.a < 0) {
				Destroy(gameObject);
			}
		}
	}
}