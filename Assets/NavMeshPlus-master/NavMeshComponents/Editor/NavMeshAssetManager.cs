using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

namespace UnityEditor.AI
{
    public class NavMeshAssetManager : ScriptableSingleton<NavMeshAssetManager>
    {
        private readonly List<AsyncBakeOperation> m_BakeOperations = new List<AsyncBakeOperation>();

        private readonly List<SavedPrefabNavMeshData> m_PrefabNavMeshDataAssets = new List<SavedPrefabNavMeshData>();

        internal List<AsyncBakeOperation> GetBakeOperations()
        {
            return m_BakeOperations;
        }

        private static string GetAndEnsureTargetPath(NavMeshSurface surface)
        {
            // Create directory for the asset if it does not exist yet.
            string activeScenePath = surface.gameObject.scene.path;

            string targetPath = "Assets";
            if (!string.IsNullOrEmpty(activeScenePath))
            {
                targetPath = Path.Combine(Path.GetDirectoryName(activeScenePath),
                    Path.GetFileNameWithoutExtension(activeScenePath));
            }
            else
            {
                PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage(surface.gameObject);
                bool isPartOfPrefab = prefabStage != null && prefabStage.IsPartOfPrefabContents(surface.gameObject);
                if (isPartOfPrefab && !string.IsNullOrEmpty(prefabStage.assetPath))
                {
                    string prefabDirectoryName = Path.GetDirectoryName(prefabStage.assetPath);
                    if (!string.IsNullOrEmpty(prefabDirectoryName))
                        targetPath = prefabDirectoryName;
                }
            }

            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);
            return targetPath;
        }

        private static void CreateNavMeshAsset(NavMeshSurface surface)
        {
            string targetPath = GetAndEnsureTargetPath(surface);

            string combinedAssetPath = Path.Combine(targetPath, "NavMesh-" + surface.name + ".asset");
            combinedAssetPath = AssetDatabase.GenerateUniqueAssetPath(combinedAssetPath);
            AssetDatabase.CreateAsset(surface.navMeshData, combinedAssetPath);
        }

        private NavMeshData GetNavMeshAssetToDelete(NavMeshSurface navSurface)
        {
            if (PrefabUtility.IsPartOfPrefabInstance(navSurface) && !PrefabUtility.IsPartOfModelPrefab(navSurface))
            {
                // Don't allow deleting the asset belonging to the prefab parent
                NavMeshSurface parentSurface = PrefabUtility.GetCorrespondingObjectFromSource(navSurface);
                if (parentSurface && navSurface.navMeshData == parentSurface.navMeshData)
                    return null;
            }

            // Do not delete the NavMeshData asset referenced from a prefab until the prefab is saved
            PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage(navSurface.gameObject);
            bool isPartOfPrefab = prefabStage != null && prefabStage.IsPartOfPrefabContents(navSurface.gameObject);
            if (isPartOfPrefab && IsCurrentPrefabNavMeshDataStored(navSurface))
                return null;

            return navSurface.navMeshData;
        }

        private void ClearSurface(NavMeshSurface navSurface)
        {
            bool hasNavMeshData = navSurface.navMeshData != null;
            StoreNavMeshDataIfInPrefab(navSurface);

            NavMeshData assetToDelete = GetNavMeshAssetToDelete(navSurface);
            navSurface.RemoveData();

            if (hasNavMeshData)
            {
                SetNavMeshData(navSurface, null);
                EditorSceneManager.MarkSceneDirty(navSurface.gameObject.scene);
            }

            if (assetToDelete)
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(assetToDelete));
        }

        public void StartBakingSurfaces(Object[] surfaces)
        {
            // Remove first to avoid double registration of the callback
            EditorApplication.update -= UpdateAsyncBuildOperations;
            EditorApplication.update += UpdateAsyncBuildOperations;

            foreach (NavMeshSurface surf in surfaces)
            {
                StoreNavMeshDataIfInPrefab(surf);

                AsyncBakeOperation oper = new AsyncBakeOperation();

                oper.bakeData = InitializeBakeData(surf);
                oper.bakeOperation = surf.UpdateNavMesh(oper.bakeData);
                oper.surface = surf;

                m_BakeOperations.Add(oper);
            }
        }

        private static NavMeshData InitializeBakeData(NavMeshSurface surface)
        {
            var emptySources = new List<NavMeshBuildSource>();
            Bounds emptyBounds = new Bounds();
            return UnityEngine.AI.NavMeshBuilder.BuildNavMeshData(surface.GetBuildSettings(), emptySources, emptyBounds
                , surface.transform.position, surface.transform.rotation);
        }

        private void UpdateAsyncBuildOperations()
        {
            foreach (AsyncBakeOperation oper in m_BakeOperations)
            {
                if (oper.surface == null || oper.bakeOperation == null)
                    continue;

                if (oper.bakeOperation.isDone)
                {
                    NavMeshSurface surface = oper.surface;
                    NavMeshData delete = GetNavMeshAssetToDelete(surface);
                    if (delete != null)
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(delete));

                    surface.RemoveData();
                    SetNavMeshData(surface, oper.bakeData);

                    if (surface.isActiveAndEnabled)
                        surface.AddData();
                    CreateNavMeshAsset(surface);
                    EditorSceneManager.MarkSceneDirty(surface.gameObject.scene);
                }
            }

            m_BakeOperations.RemoveAll(o => o.bakeOperation == null || o.bakeOperation.isDone);
            if (m_BakeOperations.Count == 0)
                EditorApplication.update -= UpdateAsyncBuildOperations;
        }

        public bool IsSurfaceBaking(NavMeshSurface surface)
        {
            if (surface == null)
                return false;

            foreach (AsyncBakeOperation oper in m_BakeOperations)
            {
                if (oper.surface == null || oper.bakeOperation == null)
                    continue;

                if (oper.surface == surface)
                    return true;
            }

            return false;
        }

        public void ClearSurfaces(Object[] surfaces)
        {
            foreach (NavMeshSurface s in surfaces)
                ClearSurface(s);
        }

        private static void SetNavMeshData(NavMeshSurface navSurface, NavMeshData navMeshData)
        {
            SerializedObject so = new SerializedObject(navSurface);
            SerializedProperty navMeshDataProperty = so.FindProperty("m_NavMeshData");
            navMeshDataProperty.objectReferenceValue = navMeshData;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private void StoreNavMeshDataIfInPrefab(NavMeshSurface surfaceToStore)
        {
            PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage(surfaceToStore.gameObject);
            bool isPartOfPrefab = prefabStage != null && prefabStage.IsPartOfPrefabContents(surfaceToStore.gameObject);
            if (!isPartOfPrefab)
                return;

            // check if data has already been stored for this surface
            foreach (SavedPrefabNavMeshData storedAssetInfo in m_PrefabNavMeshDataAssets)
            {
                if (storedAssetInfo.surface == surfaceToStore)
                    return;
            }

            if (m_PrefabNavMeshDataAssets.Count == 0)
            {
                PrefabStage.prefabSaving -= DeleteStoredNavMeshDataAssetsForOwnedSurfaces;
                PrefabStage.prefabSaving += DeleteStoredNavMeshDataAssetsForOwnedSurfaces;

                PrefabStage.prefabStageClosing -= ForgetUnsavedNavMeshDataChanges;
                PrefabStage.prefabStageClosing += ForgetUnsavedNavMeshDataChanges;
            }

            bool isDataOwner = true;
            if (PrefabUtility.IsPartOfPrefabInstance(surfaceToStore) &&
                !PrefabUtility.IsPartOfModelPrefab(surfaceToStore))
            {
                NavMeshSurface basePrefabSurface = PrefabUtility.GetCorrespondingObjectFromSource(surfaceToStore);
                isDataOwner = basePrefabSurface == null || surfaceToStore.navMeshData != basePrefabSurface.navMeshData;
            }

            m_PrefabNavMeshDataAssets.Add(new SavedPrefabNavMeshData
                {surface = surfaceToStore, navMeshData = isDataOwner ? surfaceToStore.navMeshData : null});
        }

        private bool IsCurrentPrefabNavMeshDataStored(NavMeshSurface surface)
        {
            if (surface == null)
                return false;

            foreach (SavedPrefabNavMeshData storedAssetInfo in m_PrefabNavMeshDataAssets)
            {
                if (storedAssetInfo.surface == surface)
                    return storedAssetInfo.navMeshData == surface.navMeshData;
            }

            return false;
        }

        private void DeleteStoredNavMeshDataAssetsForOwnedSurfaces(GameObject gameObjectInPrefab)
        {
            // Debug.LogFormat("DeleteStoredNavMeshDataAsset() when saving prefab {0}", gameObjectInPrefab.name);

            var surfaces = gameObjectInPrefab.GetComponentsInChildren<NavMeshSurface>(true);
            foreach (NavMeshSurface surface in surfaces)
                DeleteStoredPrefabNavMeshDataAsset(surface);
        }

        private void DeleteStoredPrefabNavMeshDataAsset(NavMeshSurface surface)
        {
            for (int i = m_PrefabNavMeshDataAssets.Count - 1; i >= 0; i--)
            {
                SavedPrefabNavMeshData storedAssetInfo = m_PrefabNavMeshDataAssets[i];
                if (storedAssetInfo.surface == surface)
                {
                    NavMeshData storedNavMeshData = storedAssetInfo.navMeshData;
                    if (storedNavMeshData != null && storedNavMeshData != surface.navMeshData)
                    {
                        string assetPath = AssetDatabase.GetAssetPath(storedNavMeshData);
                        AssetDatabase.DeleteAsset(assetPath);
                    }

                    m_PrefabNavMeshDataAssets.RemoveAt(i);
                    break;
                }
            }

            if (m_PrefabNavMeshDataAssets.Count == 0)
            {
                PrefabStage.prefabSaving -= DeleteStoredNavMeshDataAssetsForOwnedSurfaces;
                PrefabStage.prefabStageClosing -= ForgetUnsavedNavMeshDataChanges;
            }
        }

        private void ForgetUnsavedNavMeshDataChanges(PrefabStage prefabStage)
        {
            // Debug.Log("On prefab closing - forget about this object's surfaces and stop caring about prefab saving");

            if (prefabStage == null)
                return;

            var allSurfacesInPrefab = prefabStage.prefabContentsRoot.GetComponentsInChildren<NavMeshSurface>(true);
            NavMeshSurface surfaceInPrefab = null;
            int index = 0;
            do
            {
                if (allSurfacesInPrefab.Length > 0)
                    surfaceInPrefab = allSurfacesInPrefab[index];

                for (int i = m_PrefabNavMeshDataAssets.Count - 1; i >= 0; i--)
                {
                    SavedPrefabNavMeshData storedPrefabInfo = m_PrefabNavMeshDataAssets[i];
                    if (storedPrefabInfo.surface == null)
                    {
                        // Debug.LogFormat("A surface from the prefab got deleted after it has baked a new NavMesh but it hasn't saved it. Now the unsaved asset gets deleted. ({0})", storedPrefabInfo.navMeshData);

                        // surface got deleted, thus delete its initial NavMeshData asset
                        if (storedPrefabInfo.navMeshData != null)
                        {
                            string assetPath = AssetDatabase.GetAssetPath(storedPrefabInfo.navMeshData);
                            AssetDatabase.DeleteAsset(assetPath);
                        }

                        m_PrefabNavMeshDataAssets.RemoveAt(i);
                    }
                    else if (surfaceInPrefab != null && storedPrefabInfo.surface == surfaceInPrefab)
                    {
                        //Debug.LogFormat("The surface {0} from the prefab was storing the original navmesh data and now will be forgotten", surfaceInPrefab);

                        NavMeshSurface baseSurface = PrefabUtility.GetCorrespondingObjectFromSource(surfaceInPrefab);
                        if (baseSurface == null || surfaceInPrefab.navMeshData != baseSurface.navMeshData)
                        {
                            string assetPath = AssetDatabase.GetAssetPath(surfaceInPrefab.navMeshData);
                            AssetDatabase.DeleteAsset(assetPath);

                            //Debug.LogFormat("The surface {0} from the prefab has baked new NavMeshData but did not save this change so the asset has been now deleted. ({1})",
                            //    surfaceInPrefab, assetPath);
                        }

                        m_PrefabNavMeshDataAssets.RemoveAt(i);
                    }
                }
            } while (++index < allSurfacesInPrefab.Length);

            if (m_PrefabNavMeshDataAssets.Count == 0)
            {
                PrefabStage.prefabSaving -= DeleteStoredNavMeshDataAssetsForOwnedSurfaces;
                PrefabStage.prefabStageClosing -= ForgetUnsavedNavMeshDataChanges;
            }
        }

        internal struct AsyncBakeOperation
        {
            public NavMeshSurface surface;
            public NavMeshData bakeData;
            public AsyncOperation bakeOperation;
        }

        private struct SavedPrefabNavMeshData
        {
            public NavMeshSurface surface;
            public NavMeshData navMeshData;
        }
    }
}