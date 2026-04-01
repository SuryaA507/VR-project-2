using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace SkillsLab.Editor
{
    /// <summary>
    /// Unity Editor tool to find and remove missing script components from prefabs.
    /// This helps clean up prefabs that reference scripts that no longer exist or have been deleted.
    /// </summary>
    public class MissingScriptCleaner : EditorWindow
    {
        private List<GameObject> prefabsWithMissingScripts = new List<GameObject>();
        private Vector2 scrollPosition;
        private bool isScanning = false;

        [MenuItem("Tools/Clean Missing Scripts from Prefabs")]
        public static void ShowWindow()
        {
            GetWindow<MissingScriptCleaner>("Missing Script Cleaner");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Missing Script Cleaner", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.HelpBox(
                "This tool scans all prefabs in the project and removes components with missing script references. " +
                "This is useful after upgrading Unity versions or removing deprecated scripts.",
                MessageType.Info
            );

            EditorGUILayout.Space();

            GUI.enabled = !isScanning;
            if (GUILayout.Button("Scan Project for Missing Scripts", GUILayout.Height(30)))
            {
                ScanForMissingScripts();
            }
            GUI.enabled = true;

            EditorGUILayout.Space();

            if (prefabsWithMissingScripts.Count > 0)
            {
                EditorGUILayout.LabelField($"Found {prefabsWithMissingScripts.Count} prefab(s) with missing scripts:");
                
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                foreach (var prefab in prefabsWithMissingScripts)
                {
                    if (prefab != null)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);
                        if (GUILayout.Button("Select", GUILayout.Width(60)))
                        {
                            Selection.activeObject = prefab;
                            EditorGUIUtility.PingObject(prefab);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndScrollView();

                EditorGUILayout.Space();

                if (GUILayout.Button("Clean All Missing Scripts", GUILayout.Height(30)))
                {
                    CleanAllMissingScripts();
                }
            }
            else if (!isScanning)
            {
                EditorGUILayout.HelpBox("No prefabs with missing scripts found. Click 'Scan Project' to search.", MessageType.Info);
            }

            if (isScanning)
            {
                EditorGUILayout.HelpBox("Scanning... Please wait.", MessageType.Warning);
            }
        }

        private void ScanForMissingScripts()
        {
            isScanning = true;
            prefabsWithMissingScripts.Clear();

            string[] allPrefabGuids = AssetDatabase.FindAssets("t:Prefab");
            int total = allPrefabGuids.Length;

            for (int i = 0; i < total; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(allPrefabGuids[i]);
                
                if (EditorUtility.DisplayCancelableProgressBar(
                    "Scanning Prefabs",
                    $"Checking {path}",
                    (float)i / total))
                {
                    break;
                }

                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab != null && PrefabHasMissingScripts(prefab))
                {
                    prefabsWithMissingScripts.Add(prefab);
                }
            }

            EditorUtility.ClearProgressBar();
            isScanning = false;
            
            Debug.Log($"Scan complete. Found {prefabsWithMissingScripts.Count} prefab(s) with missing scripts.");
            Repaint();
        }

        private bool PrefabHasMissingScripts(GameObject prefab)
        {
            Component[] components = prefab.GetComponentsInChildren<Component>(true);
            return components.Any(c => c == null);
        }

        private void CleanAllMissingScripts()
        {
            if (!EditorUtility.DisplayDialog(
                "Confirm Cleaning",
                $"This will remove all missing script components from {prefabsWithMissingScripts.Count} prefab(s). This action cannot be undone. Continue?",
                "Yes, Clean All",
                "Cancel"))
            {
                return;
            }

            int cleanedCount = 0;
            int total = prefabsWithMissingScripts.Count;

            for (int i = 0; i < total; i++)
            {
                GameObject prefab = prefabsWithMissingScripts[i];
                if (prefab == null) continue;

                string path = AssetDatabase.GetAssetPath(prefab);
                
                if (EditorUtility.DisplayCancelableProgressBar(
                    "Cleaning Prefabs",
                    $"Processing {path}",
                    (float)i / total))
                {
                    break;
                }

                if (CleanMissingScriptsFromPrefab(prefab, path))
                {
                    cleanedCount++;
                }
            }

            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Cleaning complete. Cleaned {cleanedCount} prefab(s).");
            
            // Re-scan to update the list
            ScanForMissingScripts();
        }

        private bool CleanMissingScriptsFromPrefab(GameObject prefab, string path)
        {
            bool cleaned = false;
            
            // Get all game objects in the prefab hierarchy
            GameObject[] allObjects = prefab.GetComponentsInChildren<Transform>(true)
                .Select(t => t.gameObject)
                .ToArray();

            foreach (GameObject go in allObjects)
            {
                // Get all components including missing ones
                Component[] components = go.GetComponents<Component>();
                
                // Use SerializedObject to remove missing scripts
                SerializedObject so = new SerializedObject(go);
                SerializedProperty sp = so.FindProperty("m_Component");

                int propertyCount = sp.arraySize;
                for (int i = propertyCount - 1; i >= 0; i--)
                {
                    SerializedProperty componentProperty = sp.GetArrayElementAtIndex(i);
                    SerializedProperty componentRef = componentProperty.FindPropertyRelative("component");
                    
                    if (componentRef.objectReferenceValue == null)
                    {
                        sp.DeleteArrayElementAtIndex(i);
                        cleaned = true;
                        Debug.Log($"Removed missing script from {go.name} in {path}");
                    }
                }

                if (cleaned)
                {
                    so.ApplyModifiedProperties();
                }
            }

            if (cleaned)
            {
                // Mark the prefab as dirty and save
                EditorUtility.SetDirty(prefab);
                PrefabUtility.SavePrefabAsset(prefab);
            }

            return cleaned;
        }
    }
}
