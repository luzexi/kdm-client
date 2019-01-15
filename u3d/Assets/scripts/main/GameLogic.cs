using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
	void Start()
	{
		UIDailyMission ui_daily_mission = MenuManager.instance.CreateMenu<UIDailyMission>();
		if(ui_daily_mission != null)
		{
			ui_daily_mission.OpenScreen();
			List<Mission> lst_mis = MissionManager.instance.GetDailyMission();
			ui_daily_mission.ShowMission(lst_mis);
		}
	}
}