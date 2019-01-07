using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageUrlHttpConnect
{
	string _url;
	WWW www;
	
	public void Initial(string _imageUrl)
	{
		_url = _imageUrl;
	}
	
	public void Release()
	{
		if (www != null)
		{
			www.Dispose();
			www = null;
		}
		_url = null;
	}
	
	public void Release2()
	{
		if (www != null)
		{
			// when breaking a connecting thread, it takes a long long time in PC
			//www.Dispose();
			www = null;
		}
		_url = null;
	}
	
	public bool Connect()
	{
		if (www == null)
		{
			www = new WWW(_url);
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
	public Texture2D ParseResponse()
	{
		if (www.error != null)
		{
			return null;
		}
		else
		{
			return www.texture;
		}
	}
	
	public string GetURL()
	{
		return _url;
	}
}

