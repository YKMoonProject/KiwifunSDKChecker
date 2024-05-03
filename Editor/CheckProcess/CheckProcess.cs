using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YKMoon.SDKTools.Editor
{
    public abstract class CheckProcess
    {
        public abstract void Check(System.Action<List<ABaseProblem>> onResult);
    }
}
