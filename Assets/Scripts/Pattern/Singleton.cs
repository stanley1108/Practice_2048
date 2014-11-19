using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T:Singleton<T> {
	static private T s_instance = default(T);
	static public T Instance
	{
		get{
			if(s_instance == null)
			{
				s_instance = FindObjectOfType(typeof(T)) as T;
			}
		
			return s_instance;
		}
	}
	
	
	
	virtual protected void Awake()
	{
		s_instance = this as T;
	}
	
	public void OnApplicationQuit() {s_instance = null;}
}
