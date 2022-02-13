#define NAVMESHCOMPONENTS_SHOW_NAVMESHDATA_REF

using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

namespace UnityEditor.AI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(NavMeshSurface2d))]
    internal class NavMeshSurfaceEditor2d : Editor
    {
        private static Styles s_Styles;

        private static bool s_ShowDebugOptions;

        private static readonly Color s_HandleColor = new Color(127f, 214f, 244f, 100f) / 255;
        private static readonly Color s_HandleColorSelected = new Color(127f, 214f, 244f, 210f) / 255;

        private static readonly Color s_HandleColorDisabled =
            new Color(127f * 0.75f, 214f * 0.75f, 244f * 0.75f, 100f) / 255;

        private readonly BoxBoundsHandle m_BoundsHandle = new BoxBoundsHandle();
        private SerializedProperty m_AgentTypeID;
        private SerializedProperty m_BuildHeightMesh;
        private SerializedProperty m_Center;
        private SerializedProperty m_CollectObjects;
        private SerializedProperty m_CompressBounds;
        private SerializedProperty m_DefaultArea;

        private SerializedProperty m_HideEditorLogs;
        private SerializedProperty m_LayerMask;

#if NAVMESHCOMPONENTS_SHOW_NAVMESHDATA_REF
        private SerializedProperty m_NavMeshData;
#endif

        private SerializedProperty m_OverrideByGrid;

        private SerializedProperty m_OverrideTileSize;
        private SerializedProperty m_OverrideVector;
        private SerializedProperty m_OverrideVoxelSize;
        private SerializedProperty m_Size;
        private SerializedProperty m_TileSize;
        private SerializedProperty m_UseGeometry;
        private SerializedProperty m_UseMeshPrefab;
        private SerializedProperty m_VoxelSize;

        private bool editingCollider =>
            EditMode.editMode == EditMode.SceneViewEditMode.Collider && EditMode.IsOwner(this);

        private void OnEnable()
        {
            m_AgentTypeID = serializedObject.FindProperty("m_AgentTypeID");
            m_BuildHeightMesh = serializedObject.FindProperty("m_BuildHeightMesh");
            m_Center = serializedObject.FindProperty("m_Center");
            m_CollectObjects = serializedObject.FindProperty("m_CollectObjects");
            m_DefaultArea = serializedObject.FindProperty("m_DefaultArea");
            m_LayerMask = serializedObject.FindProperty("m_LayerMask");

            m_OverrideByGrid = serializedObject.FindProperty("m_OverrideByGrid");
            m_UseMeshPrefab = serializedObject.FindProperty("m_UseMeshPrefab");
            m_CompressBounds = serializedObject.FindProperty("m_CompressBounds");
            m_OverrideVector = serializedObject.FindProperty("m_OverrideVector");

            m_OverrideTileSize = serializedObject.FindProperty("m_OverrideTileSize");
            m_OverrideVoxelSize = serializedObject.FindProperty("m_OverrideVoxelSize");
            m_Size = serializedObject.FindProperty("m_Size");
            m_TileSize = serializedObject.FindProperty("m_TileSize");
            m_UseGeometry = serializedObject.FindProperty("m_UseGeometry");
            m_VoxelSize = serializedObject.FindProperty("m_VoxelSize");

            m_HideEditorLogs = serializedObject.FindProperty("m_HideEditorLogs");

#if NAVMESHCOMPONENTS_SHOW_NAVMESHDATA_REF
            m_NavMeshData = serializedObject.FindProperty("m_NavMeshData");
#endif
            NavMeshVisualizationSettings.showNavigation++;
        }

        private void OnDisable()
        {
            NavMeshVisualizationSettings.showNavigation--;
        }

        private void OnSceneGUI()
        {
            if (!editingCollider)
                return;

            NavMeshSurface2d navSurface = (NavMeshSurface2d) target;
            Color color = navSurface.enabled ? s_HandleColor : s_HandleColorDisabled;
            Matrix4x4 localToWorld =
                Matrix4x4.TRS(navSurface.transform.position, navSurface.transform.rotation, Vector3.one);
            using (new Handles.DrawingScope(color, localToWorld))
            {
                m_BoundsHandle.center = navSurface.center;
                m_BoundsHandle.size = navSurface.size;

                EditorGUI.BeginChangeCheck();
                m_BoundsHandle.DrawHandle();
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(navSurface, "Modified NavMesh Surface");
                    Vector3 center = m_BoundsHandle.center;
                    Vector3 size = m_BoundsHandle.size;
                    navSurface.center = center;
                    navSurface.size = size;
                    EditorUtility.SetDirty(target);
                }
            }
        }

        private Bounds GetBounds()
        {
            NavMeshSurface2d navSurface = (NavMeshSurface2d) target;
            return new Bounds(navSurface.transform.position, navSurface.size);
        }

        public override void OnInspectorGUI()
        {
            if (s_Styles == null)
                s_Styles = new Styles();

            serializedObject.Update();

            NavMeshBuildSettings bs = NavMesh.GetSettingsByID(m_AgentTypeID.intValue);

            if (bs.agentTypeID != -1)
            {
                // Draw image
                const float diagramHeight = 80.0f;
                Rect agentDiagramRect = EditorGUILayout.GetControlRect(false, diagramHeight);
                NavMeshEditorHelpers.DrawAgentDiagram(agentDiagramRect, bs.agentRadius, bs.agentHeight, bs.agentClimb,
                    bs.agentSlope);
            }

            NavMeshComponentsGUIUtility.AgentTypePopup("Agent Type", m_AgentTypeID);

            EditorGUILayout.Space();

            if ((CollectObjects) m_CollectObjects.enumValueIndex != CollectObjects.Children
                && FindObjectOfType<Grid>() == null)
                EditorGUILayout.HelpBox(
                    $"{CollectObjects.All} or {CollectObjects.Volume} is not intended to be used without root Grid object in scene. Use {CollectObjects.Children} instead.",
                    MessageType.Warning);

            EditorGUILayout.PropertyField(m_CollectObjects);
            if ((CollectObjects) m_CollectObjects.enumValueIndex == CollectObjects.Volume)
            {
                EditorGUI.indentLevel++;

                EditMode.DoEditModeInspectorModeButton(EditMode.SceneViewEditMode.Collider, "Edit Volume",
                    EditorGUIUtility.IconContent("EditCollider"), GetBounds, this);
                EditorGUILayout.PropertyField(m_Size);
                EditorGUILayout.PropertyField(m_Center);

                EditorGUI.indentLevel--;
            }
            else
            {
                if (editingCollider)
                    EditMode.QuitEditMode();
            }

            EditorGUILayout.PropertyField(m_LayerMask, s_Styles.m_LayerMask);
            EditorGUILayout.PropertyField(m_UseGeometry);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_OverrideByGrid);
            EditorGUILayout.PropertyField(m_UseMeshPrefab);
            EditorGUILayout.PropertyField(m_CompressBounds);
            EditorGUILayout.PropertyField(m_OverrideVector);

            EditorGUILayout.Space();

            m_OverrideVoxelSize.isExpanded = EditorGUILayout.Foldout(m_OverrideVoxelSize.isExpanded, "Advanced");
            if (m_OverrideVoxelSize.isExpanded)
            {
                EditorGUI.indentLevel++;

                NavMeshComponentsGUIUtility.AreaPopup("Default Area", m_DefaultArea);

                // Override voxel size.
                EditorGUILayout.PropertyField(m_OverrideVoxelSize);

                using (new EditorGUI.DisabledScope(!m_OverrideVoxelSize.boolValue ||
                                                   m_OverrideVoxelSize.hasMultipleDifferentValues))
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.PropertyField(m_VoxelSize);

                    if (!m_OverrideVoxelSize.hasMultipleDifferentValues)
                    {
                        if (!m_AgentTypeID.hasMultipleDifferentValues)
                        {
                            float voxelsPerRadius = m_VoxelSize.floatValue > 0.0f
                                ? bs.agentRadius / m_VoxelSize.floatValue
                                : 0.0f;
                            EditorGUILayout.LabelField(" ",
                                voxelsPerRadius.ToString("0.00") + " voxels per agent radius", EditorStyles.miniLabel);
                        }

                        if (m_OverrideVoxelSize.boolValue)
                            EditorGUILayout.HelpBox(
                                "Voxel size controls how accurately the navigation mesh is generated from the level geometry. A good voxel size is 2-4 voxels per agent radius. Making voxel size smaller will increase build time.",
                                MessageType.None);
                    }

                    EditorGUI.indentLevel--;
                }

                // Override tile size
                EditorGUILayout.PropertyField(m_OverrideTileSize);

                using (new EditorGUI.DisabledScope(!m_OverrideTileSize.boolValue ||
                                                   m_OverrideTileSize.hasMultipleDifferentValues))
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.PropertyField(m_TileSize);

                    if (!m_TileSize.hasMultipleDifferentValues && !m_VoxelSize.hasMultipleDifferentValues)
                    {
                        float tileWorldSize = m_TileSize.intValue * m_VoxelSize.floatValue;
                        EditorGUILayout.LabelField(" ", tileWorldSize.ToString("0.00") + " world units",
                            EditorStyles.miniLabel);
                    }

                    if (!m_OverrideTileSize.hasMultipleDifferentValues)
                        if (m_OverrideTileSize.boolValue)
                            EditorGUILayout.HelpBox(
                                "Tile size controls the how local the changes to the world are (rebuild or carve). Small tile size allows more local changes, while potentially generating more data in overal.",
                                MessageType.None);
                    EditorGUI.indentLevel--;
                }


                // Height mesh
                using (new EditorGUI.DisabledScope(true)) EditorGUILayout.PropertyField(m_BuildHeightMesh);

                EditorGUILayout.Space();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            // Hide editor logs
            EditorGUILayout.PropertyField(m_HideEditorLogs);

            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();

            bool hadError = false;
            bool multipleTargets = targets.Length > 1;
            foreach (NavMeshSurface2d navSurface in targets)
            {
                NavMeshBuildSettings settings = navSurface.GetBuildSettings();
                // Calculating bounds is potentially expensive when unbounded - so here we just use the center/size.
                // It means the validation is not checking vertical voxel limit correctly when the surface is set to something else than "in volume".
                Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
                if (navSurface.collectObjects == CollectObjects2d.Volume)
                    bounds = new Bounds(navSurface.center, navSurface.size);

                string[] errors = settings.ValidationReport(bounds);
                if (errors.Length > 0)
                {
                    if (multipleTargets)
                        EditorGUILayout.LabelField(navSurface.name);
                    foreach (string err in errors) EditorGUILayout.HelpBox(err, MessageType.Warning);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(EditorGUIUtility.labelWidth);
                    if (GUILayout.Button("Open Agent Settings...", EditorStyles.miniButton))
                        NavMeshEditorHelpers.OpenAgentSettings(navSurface.agentTypeID);
                    GUILayout.EndHorizontal();
                    hadError = true;
                }
            }

            if (hadError)
                EditorGUILayout.Space();

#if NAVMESHCOMPONENTS_SHOW_NAVMESHDATA_REF
            Rect nmdRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);

            EditorGUI.BeginProperty(nmdRect, GUIContent.none, m_NavMeshData);
            Rect rectLabel = EditorGUI.PrefixLabel(nmdRect, GUIUtility.GetControlID(FocusType.Passive),
                new GUIContent(m_NavMeshData.displayName));
            EditorGUI.EndProperty();

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.BeginProperty(nmdRect, GUIContent.none, m_NavMeshData);
                EditorGUI.ObjectField(rectLabel, m_NavMeshData, GUIContent.none);
                EditorGUI.EndProperty();
            }
#endif
            using (new EditorGUI.DisabledScope(Application.isPlaying || m_AgentTypeID.intValue == -1))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUIUtility.labelWidth);
                if (GUILayout.Button("Clear"))
                {
                    NavMeshAssetManager2d.instance.ClearSurfaces(targets);
                    SceneView.RepaintAll();
                }

                if (GUILayout.Button("Bake")) NavMeshAssetManager2d.instance.StartBakingSurfaces(targets);

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUIUtility.labelWidth);
                if (GUILayout.Button("Rotate Surface to XY"))
                    foreach (Object item in targets)
                    {
                        NavMeshSurface2d o = item as NavMeshSurface2d;
                        o.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                    }

                GUILayout.EndHorizontal();
                foreach (NavMeshSurface2d navSurface in targets)
                {
                    if (!Mathf.Approximately(navSurface.transform.eulerAngles.x, 270f))
                        EditorGUILayout.HelpBox(
                            "NavMeshSurface2d is not rotated respectively to (x-90;y0;z0). Apply rotation unless intended.",
                            MessageType.Warning);
                }
            }

            // Show progress for the selected targets
            var bakeOperations = NavMeshAssetManager2d.instance.GetBakeOperations();
            for (int i = bakeOperations.Count - 1; i >= 0; --i)
            {
                if (!targets.Contains(bakeOperations[i].surface))
                    continue;

                AsyncOperation oper = bakeOperations[i].bakeOperation;
                if (oper == null)
                    continue;

                float p = oper.progress;
                if (oper.isDone)
                {
                    SceneView.RepaintAll();
                    continue;
                }

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Cancel", EditorStyles.miniButton))
                {
                    NavMeshData bakeData = bakeOperations[i].bakeData;
                    UnityEngine.AI.NavMeshBuilder.Cancel(bakeData);
                    bakeOperations.RemoveAt(i);
                }

                EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), p, "Baking: " + (int) (100 * p) + "%");
                if (p <= 1)
                    Repaint();

                GUILayout.EndHorizontal();
            }
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.Active | GizmoType.Pickable)]
        private static void RenderBoxGizmoSelected(NavMeshSurface2d navSurface, GizmoType gizmoType)
        {
            RenderBoxGizmo(navSurface, gizmoType, true);
        }

        [DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
        private static void RenderBoxGizmoNotSelected(NavMeshSurface2d navSurface, GizmoType gizmoType)
        {
            if (NavMeshVisualizationSettings.showNavigation > 0)
                RenderBoxGizmo(navSurface, gizmoType, false);
            else
                Gizmos.DrawIcon(navSurface.transform.position, "NavMeshSurface Icon", true);
        }

        private static void RenderBoxGizmo(NavMeshSurface2d navSurface, GizmoType gizmoType, bool selected)
        {
            Color color = selected ? s_HandleColorSelected : s_HandleColor;
            if (!navSurface.enabled)
                color = s_HandleColorDisabled;

            Color oldColor = Gizmos.color;
            Matrix4x4 oldMatrix = Gizmos.matrix;

            // Use the unscaled matrix for the NavMeshSurface
            Matrix4x4 localToWorld =
                Matrix4x4.TRS(navSurface.transform.position, navSurface.transform.rotation, Vector3.one);
            Gizmos.matrix = localToWorld;

            if (navSurface.collectObjects == CollectObjects2d.Volume)
            {
                Gizmos.color = color;
                Gizmos.DrawWireCube(navSurface.center, navSurface.size);

                if (selected && navSurface.enabled)
                {
                    Color colorTrans = new Color(color.r * 0.75f, color.g * 0.75f, color.b * 0.75f, color.a * 0.15f);
                    Gizmos.color = colorTrans;
                    Gizmos.DrawCube(navSurface.center, navSurface.size);
                }
            }
            else
            {
                if (navSurface.navMeshData != null)
                {
                    Bounds bounds = navSurface.navMeshData.sourceBounds;
                    Gizmos.color = Color.grey;
                    Gizmos.DrawWireCube(bounds.center, bounds.size);
                }
            }

            Gizmos.matrix = oldMatrix;
            Gizmos.color = oldColor;

            Gizmos.DrawIcon(navSurface.transform.position, "NavMeshSurface Icon", true);
        }

        [MenuItem("GameObject/AI/NavMesh Surface 2D", false, 2000)]
        public static void CreateNavMeshSurface(MenuCommand menuCommand)
        {
            GameObject parent = menuCommand.context as GameObject;
            GameObject go = NavMeshComponentsGUIUtility.CreateAndSelectGameObject("NavMesh Surface", parent);
            go.AddComponent<NavMeshSurface2d>();
            SceneView view = SceneView.lastActiveSceneView;
            if (view != null)
                view.MoveToView(go.transform);
        }

        private class Styles
        {
            public readonly GUIContent m_LayerMask = new GUIContent("Include Layers");
            public readonly GUIContent m_ShowContours = new GUIContent("Show Contours");

            public readonly GUIContent m_ShowInputGeom = new GUIContent("Show Input Geom");
            public readonly GUIContent m_ShowPolyMesh = new GUIContent("Show Poly Mesh");
            public readonly GUIContent m_ShowPolyMeshDetail = new GUIContent("Show Poly Mesh Detail");
            public readonly GUIContent m_ShowRawContours = new GUIContent("Show Raw Contours");
            public readonly GUIContent m_ShowRegions = new GUIContent("Show Regions");
            public readonly GUIContent m_ShowVoxels = new GUIContent("Show Voxels");
        }
    }
}