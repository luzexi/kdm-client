using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using System;
using System.IO;


// statistics manager
public class StatisticsManager : CSingleton<StatisticsManager>
{
	public long MaxID = 1;
	List<MissionLog> mListMissionLog = new List<MissionLog>();

	public StatisticsManager()
	{
		//
	}

	public MissionLog AddLog(int _missionId)
	{
		return AddLog(_missionId, DateTime.Now);
	}

	public MissionLog AddLog(int _missionId, DateTime _time)
	{
		MissionLog log = new MissionLog();
		log.mId = MaxID++;
		log.mDateTime = _time;
		log.mMissionId = _missionId;

		mListMissionLog.Add(log);

		return log;
	}

	public void RemoveLog(MissionLog _log)
	{
		if(_log == null) return;

		int index = CQuickSort.Search<MissionLog>(mListMissionLog, _log, CompareLog);
		if(index >= 0)
		{
			mListMissionLog.RemoveAt(index);
		}
	}

	void ToModel(List<object> lst_obj)
	{
		mListMissionLog.Clear();
		for(int i = 0 ; i<lst_obj.Count ; i++)
		{
			Dictionary<string,object> dic = lst_obj[i] as Dictionary<string,object>;
			MissionLog mislog = new MissionLog();
			mislog.ToModel(dic);
			if(mislog.mId > MaxID)
			{
				MaxID = mislog.mId + 1;
			}
			mListMissionLog.Add(mislog);
		}
	}

	List<object> ToDic()
	{
		List<object> lst = new List<object>();
		for(int i = 0 ; i<mListMissionLog.Count ; i++)
		{
			Dictionary<string,object> dic = mListMissionLog[i].ToDic();
			lst.Add(dic);
		}
		return lst;
	}

	public void Save()
	{
		List<object> lst = ToDic();
		string json_str = Json.Serialize(lst);
		string path = Misc.GetPersistentDataPath() + "/statistics_data.json";
		File.WriteAllText(path, json_str);
	}

	public void Load()
	{
		string path = Misc.GetPersistentDataPath() + "/statistics_data.json";
		if(File.Exists(path))
		{
			string json_str = File.ReadAllText(path);
			//Debug.Log("json_str " + json_str);
			List<object> lst = Json.Deserialize(json_str) as List<object>;
			if(lst != null)
			{
				ToModel(lst);
			}
		}
	}

	int CompareLog(MissionLog _a, MissionLog _b)
	{
		if(_a.mId > _b.mId) return 1;
		else if(_a.mId < _b.mId) return -1;
		else return 0;
	}
}