using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Utils provides utility functions that other classes can use.
/// </summary>
public static class Utils
{
	public static bool IsInLayerMask(LayerMask layerMask, int layer)
	{
		return (layerMask.value & (1 << layer)) > 0;
	}

	public static float Vector2ToDeg(Vector2 vector2)
	{
		return Mathf.Rad2Deg * Mathf.Atan2(vector2.y, vector2.x);
	}
	
	public static bool ToggleInput(bool isPaused, PlayerInput input)
	{
		string actionMap = isPaused ? "UI" : "Game";
		input.SwitchCurrentActionMap(actionMap);
		return isPaused;
	}
    
    public static Vector2 RotateVector2(Vector2 originalVector2, float degreesInClockwise)
    {
        return new Vector2(
            originalVector2.x * Mathf.Cos(degreesInClockwise * Mathf.Deg2Rad) - originalVector2.y * Mathf.Sin(degreesInClockwise * Mathf.Deg2Rad),
            originalVector2.x * Mathf.Sin(degreesInClockwise * Mathf.Deg2Rad) + originalVector2.y * Mathf.Cos(degreesInClockwise * Mathf.Deg2Rad));
        // Explination: https://en.wikipedia.org/wiki/Rotation_matrix

    }
}