
using System;


//time convert
public class TimeConvert
{
    public const long TICKS_TO_SECOND = 10000000L;
    public const long DAY_SECOND = 60*60*24;

    public static int NowDay()
    {
        DateTime dt = DateTime.Now;
        return GetDays(dt);
    }

    public static int GetDays(DateTime dt)
    {
        long timel = DateTimeToUNIXTime(dt);
        int day = (int)(timel/DAY_SECOND);
        return day;
    }

    public static DateTime GetNow()
    {
        return DateTime.Now;
    }

    public static DateTime GetTommorrow()
    {
        return new DateTime(DateTime.Now.Ticks + DAY_SECOND*TICKS_TO_SECOND);
    }

	//convert from unix time to string date time
    public static string UNIXTimeToDateTimeString(long time)
    {
        DateTime date = UNIXTimeToDateTime(time);
        return date.ToString("yyyy/MM/dd hh:mm:ss");
    }

    //convert from unix time to string date
    public static string UNIXTimeToDateString(long time)
    {
        DateTime date = UNIXTimeToDateTime(time);
        return date.ToString("yyyy-MM-dd");
    }

    //convert form unix time to c# time
    public static DateTime UNIXTimeToDateTime(long time)
    {
        long timeL = time * TICKS_TO_SECOND + (new DateTime(1970, 1, 1, 8, 0, 0).Ticks);

        DateTime date = new DateTime(timeL);
        return date;
    }

    //c# date time convert to unix time
    public static long DateTimeToUNIXTime( DateTime dt )
    {
    	long timeL = (dt.Ticks - (new DateTime(1970, 1, 1, 8, 0, 0).Ticks)) / TICKS_TO_SECOND;
		return timeL;
    }

    // Show "22m 22.351s"
    public static string MilSecToString(long _milSec)
    {
        int inSec = (int)(_milSec / 1000);
        
        int minute = inSec / 60;
        int second = inSec % 60;
        int msecond = (int)(_milSec % 1000);
        
        string result = " ";
        if (minute > 0)
            result = minute + "m";
        
        if (second != 0 || msecond != 0)
            result = result + " " + second + "." + msecond + "s";
        
        return result;
    }
    
    public static string SecondToString(float second)
    {
        return SecondToString((int)second);
    }
    
    // Show "22d 22h" or "22m 22s" or "22h 22m"
    public static string SecondToString(int second)
    {
        string dd = "d";
        string hh = "h";
        string mm = "m";
        string ss = "s";

        string str = string.Empty;
        if (second <= 0)
            return "0" + ss;
        
        int day = second / 86400;
        int hour = (second - day * 86400) / 3600;
        int minute = (second - day * 86400 - hour * 3600) / 60;
        second = second - day * 86400 - hour * 3600 - minute * 60;
        if (day > 0)
        {
            str = str + day + dd;
            if (hour > 0) str = str + " ";
        }
        if (hour > 0)
        {
            if (hour < 10)
                str = str + " " + hour + hh;
            else
                str = str + hour + hh;
            if (day == 0 && minute > 0) str = str + " ";
        }
        // max show two time segments 5d 20h
        if (day == 0)
        {
            if (minute > 0)
            {
                if (minute < 10)
                    str = str + " " + minute + mm;
                else
                    str = str + minute + mm;
                if (hour == 0 && second > 0) str = str + " ";
            }
            // max show two time segments 20h 43m
            if (hour == 0)
            {
                if (second > 0)
                {
                    if (second < 10)
                        str = str + " " + second + ss;
                    else
                        str = str + second + ss;
                }
            }
        }
        return str;
    }
    
    
    
    // Show "22:22:22"
    public static string SecondToString2(int second)
    {
        string str = string.Empty;
        int hour = second / 3600;
        int minute = (second - hour * 3600) / 60;
        second = second - hour * 3600 - minute * 60;
        if (hour != 0)
        {
            if (hour < 10)
                str = str + "0" + hour + ":";
            else
                str = str + hour + ":";
        }
        if (minute < 10)
            str = str + "0" + minute + ":";
        else
            str = str + minute + ":";
        if (second < 10)
            str = str + "0" + second;
        else
            str = str + second;
        return str;
    }

    public static string SecondToString3(int second)
    {
        string str = string.Empty;
        int hour = second / 3600;
        int minute = (second - hour * 3600) / 60;
        second = second - hour * 3600 - minute * 60;
        if (hour != 0)
        {
            if (hour < 10)
                str = str + "0" + hour + ":";
            else
                str = str + hour + ":";
        }
        else
        {
            str = str + "00:";
        }
        if (minute < 10)
            str = str + "0" + minute + ":";
        else
            str = str + minute + ":";
        if (second < 10)
            str = str + "0" + second;
        else
            str = str + second;
        return str;
    }
}