using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// ui mission editor
public class UIMissionEditor : ScreenBaseHandler
{
	public UI_Event mBtnPersonal;
    public UI_Event mBtnBack;
    public UI_Event mBtnOk;
    public UI_Event mBtnCamera;
    public UI_Event mBtnNewTemplate;

    public override void Init()
    {
        base.Init();
        mBtnPersonal.onClick = BtnPersonalOnClick;
        mBtnBack.onClick = BtnBackOnClick;
        mBtnOk.onClick = BtnOkOnClick;
        mBtnCamera.onClick = BtnCameraOnClick;
        mBtnNewTemplate.onClick = BtnNewTemplateOnClick;
    }

    public override void CloseScreen()
    {
        base.CloseScreen();
    }

    public override void OpenScreen()
    {
        base.OpenScreen();
    }

    //////////////////////////////// button event
    void BtnPersonalOnClick(PointerEventData eventData , UI_Event ev)
    {
        //
    }

    void BtnBackOnClick(PointerEventData eventData , UI_Event ev)
    {
        //
    }

    void BtnOkOnClick(PointerEventData eventData , UI_Event ev)
    {
        //
    }

    void BtnCameraOnClick(PointerEventData eventData , UI_Event ev)
    {
        //
    }

    void BtnNewTemplateOnClick(PointerEventData eventData , UI_Event ev)
    {
        //
    }
    /////////////////////////////////////
}