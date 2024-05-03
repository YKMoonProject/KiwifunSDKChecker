using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YKMoon.SDKTools.Editor
{
    public class AndroidProjectSettingsProblem : ABaseProblem, IProblemResolver
    {
        public override void OnGUI()
        {
        }

        public bool TryAutoFix()
        {
            return true;
        }
    }
    public class CheckProcess_AndroidProjectSettings : CheckProcess
    {
        public override void Check(Action<List<ABaseProblem>> onResult)
        {
            List<ABaseProblem> result = new List<ABaseProblem>();


            if(result.Count == 0) {
                result.Add(new ProblemOK("AndroidProjectSettings"));
            }
            onResult?.Invoke(result);
        }
    }
}
