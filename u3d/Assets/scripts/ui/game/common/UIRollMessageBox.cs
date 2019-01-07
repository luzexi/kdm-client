using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//message box yes no ok function
public class UIRollMessageBox : ScreenBaseHandler
{
	public Text mTextMessage;

	private float mStartTime;
	private const float DURATION = 1.5f;
	private bool mStart = false;
	private Vector3 mSpeed = new Vector3(0,60f,0);
	private Color mStartColor = new Color(1,1,1,1);
	private Color mEndColor = new Color(1,1,1,0);

	public override void Init()
	{
		base.Init();
	}

	public override void CloseScreen()
    {
        base.CloseScreen();
    }

    public override void OpenScreen()
    {
    	base.OpenScreen();
    }

    public void SetMessage(string str)
    {
    	mTextMessage.gameObject.SetActive(true);
    	mTextMessage.text = str;
    	mTextMessage.transform.localPosition = Vector3.zero;
    	mStartTime = Time.time;
    	mStart = true;
    }

    void Update()
    {
    	if(!mStart) return;

    	float difTime = Time.time - mStartTime;
    	if(difTime > DURATION)
    	{
    		mTextMessage.gameObject.SetActive(false);
    		mStart = false;
    	}

    	mTextMessage.transform.localPosition += (mSpeed*Time.deltaTime);
    	float rate_color = difTime / DURATION;
    	mTextMessage.color = Color.Lerp(mStartColor,mEndColor,rate_color);
    }
}