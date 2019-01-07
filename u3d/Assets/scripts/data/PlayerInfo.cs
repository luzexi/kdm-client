using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : CSingleton<PlayerInfo>
{
	private string mName;
	public string Name
	{
		get
		{
			return mName;
		}
	}

	public int mLevel;

	public int mExp;

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
		MessageBox.instance.OpenMessage(MessageBox.TYPE.RollMessage,"","Add Gold " + _gold);

		mGold += _gold;
	}

	public void DecGold(int _gold)
	{
		MessageBox.instance.OpenMessage(MessageBox.TYPE.RollMessage,"","Decrease Gold " + _gold);

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

        MessageBox.instance.OpenMessage(MessageBox.TYPE.RollMessage, "", "Level UP!! Current Level:" + mLevel);
    }

	public void SetGold(int _gold)
	{
		mGold = _gold;
	}
}
