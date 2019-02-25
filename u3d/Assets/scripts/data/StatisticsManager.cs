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
	List<MissionDayLog> mListMissionDayLog = new List<MissionDayLog>();
	Dictionary<string, MissionNameLog> mDicMissionNameLog = new Dictionary<string, MissionNameLog>();

	public StatisticsManager()
	{
		//
	}

	public List<MissionLog> GetAllMissionLog()
	{
		return new List<MissionLog>(mListMissionLog);
	}

	public List<MissionDayLog> GetAllMissionDayLog()
	{
		return new List<MissionDayLog>(mListMissionDayLog);
	}

	public MissionLog AddLog(int _missionId, string _name)
	{
		return AddLog(_missionId, _name, DateTime.Now);
	}

	public MissionLog AddLog(int _missionId, string _name, DateTime _time)
	{
		MissionLog log = new MissionLog();
		log.mId = MaxID++;
		log.mMissionName = _name;
		log.mDateTime = _time;
		log.mMissionId = _missionId;

		mListMissionLog.Add(log);

		MissionDayLog daylog = null;
		if(mListMissionDayLog.Count > 0)
		{
			int _day_time = TimeConvert.GetDays(mListMissionDayLog[mListMissionDayLog.Count - 1].mDateTime);
			if( _day_time == TimeConvert.NowDay())
			{
				daylog = mListMissionDayLog[mListMissionDayLog.Count - 1];
			}
		}
		if(daylog == null)
		{
			daylog = new MissionDayLog();
			daylog.mCount = 0;
			daylog.mDateTime = TimeConvert.GetNow();
			mListMissionDayLog.Add(daylog);
		}
		daylog.mCount++;
		daylog.mTotalExp = PlayerInfo.Exp;

		MissionNameLog namelog = null;
		if(!mDicMissionNameLog.TryGetValue(_name, out namelog))
		{
			namelog = new MissionNameLog();
			namelog.mName = _name;
			namelog.mCount = 0;
			namelog.mDateTime = TimeConvert.GetNow();
			mDicMissionNameLog.Add(_name, namelog);
		}
		namelog.mCount++;

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

	void ToModel(Dictionary<string, object> dic_obj)
	{
		mListMissionLog.Clear();
		mListMissionDayLog.Clear();
		mDicMissionNameLog.Clear();

		if(dic_obj.ContainsKey("log"))
		{
			List<object> lst_obj = dic_obj["log"] as List<object>;
			if(lst_obj != null)
			{
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
		}

		if(dic_obj.ContainsKey("daylog"))
		{
			List<object> lst_obj = dic_obj["daylog"] as List<object>;
			if(lst_obj != null)
			{
				for(int i = 0 ; i<lst_obj.Count ; i++)
				{
					Dictionary<string,object> dic = lst_obj[i] as Dictionary<string,object>;
					MissionDayLog mislog = new MissionDayLog();
					mislog.ToModel(dic);
					mListMissionDayLog.Add(mislog);
				}
			}
		}

		if(dic_obj.ContainsKey("namelog"))
		{
			List<object> lst_obj = dic_obj["namelog"] as List<object>;
			if(lst_obj != null)
			{
				for(int i = 0 ; i<lst_obj.Count ; i++)
				{
					Dictionary<string,object> dic = lst_obj[i] as Dictionary<string,object>;
					MissionNameLog mislog = new MissionNameLog();
					mislog.ToModel(dic);
					mDicMissionNameLog.Add(mislog.mName, mislog);
				}
			}
		}
	}

	Dictionary<string, object> ToDic()
	{
		Dictionary<string, object> dic = new Dictionary<string, object>();

		List<object> lst = new List<object>();
		for(int i = 0 ; i<mListMissionLog.Count ; i++)
		{
			Dictionary<string,object> tmp_dic = mListMissionLog[i].ToDic();
			lst.Add(tmp_dic);
		}
		dic.Add("log", lst);

		lst = new List<object>();
		for(int i = 0 ; i<mListMissionDayLog.Count ; i++)
		{
			Dictionary<string,object> tmp_dic = mListMissionDayLog[i].ToDic();
			lst.Add(tmp_dic);
		}
		dic.Add("daylog", lst);

		lst = new List<object>();
		List<MissionNameLog> lst_name_log = new List<MissionNameLog>(mDicMissionNameLog.Values);
		for(int i = 0 ; i<lst_name_log.Count ; i++)
		{
			Dictionary<string,object> tmp_dic = lst_name_log[i].ToDic();
			lst.Add(tmp_dic);
		}
		dic.Add("namelog", lst);

		return dic;
	}

	public void Save()
	{
		Dictionary<string, object> lst = ToDic();
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
			Dictionary<string, object> lst = Json.Deserialize(json_str) as Dictionary<string, object>;
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