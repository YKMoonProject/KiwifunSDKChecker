using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YKMoon.SDKTools.Editor
{
    public class AndroidManifestProblem_DebugFlag : ABaseProblem, IProblemResolver
    {
        public override void OnGUI()
        {
            throw new NotImplementedException();
        }

        public bool TryAutoFix()
        {
            throw new NotImplementedException();
        }
    }
    public class AndroidManifestProblem_RemoveAll : ABaseProblem, IProblemResolver
    {
        public override void OnGUI()
        {
            //TODO:check remover
            //<!--AnalyticsFixPropertyRemover-->
            throw new NotImplementedException();
        }

        public bool TryAutoFix()
        {
            throw new NotImplementedException();
        }
    }
    public class CheckProcess_AndroidManifest : CheckProcess
    {
        public override void Check(Action<List<ABaseProblem>> onResult)
        {
            List<ABaseProblem> result = new List<ABaseProblem>();

            string filePath = "Assets/Plugins/Android/AndroidManifest.xml";

            string fileStr = System.IO.File.ReadAllText(filePath);

            Debug.Log(fileStr);

            if(!CheckDebugFlag()) {
                result.Add(new AndroidManifestProblem_DebugFlag());
            }
            if(!fileStr.Contains("<!--AnalyticsFixPropertyRemover-->")) {
                result.Add(new AndroidManifestProblem_RemoveAll());
            }

            /*if(!CheckRemoveAllDesc()) {
                result.Add(new AndroidManifestProblem_RemoveAll());
            }*/
            if (result.Count == 0) {
                result.Add(new ProblemOK("AndroidManifest"));
            }
            onResult?.Invoke(result);
        }

        private bool CheckDebugFlag()
        {
            return true;
        }
        private bool CheckRemoveAllDesc()
        {
            return true;
        }
    }

    public class AndroidManifestEditor
    {
        public void Load(string path)
        {
             
        }
    }
}