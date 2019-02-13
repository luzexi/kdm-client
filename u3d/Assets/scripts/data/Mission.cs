using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MissionType
{
	None,
	Daily = 1,
	Week,
	Month,
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


	/// temp var
	public MissionLog mLog = null;
	///

	public Texture texture
	{
		get
		{
			if(mTexture == null)
			{
				// todo load texture
				mTexture = AssetHelper.LoadTableMissionPic(mTextureName);
				if(mTexture == null)
				{
					mTexture = AssetHelper.LoadMissionPic(mTextureName);
				}
				return mTexture;
			}
			return mTexture;
		}
	}

	public bool IsOld()
	{
		if(mDateTime == null) return false;
		int day1 = TimeConvert.GetDays(mDateTime);
		int day2 = TimeConvert.NowDay();
		if(day2 != day1) return true;
		return false;
	}

	public bool IsFinished()
	{
		int day1 = TimeConvert.NowDay();
		if(day1 != mFinished) return false;
		return true;
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
