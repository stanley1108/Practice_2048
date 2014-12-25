﻿using UnityEngine;
using UnityEditor;

class AutoBuild:MonoBehaviour
{
	static void PerformBuild ()
	{
		string[] scenes = { "Assets/Scene/Gameplay.unity" };
		BuildPipeline.BuildPlayer(scenes, "Build/my2048.app", BuildTarget.StandaloneOSXIntel, BuildOptions.None);
	}
}