using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class PlayerInfo : CSingleton<PlayerInfo>
{
	private string mName = "Test";
	public string Name
	{
		get
		{
			return mName;
		}
	}

	private string mSign = "empty";
	public string Sign
	{
		get
		{
			return mSign;
		}
	}

	private int mLevel = 1;
	public int Level
	{
		get
		{
			return mLevel;
		}
	}

	private int mExp;
	public int Exp
	{
		get
		{
			return mExp;
		}
	}

	private int mGold = 0;	//gold
	public int Gold
	{
		get{return mGold;}
	}

	public TablePlayerLevel.Data LevelTable
	{
		get
		{
			TablePlayerLevel.Data level_data = TableManager.instance.GetPlayerLevelByLevel(mLevel);
			return level_data;
		}
	}

	public PlayerInfo()
	{
		//
	}

	public void SetName(string _name)
	{
		mName = _name;
		Save();
	}

	public void AddGold(int _gold)
	{
		//MessageBox.instance.OpenMessage(MessageBox.TYPE.RollMessage,"","Add Gold " + _gold);

		mGold += _gold;
		Save();
	}

	public void DecGold(int _gold)
	{
		//MessageBox.instance.OpenMessage(MessageBox.TYPE.RollMessage,"","Decrease Gold " + _gold);

		mGold -= _gold;
		Save();
	}

    public void AddExp(int _exp)
    {
        mExp += _exp;

        TablePlayerLevel.Data tableLevelData = TableManager.instance.GetPlayerLevelByLevel(mLevel + 1);

        if(tableLevelData == null)
            return;

        if (mExp >= tableLevelData.mExp)
            mLevel++;
        else
            return;

        //MessageBox.instance.OpenMessage(MessageBox.TYPE.RollMessage, "", "Level UP!! Current Level:" + mLevel);

        Save();
    }

	public void SetGold(int _gold)
	{
		mGold = _gold;
	}

	void ToModel(Dictionary<string,object> dic)
	{
		if(dic == null) return;
		mName = dic.ContainsKey("name")?dic["name"].ToString():"Test";
		mSign = dic.ContainsKey("sign")?dic["sign"].ToString():"empty";
		mLevel = dic.ContainsKey("level")?int.Parse(dic["level"].ToString()):1;
		mExp = dic.ContainsKey("exp")?int.Parse(dic["exp"].ToString()):0;
		mGold = dic.ContainsKey("gold")?int.Parse(dic["gold"].ToString()):0;
	}

	Dictionary<string, object> ToDic()
	{
		Dictionary<string, object> dic = new Dictionary<string, object>();
		dic.Add("name", mName);
		dic.Add("level", mLevel);
		dic.Add("sign", mSign);
		dic.Add("exp", mExp);
		dic.Add("gold", mGold);

		return dic;
	}

	public void Load()
	{
		string path = Misc.GetPersistentDataPath() + "/player_data.json";
		if(File.Exists(path))
		{
			string json_str = File.ReadAllText(path);
			//Debug.Log("json_str " + json_str);
			Dictionary<string, object> obj = Json.Deserialize(json_str) as Dictionary<string, object>;
			if(obj != null)
			{
				ToModel(obj);
			}
		}
	}

	public void Save()
	{
		object obj = ToDic();
		string json_str = Json.Serialize(obj);
		string path = Misc.GetPersistentDataPath() + "/player_data.json";
		File.WriteAllText(path, json_str);
	}
}
