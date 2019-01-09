using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// ui daily mission
public class UIDailyMission : ScreenBaseHandler
{
	public UI_Event mBtnPersonal;
    public UI_Event mBtnSidebar;

    public override void Init()
    {
        base.Init();
        mBtnPersonal.onClick = BtnPersonalOnClick;
    }

    public override void CloseScreen()
    {
        base.CloseScreen();
    }

    public override void OpenScreen()
    {
        base.OpenScreen();
    }

    void BtnPersonalOnClick(PointerEventData eventData , UI_Event ev)
    {
        //
    }
}