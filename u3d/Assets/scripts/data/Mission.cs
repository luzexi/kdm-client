using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MissionType
{
	None,
	Daily = 1,
	Week,
	Month
}


public class Mission
{
	public int mId;
	public MissionType mType;
	public int mDateTime;
	public string mTextureName;
	private Texture mTexture;
	public string mDesc;
	public int mCount;

	public Texture texture
	{
		get
		{
			if(mTexture == null)
			{
				// todo load texture
				return null;
			}
			return mTexture;
		}
	}

	public Dictionary<string,object> ToDic()
	{
		Dictionary<string,object> dic = new Dictionary<string,object>();
		dic.Add("id",mId);
		dic.Add("type",(int)mType);
		dic.Add("time",mDateTime);
		dic.Add("tex_name",mTextureName);
		dic.Add("desc",mDesc);
		dic.Add("count",mCount);
		return dic;
	}

	public void ToModel(Dictionary<string, object> _dic)
	{
		mId = Convert.ToInt32(_dic["id"]);
		mType = (MissionType)Convert.ToInt32(_dic["type"]);
		mDateTime = Convert.ToInt32(_dic["time"]);
		mTextureName = _dic["tex_name"].ToString();
		mDesc = _dic["desc"].ToString();
		mCount = Convert.ToInt32(_dic["count"]);
	}
}
