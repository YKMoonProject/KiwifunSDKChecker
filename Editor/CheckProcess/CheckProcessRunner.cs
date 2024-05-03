using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YKMoon.SDKTools.Editor
{
    public class CheckProcessRunner
    {
        public string procName = "";
        private List<CheckProcess> procList = new List<CheckProcess>();
        private List<ABaseProblem> allProblemList = new List<ABaseProblem>();

        private System.Action<List<ABaseProblem>> onComplete;

        public void AddProc(CheckProcess proc)
        {
            procList.Add(proc);
        }

        public void RunCheck(System.Action<List<ABaseProblem>> onComplete)
        {
            this.onComplete = onComplete;
            allProblemList.Clear();
            //ShowProgress(procName, "Checking", progress, maxProgress);
            for(int i = 0; i < procList.Count; i++) {
                var proc = procList[i];
                proc.Check(OnProcEnd);
            }
            //EditorUtility.ClearProgressBar();
            onComplete?.Invoke(allProblemList);
        }

        private void OnProcEnd(List<ABaseProblem> problems)
        {
            if(problems == null || problems.Count == 0) {
                return;
            }
            allProblemList.AddRange(problems);
        }

        private void ShowProgress(string title, string info, int progress, int maxProgress)
        {
            EditorUtility.DisplayProgressBar(title, info, maxProgress == 0 ? 0 : ((float)progress / maxProgress));
        }
    }
}