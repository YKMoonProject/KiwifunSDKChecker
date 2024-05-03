//#define UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

namespace YKMoon.SDKTools.Editor
{
    public class SDKPostProcessBuild
    {
#if UNITY_IOS
        [PostProcessBuild(1500)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if(target != BuildTarget.iOS) {
                return;
            }

            string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
            string pbxProjectPath = PBXProject.GetPBXProjectPath(path);
            PBXProject pbxProject = new PBXProject();

            pbxProject.ReadFromFile(pbxProjectPath);

            string target_name = "Unity-iPhone";
#if UNITY_2019_3_OR_NEWER
            string target_guid = pbxProject.GetUnityMainTargetGuid();
#else
            string target_guid = pbxProject.TargetGuidByName(target_name);
#endif
            string src = string.Format("{0}/IOSEntitlements/{1}.entitlements", UPMUtility.packageResPath, "PushNotifications");

            var file_name = Path.GetFileName(src);
            string dst = path + "/" + target_name + "/" + file_name;

            FileUtil.CopyFileOrDirectory(src, dst);
            pbxProject.AddFile(target_name + "/" + file_name, file_name);
            pbxProject.AddBuildProperty(target_guid, "CODE_SIGN_ENTITLEMENTS", target_name + "/" + file_name);

            pbxProject.WriteToFile(pbxProjectPath);
        }
#endif
    }
}
