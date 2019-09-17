using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;


namespace Editor
{
    public abstract class BuildScript
    {
        protected static string[] FindEnabledEditorScenes()
        {
            var editorScenes = new List<string>();

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled)
                    continue;

                editorScenes.Add(scene.path);
            }

            return editorScenes.ToArray();
        }

        protected static void GenericBuild(string[] scenes, string targetDir, BuildTarget buildTarget,
            BuildOptions buildOptions, BuildTargetGroup buildTargetGroup, bool headless = false)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup,
                buildTarget);
            EditorUserBuildSettings.enableHeadlessMode = headless;
            BuildReport res = BuildPipeline.BuildPlayer(scenes, targetDir, buildTarget, buildOptions);
            if (res.summary.result != BuildResult.Succeeded)
            {
                throw new Exception("BuildPlayer failure: " + res);
            }
        }

        protected static void DeleteTargetDir(string targetDir)
        {
            string path = Path.GetDirectoryName(targetDir);

            if (!Directory.Exists(path))
                return;

            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        protected static void SetDefines(Action generateBuild, BuildTargetGroup buildTargetGroup, string defines)
        {
            var oldDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
            generateBuild();
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, oldDefines);
        }
    }
}