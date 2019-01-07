using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
//using MsgPack;
using MiniJSON;

public class CombinedHttpConnect
{
	WWW www;

	// combined requests for current HttpRequest
	Dictionary<int, NetRequest> requests = new Dictionary<int, NetRequest>();
	List<NetRequest> list_requests = new List<NetRequest>();
	
	// request data
	string requestStringData;
	Dictionary<string, object> requestData;
	WWWForm requestForm;

	int mResult_error_code;
	string mError_message;
	
	public void Initial(List<NetRequest> _requests, int maxSerialId)
	{
		list_requests.Clear();
		requests.Clear();
		
		if (!_requests[0].canCombined)
		{
			requests.Add(_requests[0].serialId, _requests[0]);
			list_requests.Add(_requests[0]);
		}
		else
		{
			bool existDoneRequest = false;
			for (int i = 0; i < _requests.Count; i++)
			{
				if (_requests[i].canCombined && _requests[i].serialId < maxSerialId)
				{
					// prevent send the done request for bug
					if (_requests[i].isDone)
					{
						existDoneRequest = true;
					}
					else
					{
						requests.Add(_requests[i].serialId, _requests[i]);
						list_requests.Add(_requests[i]);
					}
				}
				else
				{
					break;
				}
			}
			
			// prevent send the Done request for bug, clear the Done requests from stack
			if (existDoneRequest)
			{
				Debug.LogError("Error: Send same request");
				for (int i = 0; i <_requests.Count; )
				{
					if (_requests[i].isDone)
					{
						_requests.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
		}
		
		
		ContructRequestData();
	}
	
	void ContructRequestData()
	{
		// construct multiple requests
		if (list_requests.Count > 0)
		{
			requestData = new Dictionary<string, object>();
            if(AccountInfo.instance.Uid != null)
            	requestData.Add("u", AccountInfo.instance.Uid); //uid
			requestData.Add("t", list_requests.Count); //request count
			if(AccountInfo.instance.Token != null)
            	requestData.Add("s", AccountInfo.instance.Token); // sessionid
            
// #if false
// 			string combinedKey = "o=" + list_requests[0].serialId.ToString();
// 			//combinedKey += "&s=" + AccountManager.instance.GetAccount().mSessionId;
//             combinedKey += "&s=" + AccountService.Instance.SessionId;
// 			combinedKey += "&t=" + list_requests.Count.ToString();
// 			//combinedKey += "&u=" + AccountManager.instance.GetAccount().mUId;
//             combinedKey += "&u=" + AccountService.Instance.UId;
// 			string md5 = Utils.GetMd5Hash(combinedKey + NetworkManager.sKey);
// 			requestData.Add("k", md5); // key
// #endif
			
			List<object> data = new List<object>();
			for (int i=0; i<list_requests.Count; i++)
			{
				data.Add(list_requests[i].ToDictionary());
			}
			requestData.Add("d", data);
			
			{
				requestStringData = Json.Serialize(requestData);
				Debug.Log("Request :"+"         Time: "+ Time.realtimeSinceStartup.ToString() + "\n" + requestStringData );
				requestForm = new WWWForm();

				if ((NetworkDefine.sCompressedRequest & NetworkDefine.COMPRESSFLAG_GAME) != 0)
				{						
// 					byte[] inBuffer = null;					
// 					inBuffer = Encoding.UTF8.GetBytes(requestStringData);
//                     using (MemoryStream output = new MemoryStream())
//                     {
//                         NetworkManager.compressFileOld(inBuffer, output);
//                         byte[] outBuffer = output.ToArray();
//                         //outBuffer = NetworkManager.encryptFile(outBuffer);
//                         //int outLength = outBuffer.Length;						

//                         string str = Convert.ToBase64String(SC.E(outBuffer));
//                         requestForm.AddField("data", str);
//                         requestForm.AddField("k", Utils.GetMd5Hash(str + NetworkManager.sKey));
// #if !UNITY_METRO
// 						//No need to close on winmetro
//                         output.Close();
// #endif
//                         if (str != null)
//                         {
//                             DebugUtils.LogNetwork("Send " + requestStringData.Length + " bytes, base64 size is " + str.Length + ", \n" + "str: " + str);
//                         }
//                         else
//                         {
//                             NetworkManager.instance.mErrorText = "Failed to compress the request.";
//                             NetworkManager.instance.mErrorCode = 302;
//                             DebugUtils.Error(NetworkManager.instance.mErrorText);
//                         }
//                     }
					
				}
				else
				{
					requestForm.AddField("d", requestStringData);
					requestForm.AddField("k", "md5key");
                    // requestForm.AddField("k", Utils.GetMd5Hash(requestStringData + NetworkManager.sKey));
				}
			}
			
		}
	}
	
	public void Release()
	{
		list_requests.Clear();
		requests.Clear();
		requestStringData = null;
		requestData = null;
		if (www != null)
		{
			//www.Dispose();
			www = null;
		}
		requestForm = null;
	}
	
	public bool Connect()
	{
		if (www == null)
		{
			{
				if (requestForm != null)
				{
                    string url = GetGameServerURL();
                    www = new WWW(url, requestForm);
                    Debug.Log("Connecting (combined) to " + url + " requestStringData=" + requestStringData);  

					return true;
				}
			}
		}
		
		return false;
	}
    public string GetGameServerURL()
    {
        // if (NetworkDefine.sURL_Game == null || NetworkDefine.sURL_Game.Length == 0)
        //     return "";
        // int index = NetworkManager.instance.mRetryCombineTimes;
        // if (index >= NetworkDefine.sURL_Game.Length)
        //     index = 0;
        return NetworkDefine.sRootUrl;
        // return "http://10.246.84.246:9005/handler/TestHandler.ashx?method=GetParam";
        // return "url";
    }
	void MakeSerialIdTrace()
	{
		Dictionary<string, object> result = Json.Deserialize(requestStringData) as Dictionary<string, object>;
		if(result.ContainsKey("d"))
		{
			List<object> dataListX = (List<object>)result["d"];
			for(int j = 0;j<dataListX.Count;j++)
			{
				Dictionary<string, object> dicDataX  = dataListX[j] as Dictionary<string,object>;
				if(dicDataX.ContainsKey("o"))
				{
					Debug.LogError("sealed class CombinedHttpConnect--->>>public bool Connect()-->>request.serialId-->>" + (string)dicDataX["o"].ToString());
				}
			}
		} 
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
		NetworkManager.instance.mErrorCode = 0;
		if (www.error != null)
		{
			NetworkManager.instance.mErrorCode = 201;
			NetworkManager.instance.mErrorText = www.error;
			Debug.Log("Network response error:"+NetworkManager.instance.mErrorText);
			return false;
		}
		
		bool parseResult = true;
		{
			string jsontext = null;

			if ((NetworkDefine.sCompressedReponse & NetworkDefine.COMPRESSFLAG_GAME) != 0)
			{
				// try
				// {
				// 	byte[] inBuffer = GetResultBytes();
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
				// 		NetworkManager.instance.mErrorText = "Failed to decompress the response.";
				// 		NetworkManager.instance.mErrorCode = 301;
				// 		DebugUtils.Error(NetworkManager.instance.mErrorText);
				// 	}
				// }
				// catch (Exception exception)
				// {
    //                 NetworkManager.instance.mErrorText = "Exception when decompress the response from " + www.url + ":\n msg=" + exception.Message + ":\n data=";
    //                 NetworkManager.instance.mErrorText += Utils.GetString(GetResultBytes());
    //                 NetworkManager.instance.mErrorText += ":\n as text="+www.text;
				// 	NetworkManager.instance.mErrorCode = 401;
				// 	DebugUtils.Error(NetworkManager.instance.mErrorText);
				// }
			}
			else
			{
				jsontext = GetResultString();
				NetworkManager.instance.mErrorText = jsontext;
			}
							
			Debug.Log("Response :"+"       Time: "+ Time.realtimeSinceStartup.ToString()+"\n"+jsontext);

			Dictionary<string,object> jsonObj = Json.Deserialize(jsontext) as Dictionary<string,object>;

			object result;
			mResult_error_code = 0;
			if(jsonObj.TryGetValue("r",out result))
			{
				Debug.Log("result " + result.ToString());
				mResult_error_code = int.Parse(result.ToString());
			}
			
			object message;
			if(jsonObj.TryGetValue("m",out message))
			{
				if(!string.IsNullOrEmpty(message.ToString()))
				{
					Debug.LogError("message " + message.ToString());
					mError_message = message.ToString();
					MessageBox.instance.OpenMessage(MessageBox.TYPE.Ok,"Network error!!!", "Network error message : " + message.ToString(), ErrorMessageClickOK);
				}
			}
			
			if(mResult_error_code == NetworkDefine.ERROR_CODE_SUCCESS)
			{
				object data;
				if(jsonObj.TryGetValue("d",out data))
				{
					List<object> arrays = data as List<object>;
					if (arrays != null)
					{
						for (int i=0; i<arrays.Count; i++)
							parseResult = ParseOneResponse(arrays[i]) && parseResult;
					}
					else
					{
						parseResult = false;
					}
				}
			}
		}
					
		return parseResult;
	}

	private void ErrorMessageClickOK()
	{
		if(mResult_error_code == NetworkDefine.ERROR_CODE_STOP)
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.Step();
#endif
			Application.Quit();
		}
	}
	
	bool ParseOneResponse(object obj)
	{
		Dictionary<string, object> dic = (Dictionary<string, object>)obj;
		if (dic != null)
		{
			if (dic.ContainsKey("o"))
			{
				int selId = int.Parse(dic["o"].ToString());
				if (requests.ContainsKey((int)selId))
				{
					NetRequest request = requests[(int)selId];
					request.Respond(dic);
					
					//if (request.resultFlag == 0)
					//{
					//	return true;
					//}
					if (request.isDone)
					{
						lock (NetworkManager.instance.mRequestStack)
						{
							NetworkManager.instance.mRequestStack.Remove(request);
						}
						return true;
					}
					
					NetworkManager.instance.mErrorRequestSerialId = request.serialId;
					NetworkManager.instance.mErrorCommandId = request.cmdId;
					NetworkManager.instance.mErrorCode = request.resultFlag;
				}
			}
		}
		
		return false;
	}
			
	byte[] GetResultBytes()
	{
		if (www != null)
		{
			return www.bytes;
		}
		
		return null;
	}
	
	string GetResultString()
	{
		if (www != null)
		{
			return www.text;
		}
		
		return null;
	}
}