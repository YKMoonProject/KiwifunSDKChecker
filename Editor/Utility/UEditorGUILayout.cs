using System;
using UnityEngine;
using UnityEditor;

namespace YKMoon.SDKTools.Editor
{
    public class UEditorGUILayout
    {
        const string Editor_IconPlus = "d_Toolbar Plus";
        const string Editor_IconMinus = "d_Toolbar Minus";
        const string Editor_IconUp = "d_ProfilerTimelineRollUpArrow@2x";
        const string Editor_IconDown = "d_ProfilerTimelineDigDownArrow@2x";

        public static int DrawSortingLayerPopup(GUIContent content, int sortingLayerID, params GUILayoutOption[] options)
        {
            var sortingLayerNames = UEditorUtility.GetSortingLayerNames();
            string layername = SortingLayer.IDToName(sortingLayerID);
            int index = sortingLayerNames.IndexOf(layername);
            if(content != null) {
                index = EditorGUILayout.Popup(content, index, sortingLayerNames.ToArray(), options);
            } else {
                index = EditorGUILayout.Popup(index, sortingLayerNames.ToArray(), options);
            }
            return SortingLayer.NameToID(sortingLayerNames[index]);
        }

        public static int DrawSortinglayerPopup(int sortingLayerID)
        {
            return DrawSortingLayerPopup(null, sortingLayerID);
        }

        private static Rect GetIndentedRect(params GUILayoutOption[] options)
        {
            return EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(options));
        }

        public static int Toolbar(int selected, GUIContent[] contents, GUIStyle style, params GUILayoutOption[] options)
        {
            return GUI.Toolbar(GetIndentedRect(options), selected, contents, style);
        }

        #region Foldout

        public static bool FoldoutHeader(bool foldout, string text, params GUILayoutOption[] options)
        {
            return FoldoutHeader(foldout, text, "foldoutHeader", options);
        }

        public static bool FoldoutHeader(bool foldout, string text, [UnityEngine.Internal.DefaultValue("EditorStyles.foldoutHeader")] GUIStyle style, params GUILayoutOption[] options)
        {
            var rect = EditorGUILayout.GetControlRect(options);
            var result = EditorGUI.Toggle(rect, "", foldout, style);
            rect.x += 20;
            EditorGUI.LabelField(rect, new GUIContent(text));
            return result;
        }

        #endregion

        #region Label
        public static void Label(string text, params GUILayoutOption[] options)
        {
            GUI.Label(GetIndentedRect(options), text);
        }
        public static void Label(GUIContent content, params GUILayoutOption[] options)
        {
            GUI.Label(GetIndentedRect(options), content);
        }
        public static void Label(string text, GUIStyle style, params GUILayoutOption[] options)
        {
            GUI.Label(GetIndentedRect(options), text, style);
        }
        public static void Label(Texture image, GUIStyle style, params GUILayoutOption[] options)
        {
            GUI.Label(GetIndentedRect(options), image, style);
        }
        public static void Label(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
            GUI.Label(GetIndentedRect(options), content, style);
        }
        public static void Label(Texture image, params GUILayoutOption[] options)
        {
            GUI.Label(GetIndentedRect(options), image);
        }
        #endregion

        public static void DrawRect(Color color, params GUILayoutOption[] options)
        {
            EditorGUI.DrawRect(GetIndentedRect(options), color);
        }

        #region Button

        public static bool Button(Texture image, params GUILayoutOption[] options)
        {
            return GUI.Button(GetIndentedRect(options), image);
        }
        public static bool Button(string text, params GUILayoutOption[] options)
        {
            return GUI.Button(GetIndentedRect(options), text);
        }
        public static bool Button(GUIContent content, params GUILayoutOption[] options)
        {
            return GUI.Button(GetIndentedRect(options), content);
        }
        public static bool Button(Texture image, GUIStyle style, params GUILayoutOption[] options)
        {
            return GUI.Button(GetIndentedRect(options), image, style);
        }
        public static bool Button(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
            return GUI.Button(GetIndentedRect(options), content, style);
        }
        public static bool Button(string text, GUIStyle style, params GUILayoutOption[] options)
        {
            return GUI.Button(GetIndentedRect(options), text, style);
        }
        #endregion


        public static void HorizontalLine(float height, Color color, params GUILayoutOption[] options)
        {
            HorizontalLine(GetIndentedRect(options), height, color);
        }
        public static void HorizontalLine(Rect rect, float height, Color color)
        {
            rect.height = height;
            EditorGUI.DrawRect(rect, color);
        }

        #region List
        public static void DrawList(SerializedProperty property, System.Action<SerializedProperty> customDrawElement = null)
        {
            if(!property.isArray) {
                throw new Exception("Property is not array type. " + property.ToString());
            }

            GUILayoutOption[] options = new GUILayoutOption[] {
                GUILayout.ExpandWidth(false),
                //GUILayout.ExpandHeight(false),
            };

            for(int i = 0; i < property.arraySize; i++) {
                var p_element = property.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal(options);
                EditorGUI.indentLevel += 1;
                if(customDrawElement == null) {
                    //var rect = EditorGUILayout.GetControlRect();
                    //EditorGUI.PropertyField(rect, p_element, true);
                    EditorGUILayout.PropertyField(p_element, true);
                } else {
                    customDrawElement.Invoke(p_element);
                }
                if(p_element.isExpanded) {
                    var rect2 = EditorGUILayout.BeginVertical(options);
                    {
#if UNITY_2019_4_OR_ABOVE
                        EditorGUILayout.Space(20, false);
#else
                        EditorGUILayout.Space();
#endif
                        DrawArrayElementController(property, i);
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUI.indentLevel -= 1;
                EditorGUILayout.EndHorizontal();
            }
            if(UEditorGUILayout.Button(EditorGUIUtility.IconContent(Editor_IconPlus))) {
                property.arraySize += 1;
            }
        }

        private static Rect DrawArrayElementController(SerializedProperty arrayProperty, int index)
        {
            GUILayoutOption[] options = new GUILayoutOption[] {
                //GUILayout.Width(20),
                //GUILayout.Height(20),
                //GUILayout.ExpandWidth(false),
                //GUILayout.ExpandHeight(false),
            };
            var rect = EditorGUILayout.BeginHorizontal(GUILayout.Width(40), GUILayout.ExpandWidth(false));
            {
                EditorGUILayout.BeginVertical();
                {
                    if(GUILayout.Button(EditorGUIUtility.TrIconContent(Editor_IconUp, "MoveUp"), options)) {
                        int from = index;
                        int to = Mathf.Max(index - 1, 0);
                        if(from != to) {
                            arrayProperty.MoveArrayElement(from, to);
                        }
                    }
                    if(GUILayout.Button(EditorGUIUtility.TrIconContent(Editor_IconDown, "MoveDown"), options)) {
                        int from = index;
                        int to = Mathf.Min(index + 1, arrayProperty.arraySize - 1);
                        if(from != to) {
                            arrayProperty.MoveArrayElement(from, to);
                        }
                    }
                }
                EditorGUILayout.EndVertical();
                {
                    if(GUILayout.Button(EditorGUIUtility.TrIconContent(Editor_IconMinus, "Delete"), options)) {
                        arrayProperty.DeleteArrayElementAtIndex(index);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            return rect;
        }

        #endregion
        public static void DrawMinMaxSlider(GUIContent content, SerializedProperty minProperty, SerializedProperty maxProperty, float minLimit, float maxLimit)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(content);
            DrawMinMaxSlider(minProperty, maxProperty, minLimit, maxLimit);
            EditorGUILayout.EndHorizontal();
        }
        public static void DrawMinMaxSlider(SerializedProperty minProperty, SerializedProperty maxProperty, float minLimit, float maxLimit)
        {
            float minValue = minProperty.floatValue;
            float maxValue = maxProperty.floatValue;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginChangeCheck();
                {
                    minValue = EditorGUILayout.FloatField(minValue);
                    if(EditorGUI.EndChangeCheck()) {
                        minProperty.floatValue = Mathf.Min(Mathf.Max(minValue, minLimit), maxValue);
                    }
                }
                EditorGUI.BeginChangeCheck();
                {
                    EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, minLimit, maxLimit);
                    if(EditorGUI.EndChangeCheck()) {
                        minProperty.floatValue = Mathf.Min(Mathf.Max(minValue, minLimit), maxValue);
                        maxProperty.floatValue = Mathf.Max(Mathf.Min(maxValue, maxLimit), minValue);
                    }
                }
                EditorGUI.BeginChangeCheck();
                {
                    maxValue = EditorGUILayout.FloatField(maxValue);
                    if(EditorGUI.EndChangeCheck()) {
                        maxProperty.floatValue = Mathf.Max(Mathf.Min(maxValue, maxLimit), minValue);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}