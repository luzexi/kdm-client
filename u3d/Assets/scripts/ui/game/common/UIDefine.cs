using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UIDefine
{
	public const int SCREEN_WIDTH = 750;
	public const int SCREEN_HEIGHT = 1334;

	private static int mScreenWidth = 750;
	public static int UI_SCREEN_WIDTH
	{
		get
		{
			return mScreenWidth;
		}
		set
		{
			mScreenWidth = value;
		}
	}

	private static int mScreenHeight = 1334;
	public static int UI_SCREEN_HEIGHT
	{
		get
		{
			return mScreenHeight;
		}
		set
		{
			mScreenHeight = value;
		}
	}
}