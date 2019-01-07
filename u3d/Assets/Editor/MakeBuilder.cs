using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MakeBuilder
{
	static List<string> levels = new List<string> ();

	public static void SwitchToAndroid()
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTargetGroup.Android, BuildTarget.Android);
	}

	public static void SwitchToIOS()
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTargetGroup.iOS, BuildTarget.iOS);
	}

	public static void BuildAndroid()
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTargetGroup.Android, BuildTarget.Android);
		AddScene();

		string[] args = System.Environment.GetCommandLineArgs();
		string _define = "";
		string _version = "";
		for(int i = 0 ; i<args.Length ; i++)
		{
			if(args[i] == "-codedefine" && i < args.Length -1)
			{
				_define = args[i+1];
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, _define);
			}
			else if(args[i] == "-gameversion" && i < args.Length -1)
			{
				_version = args[i+1];
				PlayerSettings.bundleVersion = _version;
				Consts.VERSION_STR = _version;
				PlayerSettings.Android.bundleVersionCode = Consts.VERSION;
			}
		}

		PlayerSettings.Android.keystoreName = Application.dataPath + "/../../makebuild/android.keystore";
		PlayerSettings.Android.keyaliasName = "android.keystore";
		PlayerSettings.Android.keystorePass = "121212";
		PlayerSettings.Android.keyaliasPass = "121212";
		PlayerSettings.applicationIdentifier = "com.sdo.qihang.glb";
        // BuildAssetsMgr.BuildAllOfPlatform(BuildTarget.Android);
        BuildPipeline.BuildPlayer (levels.ToArray(), Application.dataPath + "/../../makebuild/android/android.apk", BuildTarget.Android, BuildOptions.None);
	}

	public static void BuildIOS()
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTargetGroup.iOS, BuildTarget.iOS);

		AddScene();
		string[] args = System.Environment.GetCommandLineArgs();
		string _define = "";
		for(int i = 0 ; i<args.Length ; i++)
		{
			if(args[i] == "-codedefine" && i < args.Length -1)
			{
				_define = args[i+1];
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, _define);
			}
			// else if(args[i] == "-gameversion" && i < args.Length -1)
			// {
			// 	_version = args[i+1];
			// 	PlayerSettings.bundleVersion = _version;
			// }
		}

		PlayerSettings.applicationIdentifier = "com.sdo.qihang.glb";
        // BuildAssetsMgr.BuildAllOfPlatform(BuildTarget.iOS);
        BuildPipeline.BuildPlayer (levels.ToArray (), Application.dataPath + "/../../makebuild/ios/xcode-project/", BuildTarget.iOS, BuildOptions.None);
	}

	private static void AddScene()
	{
		string[] args = System.Environment.GetCommandLineArgs ();
		Debug.Log ("build android " + args);
		foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
		{
			if (!scene.enabled) continue;
			levels.Add (scene.path);
		}
	}
}