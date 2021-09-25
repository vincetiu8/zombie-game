using System.ComponentModel;
using UnityEngine;

namespace Objects
{
	// Window controller handles zombies breaking the window
	public class WindowController : Health
	{
		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

			zombieAtWindow = false;
		}


		private void OnTriggerStay2D(Collider2D collision)
		{
			if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

			ChangeHealth(-barricadeBreakRate * Time.deltaTime);
			zombieAtWindow = true;
		}

		public override void ChangeHealth(float change)
		{
			float previousHealth = health;
			health = Mathf.Clamp(health + change, 0, initialHealth);
			if (previousHealth == health)
				return; // If zombies hitting an already destroyed window or player fixing an already fixed window

			windowCollider.SetActive(health != 0);

			if (health - previousHealth < 0 && Mathf.CeilToInt(health) != 6)
				barricadesGraphics.transform.GetChild(Mathf.CeilToInt(health)).gameObject.SetActive(false);
			else if (health - previousHealth > 0)
				barricadesGraphics.transform.GetChild((int)health - 1).gameObject.SetActive(true);
		}

		#region Variables

		[HideInInspector] public bool zombieAtWindow;

		[Header("Window Settings")] [Description("The graphics for each barricade")] [SerializeField]
		private GameObject barricadesGraphics;

		[Description("The window's collider")] [SerializeField]
		private GameObject windowCollider;

		[Description("The rate at which health is decreased")] [SerializeField] [Range(0.25f, 10)]
		private float barricadeBreakRate;

		#endregion
	}
}