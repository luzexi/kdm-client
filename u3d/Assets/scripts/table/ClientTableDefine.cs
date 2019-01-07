using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TableData
{
	// table
	protected RawTable mTableData;
    protected uint mHash = 0;
    static Dictionary<int, uint> s_hashDictionary = new Dictionary<int, uint>();
    public static void Reset()
    {
        s_hashDictionary.Clear();
    }
    static void CheckHash(string _path, uint _hash)
    {
        int key = _path.GetHashCode();
        uint oldHash;
        if (s_hashDictionary.TryGetValue(key, out oldHash))
        {
            if (oldHash != _hash)
            {
                Application.Quit();
            }
        }
        else
            s_hashDictionary.Add(key, _hash);
    }

    public static uint ComputeHash(byte[] s)
    {
        uint h = 0;
        for (int i = s.Length - 1; i >= 0; --i)
        {
            h = (h << 5) - h + s[i];
        }
        return h;
    }
    public static uint ComputeHash(char[] s)
    {
        uint h = 0;
        for (int i = s.Length - 1; i >= 0; --i)
        {
            h = (h << 5) - h + s[i];
        }
        return h;
    }
    protected virtual uint GetHash()
    {
        return 0;
    }
    protected abstract string GetPath();
    protected abstract void _ParseData();

    public uint CheckHash()
    {
        uint hash = GetHash();

        CheckHash(GetPath(), hash);

        return hash;
    }
    public void ReadTable()
    {
        if (mTableData == null)
            mTableData = new RawTable();

        mTableData.readBinary(GetPath());
    }
    public void ParseData()
    {
        _ParseData();
        // CheckHash(); //暂时关闭hash检查
        mTableData = null;
    }
}


