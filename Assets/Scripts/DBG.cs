using UnityEngine;
using System.Collections;

//for remove debug messgae easily
static public class DBG {
#if MYDEBUG
	static public void Log(string msg)
	{
		Debug.Log(msg);
	}

	static public void LogError(string msg)
	{
		Debug.LogError(msg);
	}
#else
	static public void Log(string msg)
	{

	}
	
	static public void LogError(string msg)
	{
	
	}
#endif
}
