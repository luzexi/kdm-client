using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MissionLog
{
	public long mId;
	public int mMissionId;
	public string mMissionName;
	public DateTime mDateTime;

	public Dictionary<string,object> ToDic()
	{
		Dictionary<string,object> dic = new Dictionary<string,object>();
		dic.Add("id",mId);
		dic.Add("missionId",mMissionId);
		dic.Add("name",mMissionName);
		dic.Add("time",mDateTime.Ticks);
		return dic;
	}

	public void ToModel(Dictionary<string, object> _dic)
	{
		mId = Convert.ToInt64(_dic["id"]);
		mMissionId = Convert.ToInt32(_dic["missionId"]);
		mMissionName = _dic["name"].ToString();
		mDateTime = new DateTime(Convert.ToInt64(_dic["time"]));
	}
}

public class MissionDayLog
{
	public int mCount;
	public int mTotalExp;
	public DateTime mDateTime;

	public Dictionary<string,object> ToDic()
	{
		Dictionary<string,object> dic = new Dictionary<string,object>();
		dic.Add("count",mCount);
		dic.Add("time",mDateTime.Ticks);
		return dic;
	}

	public void ToModel(Dictionary<string, object> _dic)
	{
		mCount = Convert.ToInt32(_dic["count"]);
		mDateTime = new DateTime(Convert.ToInt64(_dic["time"]));
	}
}


public class MissionNameLog
{
	public string mName;
	public int mCount;
	public DateTime mDateTime;

	public Dictionary<string,object> ToDic()
	{
		Dictionary<string,object> dic = new Dictionary<string,object>();
		dic.Add("name",mName);
		dic.Add("count",mCount);
		dic.Add("time",mDateTime.Ticks);
		return dic;
	}

	public void ToModel(Dictionary<string, object> _dic)
	{
		mName = _dic["name"].ToString();
		mCount = Convert.ToInt32(_dic["count"]);
		mDateTime = new DateTime(Convert.ToInt64(_dic["time"]));
	}
}

