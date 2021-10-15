using Enemy;
using Interact;
using UnityEditor;
using Weapons;

namespace Editor
{
	[CustomPropertyDrawer(typeof(AmmoDict))]
	[CustomPropertyDrawer(typeof(InteractableSpritesDict))]
	[CustomPropertyDrawer(typeof(ItemDict))]
	public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer
	{
	}
}