using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AccountRequest : NetRequest
{
    public AccountRequest(CallbackFunc _func)
        : base(_func)
    {
        cmdId = 1001;
    }

    // Request data
    public string deviceId;
    public string platOS;
    public string channel = string.Empty;
    public string channel_id = string.Empty;

    // Response data
    public string uid;
    public string token;
    public int error;

    // Make Request Param
    protected override object MakeRequestParam()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();

        param.Add("deviceId", deviceId);
        param.Add("platOS", platOS);
        if(channel != string.Empty)
        {
            param.Add("channel", channel);
            param.Add("channel_id", channel_id);
        }

        return param;
    }

    protected override bool ParseResponseData(object data)
    {
        Dictionary<string,object> _data = data as Dictionary<string,object>;

        if(_data != null)
        {
            if(_data.ContainsKey("uid"))
            {
                uid = _data["uid"].ToString();
            }
            if(_data.ContainsKey("token"))
            {
                token = _data["token"].ToString();
            }
            if(_data.ContainsKey("error"))
            {
                error = int.Parse(_data["error"].ToString());
            }
        }

        return true;
    }
}