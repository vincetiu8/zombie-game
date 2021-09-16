using UnityEditor;
using Weapons;

namespace Editor
{
    [CustomPropertyDrawer(typeof(AmmoDict))]
    [CustomPropertyDrawer(typeof(ItemDict))]
    public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
}