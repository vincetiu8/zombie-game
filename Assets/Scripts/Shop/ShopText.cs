using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
	public class ShopText : MonoBehaviour
	{
		[HideInInspector] public static ShopText Instance;

		[SerializeField] [Range(0.5f, 5f)] private float       fadeSpeed;
		private                                    CanvasGroup _canvasGroup;
		private                                    Coroutine   _fadeCoroutine;
		private                                    Text        _shopText;

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(this);
				return;
			}

			Instance = this;
		}

		private void Start()
		{
			_shopText = GetComponentInChildren<Text>();
			_canvasGroup = GetComponent<CanvasGroup>();
			_canvasGroup.alpha = 0;
		}

		public void SetText(string text, Color color)
		{
			_shopText.text = text;
			_shopText.color = color;
		}

		public void ToggleVisibility(bool toggle)
		{
			if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

			_fadeCoroutine = StartCoroutine(ToggleVisibilityCoroutine(toggle));
		}

		private IEnumerator ToggleVisibilityCoroutine(bool toggle)
		{
			int target = toggle ? 1 : 0;
			float change = (toggle ? 1 : -1) * fadeSpeed;

			while (Mathf.Abs(_canvasGroup.alpha - target) > 0.01f)
			{
				_canvasGroup.alpha += change * Time.deltaTime;
				yield return null;
			}

			_canvasGroup.alpha = target;
			_fadeCoroutine = null;
		}
	}
}