using UnityEditor;
using Weapons;

namespace Editor
{
    [CustomPropertyDrawer(typeof(AmmoDict))]
    public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
}