using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System.IO;


namespace YKMoon.SDKTools.Editor
{
    public class UEditorUtility
    {
        public static bool GameObjectIsPrefab(GameObject gameObject)
        {
            if(gameObject == null) {
                return false;
            }
            string path = AssetDatabase.GetAssetPath(gameObject);
            if(string.IsNullOrEmpty(path)) {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Get all sortingLayerNames.
        /// </summary>
        public static List<string> GetSortingLayerNames()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            var sortingLayers = (string[])sortingLayersProperty.GetValue(null, new object[0]);
            return new List<string>(sortingLayers);
        }

        public static void DrawMonoScriptLine(MonoBehaviour mono)
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour(mono), mono.GetType(), false);
            GUI.enabled = true;
        }
        public static void DrawMonoScriptLine<T>(UnityEngine.Object target) where T : MonoBehaviour
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((T)target), typeof(T), false);
            GUI.enabled = true;
        }

        public static void DrawMonoScriptLine(UnityEngine.Object target)
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour(target as MonoBehaviour), target.GetType(), false);
            GUI.enabled = true;
        }

        public static List<T> LoadAllAssetsAtPath<T>(string path, string ext) where T : UnityEngine.Object
        {

            List<T> objects = new List<T>();
            if(Directory.Exists(path)) {
                string[] assets = Directory.GetFiles(path);
                foreach(string assetPath in assets) {
                    if(assetPath.Contains(".meta")) {
                        continue;
                    }
                    if(assetPath.EndsWith(ext)) {
                        objects.Add(AssetDatabase.LoadAssetAtPath<T>(assetPath));
                    }
                }
            }
            return objects;
        }

        public static bool TryGetType(string typeName, out System.Type type)
        {
            type = null;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var assemble in assemblies) {
                if(assemble.FullName.StartsWith("System.")) {
                    continue;
                }
                var t = assemble.GetType(typeName);
                if(t != null) {
                    type = t;
                    return true;
                }
            }
            return false;
        }
    }
}