using UnityEngine;
using UnityEditor;

class AutoBuild:MonoBehaviour
{
	static void PerformBuild ()
	{
		string[] scenes = { "Assets/Scene/Gameplay.unity" };
		BuildPipeline.BuildPlayer(GetScenePaths(), Application.dataPath + "/my2048.app", BuildTarget.StandaloneOSXIntel, BuildOptions.None);
	}

	static string[] GetScenePaths()
	{
		string[] scenes = new string[EditorBuildSettings.scenes.Length];
		
		for(int i = 0; i < scenes.Length; i++)
		{
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}
		
		return scenes;
	}
}