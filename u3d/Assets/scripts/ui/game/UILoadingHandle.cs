using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//ui loading handle
public class UILoadingHandle : ScreenBaseHandler
{
	public GameObject mButton;
	public Slider mSlider;
	public Text mTextBar;
	public System.Action mFinishCall;

	public override void Init()
	{
		base.Init();

		UI_Event ev = UI_Event.Get(mButton);
		ev.onClick = ButtonOnClick;
	}

	public override void OpenScreen()
	{
		base.OpenScreen();
		mButton.SetActive(false);
	}

	public override void CloseScreen()
	{
		base.CloseScreen();
	}

	public void ShowFinishButton()
	{
		mButton.SetActive(true);
		SetRate(1f);
		this.mTextBar.text = "可以开始游戏";
	}

	public void SetTextBar(string _str)
	{
		this.mTextBar.text = _str;
	}

	public void SetRate(float rate)
	{
		mSlider.value = rate;
	}

	// input event
	public void ButtonOnClick( PointerEventData eventData , UI_Event ev)
	{
		if(mFinishCall != null)
		{
			mFinishCall();
		}
	}
}
