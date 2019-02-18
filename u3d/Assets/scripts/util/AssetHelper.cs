using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class AssetHelper
{
	private const string TABLE_MISSION_PIC_PATH = "ui/texture/mission_pic/";
	private const string MISSION_PIC_PATH = "mission_pic/";

	public static Texture LoadMissionPic(string pic_name)
	{
		Texture2D tex = null;
		string path = Misc.GetPersistentDataPath() + MISSION_PIC_PATH + pic_name;
		byte[] _data = CFile.ReadBinaryFile(path);

		if(_data == null)
		{
			Debug.LogError("file is not exist");
		}
		tex = new Texture2D(Consts.PIC_WIDTH, Consts.PIC_HIGHT, TextureFormat.RGBA32, false);
		tex.LoadRawTextureData(_data);
		tex.Apply();
		return tex;
	}

	public static Texture LoadTableMissionPic(string pic_name)
	{
		Texture tex = Resources.Load(TABLE_MISSION_PIC_PATH + pic_name) as Texture;
		return tex;
	}

	public static string SaveMissionPic(Texture2D tex)
	{
		string path = "";
		return path;
	}
}
