using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
//using MsgPack;
using MiniJSON;


public class CommonHttpConnect
{
	WWW www;
	HttpRequest request;
	
	public void Initial(HttpRequest _request)
	{
		request = _request;
	}
	
	public void Release()
	{
		if (www != null)
		{
			//www.Dispose();
			www = null;
		}
		request = null;
	}
	
	public bool Connect()
	{
		if (www == null)
		{
			WWWForm form = request.GetForm();
			if (form == null)
			{
				www = new WWW(request.url);
			}
			else
			{
				www = new WWW(request.url, form);
			}

            Debug.Log("Connecting to " + request.url);
			//Debug.LogError("sealed class CommonHttpConnect--->>>public bool Connect()-->>request.serialId  -->>" + request.serialId);
			return true;
		}
		
		return false;
	}
	
	public bool IsGotResponse()
	{
		if (www != null)
		{
			return www.isDone;
		}
		
		return false;
	}
	
	// true: result success,    false: result fail
	public bool ParseResponse()
	{
		if (www.error != null)
		{
			//request.isError = true;
			NetworkManager.instance.mErrorCode = 101;
			NetworkManager.instance.mErrorText = www.url + www.error;
			Debug.Log("Network response error:"+NetworkManager.instance.mErrorText);
			return false;
		}
		else
		{
			string jsontext = null;

			if (request.mCompressRep)
			{
				// try
				// {
				// 	byte[] inBuffer = www.bytes;
				// 	int inLength = inBuffer.Length;
				// 	byte[] outarr = null;
				// 	NetworkManager.decompressFile(inBuffer, out outarr);
				// 	if (outarr != null)
				// 	{
				// 		int outLength = outarr.Length;
				// 		jsontext = Encoding.UTF8.GetString(outarr, 0, outarr.Length);
				// 		NetworkManager.instance.mErrorText = jsontext;
				// 		DebugUtils.LogNetwork("Receive "+inLength+" bytes, decompressed size is "+outLength);
				// 	}
				// 	else
				// 	{
				// 		NetworkManager.instance.mErrorText = "Failed to decompress the response."+www.url;
				// 		NetworkManager.instance.mErrorCode = 301;
				// 		DebugUtils.Error(NetworkManager.instance.mErrorText);
				// 	}
				// }
				// catch (Exception exception)
				// {
				// 	NetworkManager.instance.mErrorText = "Exception when decompress the response from "+www.url+":\n msg="+exception.Message+":\n data=";
    //                 NetworkManager.instance.mErrorText += Utils.GetString(www.bytes);
    //                 NetworkManager.instance.mErrorText += ":\n Text=" + www.text;
				// 	NetworkManager.instance.mErrorCode = 401;
				// 	DebugUtils.Error(NetworkManager.instance.mErrorText);
				// }
			}
			else
			{
				jsontext = www.text;
				NetworkManager.instance.mErrorText = jsontext;
			}
            
			if (jsontext != null && (jsontext.StartsWith("{") || (jsontext.StartsWith("["))))
			{
				//request.isDone = true;
				request.result = jsontext;
				request.callback();
			}
			else
			{
				return false;
			}
							
			Debug.Log("Response: "+"       Time :"+ Time.realtimeSinceStartup.ToString()+".\n"+ jsontext);
			
			lock (NetworkManager.instance.mHttpRequests)
			{
				NetworkManager.instance.mHttpRequests.Remove(request);
			}
		}
		
		return true;
	}
	
	public void Ignore()
	{
		//request.isDone = true;
		// the requests should be respondable for string.Empty result
		request.callback();
		
		lock (NetworkManager.instance.mHttpRequests)
		{
			if (NetworkManager.instance.mHttpRequests.Contains(request))
				NetworkManager.instance.mHttpRequests.Remove(request);
		}
	}
	
	public bool CanRetryWhenErrorOrTimeout()
	{
		if (request != null)
		{
			return request.CanRetryWhenErrorOrTimeout;
		}
		return false;
	}
	
	public bool CanIgnoreWhenErrorOrTimeout()
	{
		if (request != null)
		{
			return request.CanIgnoreWhenErrorOrTimeout;
		}
		return false;
	}
    public HttpRequest.FailOperationType FailOperation()
    {
        return request.FailOperation;
    }
    public System.Action FailCallBack()
    {
        return request.FailCallBack;
    }
    public string URL
    {
        get
        {
            return request.url;
        }
    }
}	

