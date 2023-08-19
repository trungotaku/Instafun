using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalData : MonoBehaviour
{
	private static Dictionary<string, GameObject> cache = new Dictionary<string, GameObject>();

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		
		if (cache.ContainsKey(name))
		{
			Debug.Log("Object [" + name + "] exists. Destroy new one");
			Object.DestroyImmediate(this.gameObject);
		}
		else
			cache[name] = gameObject;
	}
}