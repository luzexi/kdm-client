using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// ui mission editor
public class UIMissionEditor : ScreenBaseHandler
{
    public UI_Event mBtnBack;
    public UI_Event mBtnOk;
    public UI_Event mBtnCamera;

    public GameObject mSelectMissionParent;
    public bool mIsEdit = false;

    public RawImage mImagePic;
    public Text mTextDesc;

    public Mission mMission = new Mission();
    public List<UIDailyMissionItem> mListSelectMission = new List<UIDailyMissionItem>();

    private const int WIDTH_ITEM  = 300;

    public override void Init()
    {
        base.Init();
        mBtnBack.onClick = BtnBackOnClick;
        mBtnOk.onClick = BtnOkOnClick;
        mBtnCamera.onClick = BtnCameraOnClick;
    }

    public override void CloseScreen()
    {
        base.CloseScreen();
    }

    public override void OpenScreen()
    {
        base.OpenScreen();
        List<TableMissionSelect.Data> lst_mis_table = TableManager.instance.GetMissionSelectAll();
        List<Mission> lst_mis = new List<Mission>();
        for(int i = 0 ; i<lst_mis_table.Count ; i++)
        {
            Mission _mis = new Mission();
            _mis.mTextureName = lst_mis_table[i].mPic;
            _mis.mDesc = lst_mis_table[i].mDesc;
            lst_mis.Add(_mis);
        }
        SetSelectMission(lst_mis);
    }

    public void SetEditMission(Mission _mis)
    {
        mMission = _mis;
        mImagePic.texture = _mis.texture;
        mTextDesc.text = _mis.mDesc;
    }

    void SetSelectMission(List<Mission> lst_mis)
    {
        for(int i = 0 ; i<lst_mis.Count ; i++)
        {
            GameObject obj = GameObject.Instantiate(Resources.Load("UIMissionEditorItem")) as GameObject;
            obj.transform.parent = mSelectMissionParent.transform;
            obj.transform.localPosition = new Vector3(WIDTH_ITEM * i,0,0);
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
        Mission mis = ev.GetData<Mission>("d");
        SetEditMission(mis);
    }

    void BtnBackOnClick(PointerEventData eventData , UI_Event ev)
    {
        CloseScreen();
    }

    void BtnOkOnClick(PointerEventData eventData , UI_Event ev)
    {
        if(mIsEdit)
        {
            CloseScreen();

            UIDailyMission ui_daily_mission = MenuManager.instance.FindMenu<UIDailyMission>();
            if(ui_daily_mission != null)
            {
                ui_daily_mission.FinishEditMission(mMission);
            }
        }
        else
        {
            Mission mis = new Mission();
            mis.mId = MissionManager.instance.MaxID++;
            mis.mTextureName = mMission.mTextureName;
            mis.mDesc = mMission.mDesc;
            mis.mDateTime = TimeConvert.GetNow();
            MissionManager.instance.AddMission(mis);
            CloseScreen();

            UIDailyMission ui_daily_mission = MenuManager.instance.FindMenu<UIDailyMission>();
            if(ui_daily_mission != null)
            {
                ui_daily_mission.FinishAddMission(mis);
            }
        }
    }

    void BtnCameraOnClick(PointerEventData eventData , UI_Event ev)
    {
        // select photo or take a picture
    }
    /////////////////////////////////////
}