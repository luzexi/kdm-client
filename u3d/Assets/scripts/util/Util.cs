using System.Collections;
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
}


public class CFile
{
    static public string RemoveExtension(string path)
    {
        string ext = Path.GetExtension(path);
        if (ext.Length > 0)
        {
            string pathWithoutExt = path.Remove(path.Length - ext.Length);
            return pathWithoutExt;
        }
        else
        {
            return path;
        }
    }
    static public string RemoveExtension(string path, string ext)
    {
        if (path.EndsWith(ext))
        {
            return path.Remove(path.Length - ext.Length);
        }
        else
        {
            return path;
        }
    }
    static public void CreateDirectoryIfNotExist(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    // 拼凑本地URL, file://
    static public string MakeLocalUrl(string path)
    {
#if UNITY_ANDROID
        return path;    // jar:file://
#else
        return "file://" + path;
#endif
    }

    // 压缩文件
    static public void CompressStream(Stream input, Stream output)
    {
        SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
        // Write the encoder properties
        coder.WriteCoderProperties(output);

        // Write the decompressed file size.
        output.Write(System.BitConverter.GetBytes(input.Length), 0, 8);

        // Encode the file.
        coder.Code(input, output, input.Length, -1, null);
        output.Flush();
    }
    // 解压文件
    static public void UnCompressStream(Stream input, Stream output)
    {
        // Read the decoder properties
        byte[] properties = new byte[5];
        input.Read(properties, 0, 5);

        // Read in the decompress file size.
        byte[] fileLengthBytes = new byte[8];
        input.Read(fileLengthBytes, 0, 8);
        long fileLength = System.BitConverter.ToInt64(fileLengthBytes, 0);

        SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
        coder.SetDecoderProperties(properties);
        coder.Code(input, output, input.Length, fileLength, null);
        output.Flush();
    }

    // 压缩文件
    static public void CompressFile(string inFile, string outFile)
    {
        string outDir = Path.GetDirectoryName(outFile);
        CreateDirectoryIfNotExist(outDir);

        using (FileStream input = new FileStream(inFile, FileMode.Open))
        {
            using (FileStream output = new FileStream(outFile, FileMode.Create))
            {
                CompressStream(input, output);
            }
        }
    }
    // 解压文件
    static public void UnCompressFile(string inFile, string outFile)
    {
        string outDir = Path.GetDirectoryName(outFile);
        CreateDirectoryIfNotExist(outDir);

        using (FileStream input = new FileStream(inFile, FileMode.Open))
        {
            using (FileStream output = new FileStream(outFile, FileMode.Create))
            {
                UnCompressStream(input, output);
            }
        }
    }
    static public void CompressBuffer(byte[] inData, byte[] outData)
    {
        using (MemoryStream input = new MemoryStream(inData))
        {
            using (MemoryStream output = new MemoryStream(outData))
            {
                CompressStream(input, output);
            }
        }
    }
    static public void UnCompressBuffer(byte[] inData, byte[] outData)
    {
        using (MemoryStream input = new MemoryStream(inData))
        {
            using (MemoryStream output = new MemoryStream(outData))
            {
                UnCompressStream(input, output);
            }
        }
    }
    // 获取文件大小
    static public long GetFileSize(string path)
    {
        FileInfo fi = new FileInfo(path);
        return fi.Length;   // if file not exist, exception will be thrown
    }
    // 读取文件
    static public byte[] ReadFile(string path)
    {
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            return data;
        }
    }
    // 写入文件
    static public void WriteFile(string path, byte[] data)
    {
        string dir = Path.GetDirectoryName(path);
        CreateDirectoryIfNotExist(dir);

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            fs.Write(data, 0, data.Length);
            fs.Close();
        }
    }

    // md5
    static public string CalcMD5(byte[] data)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string md5Value = string.Empty;
        for (int i = 0; i < md5Data.Length; i++)
        {
            md5Value += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        md5Value = md5Value.PadLeft(32, '0');
        return md5Value;
    }
    static public string CalcFileMD5(string path)
    {
        byte[] data = ReadFile(path);
        string md5 = CalcMD5(data);
        return md5;
    }
}
