using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;


// mission manager
public class MissionManager : CSingleton<MissionManager>
{
	public int MaxID = 1;
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

	void ToModel(List<object> lst_obj)
	{
		mListMission.Clear();
		for(int i = 0 ; i<lst_obj.Count ; i++)
		{
			Dictionary<string,object> dic = lst_obj[i] as Dictionary<string,object>;
			Mission mis = new Mission();
			mis.ToModel(dic);
			if(mis.mId > MaxID)
			{
				MaxID = mis.mId + 1;
			}
			mListMission.Add(mis);
		}
	}

	List<object> ToDic()
	{
		List<object> lst = new List<object>();
		for(int i = 0 ; i<mListMission.Count ; i++)
		{
			Dictionary<string,object> dic = mListMission[i].ToDic();
			lst.Add(dic);
		}
		return lst;
	}

	public void Save()
	{
		List<object> lst = ToDic();
		string json_str = Json.Serialize(lst);
	}

	public void Load()
	{
		string json_str = "";
		List<object> lst = Json.Deserialize(json_str) as List<object>;
		if(lst != null)
		{
			ToModel(lst);
		}
	}
}