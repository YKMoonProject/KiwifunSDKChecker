using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YKMoon.SDKTools.Editor
{
    public class IOSDependenciesProblem : ABaseProblem, IProblemResolver
    {
        public string pkgName { get; private set; }
        public List<KeyValuePair<string, DependencyObj>> depInfo { get; private set; }
        public IOSDependenciesProblem(string pkg, List<KeyValuePair<string, DependencyObj>> depInfo)
        {
            this.pkgName = pkg;
            this.depInfo = depInfo;
        }

        public override void OnGUI()
        {
            Rect rect = EditorGUILayout.BeginVertical();
            {
                DrawTitle("Same Package has different version dependencies.", "Warning", MessageType.Warning);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(pkgName);
                GUILayout.FlexibleSpace();
                GUILayout.Label(string.Format("XML Count = {0}", depInfo.Count));
                EditorGUILayout.EndHorizontal();

                foreach(var info in depInfo) {
                    var version = info.Key;
                    var dep = info.Value;
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(version, GUILayout.Width(200));
                    GUILayout.Label(dep.path.ToString(), EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();
                    if(GUILayout.Button("Select", GUILayout.Width(200))) {
                        EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(dep.path));
                    }
                    EditorGUILayout.EndHorizontal();
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
    public class CheckProcess_IOSDependencies : CheckProcess
    {
        public override void Check(Action<List<ABaseProblem>> onResult)
        {
            List<ABaseProblem> problems = new List<ABaseProblem>();
            DependencyManager.Instance.Init();
            DependencyManager.Instance.CheckIOS();
            if(DependencyManager.Instance.multyVersionPackages_IOS.Count > 0) {                
                foreach(var info in DependencyManager.Instance.multyVersionPackages_IOS) {
                    problems.Add(new IOSDependenciesProblem(info.Key, info.Value));
                }
            } else {
                problems.Add(new ProblemOK("IOSDependencies"));
            }
            onResult?.Invoke(problems);
        }
    }
}