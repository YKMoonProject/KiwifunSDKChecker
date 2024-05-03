using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YKMoon.SDKTools.Editor
{
    public class AndroidGradleProblem_Version : ABaseProblem, IProblemResolver
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
    public class CheckProcess_AndroidGradle : CheckProcess
    {
        public override void Check(Action<List<ABaseProblem>> onResult)
        {
            List<ABaseProblem> result = new List<ABaseProblem>();

            //TODO:检查gradle，尤其是values导致的问题
            if(result.Count == 0) {
                result.Add(new ProblemOK("AndroidGradle"));
            }
            onResult?.Invoke(result);
        }
    }
}
