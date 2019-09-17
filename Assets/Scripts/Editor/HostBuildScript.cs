using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Editor
{
    public  class HostBuildScript:BuildScript
    {
        private const string AppName = "FishingStar";
        private const string TargetDir = "Builds";

        private const string LOCAL_SERVER = "LOCAL_SERVER";

        
        [MenuItem("MaxieMind/Build/MacHost")]
        public static void PerformMacHostBuild()
        {
            var targetDir = TargetDir + "/Mac/" + AppName + "_host" + ".app";
            DeleteTargetDir(targetDir);
            var hostScenes = FindEnabledEditorScenes();
            SetDefines(() => GenericBuild(hostScenes, targetDir, BuildTarget.StandaloneOSX, BuildOptions.None,
                BuildTargetGroup.Standalone), BuildTargetGroup.Standalone, string.Empty);
        }

        [MenuItem("MaxieMind/Build/WinHost")]
        public static void PerformWinHostBuild()
        {
            var targetDir = TargetDir + "/Windows/" + AppName + "_host.exe";
            DeleteTargetDir(targetDir);
            var hostScenes = FindEnabledEditorScenes();
            SetDefines(() => GenericBuild(hostScenes, targetDir, BuildTarget.StandaloneWindows64, BuildOptions.None,
                BuildTargetGroup.Standalone, true), BuildTargetGroup.Standalone, string.Empty);
        }

        [MenuItem("MaxieMind/Build/LinuxHost")]
        public static void LinuxHostBuild()
        {
            var targetDir = TargetDir + "/Linux/" + AppName + "_host" + ".app";
            DeleteTargetDir(targetDir);
            var hostScenes = FindEnabledEditorScenes();
            SetDefines(() => GenericBuild(hostScenes, targetDir, BuildTarget.StandaloneLinux64,
                    BuildOptions.EnableHeadlessMode, BuildTargetGroup.Standalone, true)
                , BuildTargetGroup.Standalone, string.Empty);
        }
        
    }
}