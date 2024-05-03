using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YKMoon.SDKTools.Editor
{
    public class AndroidDependenciesProblem : ABaseProblem, IProblemResolver
    {
        public string pkgName { get; private set; }
        public List<KeyValuePair<string, DependencyObj>> depInfo { get; private set; }
        public AndroidDependenciesProblem(string pkg, List<KeyValuePair<string, DependencyObj>> depInfo)
        {
            this.pkgName = pkg;
            this.depInfo = depInfo;
        }

        public override void OnGUI()
        {
            Rect rect = EditorGUILayout.BeginVertical();
            {
                DrawTitle(string.Format("Same Package has different version dependencies."), "Warning", MessageType.Warning);

                //EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.SelectableLabel(pkgName);
                EditorGUI.indentLevel += 1;
                EditorGUILayout.SelectableLabel(pkgName);
                EditorGUI.indentLevel -= 1;
                //GUILayout.FlexibleSpace();
                //GUILayout.Label(string.Format("XML Count = {0}", depInfo.Count));
                //EditorGUILayout.EndHorizontal();

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

    public class CheckProcess_AndroidDependencies : CheckProcess
    {
        public override void Check(Action<List<ABaseProblem>> onResult)
        {
            List<ABaseProblem> problems = new List<ABaseProblem>();
            DependencyManager.Instance.Init();
            DependencyManager.Instance.CheckAndroid();
            if(DependencyManager.Instance.multyVersionPackages_Android.Count > 0) {
                foreach(var info in DependencyManager.Instance.multyVersionPackages_Android) {
                    problems.Add(new AndroidDependenciesProblem(info.Key, info.Value));
                }
            } else {
                problems.Add(new ProblemOK("AndroidDependencies"));
            }
            onResult?.Invoke(problems);
        }
    }
}