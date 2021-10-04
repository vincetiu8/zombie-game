using UnityEngine;

namespace Utils
{
	public class VectorUtils
	{
		public static float Vector2ToDeg(Vector2 vector2)
		{
			return Mathf.Rad2Deg * Mathf.Atan2(vector2.y, vector2.x);
		}
	}
}