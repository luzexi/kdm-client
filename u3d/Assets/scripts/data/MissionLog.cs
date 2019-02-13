using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MissionLog
{
	public long mId;
	public int mMissionId;
	public DateTime mDateTime;

	public Dictionary<string,object> ToDic()
	{
		Dictionary<string,object> dic = new Dictionary<string,object>();
		dic.Add("id",mId);
		dic.Add("missionId",mMissionId);
		dic.Add("time",mDateTime.Ticks);
		return dic;
	}

	public void ToModel(Dictionary<string, object> _dic)
	{
		mId = Convert.ToInt64(_dic["id"]);
		mMissionId = Convert.ToInt32(_dic["missionId"]);
		mDateTime = new DateTime(Convert.ToInt64(_dic["time"]));
	}
}

