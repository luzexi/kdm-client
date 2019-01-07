using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
//using MsgPack;
using MiniJSON;


public sealed class HttpRequest
{
	// callback
	public delegate void CallbackFunc(HttpRequest request); // response

    public enum FailOperationType
    {
        None,
        ErrorDialog,
    }
	
	public int serialId = 0;
	
	string mUrl;
	Dictionary<string, object> mHash;
	string mKey;
	//bool mDone = false;
	//bool mError = false;
	bool mRetryable = false;
	bool mIgnorable = false;
	public bool mCompressRep = false;
	string mResult = string.Empty;
	CallbackFunc pFunc;
    System.Action pFailFunc;
    FailOperationType mFailOperation = FailOperationType.ErrorDialog;

    public HttpRequest(string _url, Dictionary<string, object> _hash, string _key, CallbackFunc _cb, bool _compressed)
	{
		mUrl = _url;
		mHash = _hash;
		mKey = _key;
		pFunc = _cb;
		mCompressRep = _compressed;
	}
	
	public string url
	{
		get
		{
			return mUrl;
   //          if (mUrl == null || mUrl.Length == 0)
   //              return "";

   //          int index = NetworkManager.instance.mRetryTimes;
            
   //          if (index >= mUrl.Length)
   //              index = 0;
   //          if (mUrl[index] == null)
   //          {
   //              for (int i = 0; i < mUrl.Length; ++i)
   //              {
   //                  if (mUrl[i] != null)
   //                  {
   //                      index = i;
   //                      break;
   //                  }
   //              }
   //          }
			// return mUrl[index];
		}
	}
	
	public WWWForm GetForm()
	{
		if (mHash != null)
		{
			WWWForm form = new WWWForm();
			string combinedKey = string.Empty;
			List<string> list = new List<string>(mHash.Keys);
			list.Sort();
			for (int i=0; i<list.Count; i++)
			{
				string str = (string)list[i];
				form.AddField(str, (string)mHash[str]);
				combinedKey += str + "=";
				combinedKey += (string)mHash[str] + "&";
			}
			combinedKey = combinedKey.Substring(0, combinedKey.Length - 1);
			
			// string md5 = Utils.GetMd5Hash(combinedKey + mKey);
			string md5 = "md5";
			form.AddField("verify", md5);

            Debug.Log("Request :" + "         Time :" + Time.realtimeSinceStartup.ToString() + ". \n" + url + "\n[data]:" + combinedKey + "  [md5]:" + md5);
			
			return form;
		}
		return null;
	}
	
	/*public bool isDone
	{
		get
		{
			return mDone;
		}
		set
		{
			mDone = value;
		}
	}
	
	public bool isError
	{
		get
		{
			return mError;
		}
		set
		{
			mError = value;
		}
	}*/
	
	public bool CanRetryWhenErrorOrTimeout
	{
		get
		{
			return mRetryable;
		}
		set
		{
			mRetryable = value;
		}
	}
	
	// SHOULD BE respondable to string.Empty result
	public bool CanIgnoreWhenErrorOrTimeout
	{
		get
		{
			return mIgnorable;
		}
		set
		{
			mIgnorable = value;
		}
	}
	
	public string result
	{
		get
		{
			return mResult;
		}
		set
		{
			mResult = value;
		}
	}
    public System.Action FailCallBack
    {
        get
        {
            return pFailFunc;
        }
        set
        {
            pFailFunc = value;
        }
    }
    public FailOperationType FailOperation
    {
        get
        {
            return mFailOperation;
        }
        set
        {
            mFailOperation = value;
        }
    }
	
	public void callback()
	{
		if (pFunc != null)
		{
			pFunc(this);
		}
	}
	
	public Dictionary<string, object> GetHashData()
	{
		return mHash;
	}
}

