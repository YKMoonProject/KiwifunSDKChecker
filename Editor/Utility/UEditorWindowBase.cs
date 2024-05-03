using UnityEngine;
using UnityEditor;

namespace YKMoon.SDKTools.Editor
{
    public class UEditorWindowBase : EditorWindow
    {
        protected bool BeginFoldoutHeaderGroup(string headerKey, bool defaultOpen, GUIContent content)
        {
            bool isFoldout = GetValue(headerKey, defaultOpen);
            var result = EditorGUILayout.BeginFoldoutHeaderGroup(isFoldout, content);
            if(result != isFoldout) {
                SetValue(headerKey, result);
            }
            return result;
        }

        protected void DrawFoldoutHeaderGroup(string headerKey, bool defaultOpen, GUIContent content, System.Action onGUI)
        {
            if(BeginFoldoutHeaderGroup(headerKey, defaultOpen, content)) {
                EditorGUI.indentLevel += 1;
                onGUI?.Invoke();
                EditorGUI.indentLevel -= 1;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        protected void DrawFoldoutHeaderGroup(GUIContent content, System.Action onGUI)
        {
            if(EditorGUILayout.BeginFoldoutHeaderGroup(true, content)) {
                EditorGUI.indentLevel += 1;
                onGUI?.Invoke();
                EditorGUI.indentLevel -= 1;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        #region EditorPrefs
        public int GetValue(string key, int defaultValue)
        {
            return EditorPrefs.GetInt(GetPrefKey(key), defaultValue);
        }
        public void SetValue(string key, int value)
        {
            EditorPrefs.SetInt(GetPrefKey(key), value);
        }

        public bool GetValue(string key, bool defaultValue)
        {
            return EditorPrefs.GetBool(GetPrefKey(key), defaultValue);
        }
        public void SetValue(string key, bool value)
        {
            EditorPrefs.SetBool(GetPrefKey(key), value);
        }

        public float GetValue(string key, float defaultValue)
        {
            return EditorPrefs.GetFloat(GetPrefKey(key), defaultValue);
        }
        public void SetValue(string key, float value)
        {
            EditorPrefs.SetFloat(GetPrefKey(key), value);
        }
        public string GetValue(string key, string defaultValue)
        {
            return EditorPrefs.GetString(GetPrefKey(key), defaultValue);
        }
        public void SetValue(string key, string value)
        {
            EditorPrefs.SetString(GetPrefKey(key), value);
        }

        protected string GetPrefKey(string key)
        {
            return string.Format("{0}_{1}", GetType(), key);
        }
        #endregion
    }
}
