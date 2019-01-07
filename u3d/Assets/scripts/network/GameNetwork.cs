using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;


public partial class GameNetwork
{
	public static void RequestServerInfo(Action _callback)
	{
		ServerInfoRequest request = new ServerInfoRequest(ResponseServerInfo);

		request._call_back = _callback;

		request.deviceId = AccountInfo.instance.DeviceId;
#if UNITY_EDITOR
		request.platOS = "1";
#elif UNITY_IPHONE
		request.platOS = "2";
#elif UNITY_ANDROID
		request.platOS = "3";
#endif

		NetworkManager.instance.AddGameRequest(request,false);
	}

	public static void ResponseServerInfo(NetRequest _request)
	{
		ServerInfoRequest request = (ServerInfoRequest)_request;

		NetworkDefine.sAssetBundleDownloadUrl = request.ab_url;
		NetworkDefine.sPackageDownloadUrl = request.package_url;

		Debug.LogError("request.version " + request.version);
		Debug.LogError("request.ab_url " + request.ab_url);
		Debug.LogError("request.package_url " + request.package_url);
		Debug.LogError("Consts.VERSION " + Consts.VERSION);

#if !UNITY_EDITOR
		if(request.version > Consts.VERSION)
		{
			// update package
			string title = "version is too old";
			string content = "version is too old, click ok to update a new one";
			MessageBox.instance.OpenMessage(MessageBox.TYPE.Ok,title, content, OpenUrlToUpdate);
			return;
		}
#endif

		if(request._call_back != null)
		{
			request._call_back();
		}
	}

	static void OpenUrlToUpdate()
	{
		Application.OpenURL(NetworkDefine.sPackageDownloadUrl);
	}

	public static void RequestGetAccount()
	{
		AccountRequest request = new AccountRequest(ResponseGetAccount);

		request.deviceId = AccountInfo.instance.DeviceId;
#if UNITY_EDITOR
		request.platOS = "1";
#elif UNITY_IPHONE
		request.platOS = "2";
#elif UNITY_ANDROID
		request.platOS = "3";
#endif

		NetworkManager.instance.AddGameRequest(request,false);
	}

	public static void ResponseGetAccount(NetRequest _request)
	{
		AccountRequest request = (AccountRequest)_request;
        if (request.error != 0)
        {
            //TextManager.instance.ShowErrorCodeMsg(request.cmdId, request.error);
            return;
        }

		AccountInfo.instance.Uid = request.uid;
		AccountInfo.instance.Token = request.token;
		
		Debug.LogError("AccountInfo.instance.Uid " + AccountInfo.instance.Uid);
		Debug.LogError("AccountInfo.instance.Token " + AccountInfo.instance.Token);

		RequestGetRoleInfo();
	}

	public static void RequestGetRoleInfo()
	{
		GetRoleInfoRequest request = new GetRoleInfoRequest(ResponseGetRoleInfo);

		NetworkManager.instance.AddGameRequest(request,false);
	}

	public static void ResponseGetRoleInfo(NetRequest _request)
	{
		GetRoleInfoRequest request = (GetRoleInfoRequest)_request;
	}
}
