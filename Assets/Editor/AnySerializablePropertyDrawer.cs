using Enemy;
using PlayerScripts;
using UnityEditor;

namespace Editor
{
	[CustomPropertyDrawer(typeof(AmmoDict))]
	[CustomPropertyDrawer(typeof(InteractableSpritesDict))]
	[CustomPropertyDrawer(typeof(ItemDict))]
	public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer
	{
	}
}