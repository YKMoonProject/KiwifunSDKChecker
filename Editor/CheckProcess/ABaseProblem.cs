using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YKMoon.SDKTools.Editor
{
    public interface IProblemResolver
    {
        bool TryAutoFix();
    }

    public abstract class ABaseProblem
    {
        public abstract void OnGUI();

        protected void DrawTitle(string title, string right, MessageType messageType)
        {
            var titleRect = EditorGUILayout.BeginHorizontal();
            var fontStyle = UEditorGUIUtility.GetTitleFontStyle(FontStyle.Bold, messageType);

            GUILayout.Label(title, fontStyle);
            GUILayout.FlexibleSpace();
            GUILayout.Label(right, fontStyle);
            EditorGUILayout.EndHorizontal();
            if(messageType != MessageType.None) {
                GUI.Box(titleRect, GUIContent.none, EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("HelpBox"));
            }
        }
    }

    public class ProblemOK: ABaseProblem
    {
        private string title;
        public ProblemOK(string title)
        {
            this.title = title;
        }
        public override void OnGUI()
        {
            DrawTitle(title, "OK", MessageType.Info);
        }
    }
}
