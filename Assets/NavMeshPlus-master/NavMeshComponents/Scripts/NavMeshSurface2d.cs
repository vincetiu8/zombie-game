using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace UnityEngine.AI
{
    public enum CollectObjects2d
    {
        All = 0,
        Volume = 1,
        Children = 2
    }

    [ExecuteAlways]
    [DefaultExecutionOrder(-102)]
    [AddComponentMenu("Navigation/NavMeshSurface2d", 30)]
    [HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
    public class NavMeshSurface2d : MonoBehaviour
    {
        [SerializeField] private int m_AgentTypeID;

        [SerializeField] private CollectObjects2d m_CollectObjects = CollectObjects2d.All;

        [SerializeField] private Vector3 m_Size = new Vector3(10.0f, 10.0f, 10.0f);

        [SerializeField] private Vector3 m_Center = new Vector3(0, 2.0f, 0);

        [SerializeField] private LayerMask m_LayerMask = ~0;

        [SerializeField] private NavMeshCollectGeometry m_UseGeometry = NavMeshCollectGeometry.RenderMeshes;

        [SerializeField] private bool m_OverrideByGrid;

        [SerializeField] private GameObject m_UseMeshPrefab;

        [SerializeField] private bool m_CompressBounds;

        [SerializeField] private Vector3 m_OverrideVector = Vector3.one;

        [SerializeField] private int m_DefaultArea;

        [SerializeField] private bool m_IgnoreNavMeshAgent = true;

        [SerializeField] private bool m_IgnoreNavMeshObstacle = true;

        [SerializeField] private bool m_OverrideTileSize;

        [SerializeField] private int m_TileSize = 256;

        [SerializeField] private bool m_OverrideVoxelSize;

        [SerializeField] private float m_VoxelSize;

        // Currently not supported advanced options
        [SerializeField] private bool m_BuildHeightMesh;

        [SerializeField] private bool m_HideEditorLogs;

        // Reference to whole scene navmesh data asset.
        [FormerlySerializedAs("m_BakedNavMeshData")] [SerializeField]
        private NavMeshData m_NavMeshData;

        private Vector3 m_LastPosition = Vector3.zero;
        private Quaternion m_LastRotation = Quaternion.identity;

        // Do not serialize - runtime only state.
        private NavMeshDataInstance m_NavMeshDataInstance;

        public int agentTypeID
        {
            get => m_AgentTypeID;
            set => m_AgentTypeID = value;
        }

        public CollectObjects2d collectObjects
        {
            get => m_CollectObjects;
            set => m_CollectObjects = value;
        }

        public Vector3 size
        {
            get => m_Size;
            set => m_Size = value;
        }

        public Vector3 center
        {
            get => m_Center;
            set => m_Center = value;
        }

        public LayerMask layerMask
        {
            get => m_LayerMask;
            set => m_LayerMask = value;
        }

        public NavMeshCollectGeometry useGeometry
        {
            get => m_UseGeometry;
            set => m_UseGeometry = value;
        }

        public bool overrideByGrid
        {
            get => m_OverrideByGrid;
            set => m_OverrideByGrid = value;
        }

        public GameObject useMeshPrefab
        {
            get => m_UseMeshPrefab;
            set => m_UseMeshPrefab = value;
        }

        public bool compressBounds
        {
            get => m_CompressBounds;
            set => m_CompressBounds = value;
        }

        public Vector3 overrideVector
        {
            get => m_OverrideVector;
            set => m_OverrideVector = value;
        }

        public int defaultArea
        {
            get => m_DefaultArea;
            set => m_DefaultArea = value;
        }

        public bool ignoreNavMeshAgent
        {
            get => m_IgnoreNavMeshAgent;
            set => m_IgnoreNavMeshAgent = value;
        }

        public bool ignoreNavMeshObstacle
        {
            get => m_IgnoreNavMeshObstacle;
            set => m_IgnoreNavMeshObstacle = value;
        }

        public bool overrideTileSize
        {
            get => m_OverrideTileSize;
            set => m_OverrideTileSize = value;
        }

        public int tileSize
        {
            get => m_TileSize;
            set => m_TileSize = value;
        }

        public bool overrideVoxelSize
        {
            get => m_OverrideVoxelSize;
            set => m_OverrideVoxelSize = value;
        }

        public float voxelSize
        {
            get => m_VoxelSize;
            set => m_VoxelSize = value;
        }

        public bool buildHeightMesh
        {
            get => m_BuildHeightMesh;
            set => m_BuildHeightMesh = value;
        }

        public bool hideEditorLogs
        {
            get => m_HideEditorLogs;
            set => m_HideEditorLogs = value;
        }

        public NavMeshData navMeshData
        {
            get => m_NavMeshData;
            set => m_NavMeshData = value;
        }

        public static List<NavMeshSurface2d> activeSurfaces { get; } = new List<NavMeshSurface2d>();

        private void OnEnable()
        {
            Register(this);
            AddData();
        }

        private void OnDisable()
        {
            RemoveData();
            Unregister(this);
        }

        public void AddData()
        {
#if UNITY_EDITOR
            bool isInPreviewScene = EditorSceneManager.IsPreviewSceneObject(this);
            bool isPrefab = isInPreviewScene || EditorUtility.IsPersistent(this);
            if (isPrefab)
                //Debug.LogFormat("NavMeshData from {0}.{1} will not be added to the NavMesh world because the gameObject is a prefab.",
                //    gameObject.name, name);
                return;
#endif
            if (m_NavMeshDataInstance.valid)
                return;

            if (m_NavMeshData != null)
            {
                m_NavMeshDataInstance = NavMesh.AddNavMeshData(m_NavMeshData, transform.position, transform.rotation);
                m_NavMeshDataInstance.owner = this;
            }

            m_LastPosition = transform.position;
            m_LastRotation = transform.rotation;
        }

        public void RemoveData()
        {
            m_NavMeshDataInstance.Remove();
            m_NavMeshDataInstance = new NavMeshDataInstance();
        }

        public NavMeshBuildSettings GetBuildSettings()
        {
            NavMeshBuildSettings buildSettings = NavMesh.GetSettingsByID(m_AgentTypeID);
            if (buildSettings.agentTypeID == -1)
            {
                if (!m_HideEditorLogs) Debug.LogWarning("No build settings for agent type ID " + agentTypeID, this);
                buildSettings.agentTypeID = m_AgentTypeID;
            }

            if (overrideTileSize)
            {
                buildSettings.overrideTileSize = true;
                buildSettings.tileSize = tileSize;
            }

            if (overrideVoxelSize)
            {
                buildSettings.overrideVoxelSize = true;
                buildSettings.voxelSize = voxelSize;
            }

            return buildSettings;
        }

        public void BuildNavMesh()
        {
            var sources = CollectSources();

            // Use unscaled bounds - this differs in behaviour from e.g. collider components.
            // But is similar to reflection probe - and since navmesh data has no scaling support - it is the right choice here.
            Bounds sourcesBounds = new Bounds(m_Center, Abs(m_Size));
            if (m_CollectObjects != CollectObjects2d.Volume) sourcesBounds = CalculateWorldBounds(sources);

            NavMeshData data = NavMeshBuilder.BuildNavMeshData(GetBuildSettings(),
                sources, sourcesBounds, transform.position, transform.rotation);

            if (data != null)
            {
                data.name = gameObject.name;
                RemoveData();
                m_NavMeshData = data;
                if (isActiveAndEnabled)
                    AddData();
            }
        }

        // Source: https://github.com/Unity-Technologies/NavMeshComponents/issues/97#issuecomment-528692289
        public AsyncOperation BuildNavMeshAsync()
        {
            RemoveData();
            m_NavMeshData = new NavMeshData(m_AgentTypeID)
            {
                name = gameObject.name,
                position = transform.position,
                rotation = transform.rotation
            };

            if (isActiveAndEnabled) AddData();

            return UpdateNavMesh(m_NavMeshData);
        }

        public AsyncOperation UpdateNavMesh(NavMeshData data)
        {
            var sources = CollectSources();

            // Use unscaled bounds - this differs in behaviour from e.g. collider components.
            // But is similar to reflection probe - and since navmesh data has no scaling support - it is the right choice here.
            Bounds sourcesBounds = new Bounds(m_Center, Abs(m_Size));
            if (m_CollectObjects != CollectObjects2d.Volume)
                sourcesBounds = CalculateWorldBounds(sources);

            return NavMeshBuilder.UpdateNavMeshDataAsync(data, GetBuildSettings(), sources, sourcesBounds);
        }

        private static void Register(NavMeshSurface2d surface)
        {
#if UNITY_EDITOR
            bool isInPreviewScene = EditorSceneManager.IsPreviewSceneObject(surface);
            bool isPrefab = isInPreviewScene || EditorUtility.IsPersistent(surface);
            if (isPrefab)
                //Debug.LogFormat("NavMeshData from {0}.{1} will not be added to the NavMesh world because the gameObject is a prefab.",
                //    surface.gameObject.name, surface.name);
                return;
#endif
            if (activeSurfaces.Count == 0)
                NavMesh.onPreUpdate += UpdateActive;

            if (!activeSurfaces.Contains(surface))
                activeSurfaces.Add(surface);
        }

        private static void Unregister(NavMeshSurface2d surface)
        {
            activeSurfaces.Remove(surface);

            if (activeSurfaces.Count == 0)
                NavMesh.onPreUpdate -= UpdateActive;
        }

        private static void UpdateActive()
        {
            for (int i = 0; i < activeSurfaces.Count; ++i)
                activeSurfaces[i].UpdateDataIfTransformChanged();
        }

        private void AppendModifierVolumes(ref List<NavMeshBuildSource> sources)
        {
#if UNITY_EDITOR
            StageHandle myStage = StageUtility.GetStageHandle(gameObject);
            if (!myStage.IsValid())
                return;
#endif
            // Modifiers
            List<NavMeshModifierVolume> modifiers;
            if (m_CollectObjects == CollectObjects2d.Children)
            {
                modifiers = new List<NavMeshModifierVolume>(GetComponentsInChildren<NavMeshModifierVolume>());
                modifiers.RemoveAll(x => !x.isActiveAndEnabled);
            }
            else
            {
                modifiers = NavMeshModifierVolume.activeModifiers;
            }

            foreach (NavMeshModifierVolume m in modifiers)
            {
                if ((m_LayerMask & (1 << m.gameObject.layer)) == 0)
                    continue;
                if (!m.AffectsAgentType(m_AgentTypeID))
                    continue;
#if UNITY_EDITOR
                if (!myStage.Contains(m.gameObject))
                    continue;
#endif
                Vector3 mcenter = m.transform.TransformPoint(m.center);
                Vector3 scale = m.transform.lossyScale;
                Vector3 msize = new Vector3(m.size.x * Mathf.Abs(scale.x), m.size.y * Mathf.Abs(scale.y),
                    m.size.z * Mathf.Abs(scale.z));

                NavMeshBuildSource src = new NavMeshBuildSource();
                src.shape = NavMeshBuildSourceShape.ModifierBox;
                src.transform = Matrix4x4.TRS(mcenter, m.transform.rotation, Vector3.one);
                src.size = msize;
                src.area = m.area;
                sources.Add(src);
            }
        }

        private List<NavMeshBuildSource> CollectSources()
        {
            var sources = new List<NavMeshBuildSource>();
            var markups = new List<NavMeshBuildMarkup>();

            List<NavMeshModifier> modifiers;
            if (m_CollectObjects == CollectObjects2d.Children)
            {
                modifiers = new List<NavMeshModifier>(GetComponentsInChildren<NavMeshModifier>());
                modifiers.RemoveAll(x => !x.isActiveAndEnabled);
            }
            else
            {
                modifiers = NavMeshModifier.activeModifiers;
            }

            foreach (NavMeshModifier m in modifiers)
            {
                if ((m_LayerMask & (1 << m.gameObject.layer)) == 0)
                    continue;
                if (!m.AffectsAgentType(m_AgentTypeID))
                    continue;
                NavMeshBuildMarkup markup = new NavMeshBuildMarkup();
                markup.root = m.transform;
                markup.overrideArea = m.overrideArea;
                markup.area = m.area;
                markup.ignoreFromBuild = m.ignoreFromBuild;
                markups.Add(markup);
            }

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                if (m_CollectObjects == CollectObjects2d.All)
                {
                    UnityEditor.AI.NavMeshBuilder.CollectSourcesInStage(
                        null, m_LayerMask, m_UseGeometry, m_DefaultArea, markups, gameObject.scene, sources);
                }
                else if (m_CollectObjects == CollectObjects2d.Children)
                {
                    UnityEditor.AI.NavMeshBuilder.CollectSourcesInStage(
                        transform, m_LayerMask, m_UseGeometry, m_DefaultArea, markups, gameObject.scene, sources);
                }
                else if (m_CollectObjects == CollectObjects2d.Volume)
                {
                    Matrix4x4 localToWorld = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
                    Bounds worldBounds = GetWorldBounds(localToWorld, new Bounds(m_Center, m_Size));

                    UnityEditor.AI.NavMeshBuilder.CollectSourcesInStage(
                        worldBounds, m_LayerMask, m_UseGeometry, m_DefaultArea, markups, gameObject.scene, sources);
                }

                if (!hideEditorLogs && !Mathf.Approximately(transform.eulerAngles.x, 270f))
                    Debug.LogWarning(
                        "NavMeshSurface2d is not rotated respectively to (x-90;y0;z0). Apply rotation unless intended.");
                NavMeshBuilder2dWrapper builder = new NavMeshBuilder2dWrapper();
                builder.defaultArea = defaultArea;
                builder.layerMask = layerMask;
                builder.agentID = agentTypeID;
                builder.useMeshPrefab = useMeshPrefab;
                builder.overrideByGrid = overrideByGrid;
                builder.compressBounds = compressBounds;
                builder.overrideVector = overrideVector;
                builder.CollectGeometry = useGeometry;
                builder.CollectObjects = collectObjects;
                builder.parent = gameObject;
                builder.hideEditorLogs = hideEditorLogs;
                NavMeshBuilder2d.CollectSources(sources, builder);
            }
            else
#endif
            {
                if (m_CollectObjects == CollectObjects2d.All)
                {
                    NavMeshBuilder.CollectSources(null, m_LayerMask, m_UseGeometry, m_DefaultArea, markups, sources);
                }
                else if (m_CollectObjects == CollectObjects2d.Children)
                {
                    NavMeshBuilder.CollectSources(transform, m_LayerMask, m_UseGeometry, m_DefaultArea, markups,
                        sources);
                }
                else if (m_CollectObjects == CollectObjects2d.Volume)
                {
                    Matrix4x4 localToWorld = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
                    Bounds worldBounds = GetWorldBounds(localToWorld, new Bounds(m_Center, m_Size));
                    NavMeshBuilder.CollectSources(worldBounds, m_LayerMask, m_UseGeometry, m_DefaultArea, markups,
                        sources);
                }

                if (!hideEditorLogs && !Mathf.Approximately(transform.eulerAngles.x, 270f))
                    Debug.LogWarning(
                        "NavMeshSurface2d is not rotated respectively to (x-90;y0;z0). Apply rotation unless intended.");
                NavMeshBuilder2dWrapper builder = new NavMeshBuilder2dWrapper();
                builder.defaultArea = defaultArea;
                builder.layerMask = layerMask;
                builder.agentID = agentTypeID;
                builder.useMeshPrefab = useMeshPrefab;
                builder.overrideByGrid = overrideByGrid;
                builder.compressBounds = compressBounds;
                builder.overrideVector = overrideVector;
                builder.CollectGeometry = useGeometry;
                builder.CollectObjects = collectObjects;
                builder.parent = gameObject;
                builder.hideEditorLogs = hideEditorLogs;
                NavMeshBuilder2d.CollectSources(sources, builder);
            }

            if (m_IgnoreNavMeshAgent)
                sources.RemoveAll(x =>
                    global::x.component != null && global::x.component.gameObject.GetComponent<NavMeshAgent>() != null);

            if (m_IgnoreNavMeshObstacle)
                sources.RemoveAll(x =>
                    global::x.component != null &&
                    global::x.component.gameObject.GetComponent<NavMeshObstacle>() != null);

            AppendModifierVolumes(ref sources);

            return sources;
        }

        private static Vector3 Abs(Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        public static Bounds GetWorldBounds(Matrix4x4 mat, Bounds bounds)
        {
            Vector3 absAxisX = Abs(mat.MultiplyVector(Vector3.right));
            Vector3 absAxisY = Abs(mat.MultiplyVector(Vector3.up));
            Vector3 absAxisZ = Abs(mat.MultiplyVector(Vector3.forward));
            Vector3 worldPosition = mat.MultiplyPoint(bounds.center);
            Vector3 worldSize = absAxisX * bounds.size.x + absAxisY * bounds.size.y + absAxisZ * bounds.size.z;
            return new Bounds(worldPosition, worldSize);
        }

        private Bounds CalculateWorldBounds(List<NavMeshBuildSource> sources)
        {
            // Use the unscaled matrix for the NavMeshSurface
            Matrix4x4 worldToLocal = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            worldToLocal = worldToLocal.inverse;
            Bounds result = new Bounds();
            if (collectObjects != CollectObjects2d.Children) result.Encapsulate(CalculateGridWorldBounds(worldToLocal));

            foreach (NavMeshBuildSource src in sources)
            {
                switch (src.shape)
                {
                    case NavMeshBuildSourceShape.Mesh:
                    {
                        Mesh m = src.sourceObject as Mesh;
                        result.Encapsulate(GetWorldBounds(worldToLocal * src.transform, m.bounds));
                        break;
                    }
                    case NavMeshBuildSourceShape.Terrain:
                    {
                        // Terrain pivot is lower/left corner - shift bounds accordingly
                        TerrainData t = src.sourceObject as TerrainData;
                        result.Encapsulate(GetWorldBounds(worldToLocal * src.transform,
                            new Bounds(0.5f * t.size, t.size)));
                        break;
                    }
                    case NavMeshBuildSourceShape.Box:
                    case NavMeshBuildSourceShape.Sphere:
                    case NavMeshBuildSourceShape.Capsule:
                    case NavMeshBuildSourceShape.ModifierBox:
                        result.Encapsulate(GetWorldBounds(worldToLocal * src.transform,
                            new Bounds(Vector3.zero, src.size)));
                        break;
                }
            }

            // Inflate the bounds a bit to avoid clipping co-planar sources
            result.Expand(0.1f);
            return result;
        }

        private static Bounds CalculateGridWorldBounds(Matrix4x4 worldToLocal)
        {
            Bounds bounds = new Bounds();
            Grid grid = FindObjectOfType<Grid>();
            var tilemaps = grid.GetComponentsInChildren<Tilemap>();
            if (tilemaps == null || tilemaps.Length < 1) throw new NullReferenceException("Add at least one tilemap");
            foreach (Tilemap tilemap in tilemaps)
            {
                //Debug.Log($"From Local Bounds [{tilemap.name}]: {tilemap.localBounds}");
                Bounds lbounds = GetWorldBounds(worldToLocal * tilemap.transform.localToWorldMatrix,
                    tilemap.localBounds);
                bounds.Encapsulate(lbounds);
                //Debug.Log($"To World Bounds: {bounds}");
            }

            bounds.Expand(0.1f);
            return bounds;
        }

        private bool HasTransformChanged()
        {
            if (m_LastPosition != transform.position) return true;
            if (m_LastRotation != transform.rotation) return true;
            return false;
        }

        private void UpdateDataIfTransformChanged()
        {
            if (HasTransformChanged())
            {
                RemoveData();
                AddData();
            }
        }

#if UNITY_EDITOR
        private bool UnshareNavMeshAsset()
        {
            // Nothing to unshare
            if (m_NavMeshData == null)
                return false;

            // Prefab parent owns the asset reference
            bool isInPreviewScene = EditorSceneManager.IsPreviewSceneObject(this);
            bool isPersistentObject = EditorUtility.IsPersistent(this);
            if (isInPreviewScene || isPersistentObject)
                return false;

            // An instance can share asset reference only with its prefab parent
            NavMeshSurface2d prefab = PrefabUtility.GetCorrespondingObjectFromSource(this);
            if (prefab != null && prefab.navMeshData == navMeshData)
                return false;

            // Don't allow referencing an asset that's assigned to another surface
            for (int i = 0; i < activeSurfaces.Count; ++i)
            {
                NavMeshSurface2d surface = activeSurfaces[i];
                if (surface != this && surface.m_NavMeshData == m_NavMeshData)
                    return true;
            }

            // Asset is not referenced by known surfaces
            return false;
        }

        private void OnValidate()
        {
            if (UnshareNavMeshAsset())
            {
                if (!m_HideEditorLogs)
                    Debug.LogWarning("Duplicating NavMeshSurface does not duplicate the referenced navmesh data", this);
                m_NavMeshData = null;
            }

            NavMeshBuildSettings settings = NavMesh.GetSettingsByID(m_AgentTypeID);
            if (settings.agentTypeID != -1)
            {
                // When unchecking the override control, revert to automatic value.
                const float kMinVoxelSize = 0.01f;
                if (!m_OverrideVoxelSize)
                    m_VoxelSize = settings.agentRadius / 3.0f;
                if (m_VoxelSize < kMinVoxelSize)
                    m_VoxelSize = kMinVoxelSize;

                // When unchecking the override control, revert to default value.
                const int kMinTileSize = 16;
                const int kMaxTileSize = 1024;
                const int kDefaultTileSize = 256;

                if (!m_OverrideTileSize)
                    m_TileSize = kDefaultTileSize;
                // Make sure tilesize is in sane range.
                if (m_TileSize < kMinTileSize)
                    m_TileSize = kMinTileSize;
                if (m_TileSize > kMaxTileSize)
                    m_TileSize = kMaxTileSize;
            }
        }
#endif
    }
}