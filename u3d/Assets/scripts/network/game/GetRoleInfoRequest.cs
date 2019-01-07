using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GetRoleInfoRequest : NetRequest
{
    public GetRoleInfoRequest(CallbackFunc _func)
        : base(_func)
    {
        cmdId = 1101;
    }

    // Request data

    // Response data
    public string name;
    public int gold;
    public int food;
    public int seaman;
    public int workman;
    public int diamond;
    public int Level;
    public int Exp;

    public int mWindyDirection;
    public int mWindyLevel;
    public float mWindyRemainTime;

    public int PopularSeaPointId;
    public int PopularProduceId;
    public int PopularProduceRate;
    public float PopularRemainTime;

    // Make Request Param
    protected override object MakeRequestParam()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();

        return param;
    }

    protected override bool ParseResponseData(object data)
    {
        Dictionary<string,object> _data = data as Dictionary<string,object>;

        if(_data != null)
        {
            if(_data.ContainsKey("name"))
            {
                name = _data["name"].ToString();
            }
            if(_data.ContainsKey("gold"))
            {
                gold = int.Parse(_data["gold"].ToString());
            }
            if(_data.ContainsKey("food"))
            {
                food = int.Parse(_data["food"].ToString());
            }
            if(_data.ContainsKey("seaman"))
            {
                seaman = int.Parse(_data["seaman"].ToString());
            }
            if(_data.ContainsKey("workman"))
            {
                workman = int.Parse(_data["workman"].ToString());
            }
            if(_data.ContainsKey("diamond"))
            {
                diamond = int.Parse(_data["diamond"].ToString());
            }
            if(_data.ContainsKey("Level"))
            {
                Level = int.Parse(_data["Level"].ToString());
            }
            if (_data.ContainsKey("Exp"))
            {
                Exp = int.Parse(_data["Exp"].ToString());
            }
            if (_data.ContainsKey("Windy_dir"))
            {
                mWindyDirection = int.Parse(_data["Windy_dir"].ToString());
            }
            if (_data.ContainsKey("Windy_level"))
            {
                mWindyLevel = int.Parse(_data["Windy_level"].ToString());
            }
            if(_data.ContainsKey("Windy_remain_time"))
            {
                mWindyRemainTime = float.Parse(_data["Windy_remain_time"].ToString());
            }
            if(_data.ContainsKey("PopularSeaPointId"))
            {
                PopularSeaPointId = int.Parse(_data["PopularSeaPointId"].ToString());
            }
            if(_data.ContainsKey("PopularProduceId"))
            {
                PopularProduceId = int.Parse(_data["PopularProduceId"].ToString());
            }
            if(_data.ContainsKey("PopularProduceRate"))
            {
                PopularProduceRate = int.Parse(_data["PopularProduceRate"].ToString());
            }
            if(_data.ContainsKey("PopularRemainTime"))
            {
                PopularRemainTime = float.Parse(_data["PopularRemainTime"].ToString());
            }
        }

        return true;
    }
}