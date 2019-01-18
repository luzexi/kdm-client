using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


// ui daily mission
public class UIDailyMission : ScreenBaseHandler
{
	public UI_Event mBtnPersonal;
    public UI_Event mBtnSidebar;
    public UI_Event mBtnAddNew;

    public GameObject mOldItemParent;
    public GameObject mNowItemParent;
    public GameObject mFinishedItemParent;

    public ScrollRect mScrollRect;

    public List<UIDailyMissionItem> mListOldMission = new List<UIDailyMissionItem>();
    public List<UIDailyMissionItem> mListNowMission = new List<UIDailyMissionItem>();
    public List<UIDailyMissionItem> mListFinishedMission = new List<UIDailyMissionItem>();

    public override void Init()
    {
        base.Init();
        mBtnPersonal.onClick = BtnPersonalOnClick;
        mBtnSidebar.onClick = BtnSidebarOnClick;
        mBtnAddNew.onClick = BtnAddNewOnClick;
    }

    public override void CloseScreen()
    {
        base.CloseScreen();
    }

    public override void OpenScreen()
    {
        base.OpenScreen();
    }

    public void ShowMission(List<Mission> _lstmission)
    {
        List<Mission> lst_old = new List<Mission>();
        List<Mission> lst_now = new List<Mission>();
        List<Mission> lst_finished = new List<Mission>();

        for(int i = 0; i<_lstmission.Count ; i++)
        {
            Mission mis = _lstmission[i];
            int day_time = TimeConvert.NowDay();
            if(mis.IsFinished())
            {
                lst_finished.Add(mis);
            }
            else if(mis.IsOld())
            {
                lst_old.Add(mis);
            }
            else
            {
                lst_now.Add(mis);
            }
        }

        int y_pos = -100;

        mOldItemParent.transform.localPosition = new Vector3(0,y_pos,0);
        if(lst_old.Count > 0)
        {
            mOldItemParent.SetActive(true);
            for(int i = 0 ; i<lst_old.Count ; i++)
            {
                GameObject obj = GameObject.Instantiate(Resources.Load("ui/UIDailyMissionItem")) as GameObject;
                obj.transform.parent = mOldItemParent.transform;
                obj.transform.localPosition = new Vector3(0,-300*i,0);
                obj.transform.localScale = Vector3.one;

                UIDailyMissionItem mission_item = obj.GetComponent<UIDailyMissionItem>();
                mission_item.SetMission(lst_old[i]);

                mission_item.mBtnBg.onBeginDrag = BtnItemOnBeginDrag;
                mission_item.mBtnBg.onDrag = BtnItemOnDrag;
                mission_item.mBtnBg.onEndDrag = BtnItemOnEndDrag;

                mission_item.mBtnFinish.SetData("d",lst_old[i]);
                mission_item.mBtnFinish.SetData("t",1);
                mission_item.mBtnFinish.onClick = BtnMissionFinishOnClick;
                mission_item.mBtnCancel.SetData("d",lst_old[i]);
                mission_item.mBtnCancel.SetData("t",1);
                mission_item.mBtnCancel.onClick = BtnMissionCancelOnClick;
                mission_item.mBtnDelete.SetData("d",lst_old[i]);
                mission_item.mBtnDelete.onClick = BtnMissionDeleteOnClick;
                mission_item.mBtnEditor.SetData("d",lst_old[i]);
                mission_item.mBtnEditor.onClick = BtnMissionEditorOnClick;
                mission_item.mDeleteEditorNode.gameObject.SetActive(false);
            }
        }
        else
        {
            mOldItemParent.SetActive(false);
        }

        if(lst_now.Count > 0 )
        {
            mNowItemParent.SetActive(true);
            y_pos -= lst_old.Count * 300;
            mNowItemParent.transform.localPosition = new Vector3(0,y_pos,0);
            for(int i = 0 ; i<lst_now.Count ; i++)
            {
                GameObject obj = GameObject.Instantiate(Resources.Load("ui/UIDailyMissionItem")) as GameObject;
                obj.transform.parent = mNowItemParent.transform;
                obj.transform.localPosition = new Vector3(0,-300*i,0);
                obj.transform.localScale = Vector3.one;

                UIDailyMissionItem mission_item = obj.GetComponent<UIDailyMissionItem>();
                mission_item.SetMission(lst_now[i]);

                mission_item.mBtnBg.onBeginDrag = BtnItemOnBeginDrag;
                mission_item.mBtnBg.onDrag = BtnItemOnDrag;
                mission_item.mBtnBg.onEndDrag = BtnItemOnEndDrag;

                mission_item.mBtnFinish.SetData("d",lst_now[i]);
                mission_item.mBtnFinish.SetData("t",2);
                mission_item.mBtnFinish.onClick = BtnMissionFinishOnClick;
                mission_item.mBtnCancel.gameObject.SetActive(false);
                mission_item.mBtnDelete.SetData("d",lst_now[i]);
                mission_item.mBtnDelete.onClick = BtnMissionDeleteOnClick;
                mission_item.mBtnEditor.SetData("d",lst_now[i]);
                mission_item.mBtnEditor.onClick = BtnMissionEditorOnClick;
                mission_item.mDeleteEditorNode.gameObject.SetActive(false);
            }
        }
        else
        {
            mNowItemParent.SetActive(false);
        }

        if(lst_finished.Count > 0)
        {
            mFinishedItemParent.SetActive(true);
            y_pos -= lst_now.Count * 300;
            mFinishedItemParent.transform.localPosition = new Vector3(0,y_pos,0);
            for(int i = 0 ; i<lst_finished.Count ; i++)
            {
                GameObject obj = GameObject.Instantiate(Resources.Load("ui/UIDailyMissionItem")) as GameObject;
                obj.transform.parent = mFinishedItemParent.transform;
                obj.transform.localPosition = new Vector3(0,-300*i,0);
                obj.transform.localScale = Vector3.one;

                UIDailyMissionItem mission_item = obj.GetComponent<UIDailyMissionItem>();
                mission_item.SetMission(lst_finished[i]);

                mission_item.mBtnBg.onBeginDrag = BtnItemOnBeginDrag;
                mission_item.mBtnBg.onDrag = BtnItemOnDrag;
                mission_item.mBtnBg.onEndDrag = BtnItemOnEndDrag;

                mission_item.mBtnCancel.SetData("d",lst_finished[i]);
                mission_item.mBtnCancel.SetData("t",3);
                mission_item.mBtnCancel.onClick = BtnMissionCancelOnClick;
                mission_item.mBtnFinish.gameObject.SetActive(false);
                mission_item.mBtnDelete.SetData("d",lst_finished[i]);
                mission_item.mBtnDelete.onClick = BtnMissionDeleteOnClick;
                mission_item.mBtnEditor.SetData("d",lst_finished[i]);
                mission_item.mBtnEditor.onClick = BtnMissionEditorOnClick;
                mission_item.mDeleteEditorNode.gameObject.SetActive(false);
            }
        }
        else
        {
            mFinishedItemParent.SetActive(false);
        }
    }


    ///////////////// button
    void BtnItemOnBeginDrag(PointerEventData eventData , UI_Event ev)
    {
        mScrollRect.OnBeginDrag(eventData);
    }

    void BtnItemOnDrag(PointerEventData eventData , UI_Event ev)
    {
        mScrollRect.OnDrag(eventData);
    }

    void BtnItemOnEndDrag(PointerEventData eventData , UI_Event ev)
    {
        mScrollRect.OnEndDrag(eventData);
    }

    void BtnPersonalOnClick(PointerEventData eventData , UI_Event ev)
    {
        CloseScreen();
        UIPersonal ui_personal = MenuManager.instance.CreateMenu<UIPersonal>();
        ui_personal.OpenScreen();
    }

    void BtnSidebarOnClick(PointerEventData eventData , UI_Event ev)
    {
        //
    }

    void BtnAddNewOnClick(PointerEventData eventData , UI_Event ev)
    {
        CloseScreen();
        UIMissionEditor ui_editor = MenuManager.instance.CreateMenu<UIMissionEditor>();
        ui_editor.OpenScreen();
    }

    void BtnMissionFinishOnClick(PointerEventData eventData , UI_Event ev)
    {
        Mission mis = ev.GetData<Mission>("d");
        mis.mDateTime = DateTime.Now;
        mis.mFinished = 1;
    }

    void BtnMissionDeleteOnClick(PointerEventData eventData , UI_Event ev)
    {
        Mission mis = ev.GetData<Mission>("d");
        MissionManager.instance.RemoveMission(mis);
    }

    void BtnMissionEditorOnClick(PointerEventData eventData , UI_Event ev)
    {
        Mission mis = ev.GetData<Mission>("d");
        UIMissionEditor ui_editor = MenuManager.instance.CreateMenu<UIMissionEditor>();
        ui_editor.OpenScreen();
        ui_editor.SetEditMission(mis);
    }

    void BtnMissionCancelOnClick(PointerEventData eventData , UI_Event ev)
    {
        int type = ev.GetData<int>("t");
        Mission mis = ev.GetData<Mission>("d");

        if(type == 1)
        {
            mis.mDateTime = TimeConvert.GetNow();
        }
        else if(type == 3)
        {
            mis.mFinished = 0;
        }
    }
}