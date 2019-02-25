using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ChartAndGraph;


// ui statistics
public class UIStatistics : ScreenBaseHandler
{
    private const string EXP_CATEGORY_NAME = "EXP";
	public UI_Event mBtnBack;
    public GraphChartBase mGraphExp;

    public override void Init()
    {
        base.Init();
        mBtnBack.onClick = BtnBackOnClick;        
    }

    public override void CloseScreen()
    {
        base.CloseScreen();
    }

    public override void OpenScreen()
    {
        base.OpenScreen();
    }

    void InitGraphic()
    {
        List<MissionDayLog> lst_mis_day_log = StatisticsManager.instance.GetAllMissionDayLog();
        for(int i = 0 ; i<lst_mis_day_log.Count ; i++)
        {
            MissionDayLog day_log = lst_mis_day_log[i];
            mGraphExp.DataSource.AddPointToCategory(EXP_CATEGORY_NAME, i*10, day_log.mTotalExp, 1);
        }

        mGraphExp.DataSource.StartBatch();
        for (int i=0; i<Categories.Length; i++)
        {

            
            mGraphExp.DataSource.AddCategory(EXP_CATEGORY_NAME, null, 0, new MaterialTiling(), null, false, null, 0);
            mGraphExp.DataSource.RestoreCategory(EXP_CATEGORY_NAME, visualStyle);    // set the visual style of the category to the one in the prefab
            var loader = mLoaders[cat.DataType];    // find the loader based on the data type
            loader(cat); // load the category data
        }
        mGraphExp.DataSource.EndBatch();
        
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

