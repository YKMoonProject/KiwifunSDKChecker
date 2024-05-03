using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YKMoon.SDKTools.Editor
{
    public class IOSProjectSettingsProblem : ABaseProblem, IProblemResolver
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
    public class CheckProcess_IOSProjectSettings : CheckProcess
    {
        public override void Check(Action<List<ABaseProblem>> onResult)
        {
            List<ABaseProblem> result = new List<ABaseProblem>();


            if(result.Count == 0) {
                result.Add(new ProblemOK("IOSProjectSettings"));
            }
            onResult?.Invoke(result);
        }
    }
}
