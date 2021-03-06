using Networking;
using Photon.Pun;
using Shop;
using UnityEngine;

namespace Interact
{
	/// <summary>
	///     Used for things like Unlockable areas and the power switch
	/// </summary>
	public class Door : Unlockable
	{
		private static readonly int OpenDoor = Animator.StringToHash("OpenDoor");

		[SerializeField] private string   firstRoomToUnlock;
		[SerializeField] private string   secondRoomToUnlock;
		private                  Animator _animator;

		private AreaManager  _areaManager;
		private Collider2D[] _collider2Ds;


		protected override void Start()
		{
			base.Start();
			_areaManager = GameManager.Instance.GetComponent<AreaManager>();
			_animator = GetComponent<Animator>();
			_collider2Ds = GetComponentsInChildren<Collider2D>();
		}

		[PunRPC]
		protected override void ToggleUnlock(bool toggle)
		{
			foreach (Collider2D c in _collider2Ds) c.enabled = !toggle;

			if (!toggle) return; // Right now we don't trigger relocking doors

			// Unlock both rooms
			_areaManager.UnlockRoom(firstRoomToUnlock);
			_areaManager.UnlockRoom(secondRoomToUnlock);

			_animator.SetTrigger(OpenDoor);
		}

		/*#region Editor
	#if UNITY_EDITOR
		
		[CustomEditor(typeof(Door))]
		public class DoorEditor : Editor
		{
		    
		    public override void OnInspectorGUI()
		    {
		        //base.OnInspectorGUI();
		        
		        EditorGUIUtility.labelWidth = 220;

		        // Add reference to script
		        Door door = (Door)target;
		        
		        EditorGUILayout.LabelField("Info to display", EditorStyles.boldLabel);
		        
		        EditorGUILayout.BeginHorizontal();
		        
		            EditorGUILayout.LabelField("Name of room", GUILayout.MaxWidth(120));
		            door.itemName = EditorGUILayout.TextField(door.itemName, GUILayout.MaxWidth(100));
		        
		            EditorGUILayout.LabelField("Cost to open door", GUILayout.MaxWidth(120));
		            door.purchasePrice = EditorGUILayout.IntField(door.purchasePrice, GUILayout.MaxWidth(100));
		        
		        EditorGUILayout.EndHorizontal();
		        // Add separator
		        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

		        // Begin sprite area
		        door._hasSprite = EditorGUILayout.Toggle("Has Sprite?", door._hasSprite, GUILayout.MaxWidth(150));
		        if (door._hasSprite)
		        {
		            EditorGUI.indentLevel++;
		            
		            door.spriteRenderer = (SpriteRenderer) EditorGUILayout.ObjectField("Sprite Renderer", door
		            .spriteRenderer, typeof(SpriteRenderer));
		            
		            door.beforeUnlock = (Sprite) EditorGUILayout.ObjectField("Sprite before unlock", door
		                .beforeUnlock, typeof(Sprite), GUILayout.Height(200));
		            
		            door.afterUnlock = (Sprite) EditorGUILayout.ObjectField("Sprite before unlock", door
		                .afterUnlock, typeof(Sprite), GUILayout.Height(200));
		            
		            EditorGUI.indentLevel--;

		            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
		        }

		        // Begin Canvas area
		        door._hasCanvas = EditorGUILayout.Toggle("Has Canvas?", door._hasCanvas, GUILayout.MaxWidth(150));
		        if (door._hasCanvas)
		        {
		            EditorGUI.indentLevel++;

		            door.myCanvas = (Canvas) EditorGUILayout.ObjectField("This side of the door's Canvas", door
		                .myCanvas, typeof(Canvas));
		            
		            door.itemNameUI = (Text) EditorGUILayout.ObjectField("Text object to display area name", door
		                .itemNameUI, typeof(Text));
		            
		            door.itemPriceUI = (Text) EditorGUILayout.ObjectField("Text object to display area price", door
		                .itemPriceUI, typeof(Text));

		            EditorGUI.indentLevel--;
		            
		            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
		        }
		        
		        // Begin enemy spawnpoint area
		        door._enemySpawnpoint = EditorGUILayout.Toggle("Enemy Spawnpoint in new area?", door._enemySpawnpoint, GUILayout.MaxWidth(150));
		        if (door._enemySpawnpoint)
		        {
		            EditorGUI.indentLevel++;

		            door.roomToUnlock =
		                (Room) EditorGUILayout.ObjectField("Room to unlock", door.roomToUnlock, typeof(Room));

		            //door.enemySpawnpoint = (Transform) EditorGUILayout.ObjectField("Enemy Spawn Points Gameobject", door.enemySpawnpoint, typeof
		            //(Transform));
		            
		            EditorGUI.indentLevel--;
		            
		        }
		        // Saves all settings to script (VERY IMPORTANT)
		        EditorUtility.SetDirty(door);
		    }//
		}
		
	#endif
		#endregion*/
	}
}