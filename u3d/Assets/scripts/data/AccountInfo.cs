using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountInfo : CSingleton<AccountInfo>
{
	public string DeviceId;

	public string Uid = null;

	public string Token = null;

	public string ChannelType;

	public string ChannelId;


	public void Init()
	{
		DeviceId = Misc.getDeviceId();
		Debug.Log("DeviceId " + DeviceId);
	}
}