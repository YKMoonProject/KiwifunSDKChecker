using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YKMoon.SDKTools.Editor
{
    public partial class UEditorGUIUtility
    {
        public static GUIStyle GetTitleFontStyle(FontStyle fontStyle, MessageType messageType)
        {
            if(m_titleFontStyle == null) {
                m_titleFontStyle = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).label);
            }
            m_titleFontStyle.fontStyle = fontStyle;
            switch(messageType) {
                case MessageType.Warning:
                    m_titleFontStyle.normal.textColor = Color.yellow;
                    break;
                case MessageType.Error:
                    m_titleFontStyle.normal.textColor = Color.red;
                    break;
                case MessageType.Info:
                    m_titleFontStyle.normal.textColor = Color.green;
                    break;
                default:
                    m_titleFontStyle.normal.textColor = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).label.normal.textColor;
                    break;
            }
            return m_titleFontStyle;
        }
        private static GUIStyle m_titleFontStyle;
    }
}
