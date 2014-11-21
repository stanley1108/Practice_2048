using UnityEngine;
using System.Collections;

static public class InputManager {
#if UNITY_EDITOR || UNITY_STANDALONE

	static public bool IsSwipeRight()
	{
		return Input.GetKeyDown(KeyCode.RightArrow);
	}

	static public bool IsSwipeLeft()
	{
		return Input.GetKeyDown(KeyCode.LeftArrow);
	}

	static public bool IsSwipeUp()
	{
		return Input.GetKeyDown(KeyCode.UpArrow);
	}

	static public bool IsSwipeDown()
	{
		return Input.GetKeyDown(KeyCode.DownArrow);
	}

	static public bool IsPressAnyKey()
	{
		return Input.anyKeyDown;
	}
#else

#endif

}
