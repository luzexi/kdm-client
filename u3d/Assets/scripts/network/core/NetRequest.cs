using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NetRequest
{
    public int serialId = 0;
    public int cmdId = 0;

    public long resultFlag;

    public bool isDone = false;
    public bool canCombined = true;

    // callback
    public delegate void CallbackFunc(NetRequest request);
    private CallbackFunc pFunc;

    public NetRequest(CallbackFunc _func)
    {
        pFunc = _func;
    }

    public virtual Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();

        dic.Add("o", serialId);
        dic.Add("c", cmdId.ToString());

        object param = MakeRequestParam();
        if (param != null)
        {
            dic.Add("p", param);
        }
        return dic;
    }

    protected virtual object MakeRequestParam()
    {
        return new Dictionary<string, object>();
    }

    public virtual void Respond(Dictionary<string, object> result)
    {
        // if (result.ContainsKey("r"))
        //     resultFlag = (long)result["r"];
        // else
        //     resultFlag = 1;

        if (resultFlag == 0)
        {
            object data = result["d"];

            if (ParseResponseData(data))
            {
                SendCallback();
                isDone = true;
            }
        }
        else
        {
            isDone = ParseErrorCode();
            if (isDone)
                SendCallback();
        }
    }

    // Parse Error code
    protected virtual bool ParseErrorCode()
    {
        return false;
    }

    protected virtual bool ParseResponseData(object data)
    {
        return true;
    }

    protected virtual void SendCallback()
    {
        if (pFunc != null)
        {
            pFunc(this);
        }
    }
    public static T ParseDictionary<T>(Dictionary<string, object> _data, string _key) where T : class
    {
        object obj;
        if (_data.TryGetValue(_key, out obj))
        {
            return obj as T;
        }
        return default(T);
    }
}
