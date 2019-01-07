using UnityEngine;

public class Consts
{
    public static string VERSION_STR = "99.99.99";
    public static int VERSION
    {
        get
        {
            string[] str_ = VERSION_STR.Split('.');
            int version = 0;
            version += int.Parse(str_[0])*10000;
            version += int.Parse(str_[1])*100;
            version += int.Parse(str_[2]);

            return version;
        }
    }
}