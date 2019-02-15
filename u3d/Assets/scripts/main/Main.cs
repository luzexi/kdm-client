using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
	public enum State
	{
		None,
		Init,
		LoadingInit,
		LoadingInitWaiting,
		UpdateAB,
		UpdateABWaiting,
		InitInfo,
		InitInfoWaiting,
		StartGame,
		End,
	}

	private State mState = State.None;

	void Awake()
	{
#if !UNITY_EDITOR
		Reporter.Init();
#endif
	}

	// Use this for initialization
	void Start ()
	{
		mState = State.Init;
	}
	
	// Update is called once per frame
	void Update()
	{
		switch( mState )
		{
			case State.Init:
				Init();
				break;
			case State.LoadingInit:
				Loading();
				break;
			case State.LoadingInitWaiting:
				break;
			case State.UpdateAB:
				UpdateAB();
				break;
			case State.UpdateABWaiting:
				UpdateABWaiting();
				break;
			case State.InitInfo:
				InitInfo();
				break;
			case State.InitInfoWaiting:
				break;
			case State.StartGame:
				StartGame();
				break;
		}
	}

	void Init()
	{
		TableManager.instance.InitCommonProperty();
		
		//init ui
		MenuManager.instance.SetTransform(GlobalObject.instance.UIRoot.transform);

		//init language
		TextManager.instance.LoadLanguage(TextManager.LANGUAGE.EN);

		//get language string
		string text = TextManager.instance.GetText(TText.SUCCESS);
		Debug.LogError(text);

		AccountInfo.instance.Init();

		UIEnter enter_handle = MenuManager.instance.CreateMenu<UIEnter>();
		enter_handle.OpenScreen();
		enter_handle.mStartFunction = FinishLoading;

		mState = State.LoadingInit;
	}

	void FinishLoading()
	{
		mState = State.StartGame;
	}

	void Loading()
	{
		mState = State.LoadingInitWaiting;
	}

	void InitFinished()
	{
#if UNITY_EDITOR
		mState = State.InitInfo;
		return;
#endif
		mState = State.UpdateAB;
	}

	void UpdateAB()
	{
		mState = State.UpdateABWaiting;
	}

	void UpdateABWaiting()
	{
		mState = State.InitInfo;
		return;
	}

	public void InitInfo()
	{
		//GameNetwork.RequestGetAccount();
		mState = State.InitInfoWaiting;
	}

	void StartGame()
	{
		//close ui
		UIEnter loading_handle = MenuManager.instance.FindMenu<UIEnter>();
		if(loading_handle)
		{
			loading_handle.CloseScreen();
		}
		Debug.LogError("start game");

		MissionManager.instance.Load();
		StatisticsManager.instance.Load();
		PlayerInfo.instance.Load();

		SceneManager.LoadScene("game");

		mState = State.End;
	}
}

