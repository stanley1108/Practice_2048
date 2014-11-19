using UnityEngine;
using System.Collections;

static public class InputManager {
#if UNITY_EDITOR
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
#else

#endif

}
