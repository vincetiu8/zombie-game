using Enemy;
using PlayerScripts;
using UnityEditor;

namespace Editor
{
	[CustomPropertyDrawer(typeof(AmmoDict))]
	[CustomPropertyDrawer(typeof(InteractableSpritesDict))]
	[CustomPropertyDrawer(typeof(ItemDict))]
	[CustomPropertyDrawer(typeof(AmmoSpriteDict))]
	public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer
	{
	}
}