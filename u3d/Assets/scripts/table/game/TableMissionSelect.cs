using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//table mission
public class TableMissionSelect : TableData
{
    // file path
    public readonly string sFilePath        = "tMissionSelect";
    
    // data
    public class Data
    {
        public int mId;
        public string mName;
        public string mDesc;
        public string mPic;
        public int mMissionType;
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
            // Debug.LogError("in reading");
            Data data = new Data();
            data.mId = mTableData.GetInt(i, DataDefine.Mission_MissionSelect_id);
            data.mName = mTableData.GetStr(i, DataDefine.Mission_MissionSelect_name);
            data.mDesc = mTableData.GetStr(i, DataDefine.Mission_MissionSelect_desc);
            data.mPic = mTableData.GetStr(i, DataDefine.Mission_MissionSelect_pic);
            data.mMissionType = mTableData.GetInt(i, DataDefine.Mission_MissionSelect_mission_type);
            
            mData.Add(data.mId, data);
            mList.Add(data);
        }
        
        mTableData = null;
    }

    public Data GetDataById(int _id)
    {
        Data data = null;
        if (mData.TryGetValue(_id, out data))
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