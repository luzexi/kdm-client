using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// ui personal
public class UIPersonal : ScreenBaseHandler
{
	public UI_Event mBtnBack;
    public UI_Event mBtnStatistics;

    public Text mName;
    public Text mLevel;
    public Text mSign;
    public Text mAchievement;

    public override void Init()
    {
        base.Init();
        mBtnBack.onClick = BtnBackOnClick;
        mBtnStatistics.onClick = BtnStatisticsOnClick;
    }

    public override void CloseScreen()
    {
        base.CloseScreen();
    }

    public override void OpenScreen()
    {
        base.OpenScreen();

        mName.text = PlayerInfo.instance.Name;
        mLevel.text = "" + PlayerInfo.instance.Level;
        mSign.text = PlayerInfo.instance.Sign;
        mAchievement.text = PlayerInfo.instance.LevelTable.mAchievement;
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

