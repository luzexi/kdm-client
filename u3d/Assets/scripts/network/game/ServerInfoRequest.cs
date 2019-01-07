using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ServerInfoRequest : NetRequest
{
    public ServerInfoRequest(CallbackFunc _func)
        : base(_func)
    {
        cmdId = 1000;
    }

    // Request data
    public string deviceId;
    public string platOS;
    public string channel = string.Empty;
    public string channel_id = string.Empty;
    public Action _call_back;

    // Response data
    public int version;
    public string ab_url;
    public string package_url;

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
            if(_data.ContainsKey("version"))
            {
                version = int.Parse(_data["version"].ToString());
            }
            if(_data.ContainsKey("ab_url"))
            {
                ab_url = _data["ab_url"].ToString();
            }
            if(_data.ContainsKey("package_url"))
            {
                package_url = _data["package_url"].ToString();
            }
        }

        return true;
    }
}