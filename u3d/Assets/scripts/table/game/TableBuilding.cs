using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//table building
public class TableBuilding : TableData
{
    // file path
    public readonly string sFilePath        = "tBuilding";
    
    // data
    public class Data
    {
        public int mId;
        public string mName;
        public string mDesc;
        public int mBuildingType;
        public int mCategoryType;
        public string mIcon;
        public int[] mSize = new int[2];
        public string mIdleAction;
        public string mBuildingAction;
        public string mUpgradeAction;
        public string mDestroyAction;
        public string mExpType;
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

        // for (int i=0; i<mTableData._nRows; i++)
        // {
        //     // Debug.LogError("in reading");
        //     Data data = new Data();
        //     data.mId = mTableData.GetInt(i, DataDefine.Building_Building_id);
        //     data.mName = mTableData.GetStr(i, DataDefine.Building_Building_name);
        //     data.mDesc = mTableData.GetStr(i, DataDefine.Building_Building_desc);
        //     data.mBuildingType = mTableData.GetInt(i, DataDefine.Building_Building_building_type);
        //     data.mCategoryType = mTableData.GetInt(i, DataDefine.Building_Building_categoy_type);
        //     data.mIcon = mTableData.GetStr(i, DataDefine.Building_Building_icon);

        //     string[] str_ary = mTableData.GetStr(i, DataDefine.Building_Building_size).Split('|');
        //     data.mSize[0] = int.Parse(str_ary[0])*10;
        //     data.mSize[1] = int.Parse(str_ary[1])*10;

        //     data.mIdleAction = mTableData.GetStr(i, DataDefine.Building_Building_idle_action);
        //     data.mBuildingAction = mTableData.GetStr(i, DataDefine.Building_Building_building_action);
        //     data.mUpgradeAction = mTableData.GetStr(i, DataDefine.Building_Building_upgrade_ation);
        //     data.mDestroyAction = mTableData.GetStr(i, DataDefine.Building_Building_destroy_action);

        //     data.mExpType = mTableData.GetStr(i, DataDefine.Building_Building_exp_type);
            
            
        //     mData.Add(data.mId, data);
        //     mList.Add(data);
        // }
        
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

    public List<Data> GetDataByCategoryType(int _iType)
    {
        List <Data> dataList = new List<Data>();
        for (int i = 0; i< mList.Count; i++)
        {
            if (mList[i].mCategoryType == _iType)
            {
                dataList.Add(mList[i]);
            }
        }

        return dataList;
    }

    public List<Data> GetDataByTwoType(int _iCategoryType, int _iBuildingType)
    {
        List<Data> dataList = new List<Data>();
        for (int i = 0; i < mList.Count; i++)
        {
            if (mList[i].mCategoryType == _iCategoryType && mList[i].mBuildingType == _iBuildingType)
            {
                dataList.Add(mList[i]);
            }
        }

        return dataList;
    }
}