using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//table mission
public class TablePlayerLevel : TableData
{
    // file path
    public readonly string sFilePath        = "tPlayerLevel";
    
    // data
    public class Data
    {
        public int mLevel;
        public string mAchievement;
        public string mExp;
    }
    
    public Dictionary<int, Data> mData;
    public List<Data> mList;

    protected override string GetPath()
    {
        return sFilePath;
    }
    
    protected override void _ParseData()
    {
        mData = new Dictionary<int, Data>(mTableData._nRows);
        mList = new List<Data>(mTableData._nRows);

        for (int i=0; i<mTableData._nRows; i++)
        {
            Data data = new Data();
            data.mLevel = mTableData.GetInt(i, DataDefine.Mission_MissionSelect_id);
            data.mAchievement = mTableData.GetStr(i, DataDefine.Mission_MissionSelect_name);
            data.mExp = mTableData.GetInt(i, DataDefine.Mission_MissionSelect_desc);
            
            mData.Add(data.mLevel, data);
            mList.Add(data);
        }
        
        mTableData = null;
    }

    public Data GetDataByLevel(int _level)
    {
        Data data = null;
        if (mData.TryGetValue(_level, out data))
        {
            return data;
        }
        return null;
    }

    public List<Data> GetAll()
    {
        return mList;
    }
}