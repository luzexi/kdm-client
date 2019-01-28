using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;


// ui daily mission
public class UIDailyMission : ScreenBaseHandler
{
	public UI_Event mBtnPersonal;
    public UI_Event mBtnSidebar;
    public UI_Event mBtnAddNew;

    public RectTransform mCanvas;
    public RectTransform mView;
    public RectTransform mContent;

    public GameObject mOldItemParent;
    public GameObject mNowItemParent;
    public GameObject mFinishedItemParent;

    public ScrollRect mScrollRect;
    private const int HEIGHT_ITEM = 300;
    private const int HEIGHT_INTERVAL = 100;
    private const int HEIGHT_TEXT = 200;
    private const int VIEW_TOP = 170;

    private List<UIDailyMissionItem> mListOldMission = new List<UIDailyMissionItem>();
    private List<UIDailyMissionItem> mListNowMission = new List<UIDailyMissionItem>();
    private List<UIDailyMissionItem> mListFinishedMission = new List<UIDailyMissionItem>();

    private Vector2 mDragDownPosition;
    private const float EDIT_DRAG_DISTANCE = 20f;

    public override void Init()
    {
        base.Init();
        mBtnPersonal.onClick = BtnPersonalOnClick;
        mBtnSidebar.onClick = BtnSidebarOnClick;
        mBtnAddNew.onClick = BtnAddNewOnClick;

        UI_Event _event = UI_Event.Get(mView);
        _event.onBeginDrag = BtnItemOnBeginDrag;
        _event.onDrag = BtnItemOnDrag;
        _event.onEndDrag = BtnItemOnEndDrag;
    }

    public override void CloseScreen()
    {
        base.CloseScreen();
    }

    public override void OpenScreen()
    {
        base.OpenScreen();
    }

    public void MoveToTop()
    {
        if(mView.sizeDelta.y <= mContent.sizeDelta.y)
        {
            mContent.transform.localPosition = new Vector3(0, (mView.sizeDelta.y - mContent.sizeDelta.y)/2 ,0);
        }
        else
        {
            //nothing
        }
    }

    public void FinishAddMission(Mission _mis)
    {
        List<Mission> lst_mis = MissionManager.instance.GetDailyMission();
        ShowMission(lst_mis);
    }

    public void FinishEditMission(Mission _mis)
    {
        for(int i = 0 ; i<mListOldMission.Count ; i++)
        {
            if(mListOldMission[i].mMission.mId == _mis.mId)
            {
                mListOldMission[i].SetMission(_mis);
            }
        }

        for(int i = 0 ; i<mListNowMission.Count ; i++)
        {
            if(mListNowMission[i].mMission.mId == _mis.mId)
            {
                mListNowMission[i].SetMission(_mis);
            }
        }

        for(int i = 0 ; i<mListFinishedMission.Count ; i++)
        {
            if(mListFinishedMission[i].mMission.mId == _mis.mId)
            {
                mListFinishedMission[i].SetMission(_mis);
            }
        }
    }

    void OpenEditButton(Mission _mis)
    {
        for(int i = 0 ; i<mListOldMission.Count ; i++)
        {
            if(mListOldMission[i].mMission.mId == _mis.mId)
            {
                mListOldMission[i].ShowEdit();
            }
        }

        for(int i = 0 ; i<mListNowMission.Count ; i++)
        {
            if(mListNowMission[i].mMission.mId == _mis.mId)
            {
                mListNowMission[i].ShowEdit();
            }
        }

        for(int i = 0 ; i<mListFinishedMission.Count ; i++)
        {
            if(mListFinishedMission[i].mMission.mId == _mis.mId)
            {
                mListFinishedMission[i].ShowEdit();
            }
        }
    }

    void CloseEditButton(Mission _mis)
    {
        for(int i = 0 ; i<mListOldMission.Count ; i++)
        {
            if(mListOldMission[i].mMission.mId == _mis.mId)
            {
                mListOldMission[i].HidenEdit();
            }
        }

        for(int i = 0 ; i<mListNowMission.Count ; i++)
        {
            if(mListNowMission[i].mMission.mId == _mis.mId)
            {
                mListNowMission[i].HidenEdit();
            }
        }

        for(int i = 0 ; i<mListFinishedMission.Count ; i++)
        {
            if(mListFinishedMission[i].mMission.mId == _mis.mId)
            {
                mListFinishedMission[i].HidenEdit();
            }
        }
    }

    public void ShowMission(List<Mission> _lstmission)
    {
        mView.sizeDelta = new Vector2(mCanvas.sizeDelta.x, mCanvas.sizeDelta.y - VIEW_TOP);
        for(int i = 0 ; i<mListOldMission.Count ; i++)
        {
            Destroy(mListOldMission[i].gameObject);
        }
        mListOldMission.Clear();

        for(int i = 0 ; i<mListNowMission.Count ; i++)
        {
            Destroy(mListNowMission[i].gameObject);
        }
        mListNowMission.Clear();

        for(int i = 0 ; i<mListFinishedMission.Count ; i++)
        {
            Destroy(mListFinishedMission[i].gameObject);
        }
        mListFinishedMission.Clear();

        List<Mission> lst_old = new List<Mission>();
        List<Mission> lst_now = new List<Mission>();
        List<Mission> lst_finished = new List<Mission>();

        int sum_hight = HEIGHT_ITEM * _lstmission.Count;
        bool have_old = false;
        bool have_finished = false;
        bool have_now = false;

        for(int i = 0; i<_lstmission.Count ; i++)
        {
            Mission mis = _lstmission[i];
            int day_time = TimeConvert.NowDay();
            if(mis.IsFinished())
            {
                lst_finished.Add(mis);
                if(!have_finished)
                {
                    have_finished = true;
                    sum_hight += HEIGHT_TEXT;
                }
            }
            else if(mis.IsOld())
            {
                lst_old.Add(mis);
                if(!have_old)
                {
                    have_old = true;
                    sum_hight += HEIGHT_TEXT;
                }
            }
            else
            {
                lst_now.Add(mis);
                if(!have_now)
                {
                    have_now = true;
                    sum_hight += HEIGHT_TEXT;
                }
            }
        }

        mScrollRect.content.sizeDelta = new Vector2(mCanvas.sizeDelta.x, sum_hight);

        if(mScrollRect.content.sizeDelta.y < mView.sizeDelta.y)
        {
            mView.sizeDelta = new Vector2(mView.sizeDelta.x, mScrollRect.content.sizeDelta.y);
            mView.transform.localPosition = new Vector3(0, (mCanvas.sizeDelta.y - mView.sizeDelta.y)/2 - VIEW_TOP, 0);
        }
        else
        {
            mView.transform.localPosition = new Vector3(0, (mCanvas.sizeDelta.y - mView.sizeDelta.y)/2 - VIEW_TOP, 0);
        }

        mScrollRect.CalculateLayoutInputVertical();
        //mScrollRect.Rebuild(CanvasUpdate.Layout);
        MoveToTop();

        sum_hight = sum_hight / 2;
        int y_pos = 0;

        if(lst_old.Count > 0)
        {
            mOldItemParent.SetActive(true);
            y_pos += HEIGHT_TEXT/2;
            mOldItemParent.transform.localPosition = new Vector3(0, sum_hight - y_pos, 0);
            for(int i = 0 ; i<lst_old.Count ; i++)
            {
                GameObject obj = GameObject.Instantiate(Resources.Load("ui/UIDailyMissionItem")) as GameObject;
                obj.transform.parent = mOldItemParent.transform;
                obj.transform.localPosition = new Vector3(0,-HEIGHT_TEXT - HEIGHT_ITEM*i,0);
                obj.transform.localScale = Vector3.one;

                UIDailyMissionItem mission_item = obj.GetComponent<UIDailyMissionItem>();
                mission_item.SetMission(lst_old[i]);

                mission_item.mBtnBg.SetData("drag", 1);
                mission_item.mBtnBg.SetData("d",lst_old[i]);
                mission_item.mBtnBg.onBeginDrag = BtnItemOnBeginDrag;
                mission_item.mBtnBg.onDrag = BtnItemOnDrag;
                mission_item.mBtnBg.onEndDrag = BtnItemOnEndDrag;

                mission_item.mBtnFinish.SetData("d",lst_old[i]);
                mission_item.mBtnFinish.SetData("t",1);
                mission_item.mBtnFinish.onClick = BtnMissionFinishOnClick;

                mission_item.mBtnFinish.SetData("drag", 1);
                mission_item.mBtnFinish.onBeginDrag = BtnItemOnBeginDrag;
                mission_item.mBtnFinish.onDrag = BtnItemOnDrag;
                mission_item.mBtnFinish.onEndDrag = BtnItemOnEndDrag;

                mission_item.mBtnCancel.SetData("d",lst_old[i]);
                mission_item.mBtnCancel.SetData("t",1);
                mission_item.mBtnCancel.onClick = BtnMissionCancelOnClick;

                mission_item.mBtnCancel.SetData("drag", 1);
                mission_item.mBtnCancel.onBeginDrag = BtnItemOnBeginDrag;
                mission_item.mBtnCancel.onDrag = BtnItemOnDrag;
                mission_item.mBtnCancel.onEndDrag = BtnItemOnEndDrag;

                mission_item.mBtnDelete.SetData("d",lst_old[i]);
                mission_item.mBtnDelete.onClick = BtnMissionDeleteOnClick;
                mission_item.mBtnEditor.SetData("d",lst_old[i]);
                mission_item.mBtnEditor.onClick = BtnMissionEditorOnClick;
                mission_item.mDeleteEditorNode.gameObject.SetActive(false);

                mListOldMission.Add(mission_item);
            }
        }
        else
        {
            mOldItemParent.SetActive(false);
        }

        if(lst_now.Count > 0 )
        {
            mNowItemParent.SetActive(true);
            y_pos += HEIGHT_INTERVAL;
            y_pos += lst_old.Count * HEIGHT_ITEM;
            mNowItemParent.transform.localPosition = new Vector3(0,sum_hight - y_pos,0);
            //y_pos += HEIGHT_TEXT;
            for(int i = 0 ; i<lst_now.Count ; i++)
            {
                GameObject obj = GameObject.Instantiate(Resources.Load("ui/UIDailyMissionItem")) as GameObject;
                obj.transform.parent = mNowItemParent.transform;
                obj.transform.localPosition = new Vector3(0,-HEIGHT_TEXT - HEIGHT_ITEM*i,0);
                obj.transform.localScale = Vector3.one;

                UIDailyMissionItem mission_item = obj.GetComponent<UIDailyMissionItem>();
                mission_item.SetMission(lst_now[i]);

                mission_item.mBtnBg.SetData("drag", 1);
                mission_item.mBtnBg.SetData("d",lst_now[i]);
                mission_item.mBtnBg.onBeginDrag = BtnItemOnBeginDrag;
                mission_item.mBtnBg.onDrag = BtnItemOnDrag;
                mission_item.mBtnBg.onEndDrag = BtnItemOnEndDrag;

                mission_item.mBtnFinish.SetData("d",lst_now[i]);
                mission_item.mBtnFinish.SetData("t",2);
                mission_item.mBtnFinish.onClick = BtnMissionFinishOnClick;

                mission_item.mBtnFinish.SetData("drag", 1);
                mission_item.mBtnFinish.onBeginDrag = BtnItemOnBeginDrag;
                mission_item.mBtnFinish.onDrag = BtnItemOnDrag;
                mission_item.mBtnFinish.onEndDrag = BtnItemOnEndDrag;

                mission_item.mBtnCancel.gameObject.SetActive(false);

                mission_item.mBtnDelete.SetData("d",lst_now[i]);
                mission_item.mBtnDelete.onClick = BtnMissionDeleteOnClick;
                mission_item.mBtnEditor.SetData("d",lst_now[i]);
                mission_item.mBtnEditor.onClick = BtnMissionEditorOnClick;
                mission_item.mDeleteEditorNode.gameObject.SetActive(false);

                mListNowMission.Add(mission_item);
            }
        }
        else
        {
            mNowItemParent.SetActive(false);
        }

        if(lst_finished.Count > 0)
        {
            mFinishedItemParent.SetActive(true);
            y_pos += HEIGHT_INTERVAL;
            y_pos += lst_now.Count * HEIGHT_ITEM;
            mFinishedItemParent.transform.localPosition = new Vector3(0,sum_hight - y_pos,0);
            //y_pos += HEIGHT_TEXT;
            for(int i = 0 ; i<lst_finished.Count ; i++)
            {
                GameObject obj = GameObject.Instantiate(Resources.Load("ui/UIDailyMissionItem")) as GameObject;
                obj.transform.parent = mFinishedItemParent.transform;
                obj.transform.localPosition = new Vector3(0,-HEIGHT_TEXT - HEIGHT_ITEM * i,0);
                obj.transform.localScale = Vector3.one;

                UIDailyMissionItem mission_item = obj.GetComponent<UIDailyMissionItem>();
                mission_item.SetMission(lst_finished[i]);

                mission_item.mBtnBg.SetData("drag", 1);
                mission_item.mBtnBg.SetData("d",lst_finished[i]);
                mission_item.mBtnBg.onBeginDrag = BtnItemOnBeginDrag;
                mission_item.mBtnBg.onDrag = BtnItemOnDrag;
                mission_item.mBtnBg.onEndDrag = BtnItemOnEndDrag;

                mission_item.mBtnCancel.SetData("d",lst_finished[i]);
                mission_item.mBtnCancel.SetData("t",3);
                mission_item.mBtnCancel.onClick = BtnMissionCancelOnClick;

                mission_item.mBtnCancel.SetData("drag", 1);
                mission_item.mBtnCancel.onBeginDrag = BtnItemOnBeginDrag;
                mission_item.mBtnCancel.onDrag = BtnItemOnDrag;
                mission_item.mBtnCancel.onEndDrag = BtnItemOnEndDrag;

                mission_item.mBtnFinish.gameObject.SetActive(false);
                mission_item.mBtnDelete.SetData("d",lst_finished[i]);
                mission_item.mBtnDelete.onClick = BtnMissionDeleteOnClick;
                mission_item.mBtnEditor.SetData("d",lst_finished[i]);
                mission_item.mBtnEditor.onClick = BtnMissionEditorOnClick;
                mission_item.mDeleteEditorNode.gameObject.SetActive(false);

                mListFinishedMission.Add(mission_item);
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
        int drag = ev.GetData<int>("drag");
        if(drag <= 0) return;

        mDragDownPosition = eventData.position;
    }

    void BtnItemOnDrag(PointerEventData eventData , UI_Event ev)
    {
        mScrollRect.OnDrag(eventData);
    }

    void BtnItemOnEndDrag(PointerEventData eventData , UI_Event ev)
    {
        mScrollRect.OnEndDrag(eventData);

        int drag = ev.GetData<int>("drag");
        if(drag <= 0) return;
        Mission mis = ev.GetData<Mission>("d");


        Vector2 drag_dis = eventData.position - mDragDownPosition;
        if(Mathf.Abs(drag_dis.x) > EDIT_DRAG_DISTANCE && Mathf.Abs(drag_dis.x) > Mathf.Abs(drag_dis.y))
        {
            if(drag_dis.x < 0)
            {
                OpenEditButton(mis);
            }
            else
            {
                CloseEditButton(mis);
            }
        }
    }

    void BtnPersonalOnClick(PointerEventData eventData , UI_Event ev)
    {
        //CloseScreen();
        UIPersonal ui_personal = MenuManager.instance.CreateMenu<UIPersonal>();
        ui_personal.OpenScreen();
    }

    void BtnSidebarOnClick(PointerEventData eventData , UI_Event ev)
    {
        //
    }

    void BtnAddNewOnClick(PointerEventData eventData , UI_Event ev)
    {
        //CloseScreen();
        UIMissionEditor ui_editor = MenuManager.instance.CreateMenu<UIMissionEditor>();
        ui_editor.OpenScreen();
        Mission mis = new Mission();
        mis.mDesc = "test";
        ui_editor.SetEditMission(mis);
    }

    void BtnMissionFinishOnClick(PointerEventData eventData , UI_Event ev)
    {
        Mission mis = ev.GetData<Mission>("d");
        //mis.mDateTime = DateTime.Now;
        mis.mFinished = TimeConvert.NowDay();

        List<Mission> lst_mis = MissionManager.instance.GetDailyMission();
        ShowMission(lst_mis);
    }

    void BtnMissionDeleteOnClick(PointerEventData eventData , UI_Event ev)
    {
        Mission mis = ev.GetData<Mission>("d");
        MissionManager.instance.RemoveMission(mis);

        List<Mission> lst_mis = MissionManager.instance.GetDailyMission();
        ShowMission(lst_mis);
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

        List<Mission> lst_mis = MissionManager.instance.GetDailyMission();
        ShowMission(lst_mis);
    }
}