#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public static class XcodePostProcessBuild
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target != BuildTarget.iOS)
        {
            return;
        }

        var project = new PBXProject();
        project.ReadFromFile(PBXProject.GetPBXProjectPath(path));

        project.SetBuildProperty(project.GetUnityMainTargetGuid(), "ENABLE_BITCODE", "NO");
        project.SetBuildProperty(project.GetUnityFrameworkTargetGuid(), "ENABLE_BITCODE", "NO");
        project.WriteToFile(PBXProject.GetPBXProjectPath(path));

    }
}
#endif