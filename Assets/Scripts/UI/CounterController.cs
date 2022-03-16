using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class CounterController : MonoBehaviour
	{
		private Image    _image;
		private TMP_Text _text;

		private void Awake()
		{
			_image = GetComponentInChildren<Image>();
			_text = GetComponentInChildren<TMP_Text>();
		}

		public void SetSprite(Sprite sprite)
		{
			_image.sprite = sprite;
		}

		public void SetText(string text)
		{
			_text.text = text;
		}
	}
}