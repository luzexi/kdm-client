using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//table manager
public class TableManager : CSingleton<TableManager>
{
    //private TableCommonProperty mTableCommonProperty = null;
    private TableMissionSelect mTableMissionSelect = null;

    public void InitCommonProperty()
    {
        // if (mTableCommonProperty == null)
        // {
        //     // Debug.LogError("mTableCommonProperty ");
        //     mTableCommonProperty = new TableCommonProperty ();
        //     mTableCommonProperty.ReadTable ();
        //     mTableCommonProperty.ParseData ();
        // }
    }

    public TableMissionSelect.Data GetMissionSelectById(int _id)
    {
        if (mTableMissionSelect == null)
        {
            mTableMissionSelect = new TableMissionSelect();
            mTableMissionSelect.ReadTable();
            mTableMissionSelect.ParseData();
        }
        return mTableMissionSelect.GetDataById(_id);
    }

    public List<TableMissionSelect.Data> GetMissionSelectAll ()
    {
        if (mTableMissionSelect == null)
        {
            mTableMissionSelect = new TableMissionSelect();
            mTableMissionSelect.ReadTable();
            mTableMissionSelect.ParseData();

        }
        return mTableMissionSelect.GetAll();
    }
}