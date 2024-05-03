using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YKMoon.SDKTools.Editor
{
    public class AndroidGooggleServicesProblem : ABaseProblem, IProblemResolver
    {
        private string jsonPath;
        private GoogleFirebaseJson.Root json;
        private string xmlPath;
        private GoogleFirebaseXML.Resources xml;
        public AndroidGooggleServicesProblem(string jsonPath, GoogleFirebaseJson.Root json, string xmlPath, GoogleFirebaseXML.Resources xml)
        {
            this.jsonPath = jsonPath;
            this.json = json;
            this.xmlPath = xmlPath;
            this.xml = xml;
        }

        public override void OnGUI()
        {
            Rect rect = EditorGUILayout.BeginVertical();
            {
                DrawTitle("GooggleServices json is not match firebase's xml", "Error", MessageType.Error);

                if(GUILayout.Button("TryAutoFix")) {
                    TryAutoFix();
                }
            }
            EditorGUILayout.EndVertical();
            GUI.Box(rect, GUIContent.none);
        }

        public bool TryAutoFix()
        {
            if(System.IO.File.Exists(xmlPath)) {
                System.IO.File.Delete(xmlPath);
            }
            if(System.IO.File.Exists(jsonPath)) {
                AssetDatabase.ImportAsset(jsonPath, ImportAssetOptions.ForceUpdate);
                AssetDatabase.Refresh();
            }
            SDKCheckWindow.OnFix();
            return true;
        }
    }
    public class CheckProcess_AndroidGooggleServices : CheckProcess
    {
        public override void Check(Action<List<ABaseProblem>> onResult)
        {
            List<ABaseProblem> result = new List<ABaseProblem>();
            string xmlPath = "Assets/Plugins/Android/FirebaseApp.androidlib/res/values/google-services.xml";
            var xml = XMLUtility.LoadXML<GoogleFirebaseXML.Resources>(xmlPath);

            string jsonPath = "Assets/google-services.json";
            GoogleFirebaseJson.Root json = null;
            XMLUtility.JsonLoad(jsonPath, ref json);
            if(!IsSame(json, xml)) {                
                result.Add(new AndroidGooggleServicesProblem(jsonPath, json, xmlPath, xml));
            } else {
                result.Add(new ProblemOK("GooggleServices"));
            }
            onResult?.Invoke(result);
        }

        private static bool IsSame(GoogleFirebaseJson.Root json, GoogleFirebaseXML.Resources xml)
        {
            if(json == null || xml == null) {
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
    }
}
