using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace YKMoon.SDKTools.Editor
{
    public class DependencyManager
    {
        public static DependencyManager Instance {
            get {
                if(_instance == null) {
                    _instance = new DependencyManager();
                }
                return _instance;
            }
        }
        private static DependencyManager _instance;

        public readonly List<DependencyObj> deps = new List<DependencyObj>();
        public readonly Dictionary<string, List<KeyValuePair<string, DependencyObj>>> androidPackages = new Dictionary<string, List<KeyValuePair<string, DependencyObj>>>();
        public readonly Dictionary<string, List<KeyValuePair<string, DependencyObj>>> iosPods = new Dictionary<string, List<KeyValuePair<string, DependencyObj>>>();

        /// <summary>
        /// name, (version, depObj)
        /// </summary>
        public readonly Dictionary<string, List<KeyValuePair<string, DependencyObj>>> multyVersionPackages_Android = new Dictionary<string, List<KeyValuePair<string, DependencyObj>>>();
        public readonly Dictionary<string, List<KeyValuePair<string, DependencyObj>>> multyVersionPackages_IOS = new Dictionary<string, List<KeyValuePair<string, DependencyObj>>>();

        public void Init()
        {
            var xmls = GetFiles("Assets/");
            deps.Clear();
            foreach(var path in xmls) {
                deps.Add(new DependencyObj(path));
            }
            foreach(var dep in deps) {
                dep.Load();
            }
            Debug.LogFormat("Dependency xml file Count:{0}", deps.Count);
        }
        #region Android
        public void CheckAndroid()
        {
            androidPackages.Clear();
            foreach(var dep in deps) {
                var pkgs = dep.GetAndroidPkgInfo();
                foreach(var pkg in pkgs) {
                    string pkgName = pkg.Key;
                    string version = pkg.Value;
                    if(!androidPackages.ContainsKey(pkgName)) {
                        androidPackages.Add(pkgName, new List<KeyValuePair<string, DependencyObj>>());
                    }
                    if(!AndroidContainsSamePackage(pkgName, version)) {
                        androidPackages[pkgName].Add(new KeyValuePair<string, DependencyObj>(pkg.Value, dep));
                    }
                }
            }
            multyVersionPackages_Android.Clear();
            foreach(var info in androidPackages) {
                if(info.Value.Count <= 1) {
                    continue;
                }
                multyVersionPackages_Android.Add(info.Key, info.Value);
            }
        }
        private bool AndroidContainsSamePackage(string packageName, string version)
        {
            if(!androidPackages.ContainsKey(packageName)) {
                return false;
            }
            foreach(var pkgInfo in androidPackages[packageName]) {
                if(pkgInfo.Key.Equals(version)) {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region IOS
        public void CheckIOS()
        {
            iosPods.Clear();
            foreach(var dep in deps) {
                var pods = dep.GetIOSPodsInfo();
                foreach(var pod in pods) {
                    string name = pod.Key;
                    string version = pod.Value;
                    if(!iosPods.ContainsKey(name)) {
                        iosPods.Add(name, new List<KeyValuePair<string, DependencyObj>>());
                    }
                    if(!IOSContainsSamePods(name, version)) {
                        iosPods[name].Add(new KeyValuePair<string, DependencyObj>(pod.Value, dep));
                    }
                }
            }
            multyVersionPackages_IOS.Clear();
            foreach(var info in iosPods) {
                if(info.Value.Count <= 1) {
                    continue;
                }
                multyVersionPackages_IOS.Add(info.Key, info.Value);
            }
        }
        private bool IOSContainsSamePods(string name, string version)
        {
            if(!iosPods.ContainsKey(name)) {
                return false;
            }
            foreach(var info in iosPods[name]) {
                if(info.Key.Equals(version)) {
                    return true;
                }
            }
            return false;
        }
        #endregion

        private static List<string> GetFiles(string path)
        {
            List<string> result = new List<string>();
            if(Directory.Exists(path)) {
                string[] assets = Directory.GetFiles(path, "*Dependencies.xml", SearchOption.AllDirectories);
                foreach(string assetPath in assets) {
                    if(assetPath.Contains(".meta")) {
                        continue;
                    }
                    result.Add(assetPath);
                }
            }
            return result;
        }
    }

    public class XMLAssetPostProcessor : AssetPostprocessor
    {
        public static bool isDirty = false;
#if UNITY_2022_OR_NEWER
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
#else
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
#endif
        {
            isDirty = true;
        }
    }
}
