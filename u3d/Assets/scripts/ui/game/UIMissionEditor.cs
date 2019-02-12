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

    //public GameObject mSelectMissionParent;
    public bool mIsEdit = false;

    public RawImage mImagePic;
    public Text mTextDesc;
    public InputField mInputField;

    public RectTransform mCanvas;
    public RectTransform mView;
    public RectTransform mContent;
    public ScrollRect mScrollRect;
    private const int WIDTH_ITEM = 300;
    private const int WIDTH_INTERVAL = 100;

    public Mission mMission = new Mission();
    public List<UIDailyMissionItem> mListSelectMission = new List<UIDailyMissionItem>();

    public override void Init()
    {
        base.Init();
        mBtnBack.onClick = BtnBackOnClick;
        mBtnOk.onClick = BtnOkOnClick;
        mBtnCamera.onClick = BtnCameraOnClick;

        //UI_Event _ev = UI_Event.Get(mTextDesc);
        //_ev.onClick = BtnSelectDescOnClick;
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

        UI_Event view_event = UI_Event.Get(mView);
        view_event.onBeginDrag = ScrollOnBeginDrag;
        view_event.onDrag = ScrollOnDrag;
        view_event.onEndDrag = ScrollOnEndDrag;
    }

    public void SetEditMission(Mission _mis)
    {
        mMission = _mis;
        mImagePic.texture = _mis.texture;
        mTextDesc.text = _mis.mDesc;
        mTextDesc.gameObject.SetActive(false);
        mInputField.text = _mis.mDesc;
    }

    public void MoveToTop()
    {
        if(mView.sizeDelta.x <= mContent.sizeDelta.x)
        {
            mContent.transform.localPosition = new Vector3(-(mView.sizeDelta.x - mContent.sizeDelta.x)/2,0 ,0);
        }
        else
        {
            //nothing
        }
    }

    void SetSelectMission(List<Mission> lst_mis)
    {
        mView.sizeDelta = new Vector2(mCanvas.sizeDelta.x, mView.sizeDelta.y);
        int sum_size = WIDTH_INTERVAL + lst_mis.Count * WIDTH_ITEM;
        mContent.sizeDelta = new Vector2(sum_size, WIDTH_ITEM);
        MoveToTop();

        for(int i = 0 ; i<lst_mis.Count ; i++)
        {
            GameObject obj = GameObject.Instantiate(Resources.Load("ui/UIMissionEditorItem")) as GameObject;
            obj.transform.parent = mContent.transform;
            obj.transform.localPosition = new Vector3(- sum_size/2 + WIDTH_INTERVAL + WIDTH_ITEM * i, 0, 0);
            obj.transform.localScale = Vector3.one;

            UIDailyMissionItem mission_item = obj.GetComponent<UIDailyMissionItem>();
            mission_item.SetMission(lst_mis[i]);
            mission_item.mBtnFinish.SetData("d", lst_mis[i]);
            mission_item.mBtnFinish.onClick = BtnSelectOnClick;
            mission_item.mBtnFinish.onBeginDrag = ScrollOnBeginDrag;
            mission_item.mBtnFinish.onDrag = ScrollOnDrag;
            mission_item.mBtnFinish.onEndDrag = ScrollOnEndDrag;
        }
    }

    //////////////////////////////// button event
    void BtnSelectDescOnClick(PointerEventData eventData , UI_Event ev)
    {
        //CloseScreen();
        TouchScreenKeyboard tsk = TouchScreenKeyboard.Open("");
    }

    void BtnSelectOnClick(PointerEventData eventData , UI_Event ev)
    {
        Mission mis = ev.GetData<Mission>("d");
        Mission select_mission = new Mission();
        if(mIsEdit)
        {
            select_mission.mId = mMission.mId;
        }
        else
        {
            select_mission.mId = MissionManager.instance.MaxID++;
        }
        select_mission.mTextureName = mis.mTextureName;
        select_mission.mDesc = mis.mDesc;

        SetEditMission(select_mission);
    }

    void BtnBackOnClick(PointerEventData eventData , UI_Event ev)
    {
        CloseScreen();
    }

    void BtnOkOnClick(PointerEventData eventData , UI_Event ev)
    {
        if(mIsEdit)
        {
            MissionManager.instance.Save();
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
            mis.mType = MissionType.Daily;
            mis.mTextureName = mMission.mTextureName;
            mis.mDesc = mMission.mDesc;
            mis.mDateTime = TimeConvert.GetNow();
            MissionManager.instance.AddMission(mis);
            MissionManager.instance.Save();
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

    void ScrollOnBeginDrag(PointerEventData eventData , UI_Event ev)
    {
        mScrollRect.OnBeginDrag(eventData);
    }

    void ScrollOnDrag(PointerEventData eventData , UI_Event ev)
    {
        mScrollRect.OnDrag(eventData);
    }

    void ScrollOnEndDrag(PointerEventData eventData , UI_Event ev)
    {
        mScrollRect.OnEndDrag(eventData);
    }
    /////////////////////////////////////
}