using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObject : MonoBehaviour
{
	public GameObject UIRoot;
	public GameObject WorldRoot;

	private static GlobalObject sInstance;
	public static GlobalObject instance
	{
		get
		{
			return sInstance;
		}
	}

	void Awake()
	{
		sInstance = this;
		DontDestroyOnLoad(this.gameObject);
		DontDestroyOnLoad(UIRoot);
		NetworkManager.instance.Start();
	}

	public void InitGameScene()
	{
		WorldRoot = GameObject.Find("world");
		DontDestroyOnLoad(WorldRoot);
	}

	// Use this for initialization
	void Start ()
	{
		//
	}
	
	// Update is called once per frame
	void Update ()
	{
		NetworkManager.instance.Update();
	}
}
