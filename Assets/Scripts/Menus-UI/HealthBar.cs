using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        public Health healthController;

        private RectTransform _rectTransform;
        private float _initialWidth;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _initialWidth = _rectTransform.rect.width;
        }

        private void Update()
        {
            if (!healthController)
            {
                Destroy(gameObject);
                return;
            }
            
            _rectTransform.sizeDelta = new Vector2(_initialWidth * healthController.GetRoundedHealth() / healthController.initialHealth, _rectTransform.rect.height);
        }
    }
}