using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
	None,
	Daily = 1,
	Week,
	Month
}


public class Mission
{
	public int mId;
	public MissionType mType;
	public int mDateTime;
	public string mTextureName;
	private Texture mTexture;
	public string mDesc;
	public int mCount;

	public Texture texture
	{
		get
		{
			if(mTexture == null)
			{
				// todo load texture
				return null;
			}
			return mTexture;
		}
	}
}
