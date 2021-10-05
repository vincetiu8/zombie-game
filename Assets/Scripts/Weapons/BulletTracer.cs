using System.ComponentModel;
using UnityEngine;

namespace Weapons
{
	public class BulletTracer : MonoBehaviour
	{
		[Header("Trail settings")] [SerializeField] [Range(10, 100)]
		private float trailSpeed = 25;

		[Description("Distance after which color no longer changes")] [SerializeField] [Range(10, 50)]
		private float distanceColorCap = 25;

		private float _currentDistance;
		private float _distance;
		private Color _endColor;

		private LineRenderer _lineRenderer;
		private Color        _startColor;

		private void Awake()
		{
			_lineRenderer = GetComponent<LineRenderer>();
			_currentDistance = 0;
		}

		private void Start()
		{
			_startColor = _lineRenderer.startColor;
			Vector2 displacement = _lineRenderer.GetPosition(1) - _lineRenderer.GetPosition(0);
			_distance = displacement.magnitude;

			_endColor = _lineRenderer.colorGradient.Evaluate(_distance / distanceColorCap);
		}

		private void Update()
		{
			_currentDistance += trailSpeed * Time.deltaTime;

			if (_currentDistance > _distance)
			{
				Destroy(gameObject);
				return;
			}

			float a = _currentDistance / _distance;
			_startColor.a = 1 - a;
			_lineRenderer.startColor = _startColor;

			_endColor.a = a;
			_lineRenderer.endColor = _endColor;
		}
	}
}