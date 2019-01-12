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

    public GameObject mOldTitle;
    public GameObject mNowTitle;
    public GameObject mFinishedTitle;

    public GameObject mOldItemParent;
    public GameObject mNowItemParent;
    public GameObject mFinishedItemParent;

    public List<UIDailyMissionItem> mListOldMission = new List<UIDailyMissionItem>();
    public List<UIDailyMissionItem> mListNowMission = new List<UIDailyMissionItem>();
    public List<UIDailyMissionItem> mListFinishedMission = new List<UIDailyMissionItem>();

    public override void Init()
    {
        base.Init();
        mBtnPersonal.onClick = BtnPersonalOnClick;
        mBtnSidebar.onClick = BtnSidebarOnClick;
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

        int y_pos = 100;

        mOldItemParent.transform.localPosition = new Vector3(0,y_pos,0);

        if(lst_old.Count <= 0)
        {
            mOldTitle.SetActive(false);
        }
        else
        {
            mOldTitle.SetActive(true);
        }
        for(int i = 0 ; i<lst_old.Count ; i++)
        {
            GameObject obj = GameObject.Instantiate(Resources.Load("UIDailyMissionItem")) as GameObject;
            obj.transform.parent = mOldItemParent.transform;
            obj.transform.localPosition = new Vector3(0,300*i,0);
            obj.transform.localScale = Vector3.one;

            UIDailyMissionItem mission_item = obj.GetComponent<UIDailyMissionItem>();
            mission_item.SetMission(lst_old[i]);
            mission_item.mBtnFinish.SetData("d",lst_old[i]);
            mission_item.mBtnFinish.SetData("t",1);
            mission_item.mBtnFinish.onClick = BtnMissionFinishOnClick;
            mission_item.mBtnCancel.SetData("d",lst_old[i]);
            mission_item.mBtnCancel.SetData("t",1);
            mission_item.mBtnCancel.onClick = BtnMissionCancelOnClick;
        }

        if(lst_now.Count > 0 )
        {
            mNowItemParent.SetActive(true);
            y_pos += lst_old.Count * 300;
            mNowItemParent.transform.localPosition = new Vector3(0,y_pos,0);
            for(int i = 0 ; i<lst_now.Count ; i++)
            {
                GameObject obj = GameObject.Instantiate(Resources.Load("UIDailyMissionItem")) as GameObject;
                obj.transform.parent = mNowItemParent.transform;
                obj.transform.localPosition = new Vector3(0,300*i,0);
                obj.transform.localScale = Vector3.one;

                UIDailyMissionItem mission_item = obj.GetComponent<UIDailyMissionItem>();
                mission_item.SetMission(lst_now[i]);
                mission_item.mBtnFinish.SetData("d",lst_now[i]);
                mission_item.mBtnFinish.SetData("t",2);
                mission_item.mBtnFinish.onClick = BtnMissionFinishOnClick;
                mission_item.mBtnCancel.gameObject.SetActive(false);
            }
        }
        else
        {
            mNowItemParent.SetActive(false);
        }

        if(lst_finished.Count > 0)
        {
            mFinishedItemParent.SetActive(true);
            y_pos += lst_now.Count * 300;
            mFinishedItemParent.transform.localPosition = new Vector3(0,y_pos,0);
            for(int i = 0 ; i<lst_finished.Count ; i++)
            {
                GameObject obj = GameObject.Instantiate(Resources.Load("UIDailyMissionItem")) as GameObject;
                obj.transform.parent = mFinishedItemParent.transform;
                obj.transform.localPosition = new Vector3(0,300*i,0);
                obj.transform.localScale = Vector3.one;

                UIDailyMissionItem mission_item = obj.GetComponent<UIDailyMissionItem>();
                mission_item.SetMission(lst_finished[i]);
                mission_item.mBtnCancel.SetData("d",lst_finished[i]);
                mission_item.mBtnCancel.SetData("t",3);
                mission_item.mBtnCancel.onClick = BtnMissionCancelOnClick;
                mission_item.mBtnFinish.gameObject.SetActive(false);
            }
        }
        else
        {
            mFinishedItemParent.SetActive(false);
        }
    }


    ///////////////// button
    void BtnPersonalOnClick(PointerEventData eventData , UI_Event ev)
    {
        // todo goto peronsal ui
        CloseScreen();
        UIPersonal ui_personal = MenuManager.instance.CreateMenu<UIPersonal>();
        ui_personal.OpenScreen();
    }

    void BtnSidebarOnClick(PointerEventData eventData , UI_Event ev)
    {
        //
    }

    void BtnMissionFinishOnClick(PointerEventData eventData , UI_Event ev)
    {
        Mission mis = ev.GetData<Mission>("d");
        mis.mDateTime = DateTime.Now;
        mis.mFinished = 1;
    }

    void BtnMissionCancelOnClick(PointerEventData eventData , UI_Event ev)
    {
        int type = ev.GetData<Mission>("t");

        if(type == 1)
        {
            Mission mis = ev.GetData<Mission>("d");
            mis.mDateTime = TimeConvert.GetNow();
        }
        else if(type == 3)
        {
            Mission mis = ev.GetData<Mission>("d");
            mis.mFinished = 0;
        }
    }
}