using UnityEngine;

namespace Utils
{
	public class VectorUtils
	{
		public static float Vector2ToDeg(Vector2 vector2)
		{
			return Mathf.Rad2Deg * Mathf.Atan2(vector2.y, vector2.x);
		}

		public static Vector2 DegToVector2(float deg)
		{
			return new Vector2(Mathf.Cos(deg * Mathf.Deg2Rad), Mathf.Sin(deg * Mathf.Deg2Rad));
		}
	}
}