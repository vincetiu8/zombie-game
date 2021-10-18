using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Collider2D))]
public class ShadowShapeGenerator : MonoBehaviour
{
	private static readonly FieldInfo           MeshField;
	private static readonly FieldInfo           ShapePathField;
	private static readonly MethodInfo          GenerateShadowMeshMethod;
	private                 CompositeCollider2D _compositeCollider2D;

	static ShadowShapeGenerator()
	{
		MeshField = typeof(ShadowCaster2D).GetField("m_Mesh", BindingFlags.NonPublic | BindingFlags.Instance);
		ShapePathField =
			typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);

		GenerateShadowMeshMethod = typeof(ShadowCaster2D)
		                           .Assembly
		                           .GetType("UnityEngine.Experimental.Rendering.Universal.ShadowUtility")
		                           .GetMethod("GenerateShadowMesh", BindingFlags.Public | BindingFlags.Static);
	}

	private void Start()
	{
		_compositeCollider2D = GetComponent<CompositeCollider2D>();

		for (int i = 0; i < _compositeCollider2D.pathCount; i++)
		{
			int pathPoints = _compositeCollider2D.GetPathPointCount(i);
			Vector2[] pathVerts = new Vector2[pathPoints];
			_compositeCollider2D.GetPath(i, pathVerts);
			Vector3[] verts = new Vector3[pathPoints];
			for (int j = 0; j < pathPoints; j++) verts[j] = pathVerts[j];

			GameObject child = new GameObject
			                   {
				                   transform =
				                   {
					                   parent = transform,
					                   localPosition = Vector3.zero
				                   }
			                   };
			ShadowCaster2D shadowCaster2D = child.AddComponent<ShadowCaster2D>();
			ShapePathField.SetValue(shadowCaster2D, verts.ToArray());
			MeshField.SetValue(shadowCaster2D, new Mesh());
			GenerateShadowMeshMethod.Invoke(shadowCaster2D,
			                                new[]
			                                {
				                                MeshField.GetValue(shadowCaster2D),
				                                ShapePathField.GetValue(shadowCaster2D)
			                                });
		}
	}
}