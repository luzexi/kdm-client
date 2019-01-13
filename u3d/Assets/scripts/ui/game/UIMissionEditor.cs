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

    public GameObject mSelectMissionParent;

    public RawImage mImagePic;
    public Text mTextDesc;

    public Mission mMission = null;
    public List<UIDailyMissionItem> mListSelectMission = new List<UIDailyMissionItem>();

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

    public void SetSelectMission(List<Mission> lst_mis)
    {
        for(int i = 0 ; i<lst_mis.Count ; i++)
        {
            GameObject obj = GameObject.Instantiate(Resources.Load("UIMissionEditorItem")) as GameObject;
            obj.transform.parent = mSelectMissionParent.transform;
            obj.transform.localPosition = new Vector3(300*i,0,0);
            obj.transform.localScale = Vector3.one;

            UIDailyMissionItem mission_item = obj.GetComponent<UIDailyMissionItem>();
            mission_item.SetMission(lst_mis[i]);
            mission_item.mBtnFinish.SetData("d",lst_mis[i]);
            mission_item.mBtnFinish.onClick = BtnSelectOnClick;
        }
    }

    //////////////////////////////// button event
    void BtnSelectOnClick(PointerEventData eventData , UI_Event ev)
    {
        //
    }

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