using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml.Serialization;

namespace YKMoon.SDKTools.Editor
{
    public class DependencyCheckWindow : UEditorWindowBase
    {
        //[MenuItem("YKTools/DependencyCheckWindow")]
        public static void InitWindow()
        {
            var window = EditorWindow.GetWindow(typeof(DependencyCheckWindow));
            window.titleContent = new GUIContent("DependencyCheckWindow");
            window.Show();
        }

        private int tabIndex = 0;

        private void OnGUI()
        {
            /*if(!isInit || XMLAssetPostProcessor.isDirty) {
                InitDeps();
                XMLAssetPostProcessor.isDirty = false;
            }*/
            tabIndex = GUILayout.Toolbar(tabIndex, new string[] { "all", "Android", "IOS" });

            switch(tabIndex) {
                case 0:
                    if(GUILayout.Button("检测依赖")) {
                        DependencyManager.Instance.Init();
                    }
                    ShowAllDependencies();
                    break;
                case 1:
                    DrawFoldoutHeaderGroup("Android", true, new GUIContent("Android"), () => {
                        GUILayout.BeginHorizontal();
                        if(GUILayout.Button("检测依赖")) {
                            DependencyManager.Instance.Init();
                            DependencyManager.Instance.CheckAndroid();
                        }
                        GUILayout.EndHorizontal();
                        DrawPkgs();

                        if(GUILayout.Button("GoogleFirebaseCheck")) {
                            GoogleFirebaseCheck();
                        }
                    });
                    break;
                case 2:
                    DrawFoldoutHeaderGroup("IOS", true, new GUIContent("IOS"), () => {
                        if(GUILayout.Button("检测依赖")) {
                            DependencyManager.Instance.Init();
                            DependencyManager.Instance.CheckIOS();
                        }
                        DrawPods();
                    });
                    break;
            }
        }

        private Vector2 allPos = Vector2.zero;
        private void ShowAllDependencies()
        {
            allPos = GUILayout.BeginScrollView(allPos, GUILayout.Height(300));
            GUILayout.Label(string.Format("All \"Dependencies.xml\" Count:{0}", DependencyManager.Instance.deps.Count), EditorStyles.boldLabel);
            for(int i = 0; i < DependencyManager.Instance.deps.Count; i++) {
                GUILayout.BeginHorizontal();
                GUILayout.Label(DependencyManager.Instance.deps[i].path.ToString(), EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("Select")) {
                    EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(DependencyManager.Instance.deps[i].path));
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        private Vector2 pkgPos = Vector2.zero;

        private void DrawPkgs()
        {
            pkgPos = GUILayout.BeginScrollView(pkgPos, GUILayout.Height(300));
            GUILayout.BeginVertical();

            foreach(var info in DependencyManager.Instance.multyVersionPackages_Android) {
                string name = info.Key;
                GUILayout.Label(name, EditorStyles.boldLabel);
                EditorGUI.indentLevel += 1;
                foreach(var deps in info.Value) {
                    var version = deps.Key;
                    var dep = deps.Value;
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(version, GUILayout.Width(200));
                    GUILayout.Label(dep.path.ToString(), EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();
                    if(GUILayout.Button("Select", GUILayout.Width(200))) {
                        EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(dep.path));
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel -= 1;
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
        private Vector2 podPos = Vector2.zero;
        private void DrawPods()
        {
            podPos = GUILayout.BeginScrollView(podPos, GUILayout.Height(300));
            GUILayout.BeginVertical();
            foreach(var info in DependencyManager.Instance.multyVersionPackages_IOS) {
                string name = info.Key;
                GUILayout.Label(name, EditorStyles.boldLabel);
                EditorGUI.indentLevel += 1;
                foreach(var deps in info.Value) {
                    var version = deps.Key;
                    var dep = deps.Value;
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(version, GUILayout.Width(200));
                    GUILayout.Label(dep.path.ToString(), EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();
                    if(GUILayout.Button("Select", GUILayout.Width(200))) {
                        EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(dep.path));
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel -= 1;
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private void AndroidManifestCheck()
        {
            //需要检查那个注释是否在，还有debug的标识
        }

        private void GoogleFirebaseCheck()
        {
            //FirebaseApp.androidlib下有个自动生成的value 里面有打点的id信息，这个不会随着外面的json自动重新生成，切记切记
            string path = "Assets/Plugins/Android/FirebaseApp.androidlib/res/values/google-services.xml";
            var xml = XMLUtility.LoadXML<GoogleFirebaseXML.Resources>(path);
            /*Debug.Log(xml.String.Count);
            foreach(var i in xml.String) {
                Debug.LogFormat("{0}(Translatable={1}) = {2}", i.Name, i.Translatable, i.Text);
            }*/
            string jsonPath = "Assets/google-services.json";
            GoogleFirebaseJson.Root json = null;
            XMLUtility.JsonLoad(jsonPath, ref json);

            if (!IsSame(json, xml)) {
                EditorUtility.DisplayDialog("Firebase check", "xml is not same with json. More information in console.", "ok");
                //想重新生成就把xml删掉，然后重新import一下json文件就能自动生成xml
            } else {
                EditorUtility.DisplayDialog("Firebase check","no problem.","ok");
            }
        }

        private static bool IsSame(GoogleFirebaseJson.Root json, GoogleFirebaseXML.Resources xml)
        {
            if (json == null || xml == null) {
                return false;
            }
            string project_number = json.project_info.project_number;
            string project_id = json.project_info.project_id;
            string storage_bucket = json.project_info.storage_bucket;

            if(!xml.GetValue("gcm_defaultSenderId").Equals(project_number)) {
                Debug.LogErrorFormat("XML {0} = {1} is not equals json {2} = {3}", "gcm_defaultSenderId", xml.GetValue("gcm_defaultSenderId"), "project_number", project_number);
                return false;
            }
            if(!xml.GetValue("google_storage_bucket").Equals(storage_bucket)) {
                Debug.LogErrorFormat("XML {0} = {1} is not equals json {2} = {3}", "google_storage_bucket", xml.GetValue("google_storage_bucket"), "storage_bucket", storage_bucket);
                return false;
            }
            if(!xml.GetValue("project_id").Equals(project_id)) {
                Debug.LogErrorFormat("XML {0} = {1} is not equals json {2} = {3}", "project_id", xml.GetValue("project_id"), "project_id", project_id);
                return false;
            }
            string api_key = json.client[0].api_key[0].current_key;
            string mobilesdk_app_id = json.client[0].client_info.mobilesdk_app_id;

            if(!xml.GetValue("google_api_key").Equals(api_key)) {
                Debug.LogErrorFormat("XML {0} = {1} is not equals json {2} = {3}", "google_api_key", xml.GetValue("google_api_key"), "api_key", api_key);
                return false;
            }
            if(!xml.GetValue("google_crash_reporting_api_key").Equals(api_key)) {
                Debug.LogErrorFormat("XML {0} = {1} is not equals json {2} = {3}", "google_crash_reporting_api_key", xml.GetValue("google_crash_reporting_api_key"), "api_key", api_key);
                return false;
            }
            if(!xml.GetValue("google_app_id").Equals(mobilesdk_app_id)) {
                Debug.LogErrorFormat("XML {0} = {1} is not equals json {2} = {3}", "google_app_id", xml.GetValue("google_app_id"), "mobilesdk_app_id", mobilesdk_app_id);
                return false;
            }

            return true;
        }

        private void GoogleIOSResolverSettingCheck()
        {
            //检查iOsResolverSetting内Link frameworks statically是否打勾，这个不能打勾否则会出问题gvhprojectsetting
        }

        //TODO:检查ios的依赖包~>时要考虑特殊版
        //TODO:	检查所有dependencies.xml内~>的标识，看看是否有限制版本
        //TODO:	检查iOsResolverSetting内Link frameworks statically是否打勾，这个不能打勾否则会出问题
        //TODO:检查gradle，尤其是values导致的问题

    }
}
