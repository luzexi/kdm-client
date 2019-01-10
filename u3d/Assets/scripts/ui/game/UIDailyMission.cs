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

            UIDailyMissionItem mission_item = obj.GetComponent<UIDailyMissionItem>();
            mission_item.SetMission(lst_old[i]);
        }

        for(int i = 0 ; i<lst_now.Count ; i++)
        {
            GameObject obj = GameObject.Instantiate(Resources.Load("UIDailyMissionItem")) as GameObject;
            obj.transform.parent = mOldItemParent.transform;
            obj.transform.localPosition = new Vector3(0,300*i,0);

            UIDailyMissionItem mission_item = obj.GetComponent<UIDailyMissionItem>();
            mission_item.SetMission(lst_now[i]);
        }
        for(int i = 0 ; i<lst_finished.Count ; i++)
        {
            GameObject obj = GameObject.Instantiate(Resources.Load("UIDailyMissionItem")) as GameObject;
            obj.transform.parent = mOldItemParent.transform;
            obj.transform.localPosition = new Vector3(0,300*i,0);
            UIDailyMissionItem mission_item = obj.GetComponent<UIDailyMissionItem>();
            mission_item.SetMission(lst_finished[i]);
        }
    }


    ///////////////// button
    void BtnPersonalOnClick(PointerEventData eventData , UI_Event ev)
    {
        //
    }
}