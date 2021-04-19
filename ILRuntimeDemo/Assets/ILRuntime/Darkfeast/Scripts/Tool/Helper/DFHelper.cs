using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using System.IO;
using System.Security.Cryptography;
//因为是多个模块的合集，为了方便代码感应提示，方法开头改为模块名字
//如GetFormatDateTime  改为 DateTimeFormat
//GetFileHash  ->  FileGetHash

//仔细想了想 上面这种格式命名太麻烦了 而且不符合语言顺序  改回原样
public enum E_DateType
{
    Default,//默认时间格式
}
public class DFHelper {

	public static void ModifyLayer(Transform trs, int layerId)
    {
        if (trs.childCount > 0)
        {
            for (int i = 0; i < trs.childCount; i++)
            {
                ModifyLayer(trs.GetChild(i), layerId);
            }
        }
        trs.gameObject.layer = layerId;
    }



    //File
    /// <summary>
    /// string md5 = getFileHash("E:\\MyPro\\cubetest.unity3d");
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetFileHash(string filePath)
    {
        try
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            int len = (int)fs.Length;
            byte[] data = new byte[len];
            fs.Read(data, 0, len);
            fs.Close();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string fileMD5 = "";
            foreach (byte b in result)
            {
                fileMD5 += Convert.ToString(b, 16);
            }

            //或者
            //for (int i = 0; i < result.Length; i++)
            //{

            //	//“x2”表示输出按照16进制，且为2位对齐输出。
            //	fileMD5+=result[i].ToString("x2");
            //}

            return fileMD5;
        }
        catch (FileNotFoundException e)
        {
            Darkfeast.Log(" FileNotFoundException   " + e.Message, E_ColorType.Err);
            return "";
        }
    }

    /// <summary>
	/// string[] files = GetFiles("*.gif", "*.jpg", "*.png");
	/// </summary>
	/// <param name="dirPath"></param>
	/// <param name="searchPatterns"></param>
	/// <returns></returns>
	public static string[] GetFiles(string dirPath, params string[] searchPatterns)
    {
        if (searchPatterns.Length <= 0)
        {
            return null;
        }
        else
        {
            DirectoryInfo di = new DirectoryInfo(dirPath);
            FileInfo[][] fis = new FileInfo[searchPatterns.Length][];
            int count = 0;
            for (int i = 0; i < searchPatterns.Length; i++)
            {
                FileInfo[] fileInfos = di.GetFiles(searchPatterns[i]);
                fis[i] = fileInfos;
                count += fileInfos.Length;
            }
            string[] files = new string[count];
            int n = 0;
            for (int i = 0; i <= fis.GetUpperBound(0); i++)
            {
                for (int j = 0; j < fis[i].Length; j++)
                {
                    string temp = fis[i][j].FullName;
                    files[n] = temp;
                    n++;
                }
            }
            return files;
        }
    }


    //Time
    static List<DTItem> listDT;
    //public static void TestGetDateTime()
    //{
    //    Darkfeast.Log(DateTime.Today);
    //    Darkfeast.Log(DateTime.Now);
    //    Darkfeast.Log(DateTime.UtcNow);
    //    Darkfeast.Log(GetFormatDateTime(DateTime.Now));

    //    Darkfeast.Log(GetMonthDay());
    //}
    public static string GetFormatDateTime(DateTime ?dt=null,E_DateType t=E_DateType.Default)
    {
        switch (t)
        {
            case E_DateType.Default:
                return (dt ?? DateTime.Now).ToString("yyyy/MM/dd-HH:mm:ss");
            default:
                return "xxx";
        }
    }
    public static List<int> GetSeparateDateTime()
    {
        string dateTime = GetFormatDateTime(DateTime.Now);
        string[] date_time = dateTime.Split('-');
        string[] date = date_time[0].Split('/');
        string[] time = date_time[1].Split(':');
        int year = int.Parse(date[0]);
        int month = int.Parse(date[1]);
        int day = int.Parse(date[2]);
        int hour = int.Parse(time[0]);
        int minute = int.Parse(time[1]);
        int second = int.Parse(time[2]);

        List<int> dt = new List<int>();
        dt.Add(year);
        dt.Add(month);
        dt.Add(day);
        dt.Add(hour);
        dt.Add(minute);
        dt.Add(second);
        return dt;
    }
    public static string GetMonthDay()
    {
        List<int> dt = GetSeparateDateTime();
        int month = dt[1];
        int day = dt[2];
        return month + "-" + day;
    }
    public static string GetFormatDateFromSecond(int second)
    {
        int min = second / 60;
        if (min == 0)
        {
            return "<1m";
        }
        int hour = min / 60;
        min = min % 60;

        if (hour == 0)
            return min + "m";
        else
            return hour + "h" + min + "m";
    }

    public static void ClearDTStack()
    {
        string key = "dtStack";
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
    public static string DTStack(string dt)
    {
        //string key = "dayTimeStack";
        string newDt = "";
        //if (PlayerPrefs.HasKey(key))
        //{

        //string dtStack = PlayerPrefs.GetString(key);
        //string[] strsTemp = dtStack.Split('|');

        if (listDT.Count > 0)
        {
            if (listDT[listDT.Count - 1].md == dt)
            {
                string keys = "";
                foreach (var item in listDT)
                {
                    keys += item.md + "|";
                }
                keys = keys.Substring(0, keys.Length - 1);
                return keys;
            }
        }

        //dtStack += "|" + dt;
        //string[] strs = dtStack.Split('|');
        if (listDT.Count < 7)
        {
            DTItem item = new DTItem(dt);
            listDT.Add(item);
        }
        else
        {
            listDT.RemoveAt(0);
            DTItem item = new DTItem(dt);
            listDT.Add(item);
        }

        foreach (var item in listDT)
        {
            newDt += item.md + "|";
        }
        newDt = newDt.Substring(0, newDt.Length - 1);

        return newDt;
    }

    static Stopwatch watch;
    public static void StartTimer()
    {
        Darkfeast.Log(SceneManager.GetActiveScene().name + "  StartTimer t ", E_ColorType.UI);
        watch = new Stopwatch();
        watch.Start();
    }
    public static double StopTimer()
    {

        if (watch == null)
            return 0;
        Darkfeast.Log(SceneManager.GetActiveScene().name + "  StopTimer t ", E_ColorType.UI);
        watch.Stop();
        double time = watch.Elapsed.TotalSeconds;
        watch = null;
        return time;
    }
    public static void RecordTimeUpdate(double t, string day)
    {
        Darkfeast.Log(SceneManager.GetActiveScene().name + "  RecordTimeUpdate t " + t, E_ColorType.UI);
        if (listDT.Count == 0)
            listDT.Add(new DTItem(GetMonthDay()));
        //更新当天累积时间
        DTItem item = listDT[listDT.Count - 1];
        string keyD = item.md;
        item.time += Mathf.RoundToInt((float)t);

    }
    public static void RecordPkUpdate(double t, bool win, string day)
    {
        Darkfeast.Log(SceneManager.GetActiveScene().name + "  RecordPkUpdate t " + t + "  win  " + win, E_ColorType.Temp);
        if (listDT.Count == 0)
            listDT.Add(new DTItem(GetMonthDay()));

        DTItem item = listDT[listDT.Count - 1];
        string keyD = item.md;
        item.timePk += Mathf.RoundToInt((float)t);
        item.times += 1;
        if (win)
            item.timesWin += 1;
    }

    public static int RecordTimeGet()
    {
        //string key = "recordTimeReport";
        //int t = 0;
        //if (PlayerPrefs.HasKey(key))
        //{
        //    string record= PlayerPrefs.GetString(key);
        //    string[] strs = record.Split('|');
        //    t=int.Parse(strs[1]);
        //    Darkfeast.Log("t " + strs[0],E_ColorType.Over);
        //}
        //return t;
        DTItem item = listDT[listDT.Count - 1];
        Darkfeast.Log("t " + item.md, E_ColorType.Over);
        return item.time;
    }

    public static string DayTimeStack(string dt)
    {
        string key = "dayTimeStack";
        string newDt = "";

        if (PlayerPrefs.HasKey(key))
        {
            string dtStack = PlayerPrefs.GetString(key);
            string[] strsTemp = dtStack.Split('|');
            if (strsTemp[strsTemp.Length - 1] == dt)
            {
                return dtStack;
            }

            dtStack += "|" + dt;
            string[] strs = dtStack.Split('|');
            if (strs.Length <= 7)
            {
                PlayerPrefs.SetString(key, dtStack);
                newDt = dtStack;
            }
            else
            {
                for (int i = strs.Length - 7; i < strs.Length; i++)
                {
                    newDt += "|" + strs[i];
                }
                newDt = newDt.Substring(1, newDt.Length - 1);
                PlayerPrefs.SetString(key, newDt);
            }
        }
        else
        {
            PlayerPrefs.SetString(key, dt);
            newDt = dt;
        }
        return newDt;
    }

    public static void LoadDT()
    {
        //JsonUtility.fro
        //1-3_1111_999|
        string key = "dt";
        if (PlayerPrefs.HasKey(key))
        {
            string dtStr = PlayerPrefs.GetString(key);
            string[] dtArr = dtStr.Split('|');
            List<DTItem> listDTItem = new List<DTItem>();
            for (int i = 0; i < dtArr.Length; i++)
            {
                string[] dtItemArr = dtArr[i].Split('_');
                DTItem item = new DTItem();
                item.md = dtItemArr[0];
                item.time = int.Parse(dtItemArr[1]);
                item.times = int.Parse(dtItemArr[2]);
                item.timePk = int.Parse(dtItemArr[3]);
                item.timesWin = int.Parse(dtItemArr[4]);
                listDTItem.Add(item);
            }
            listDT = listDTItem;
            if (listDT[listDT.Count - 1].md != GetMonthDay())
            {
                listDT.Add(new DTItem(GetMonthDay()));
            }
        }
        else
        {
            listDT = new List<DTItem>();
            listDT.Add(new DTItem(GetMonthDay()));
        }
    }

    public static void SaveDT()
    {
        string key = "dt";
        string dtStr = "";
        foreach (var item in listDT)
        {
            dtStr += "|" + item.md + "_" + item.time + "_" + item.times + "_" + item.timePk + "_" + item.timesWin;
        }

        dtStr = dtStr.Substring(1, dtStr.Length - 1);
        PlayerPrefs.SetString(key, dtStr);
    }
    public static void PrintDT()
    {
        foreach (var item in listDT)
        {
            Darkfeast.Log(item.md + "  " + item.time + "  " + item.times, E_ColorType.UI);
        }
    }

    public static void ClearDT()
    {
        string key = "dt";
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
        }
    }

    public static List<DTItem> GetDT()
    {
        return listDT;
    }
}

[Serializable]
public class DT
{
    public List<DTItem> dt;
}

[Serializable]
public class DTItem
{
    public string md;
    public int time;
    public int times; //pk
    public int timePk;//pk
    public int timesWin; //pk
    public DTItem() { }

    public DTItem(string m)
    {
        md = m;
    }

}
