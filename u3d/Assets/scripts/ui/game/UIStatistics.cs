using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// ui statistics
public class UIStatistics : ScreenBaseHandler
{
	public UI_Event mBtnBack;

    public override void Init()
    {
        base.Init();
        mBtnBack.onClick = BtnBackOnClick;        
    }

    public override void CloseScreen()
    {
        base.CloseScreen();
    }

    public override void OpenScreen()
    {
        base.OpenScreen();
    }

    void BtnBackOnClick(PointerEventData eventData , UI_Event ev)
    {
        CloseScreen();
    }

    void BtnStatisticsOnClick(PointerEventData eventData , UI_Event ev)
    {
        //
    }
}

