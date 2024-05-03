using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YKMoon.SDKTools.Editor
{
    public class IOSResolveSettingProblem : ABaseProblem, IProblemResolver
    {
        private Google_IOSResolver googleType;
        public IOSResolveSettingProblem(Google_IOSResolver googleType)
        {
            this.googleType = googleType;
        }

        public override void OnGUI()
        {
            Rect rect = EditorGUILayout.BeginVertical();
            {
                DrawTitle("Google resolver setting has problem.", "Error", MessageType.Error);
                GUILayout.Label("PodfileStaticLinkFrameworks is True.It will cause crash on iphone.");
                if(GUILayout.Button("TryAutoFix")) {
                    TryAutoFix();
                }

            }
            EditorGUILayout.EndVertical();
            GUI.Box(rect, GUIContent.none);
        }

        public bool TryAutoFix()
        {
            googleType.PodfileStaticLinkFrameworks = false;
            AssetDatabase.Refresh();
            SDKCheckWindow.OnFix();
            return true;
        }
    }
    public class CheckProcess_IOSResolveSetting : CheckProcess
    {
        public override void Check(Action<List<ABaseProblem>> onResult)
        {
            List<ABaseProblem> result = new List<ABaseProblem>();
            //检查iOsResolverSetting内Link frameworks statically是否打勾，这个不能打勾否则会出问题gvhprojectsetting
            if(UEditorUtility.TryGetType("Google.IOSResolver", out System.Type iosResolverType)) {
                Google_IOSResolver handler = new Google_IOSResolver(iosResolverType);
                if(handler.PodfileStaticLinkFrameworks) {
                    result.Add(new IOSResolveSettingProblem(handler));
                }
            } else {
                result.Add(new ProblemOK("Skip(No GoogleIOSResolveSetting)"));
            }

            if(result.Count == 0) {
                result.Add(new ProblemOK("GoogleIOSResolveSetting"));
            }
            onResult?.Invoke(result);
        }
    }

    public class Google_IOSResolver
    {
        private System.Type iosResolverType;
        public Google_IOSResolver(System.Type iosResolverType)
        {
            this.iosResolverType = iosResolverType;
            PodfileStaticLinkFrameworks_getter = iosResolverType.GetMethod("get_PodfileStaticLinkFrameworks");//static
            PodfileStaticLinkFrameworks_setter = iosResolverType.GetMethod("set_PodfileStaticLinkFrameworks");//static
        }
        public bool PodfileStaticLinkFrameworks {
            get {
                return (bool)PodfileStaticLinkFrameworks_getter.Invoke(null, null);
            }
            set {
                PodfileStaticLinkFrameworks_setter.Invoke(null, new object[] { value });
            }
        }
        private System.Reflection.MethodInfo PodfileStaticLinkFrameworks_getter;
        private System.Reflection.MethodInfo PodfileStaticLinkFrameworks_setter;
    }
}
