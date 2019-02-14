using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : CSingleton<PlayerInfo>
{
	private string mName = string.Empty;
	public string Name
	{
		get
		{
			return mName;
		}
	}

	private int mLevel;
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

	public PlayerInfo()
	{
		//
	}

	public void SetName(string _name)
	{
		mName = _name;
	}

	public void AddGold(int _gold)
	{
		//MessageBox.instance.OpenMessage(MessageBox.TYPE.RollMessage,"","Add Gold " + _gold);

		mGold += _gold;
	}

	public void DecGold(int _gold)
	{
		//MessageBox.instance.OpenMessage(MessageBox.TYPE.RollMessage,"","Decrease Gold " + _gold);

		mGold -= _gold;
	}

    public void AddExp(int _exp)
    {
        mExp += _exp;

        // TableLevel.Data tableLevelData = TableManager.instance.GetLevelInfoByID(mLevel + 1);

        // if(tableLevelData == null)
        //     return;

        // if (mExp >= tableLevelData.mExp)
        //     mLevel++;
        // else
        //     return;

        //MessageBox.instance.OpenMessage(MessageBox.TYPE.RollMessage, "", "Level UP!! Current Level:" + mLevel);
    }

	public void SetGold(int _gold)
	{
		mGold = _gold;
	}

	void ToModel(Dictionary<string,object> dic)
	{
		if(dic == null) return;
		mName = dic.Containskey("name")?dic["name"].ToString():"empty";
		mLevel = dic.Containskey("level")?Convert.ToInt32(dic["level"]):1;
		mExp = dic.Containskey("exp")?Convert.ToInt32(dic["exp"]):0;
		mGold = dic.Containskey("gold")?Convert.ToInt32(dic["gold"]):0;
	}

	Dictionary<string, object> ToDic()
	{
		Dictionary<string, object> dic = new Dictionary<string, object>();
		dic.Add("name", mName);
		dic.Add("level", mLevel);
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
