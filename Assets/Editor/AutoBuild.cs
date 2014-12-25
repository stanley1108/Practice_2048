using UnityEditor;

class AutoBuild
{
	static void PerformBuild ()
	{
		string[] scenes = { "Assets/Gameplay.unity" };
		BuildPipeline.BuildPlayer(scenes, "my2048.app", BuildTarget.StandaloneOSXIntel, BuildOptions.None);
	}
}
