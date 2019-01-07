using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
//using MsgPack;
using MiniJSON;

// using GameProtocol;

#if (UNITY_METRO || UNITY_WP8)
	#if UNITY_EDITOR
	// Must use Ionic in Win metro/phone8 UNITY EDITOR because ComponentAce doesn't work.
	//using Ionic.Zlib;
	#else
	// ComponentAce and Ionic don't work on actual Windows devices.
	using System.IO.Compression;
	#endif
#endif

public class NetworkManager
{
	#region variables 
	
	public enum EState
	{
		Idle = 0,
		WaitingResponse,
		WaitingCombinedResponse,
		WaitingImageResponse,
		WaitingImageUrlResponse,
		Timeout,
		DataError,
	}
	
	//static BoxingPacker sPacker = new BoxingPacker();
	
	static NetworkManager s_instance = null;
	public static NetworkManager instance
	{
		get
		{
			if (s_instance == null)
			{
				s_instance = new NetworkManager();
			}
			return s_instance;
		}	
	}
	
	public static string sKey = string.Empty;
	
	public static bool NETWORK_STOP_WHEN_ERROR = true;
	
	public static bool NETWORK_CAL_RESPONDING_TIME = false;
	
	public static bool NETWORK_SINGLE_WWW_GET_IMAGE = false;

	public static bool NETWORK_SINGLE_URL_WWW_GET_IMAGE = true;
	
	bool mEnableNetwork = true;
	
	EState mState = EState.Idle;
		
	// common http connection for common usuage except game logic
	CommonHttpConnect mCommonHttpConnect;
	
	// list of all other requests except game logic, include account, upgrade, config etc.
	public List<HttpRequest> mHttpRequests = new List<HttpRequest>();
		
	// combine game requests into one Http connection in one time
	CombinedHttpConnect mCombinedHttpConnect;
	
	// list of all game requests
	public List<NetRequest> mRequestStack = new List<NetRequest>();

	/// <summary>
	/// url  get image
	/// </summary>
	ImageUrlHttpConnect mImageUrlHttpConnect;

	// // the Http connection to get some image
	// ImageHttpConnect mImageHttpConnect;
	
	// the bug report Http connection
	CombinedHttpConnect mReportBugHttpConnect;
	
	// // upload replay Http connection
	// CombinedHttpConnect mUploadReplayHttpConnect;
	
	
	float mConnectTime = 0;
	float mIdleTime = 0;
	float mHeartTime = 0;
	float mCurTime = 0;
	
	int mCurRequestSerialId = 0;
	
	public int mErrorRequestSerialId = 0;
	public int mErrorCommandId = 0;
	public long mErrorCode = 0;
	public string mErrorText;
	bool mHasReportedError;
	
	public float mRespondTime = 0;
	public float mMaxRespondTime = 0;
		
	bool mEnableHeartbeat = false;
	// bool mCatchingImage = false;
	// float mCatchingImageTime = 0;

	bool mCatchingImageUrl = false;
	float mCatchingImageUrlTime = 0;
	
	int mRetryTimes = 0;
	int mRetryCombineTimes = 0;
	
	// static byte[] mCompressBuffer = new byte[2000];
	
	// float HeartBeatInterval
	// {
	// 	set 
	// 	{
	// 		NetworkDefine.sHeartBeatInterval = value;
	// 	}
	// 	get
	// 	{
	// 		return NetworkDefine.sHeartBeatInterval;
	// 	}
	// }

    bool mWaitingDialog = false;

	#endregion

	#region public interface
	
	public void Start()
	{
		Clear();
		mEnableNetwork = true;
		// StopHeartbeat();
		mState = EState.Idle;
	}
	
	public void Stop()
	{
		// StopHeartbeat();
		mEnableNetwork = false;
	}
	
	private void Clear()
	{
		mHttpRequests.Clear();
		mRequestStack.Clear();
		if (mCommonHttpConnect != null)
			mCommonHttpConnect.Release();
		if (mCombinedHttpConnect != null)
			mCombinedHttpConnect.Release();
		// if (mImageHttpConnect != null)
		// 	mImageHttpConnect.Release2();
		// mCatchingImage = false;
		mRetryTimes = 0;
		mRetryCombineTimes = 0;
	}
	
	public bool IsEnableNetwork()
	{
		return mEnableNetwork;
	}
	
	public void Update()
	{
		if (!mEnableNetwork)
		{
			return;
		}
		
		mCurTime = Time.realtimeSinceStartup;
		
		switch (mState)
		{
		case EState.Idle:
	        if (mWaitingDialog && mRetryTimes == 0 && mRetryCombineTimes == 0)
	        {
	            // MessageBox.instance.CloseWaitingDialog();
	        }
			bool isFreeInStack = true;
			int commonHttpRequestSerialId = int.MaxValue;
			int combineHttpRequestSerialId = int.MaxValue;
			if (mHttpRequests.Count > 0)
			{
				isFreeInStack = false;
				commonHttpRequestSerialId = mHttpRequests[0].serialId;
			}
			if (mRequestStack.Count > 0)
			{
				isFreeInStack = false;
				combineHttpRequestSerialId = mRequestStack[0].serialId;
			}
			
			if (mHttpRequests.Count > 0 && commonHttpRequestSerialId < combineHttpRequestSerialId)
			{
				ProcessNextHttpConnect();
			}
			else if (mRequestStack.Count > 0)
			{
				if ((NetworkDefine.sRequestIntervalTime > 0 && mCurTime - mIdleTime >= NetworkDefine.sRequestIntervalTime)
					|| NetworkDefine.sRequestIntervalTime == 0 
					|| commonHttpRequestSerialId < int.MaxValue)
				{
					ProcessNextCombinedConnect(commonHttpRequestSerialId);
				}
			}
			// else if (NETWORK_SINGLE_WWW_GET_IMAGE && AvatarManager.instance.GetNextAvatarURL() != null)
			// {
			// 	ProcessNextImageConnect(AvatarManager.instance.GetNextAvatarURL());
			// }
			// else if (NETWORK_SINGLE_URL_WWW_GET_IMAGE && EventImageManager.instance.GetNextImageURL() != null)
			// {
			// 	ProcessNextImageUrlConnect(EventImageManager.instance.GetNextImageURL());
			// }
			// // only when no other requests, send heart beat
			// if (NetworkDefine.sHeartbeatRequest && mEnableHeartbeat)
			// {
			// 	if (mCurTime - mHeartTime >= 120 || (mCurTime - mHeartTime >= HeartBeatInterval && isFreeInStack))
			// 	{
			// 		UpdateHeartbeat();
			// 	}
			// }
			break;
		
		case EState.WaitingResponse:
			CheckCurrentHttpConnectStatus();
		break;
			
		case EState.WaitingCombinedResponse:
			CheckCurrentCombinedConnectStatus();
			break;
			
		case EState.WaitingImageResponse:
			// CheckCurrentImageConnectStatus();
			break;	
		case EState.WaitingImageUrlResponse:
			CheckCurrentImageUrlConnectStatus();
			break;
		case EState.Timeout:
			break;
			
		case EState.DataError:
			if (mHasReportedError)
			{
				if (mErrorText != null)
				{
					// send bug report
					// ReportBugRequest request = new ReportBugRequest(null);
					// request.content = "id:"+mErrorCommandId + " content:"+mErrorText;
					//SendBugReport(request);
					
					mErrorText = null;
				}
				mHasReportedError = false;
			}
			break;
			
		default:
			break;
		}
		
		// if (!NETWORK_SINGLE_WWW_GET_IMAGE)
		// {
		// 	if (!mCatchingImage)
		// 	{
		// 		if (AvatarManager.instance.GetNextAvatarURL() != null)
		// 		{
		// 			ProcessNextImageConnect(AvatarManager.instance.GetNextAvatarURL());
		// 		}
		// 	}
		// 	else
		// 	{
		// 		CheckCurrentImageConnectStatus();
		// 	}
		// }
		
		HandlerSpecialHttpConnect();
	}
	
	// void UpdateHeartbeat()
	// {
 //        //if (!string.IsNullOrEmpty(AccountManager.instance.GetAccount().mUId)
 //        //    && !string.IsNullOrEmpty(AccountManager.instance.GetAccount().mSessionId))
 //        if (!string.IsNullOrEmpty(AccountService.Instance.UId)
 //            && !string.IsNullOrEmpty(AccountService.Instance.SessionId))
	// 	{
	// 		mHeartTime = mCurTime;
	// 		StatisticsManager.instance.Request_Heartbeat();
	// 	}
	// }
	
	// public void StartHeartbeat()
	// {
	// 	StatisticsManager.instance.Reset();
	// 	mEnableHeartbeat = true;
		
 //        //if (!string.IsNullOrEmpty(AccountManager.instance.GetAccount().mUId)
 //        //    && !string.IsNullOrEmpty(AccountManager.instance.GetAccount().mSessionId))
 //        if (!string.IsNullOrEmpty(AccountService.Instance.UId)
 //            && !string.IsNullOrEmpty(AccountService.Instance.SessionId))
	// 	{
	// 		mHeartTime = Time.realtimeSinceStartup;
	// 		StatisticsManager.instance.Request_Heartbeat();
	// 	}
	// }
	
	// private void StopHeartbeat()
	// {
	// 	mEnableHeartbeat = false;
	// }

    //public HttpRequest CreateHttpRequest(string _url, Dictionary<string, object> _hash, string _key, HttpRequest.CallbackFunc _func, int _appFlag)
    //{
    //    string[] url = new string[]
    //    {
    //        _url
    //    };
    //    return CreateHttpRequest(url, _hash, _key, _func, _appFlag);
    //}

	// create a common Http request into request list
    public HttpRequest CreateHttpRequestWithUri(string _url, string uri, Dictionary<string, object> _hash, string _key, HttpRequest.CallbackFunc _func, int _appFlag)
    {
        // string[] urls = new string[_url.Length];
        // for (int i = 0; i < _url.Length; ++i)
        // {
        //     urls[i] = _url[i] + uri;
        // }
        return CreateHttpRequest(_url + uri, _hash, _key, _func, _appFlag);
    }
    public HttpRequest CreateHttpRequest(string _url, Dictionary<string, object> _hash, string _key, HttpRequest.CallbackFunc _func, int _appFlag)
	{
		// bool compressed_rep = (NetworkDefine.sCompressedReponse & (1<<_appFlag)) != 0;
		
		HttpRequest request = new HttpRequest(_url, _hash, _key, _func, false);
		
		mCurRequestSerialId++;
		if (mCurRequestSerialId == int.MaxValue) mCurRequestSerialId = 1;
		request.serialId = mCurRequestSerialId;
		
		lock (mHttpRequests)
		{
			mHttpRequests.Add(request);
		}
		
		return request;
	}
	
	// add a game request into request list, and that can be combined in one http connection with others
	public void AddGameRequest(NetRequest _request, bool _sync)
	{
		if (_request != null)
		{
			mCurRequestSerialId++; 
            if (mCurRequestSerialId == int.MaxValue) 
                mCurRequestSerialId = 1;
			_request.serialId = mCurRequestSerialId;
			
			if (!NetworkDefine.sCombineRequest)
			{
				_request.canCombined = false;
			}
			
			lock (mRequestStack)
			{
                mRequestStack.Add(_request);
			}
			
			// if the request is synchronized(blocked), the request will be send out in next Update() if no requests before it
			if (_sync)
			{
				mIdleTime = 0;
			}
		}
	}
	
	public EState GetState()
	{
		return mState;
	}

    bool IsIPStyle(string _url)
    {
        string[] splits = _url.Split(new char[] { ':', '.', '/' });
        int intCount = 0;
        for (int i = 0; i < splits.Length; ++i)
        {
            int tmp;
            if (int.TryParse(splits[i], out tmp) && tmp >= 0 && tmp < 256)
                intCount++;
        }
        if (intCount >= 4)
            return true;
        return false;
    }
    #endregion

    #region network logic

    void ProcessNextHttpConnect()
	{
		if (mCommonHttpConnect == null)
		{
			mCommonHttpConnect = new CommonHttpConnect();
		}
		
		mCommonHttpConnect.Initial(mHttpRequests[0]);
		
		if (mCommonHttpConnect.Connect())
		{
			mConnectTime = mCurTime;
			mState = EState.WaitingResponse;
		}
	}
	
	void CheckCurrentHttpConnectStatus()
	{
		if (mCommonHttpConnect != null)
		{
			// if request got response, handler the response and refresh requests list
			if (mCommonHttpConnect.IsGotResponse())
			{
				bool result = mCommonHttpConnect.ParseResponse();
				//bool canRetry = mCommonHttpConnect.CanRetryWhenErrorOrTimeout();
				bool canIgnore = mCommonHttpConnect.CanIgnoreWhenErrorOrTimeout();
				if (!result && canIgnore)
				{
					mCommonHttpConnect.Ignore();
				}
				
				
				if (NETWORK_CAL_RESPONDING_TIME)
				{
					mRespondTime = mCurTime - mConnectTime;
					if (mRespondTime > mMaxRespondTime)
						mMaxRespondTime = mRespondTime;
				}
				
				if (result)
				{
					mState = EState.Idle;
                    if (mRetryTimes > 0)
                    {
                        // if (IsIPStyle(mCommonHttpConnect.URL))
                        //     NetworkDefine.RegroupURLWithIPFirst();
                        // else
                        //     NetworkDefine.RegroupURLWithURLFirst();
                    }
					mRetryTimes = 0;
				}
				// www.error != null
				else
				{
                    bool canRetry = mCommonHttpConnect.CanRetryWhenErrorOrTimeout();
                    if (canRetry && mRetryTimes < NetworkDefine.sMaxRetryTime)
					{
						mRetryTimes++;
						mState = EState.Idle;
                        
                        mWaitingDialog = true;
                        // Debug.LogError("retry 11111");
                        // MessageBox.instance.ShowReconnecting(true);
                        Debug.Log("Retry "+mRetryTimes);
					}
					else
					{
                        if (mCommonHttpConnect.FailOperation() == HttpRequest.FailOperationType.ErrorDialog)
                        {
                            // string title = TextManager.GetInstance().GetText(Text.NET_ERROR);
                            // string str = TextManager.GetInstance().GetText(Text.NO_NETWORK_CONNECTION);

                            // MessageBox.instance.OpenLiveMessageBox(MessageBox.eStyle.NoButton, title, str, handler_error_release, null);

                            mHasReportedError = false;
                            mState = EState.DataError;
                        }
                        else
                        {
                            mState = EState.Idle;
                        }

                        System.Action cb = mCommonHttpConnect.FailCallBack();
                        // MessageBox.instance.CloseWaitingDialog();
                        Clear();
                        if (cb != null)
                            cb();
					}
				}

                mCommonHttpConnect.Release();
			}
			else
			{
				if (mCurTime - mConnectTime > NetworkDefine.sConnectTimeout)
				{
					if (NETWORK_CAL_RESPONDING_TIME)
					{
						mRespondTime = mCurTime - mConnectTime;
						if (mRespondTime > mMaxRespondTime)
							mMaxRespondTime = mRespondTime;
					}
					
					bool canRetry = mCommonHttpConnect.CanRetryWhenErrorOrTimeout();
					bool canIgnore = mCommonHttpConnect.CanIgnoreWhenErrorOrTimeout();
					if (canIgnore)
					{
						mCommonHttpConnect.Ignore();
					}
					
					if ((canRetry || canIgnore) && mRetryTimes < NetworkDefine.sMaxRetryTime)
					{
						mRetryTimes++;
						mState = EState.Idle;

                        mWaitingDialog = true;
                        // Debug.LogError("retry 22222");
                        // MessageBox.instance.ShowReconnecting(true);
                        Debug.Log("Retry " + mRetryTimes);
					}
					else
					{
                        if (mCommonHttpConnect.FailOperation() == HttpRequest.FailOperationType.ErrorDialog)
                        {
                            // string title = TextManager.GetInstance().GetText(Text.NET_ERROR);
                            // string str = TextManager.GetInstance().GetText(Text.NO_NETWORK_CONNECTION);

                            // MessageBox.instance.OpenLiveMessageBox(MessageBox.eStyle.NoButton, title, str, handler_error_release, null);
                            mState = EState.Timeout;
                        }
                        else
                        {
                            mState = EState.Idle;
                        }
                        System.Action cb = mCommonHttpConnect.FailCallBack();
                        // MessageBox.instance.CloseWaitingDialog();
                        Clear();
                        if (cb != null)
                            cb();
					}
                    mCommonHttpConnect.Release();
				}
			}
		}
		else
		{
			mState = EState.Idle;
		}
	}
	
	void ProcessNextCombinedConnect(int maxSerialId)
	{
		if (mCombinedHttpConnect == null)
		{
			mCombinedHttpConnect = new CombinedHttpConnect();
		}
		
		mCombinedHttpConnect.Initial(mRequestStack, maxSerialId);
		
		if (mCombinedHttpConnect.Connect())
		{
			mConnectTime = mCurTime;
			mIdleTime = mCurTime;
			mState = EState.WaitingCombinedResponse;
		}
	}
		
	void CheckCurrentCombinedConnectStatus()
	{
		if (mCombinedHttpConnect != null)
		{
			// if request got response, handler the response and refresh requests list
			if (mCombinedHttpConnect.IsGotResponse())
			{
				bool result = mCombinedHttpConnect.ParseResponse();							
				mCombinedHttpConnect.Release();
				
				if (NETWORK_CAL_RESPONDING_TIME)
				{
					mRespondTime = mCurTime - mConnectTime;
					if (mRespondTime > mMaxRespondTime)
						mMaxRespondTime = mRespondTime;
				}
				//RefreshAllRequests();
				
				if (result)
				{
					mState = EState.Idle;
                    if (mRetryCombineTimes > 0)
                    {
                        // if (IsIPStyle(mCombinedHttpConnect.GetGameServerURL()))
                        //     NetworkDefine.RegroupURLWithIPFirst();
                        // else
                        //     NetworkDefine.RegroupURLWithURLFirst();
                    }
					mRetryCombineTimes = 0;
				}
				else
				{
					// if (mErrorCode == 201 && mRetryCombineTimes < NetworkDefine.sMaxRetryTime)
					// {
					// 	mRetryCombineTimes++;
					// 	mState = EState.Idle;

     //                    mWaitingDialog = true;
     //                    // Debug.LogError("retry 333");
     //                    MessageBox.instance.ShowReconnecting(true);
     //                    DebugUtils.LogNetwork("Retry "+mRetryCombineTimes);
					// }
					// else
					// {
					// 	string title = TextManager.GetInstance().GetText(Text.NET_ERROR);
					// 	string str;
						
					// 	if (mErrorCode == 201)
					// 	{
					// 		str = TextManager.GetInstance().GetText(Text.NO_NETWORK_CONNECTION);
					// 	}
					// 	// special error for invalid session
					// 	else if (mErrorCode == 69000001)
					// 	{
					// 		str = TextManager.GetInstance().GetText(Text.LOGIN_SESSION_OUT_OF_DATE);
					// 	}
					// 	// special error string for attacking online or shield players
					// 	else if (mErrorCode == 1009012 || mErrorCode == 1009014)
					// 	{
					// 		title = TextManager.GetInstance().GetText(Text.WARN);
					// 		if (mErrorCode == 1009012)
					// 		{
					// 			str = TextManager.GetInstance().GetText(Text.BATTLE_PLAYER_ONLINE);
					// 		}
					// 		else
					// 		{
					// 			str = TextManager.GetInstance().GetText(Text.BATTLE_PLAYER_IN_SHIELD);
					// 		}
					// 	}
					// 	else
					// 	{
					// 		str = TextManager.GetInstance().GetText(Text.NET_RESPONSE_DATA_ERROR);
					// 		str += " "+mErrorRequestSerialId+", cmd:"+mErrorCommandId + ", code:"+mErrorCode;
					// 	}
					// 	MessageBox.instance.CloseWaitingDialog();

						
					// 	if (mErrorCode == 10001002 || mErrorCode == 10001003 || mErrorCode == 10001004 || mErrorCode == 10001005 || mErrorCode == 10001006 || mErrorCode == 10001009)
					// 	{ 
					// 		CommonDataManager.RequestGetResource();
					// 		title = TextManager.GetInstance().GetText(Text.WARN);
					// 		string StrWarning = TextManager.GetInstance().GetText(Text.Resource_Error_Warning);
					// 		MessageBox.instance.OpenMessageBox(MessageBox.eStyle.OK, title, StrWarning, null, null);
					// 		return;
					// 	}
					// 	MessageBox.instance.OpenLiveMessageBox(MessageBox.eStyle.NoButton, title, str, handler_error_release, null);
					// 	mState = EState.DataError;
					// 	mHasReportedError = false;
					// 	Clear();
					// }
				}
			}
			else
			{
				// Debug.LogError("mCurTime " + mCurTime);
				// Debug.LogError("mConnectTime " + mConnectTime);
				// Debug.LogError("NetworkDefine.sConnectTimeout " + NetworkDefine.sConnectTimeout);
				if (mCurTime - mConnectTime > NetworkDefine.sConnectTimeout)
				{
					mCurTime = mConnectTime + NetworkDefine.sConnectTimeout + 1f;
					// Debug.LogError("2mCurTime " + mCurTime);
					// Debug.LogError("2mConnectTime " + mConnectTime);
					// Debug.LogError("2NetworkDefine.sConnectTimeout " + NetworkDefine.sConnectTimeout);
					if (NETWORK_CAL_RESPONDING_TIME)
					{
						mRespondTime = mCurTime - mConnectTime;
						if (mRespondTime > mMaxRespondTime)
							mMaxRespondTime = mRespondTime;
					}
					
					mCombinedHttpConnect.Release();
					
					if (mRetryCombineTimes < NetworkDefine.sMaxRetryTime)
					{
						// Debug.LogError("mRetryCombineTimes " + mRetryCombineTimes);
						// Debug.LogError("NetworkDefine.sMaxRetryTime " + NetworkDefine.sMaxRetryTime);
						mRetryCombineTimes++;
						mState = EState.Idle;

                        mWaitingDialog = true;
                        // MessageBox.instance.ShowReconnecting(true);

                        Debug.Log("Retry "+mRetryCombineTimes);
					}
					else
					{
						// // Debug.LogError("retry times out");
						// string title = TextManager.GetInstance().GetText(Text.NET_ERROR);
						// string str = TextManager.GetInstance().GetText(Text.NO_NETWORK_CONNECTION);
						// MessageBox.instance.CloseWaitingDialog();
						// MessageBox.instance.OpenLiveMessageBox(MessageBox.eStyle.NoButton, title, str, handler_error_release, null);
						mState = EState.Timeout;
						Clear();
					}
				}
			}
		}
		else
		{
			mState = EState.Idle;
		}
	}
	
	void ProcessNextImageUrlConnect(string _url)
	{
		if (mImageUrlHttpConnect== null)
		{
			mImageUrlHttpConnect = new ImageUrlHttpConnect();
		}
		
		mImageUrlHttpConnect.Initial(_url);
		if (mImageUrlHttpConnect.Connect())
		{
			Debug.Log("Catching image:" + _url);
			mCatchingImageUrlTime = mCurTime;
			if (NETWORK_SINGLE_URL_WWW_GET_IMAGE)
			{
				mState = EState.WaitingImageUrlResponse;
			}
			else
			{
				mCatchingImageUrl = true;
			}
		}
	}
	// void ProcessNextImageConnect(string _url)
	// {
	// 	if (mImageHttpConnect == null)
	// 	{
	// 		mImageHttpConnect = new ImageHttpConnect();
	// 	}
		
	// 	mImageHttpConnect.Initial(_url);
	// 	//mCatchingImageTime = mCurTime;
	// 	if (mImageHttpConnect.Connect())
	// 	{
	// 		DebugUtils.LogNetwork("Catching image:" + _url);
	// 		mCatchingImageTime = mCurTime;
	// 		if (NETWORK_SINGLE_WWW_GET_IMAGE)
	// 		{
	// 			mState = EState.WaitingImageResponse;
	// 		}
	// 		else
	// 		{
	// 			mCatchingImage = true;
	// 		}
	// 	}
	// }
	
	// void CheckCurrentImageConnectStatus()
	// {
	// 	if (mImageHttpConnect != null)
	// 	{
	// 		// if request got response, handler the response and refresh requests list
	// 		if (mImageHttpConnect.IsGotResponse())
	// 		{
	// 			string url = mImageHttpConnect.GetURL();
	// 			Texture tex = mImageHttpConnect.ParseResponse();
				
	// 			AvatarManager.instance.CacheTheImage(url, tex);
	// 			DebugUtils.LogNetwork("Cached image:" + url);
				
	// 			mImageHttpConnect.Release();
				
	// 			if (NETWORK_SINGLE_WWW_GET_IMAGE)
	// 			{
	// 				mState = EState.Idle;
	// 			}
	// 			else
	// 			{
	// 				mCatchingImage = false;
	// 			}
	// 		}
	// 		else
	// 		{
	// 			// only wait 5 second to catch the facebook image
	// 			if (mCurTime - mCatchingImageTime > 10)
	// 			{
	// 				string url = mImageHttpConnect.GetURL();
	// 				DebugUtils.LogNetwork("Time out for image:"+url);
	// 				AvatarManager.instance.FailCacheTheImage(url);
					
	// 				//mCatchingImageTime = Time.realtimeSinceStartup;
	// 				mImageHttpConnect.Release2();
					
	// 				//DebugUtils.LogNetwork("release time:" + (Time.realtimeSinceStartup-mCatchingImageTime));
					
	// 				if (NETWORK_SINGLE_WWW_GET_IMAGE)
	// 				{
	// 					mState = EState.Idle;
	// 				}
	// 				else
	// 				{
	// 					mCatchingImage = false;
	// 				}
	// 			}
	// 		}
	// 	}
	// 	else
	// 	{
	// 		if (NETWORK_SINGLE_WWW_GET_IMAGE)
	// 		{
	// 			mState = EState.Idle;
	// 		}
	// 		else
	// 		{
	// 			mCatchingImage = false;
	// 		}
	// 	}
	// }
	
	void CheckCurrentImageUrlConnectStatus()
	{
		if (mImageUrlHttpConnect != null)
		{
			// if request got response, handler the response and refresh requests list
			if (mImageUrlHttpConnect.IsGotResponse())
			{
				string url = mImageUrlHttpConnect.GetURL();
				Texture2D tex = mImageUrlHttpConnect.ParseResponse();
				
				// EventImageManager.instance.CacheTheImage(url, tex);
				Debug.Log("Cached image:" + url);
				
				mImageUrlHttpConnect.Release();
				
				if (NETWORK_SINGLE_URL_WWW_GET_IMAGE)
				{
					mState = EState.Idle;
				}
				else
				{
					mCatchingImageUrl = false;
				}
			}
			else
			{
				// only wait 5 second to catch the facebook image
				if (mCurTime - mCatchingImageUrlTime > 10)
				{
					string url = mImageUrlHttpConnect.GetURL();
					Debug.Log("Time out for image:" + url);
					// EventImageManager.instance.FailCacheTheImage(url);
					
					//mCatchingImageTime = Time.realtimeSinceStartup;
					mImageUrlHttpConnect.Release2();
					
					//DebugUtils.LogNetwork("release time:" + (Time.realtimeSinceStartup-mCatchingImageTime));
					
					if (NETWORK_SINGLE_URL_WWW_GET_IMAGE)
					{
						mState = EState.Idle;
					}
					else
					{
						mCatchingImageUrl = false;
					}
				}
			}
		}
		else
		{
			if (NETWORK_SINGLE_URL_WWW_GET_IMAGE)
			{
				mState = EState.Idle;
			}
			else
			{
				mCatchingImageUrl = false;
			}
		}
	}
	
	// void SendBugReport(ReportBugRequest _request)
	// {
	// 	mCurRequestSerialId++; if (mCurRequestSerialId == int.MaxValue) mCurRequestSerialId = 1;
	// 	_request.serialId = mCurRequestSerialId;
		
	// 	if (mReportBugHttpConnect == null)
	// 	{
	// 		mReportBugHttpConnect = new CombinedHttpConnect();
	// 	}
	// 	else
	// 	{
	// 		mReportBugHttpConnect.Release();
	// 	}
		
	// 	List<NetRequest> requests = new List<NetRequest>();
	// 	requests.Add(_request);
	// 	mReportBugHttpConnect.Initial(requests, int.MaxValue);
	// 	mReportBugHttpConnect.Connect();
	// }


    void HandlerSpecialHttpConnect()
	{
		if (mReportBugHttpConnect != null)
		{
			if (mReportBugHttpConnect.IsGotResponse())
			{
				mReportBugHttpConnect.Release();
				mReportBugHttpConnect = null;
			}
		}
		// if (mUploadReplayHttpConnect != null)
		// {
		// 	if (mUploadReplayHttpConnect.IsGotResponse())
		// 	{
		// 		mUploadReplayHttpConnect.Release();
		// 		mUploadReplayHttpConnect = null;
		// 	}
		// }
	}
	
	// remove all requests which flag is "isDone"
	/*void RefreshAllRequests()
	{
		int stackCount = mRequestStack.Count - 1;
		for (int i=stackCount; i>=0; i--)
		{
			if (mRequestStack[i].isDone)
			{
				mRequestStack.RemoveAt(i);
			}
		}
		
	}*/
	
	// void handler_error_release(EQEQEvent _e)
	// {
	// 	// go to appInit Scene
	// 	if (NETWORK_STOP_WHEN_ERROR)
	// 	{
	// 		// LevelManager.Instance.LoadLevelAsync(Level.AppInit);
	// 		// Application.LoadLevel(LevelManager.Instance.GetLevelName(LevelManager.Instance.GetCurLevel()));
	// 	}
	// 	else
	// 	{
	// 		mState = EState.Idle;
	// 	}
	// }

	// public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
	// {
	// 	mCompressBuffer.Initialize();
	// 	int len;
	// 	while ((len = input.Read(mCompressBuffer, 0, 2000)) > 0)
	// 	{
	// 		output.Write(mCompressBuffer, 0, len);
	// 	}
	// 	output.Flush();
	// }

// #if UNITY_METRO
//     public static int FindFirstNonWhitespaceIdx(byte[] bytes)
//     {
//         for (int i = 0; i < bytes.Length-1; i++)
//         {
//             switch (bytes[i])
//             {
//                 case 32:
//                 case 10:
//                 case 13:
//                     // Ignore white space ascii characters
//                     break;
//                 default:
//                     return i;
//             }
//         }
//         return -1;
//     }

//     public static int ReverseEndianness(int num)
//     {
//         byte[] bytes = BitConverter.GetBytes(num);
//         byte[] reversedBytes = new byte[bytes.Length];

//         for (int i = 0; i < bytes.Length; i++)
//         {
//             reversedBytes[i] = bytes[bytes.Length - 1 - i];
//         }

//         return BitConverter.ToInt32(reversedBytes, 0);
//     }

//     protected static int Adler32(byte[] bytes)
//     {
//         const uint a32mod = 65521;
//         uint s1 = 1, s2 = 0;
//         foreach (byte b in bytes)
//         {
//             s1 = (s1 + b) % a32mod;
//             s2 = (s2 + s1) % a32mod;
//         }
//         return unchecked((int)((s2 << 16) + s1));
//     }
// #endif

// 	public static void compressFileOld(byte[] _in, MemoryStream _out)
// 	{
// 		MemoryStream msInput = new MemoryStream(_in);
// #if UNITY_EDITOR || !UNITY_METRO
// 	#if UNITY_EDITOR && (UNITY_METRO || UNITY_WP8)
// 		//ZlibStream outZStream = new ZlibStream(_out, CompressionMode.Compress, CompressionLevel.BestCompression, true);
// 	#else
// 		ComponentAce.Compression.Libs.zlib.ZOutputStream outZStream = new ComponentAce.Compression.Libs.zlib.ZOutputStream(_out, ComponentAce.Compression.Libs.zlib.zlibConst.Z_DEFAULT_COMPRESSION);
// 		try
// 		{
// 			CopyStream(msInput, outZStream);
// 		}
// 		finally
// 		{
// 			outZStream.Close();
// 			msInput.Close();
// 		}	
// #endif

// #else
//         _out.WriteByte(0x58);
//         _out.WriteByte(0x85);
//         using (DeflateStream outZStream = new DeflateStream(_out, CompressionMode.Compress, true))
//         {
//             CopyStream(msInput, outZStream);
//         }
//         _out.Write(BitConverter.GetBytes(ReverseEndianness(Adler32(_in))), 0, sizeof(uint));
// #endif
//     }
	
// 	public static void decompressFile(byte[] _in, out byte[] _out)
// 	{
// 		MemoryStream msOutput = new MemoryStream();
// #if UNITY_EDITOR || !UNITY_METRO
// 	#if UNITY_EDITOR && (UNITY_METRO || UNITY_WP8)
		
// 		//ZlibStream outZStream = new ZlibStream(msOutput, CompressionMode.Decompress, true);
//         _out = msOutput.ToArray();
// 	#else
// 		ComponentAce.Compression.Libs.zlib.ZOutputStream outZStream = new ComponentAce.Compression.Libs.zlib.ZOutputStream(msOutput);
//         try
// 		{
// 			int off = 0;
// 			for (int i=0; i<_in.Length; i++)
// 			{
// 				// eat space, /r, /n
// 				if (!(_in[i] == 32 || _in[i] == 10 || _in[i] == 13))
// 				{
// 					off = i;
// 					if (i > 0)
// 					{
// 						DebugUtils.Warning(off.ToString());
// 					}
// 					break;
// 				}
// 			}
// 			outZStream.Write(_in, off, _in.Length - off);
// 			outZStream.Flush();
			
// 			_out = msOutput.ToArray();
// 		}
// 		finally
// 		{
// 			outZStream.Close();
// 			msOutput.Close();
// 		}
// #endif

// #else
//         int startIdx = FindFirstNonWhitespaceIdx(_in);
//         if (startIdx < 0) {
//             throw new Exception("While decompressFile, couldn't find Zlib Header index."); 
//         }
//         else if (startIdx > 0)
//         {
//             string extraBytes = "";
//             for (int i = 0; i < startIdx; i++)
//             {
//                 extraBytes += "["+_in[i]+"]";
//             }
//             DebugUtils.LogNetwork("decompressFile found " + startIdx + " extra bytes preceding Zlib header. Extra bytes = " + extraBytes);
//         }
//         byte[] _strippedIn = new byte[_in.Length - startIdx - 2 - 4]; // strip zlib headers (2 bytes) and Addler checksum (4 bytes)
//         DebugUtils.LogNetwork("decompressFile byte[0]="+_in[0]+" and byte[1]=" + _in[1]);
//         Buffer.BlockCopy(_in, startIdx + 2, _strippedIn, 0, _strippedIn.Length);
//         MemoryStream originalFileStream = new MemoryStream(_strippedIn);
//         MemoryStream decompressedFileStream = new MemoryStream();
//         using (DeflateStream decompressionStream = new DeflateStream(originalFileStream, CompressionMode.Decompress))
//         {
//             CopyStream(decompressionStream, decompressedFileStream);
//             _out = decompressedFileStream.ToArray();
//         }
// #endif
//     }
	// #endregion
// #region encrypt
//     static BlowFish sBlowFish = null;
//     public static void InitEncryptor(string _key)
//     {
//         sBlowFish = new BlowFish(_key);
//     }
//     public static byte[] encryptFile(byte[] _in)
//     {
//         sBlowFish.SetRandomIV();
//         return sBlowFish.Encrypt_CBC(_in);
//     }
//     public static byte[] decryptFile(byte[] _in)
//     {
//         return sBlowFish.Decrypt_CBC(_in);
//     }
#endregion
}
