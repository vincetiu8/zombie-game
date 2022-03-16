using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTracker : MonoBehaviour
{
	public void MouseMovementAction(InputAction.CallbackContext context)
	{
		if (Camera.main == null) return;
		;
		Vector2 position = context.ReadValue<Vector2>();
		Vector2 screenPosition = Camera.main.ScreenToWorldPoint(position);
		transform.position = screenPosition;
	}
}