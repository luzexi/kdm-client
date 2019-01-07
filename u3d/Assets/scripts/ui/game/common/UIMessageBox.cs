using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//message box yes no ok function
public class UIMessageBox : ScreenBaseHandler
{
	public Text mTitle;
	public Text mContent;

	public UI_Event mBtnYes;
	public UI_Event mBtnNo;
	public UI_Event mBtnOK;
	public UI_Event mBtnClose;

	public delegate void CallBackFunction();
	public CallBackFunction mYesCallback;
	public CallBackFunction mNoCallback;
	// public CallBackFunction mOKCallback;

	public override void Init()
	{
		base.Init();

		mBtnYes.onClick = YesOnClick;
		mBtnNo.onClick = NoOnClick;
		mBtnOK.onClick = YesOnClick;
		mBtnClose.onClick = NoOnClick;
	}

	public override void CloseScreen()
    {
        base.CloseScreen();
    }

    public override void OpenScreen()
    {
    	base.OpenScreen();
    }

    void YesOnClick(PointerEventData eventData , UI_Event ev)
    {
    	if(mYesCallback != null)
    	{
    		mYesCallback();
    	}
    	CloseScreen();
    }

    void NoOnClick(PointerEventData eventData , UI_Event ev)
    {
    	if(mNoCallback != null)
    	{
    		mNoCallback();
    	}
    	CloseScreen();
    }

    void OkOnClick(PointerEventData eventData , UI_Event ev)
    {
    	// if(mOKCallback != null)
    	// {
    	// 	mOKCallback();
    	// }
    	if(mYesCallback != null)
    	{
    		mYesCallback();
    	}
    	CloseScreen();
    }
}