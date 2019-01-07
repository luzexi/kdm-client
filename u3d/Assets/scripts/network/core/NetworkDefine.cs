using UnityEngine;
using System.Collections;

public static class NetworkDefine
{
    //public static string sRootUrl = "http://10.246.84.246:9005/handler/QueryHandler.ashx";
    //public static string sRootUrl = "http://10.246.34.76:9005/handler/QueryHandler.ashx";//����Server..
    //public static string sRootUrl = "http://10.246.32.39:91/handler/QueryHandler.ashx";
    public static string sRootUrl = "http://localhost:57546/handler/QueryHandler.ashx";
	// public static string sRootUrl = "http://127.0.0.1:8080/handler/QueryHandler.ashx";

    public static string sPackageDownloadUrl = "";
    public static string sAssetBundleDownloadUrl = "";

    public const int ERROR_CODE_NONE = 0;
    public const int ERROR_CODE_SUCCESS = 1;
    public const int ERROR_CODE_STOP = 999;

	public static string sHttpHeader = "http://";

    public static string sHttpTailer = "/gateway.php?ctr=";
    public static string sHttpTailerNoCtr = "/gateway.php";
	
	public static string sDownloadTailer = "/"; // "/download.php?FileName=";
	
	public static string sSlash = "/";
	
	public static string sItunesHeader = "itms-apps://";
		
	// Heart Beat Interval (seconds)
	public static float sHeartBeatInterval = 10;
	
	// Session out time (seconds) (foreground without any actions or background)
	public static float sSessionOutTime = 5 * 60;

    // time to reboot in interrupt
    public static float sInterruptTime = 30;
	
	// http connect timeout time (seconds)
	public static float sConnectTimeout = 30;
	
	// http connect request interval time (seconds), for combine asynchronized requests, if meet a synchronized request, send immediately
	public static float sRequestIntervalTime = 0.5f;
	
	// if combine the same kind of requests
	public static bool sCombineRequest = true;
		
	// if send heartbeat
	public static bool sHeartbeatRequest = true;
	
	// Max Retry Time
	public static int sMaxRetryTime = 0;
	
	// if Compressed response for some protocols
	public static int sCompressedReponse = 0;
	public static int sCompressedRequest = 0;
	
	
	public const int APP_FLAG_GAME 								= 0;
	public const int APP_FLAG_LEARDERBOARD 						= 1;
	public const int APP_FLAG_FRIEND 							= 2;
	public const int APP_FLAG_MAIL 								= 3;
	public const int APP_FLAG_CHAT 								= 4;
	public const int APP_FLAG_ACCOUNT 							= 5;
	public const int APP_FLAG_ACHIEVEMENT		 				= 6;
	public const int APP_FLAG_STATISTIC 						= 7;
	public const int APP_FLAG_DISTRIBUTION 						= 8;
	public const int APP_FLAG_UMBRELLA 							= 9;
	
	
	public readonly static int COMPRESSFLAG_GAME 						= 1<<APP_FLAG_GAME;
	
	
}
