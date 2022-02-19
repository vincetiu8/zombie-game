using TMPro;
using UnityEngine;

namespace Objects
{
	public class Popup : MonoBehaviour
	{
		[SerializeField] private float moveYSpeed;
		[SerializeField] private float popupLifeSpan;
		[SerializeField] private float disappearSpeed;

		protected Color       TextColor;
		protected TextMeshPro TextMeshPro;

		private void Awake()
		{
			TextMeshPro = transform.GetComponent<TextMeshPro>();
			TextColor = TextMeshPro.color;
		}

		private void Update()
		{
			transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

			popupLifeSpan -= Time.deltaTime;

			if (popupLifeSpan < 0)
			{
				TextColor.a -= disappearSpeed * Time.deltaTime;
				TextMeshPro.color = TextColor;
			}

			if (TextColor.a < 0) Destroy(gameObject);
		}

		protected void Setup()
		{
			TextMeshPro.color = TextColor;
		}
	}
}