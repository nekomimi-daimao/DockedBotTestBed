using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Build.Editor
{
    public static class BuildSystem
    {
        [MenuItem("Tools/Build/HeadlessBot")]
        public static void BuildHeadlessBot()
        {
            var dir = new DirectoryInfo(Application.dataPath).Parent;
            if (dir == null)
            {
                return;
            }
            var output = Path.Combine(dir.FullName, "Output", "HeadlessBot");

            var buildOption = new BuildPlayerOptions();
            buildOption.scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray();
            buildOption.targetGroup = BuildTargetGroup.Standalone;
            buildOption.target = BuildTarget.StandaloneLinux64;
            buildOption.subtarget = (int)StandaloneBuildSubtarget.Server;
            buildOption.locationPathName = output;

            BuildPipeline.BuildPlayer(buildOption);

            EditorUtility.RevealInFinder(output);
        }
    }
}
