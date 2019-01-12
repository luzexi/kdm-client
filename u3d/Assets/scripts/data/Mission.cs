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
	public DateTime mDateTime;
	public string mTextureName;
	private Texture mTexture;
	public string mDesc;
	public int mCount;
	public int mFinished;

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
		dic.Add("time",mDateTime.Ticks);
		dic.Add("tex_name",mTextureName);
		dic.Add("desc",mDesc);
		dic.Add("count",mCount);
		dic.Add("finished",mFinished);
		return dic;
	}

	public void ToModel(Dictionary<string, object> _dic)
	{
		mId = Convert.ToInt32(_dic["id"]);
		mType = (MissionType)Convert.ToInt32(_dic["type"]);
		mDateTime = new DateTime(Convert.ToInt64(_dic["time"]));
		mTextureName = _dic["tex_name"].ToString();
		mDesc = _dic["desc"].ToString();
		mCount = Convert.ToInt32(_dic["count"]);
		mFinished = Convert.ToInt32(_dic["finished"]);
	}
}
