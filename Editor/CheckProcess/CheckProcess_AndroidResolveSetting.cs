using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace YKMoon.SDKTools.Editor
{
    public class AndroidResolveSettingProblem : ABaseProblem, IProblemResolver
    {
        public override void OnGUI()
        {
            Rect rect = EditorGUILayout.BeginVertical();
            {
                DrawTitle("GvhProjectSettings parameter has error.", "Error", MessageType.Error);
                if(GUILayout.Button("TryAutoFix")) {
                    TryAutoFix();
                }
            }
            EditorGUILayout.EndVertical();
            GUI.Box(rect, GUIContent.none);
        }

        public bool TryAutoFix()
        {
            return true;
        }
    }
    public class CheckProcess_AndroidResolveSetting : CheckProcess
    {
        public override void Check(Action<List<ABaseProblem>> onResult)
        {
            List<ABaseProblem> result = new List<ABaseProblem>();
           
            if(result.Count == 0) {
                result.Add(new ProblemOK("GoogleAndroidResolveSetting"));
            }
            onResult?.Invoke(result);
        }
    }
}
