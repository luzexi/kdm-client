using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// mission manager
public class MissionManager : CSingleton<MissionManager>
{
	List<Mission> mListMission = new List<Mission>();
	Dictionary<int,Mission> mDicMission = new Dictionary<int, Mission>();

	public void AddMission(Mission _mis)
	{
		mListMission.Add(_mis);
		mDicMission.Add(_mis.mId, _mis);
	}

	public void RemoveMission(Mission _mis)
	{
		mListMission.Remove(_mis);
		mDicMission.Remove(_mis.mId);
	}

	public Mission GetMission(int _id)
	{
		Mission mis = null;
		if(mDicMission.TryGetValue(_id, out mis))
		{
			return mis;
		}
		return null;
	}

	public List<Mission> GetDailyMission()
	{
		List<Mission> lst = new List<Mission>();
		for(int i = 0; i<mListMission.Count ; i++)
		{
			if(mListMission[i].mType == MissionType.Daily)
			{
				lst.Add(mListMission[i]);
			}
		}
		return lst;
	}
}