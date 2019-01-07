using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//table manager
public class TableManager : CSingleton<TableManager>
{
    //private TableCommonProperty mTableCommonProperty = null;
    private TableBuilding mTableBuilding = null;

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

    public TableBuilding.Data GetBuildingById (int _id)
    {
        if (mTableBuilding == null)
        {
            mTableBuilding = new TableBuilding ();
            mTableBuilding.ReadTable ();
            mTableBuilding.ParseData ();
        }
        return mTableBuilding.GetDataById (_id);
    }

    public List<TableBuilding.Data> GetBuildingAll ()
    {
        if (mTableBuilding == null)
        {
            mTableBuilding = new TableBuilding ();
            mTableBuilding.ReadTable ();
            mTableBuilding.ParseData ();

        }
        return mTableBuilding.GetAll ();
    }
}