using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml.Serialization;

namespace YKMoon.SDKTools.Editor
{
    public class SDKCheckWindow : UEditorWindowBase
    {
        private static SDKCheckWindow window;
        private static System.Action onFix;
        [MenuItem("YKTools/SDKCheck")]
        public static void InitWindow()
        {
            window = (SDKCheckWindow)EditorWindow.GetWindow(typeof(SDKCheckWindow));
            window.titleContent = new GUIContent("SDKCheckWindow");
            window.Show();
        }
        public static void OnFix()
        {
            onFix?.Invoke();
        }
        #region Tab
        private enum TabType
        {
            Android = 0,
            IOS = 1,
            //Setting = 2,
        }

        private string[] tabs {
            get {
                if(m_tabs == null) {
                    m_tabs = System.Enum.GetNames(typeof(TabType));
                }
                return m_tabs;
            }
        }
        private string[] m_tabs;

        private TabType tabType {
            get { return (TabType)GetValue("TabType", 0); }
            set { SetValue("TabType", (int)value); }
        }
        #endregion

        private void OnEnable()
        {
            onFix += this.ReCheck;
        }
        private void OnDisable()
        {
            onFix -= this.ReCheck;
        }

        private void OnGUI()
        {
            tabType = (TabType)GUILayout.Toolbar((int)tabType, tabs);
            switch(tabType) {
                //case TabType.Setting:
                //    DrawSetting();
                //    break;
                case TabType.Android:
                    DrawAndroid();
                    break;
                case TabType.IOS:
                    DrawIOS();
                    break;
            }
        }

        private void DrawSetting()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("Other Settings", MessageType.None);
            GUILayout.EndHorizontal();
        }
        
        private void ReCheck()
        {
            //Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(ReCheckItor(), this);
        //}
        //private IEnumerator ReCheckItor()
        //{
        //    yield return null;
        //    yield return null;
            switch(tabType) {
                //case TabType.Setting:

                //    break;
                case TabType.Android:
                    CheckAndroid();
                    break;
                case TabType.IOS:
                    CheckIOS();
                    break;
            }
        }
        #region Android
        private void DrawAndroid()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("Check Android Problems.", MessageType.None);
            if(GUILayout.Button("Check")) {
                CheckAndroid();
            }
            GUILayout.EndHorizontal();

            DrawAndroidProblems();
        }
       
        private List<ABaseProblem> androidProblems = new List<ABaseProblem>();

        private void CheckAndroid()
        {
            androidProblems.Clear();
            CheckProcessRunner processor = new CheckProcessRunner();
            processor.AddProc(new CheckProcess_AndroidDependencies());
            processor.AddProc(new CheckProcess_AndroidGooggleServices());
            processor.AddProc(new CheckProcess_AndroidManifest());
            processor.AddProc(new CheckProcess_AndroidGradle());
            processor.AddProc(new CheckProcess_AndroidResolveSetting());
            processor.AddProc(new CheckProcess_AndroidProjectSettings());
            processor.RunCheck(OnCompleteAndroidCheck);
        }

        private void OnCompleteAndroidCheck(List<ABaseProblem> problems)
        {
            if (problems == null || problems.Count == 0) {
                return;
            }
            androidProblems.AddRange(problems);
        }
        private Vector2 androiPos = Vector2.zero;
        private List<ABaseProblem> androidProblemCache = new List<ABaseProblem>();
        private void DrawAndroidProblems()
        {
            androidProblemCache.Clear();
            androidProblemCache.AddRange(androidProblems);
            androiPos = EditorGUILayout.BeginScrollView(androiPos);
            foreach(var problem in androidProblemCache) {
                problem.OnGUI();
            }
            EditorGUILayout.EndScrollView();
        }
        #endregion

        #region IOS

        private void DrawIOS()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("Check IOS Problems.", MessageType.None);
            if(GUILayout.Button("Check")) {
                CheckIOS();
            }
            GUILayout.EndHorizontal();

            DrawIOSProblems();
        }

        private void CheckIOS()
        {
            iosProblems.Clear();
            CheckProcessRunner processor = new CheckProcessRunner();
            processor.AddProc(new CheckProcess_IOSDependencies());
            processor.AddProc(new CheckProcess_IOSResolveSetting());
            processor.AddProc(new CheckProcess_IOSProjectSettings());
            processor.RunCheck(OnCompleteIOSCheck);
        }
        private List<ABaseProblem> iosProblems = new List<ABaseProblem>();
        private void OnCompleteIOSCheck(List<ABaseProblem> problems)
        {
            if(problems == null || problems.Count == 0) {
                return;
            }
            iosProblems.AddRange(problems);
        }

        private Vector2 iosPos = Vector2.zero;
        private List<ABaseProblem> iosProblemCache = new List<ABaseProblem>();
        private void DrawIOSProblems()
        {
            iosProblemCache.Clear();
            iosProblemCache.AddRange(iosProblems);
            iosPos = EditorGUILayout.BeginScrollView(iosPos);
            foreach(var problem in iosProblemCache) {
                problem.OnGUI();
            }
            EditorGUILayout.EndScrollView();
        }
        #endregion
    }
}

