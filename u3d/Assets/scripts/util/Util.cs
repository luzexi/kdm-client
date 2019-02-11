﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Security.Cryptography;



public class Misc
{

#if UNITY_ANDROID
    private static AndroidJavaObject sAndroidJavaObject;
#endif
    private static string sDeviceId = null;

    // 静音相关
    static public bool IsMute()
    {
        return AudioListener.volume < 1e-2f;
    }

    static public bool ToggleAudioMute()
    {
        if (IsMute())
        {
            AudioListener.volume = 1.0f;
            return false;
        }
        else
        {
            AudioListener.volume = 0.0f;
            return true;
        }
    }

    static public string PathName2ABName(string path)
    {
        return path.Replace("\\", "_").Replace("/", "_").ToLower();
    }

#if UNITY_ANDROID
    //获取当前App的Activity
    public static AndroidJavaObject GetAndroidContext()
    {
        if(sAndroidJavaObject != null)
        {
            return sAndroidJavaObject;
        }
        sAndroidJavaObject = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

        return sAndroidJavaObject;
    }
#endif

    //获取DeviceID
    public static string getDeviceId()
    {
        if(sDeviceId != null) return sDeviceId;

#if UNITY_EDITOR
        sDeviceId = SystemInfo.deviceUniqueIdentifier;
#elif UNITY_ANDROID
        AndroidJavaClass deviceUtils = new AndroidJavaClass("com.sdo.qihang.lib.DeviceUtils");
        sDeviceId = deviceUtils.CallStatic<string> ("getUniqueId");
#elif UNITY_IPHONE
        sDeviceId = SystemInfo.deviceUniqueIdentifier;
#endif
        return sDeviceId;
    }

    // 字符串和字节转换
    static public string Byte2String(byte[] bytes)
    {
        string s = new System.Text.UTF8Encoding().GetString(bytes);
        return s;
    }
    static public byte[] String2Byte(string s)
    {
        byte[] bytes = new System.Text.UTF8Encoding().GetBytes(s);
        return bytes;
    }

    private static string s_persistentDataPath = string.Empty;
    public static string GetPersistentDataPath()
    {
        if (string.IsNullOrEmpty(s_persistentDataPath))
        {
            if (string.IsNullOrEmpty(UnityEngine.Application.persistentDataPath))
            {
#if UNITY_ANDROID
                s_persistentDataPath = InternalGetPersistentDataPath();
#endif
            }
            else
            {
                s_persistentDataPath = UnityEngine.Application.persistentDataPath;
            }
        }
        return s_persistentDataPath;
    }

#if UNITY_ANDROID
    private static string InternalGetPersistentDataPath()
    {
        string path = "";
        try
        {
            IntPtr obj_context = AndroidJNI.FindClass("android.os.Environment");
            IntPtr method_getExternalStorageDirectory = AndroidJNIHelper.GetMethodID(obj_context, "getExternalStorageDirectory", "()Ljava/io/File;", true);
            IntPtr file = AndroidJNI.CallStaticObjectMethod(obj_context, method_getExternalStorageDirectory, new jvalue[0]);
            IntPtr obj_file = AndroidJNI.FindClass("java/io/File");
            IntPtr method_getAbsolutePath = AndroidJNIHelper.GetMethodID(obj_file, "getAbsolutePath", "()Ljava/lang/String;");

            path = AndroidJNI.CallStringMethod(file, method_getAbsolutePath, new jvalue[0]);

            if (path != null)
            {
                path += "/Android/data/" + GetPackageName() + "/files";
            }
            else
            {
                path = "";
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        return path;
    }

    private static string GetPackageName()
    {
        string packageName = "";
        try
        {
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    IntPtr obj_context = AndroidJNI.FindClass("android/content/ContextWrapper");
                    IntPtr method_getPackageName = AndroidJNIHelper.GetMethodID(obj_context, "getPackageName", "()Ljava/lang/String;");
                    packageName = AndroidJNI.CallStringMethod(obj_Activity.GetRawObject(), method_getPackageName, new jvalue[0]);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        return packageName;
    }
#endif
}