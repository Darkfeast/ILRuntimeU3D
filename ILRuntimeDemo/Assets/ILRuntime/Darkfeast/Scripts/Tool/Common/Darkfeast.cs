// ========================================================
// CreateTime：2020/07/07 17:50:34
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.IO;
using System.Text;

public enum E_ColorType
{
	Init,
	Normal,
	UI,
	Temp,
	Err,
	Over
}
public class Darkfeast : MonoBehaviour
{
	static StringBuilder sb = new StringBuilder();
	public static bool isRecord = false;
	
	public static void Log(object str, E_ColorType c = E_ColorType.Normal)
	{
		string formatStr = "";
		if (!DFConfig.Print) return;
		
		if (c == E_ColorType.Init)
		{
			formatStr = "<color=cyan>" + str + "</color>";
			Debug.Log(formatStr);
		}
		else if (c == E_ColorType.Normal)
		{
			formatStr = "<color=#00c0ff>" + str + "</color>";
			Debug.Log(formatStr);
		}
		else if (c == E_ColorType.UI)//#00FF21a0
		{
			formatStr = "<color=#91EC17FF>>>>>>>> " + str + "  -----------------</color>";
			Debug.Log(formatStr);
		}
		else if (c == E_ColorType.Temp)
		{
			formatStr = "<color=magenta>>>>>>>> " + str + "  -----------------</color>";
			Debug.Log(formatStr);
		}
		else if (c == E_ColorType.Err)
		{
			formatStr = "<color=#FF0000FF>-----------" + str + "</color>";//FF0000FF  C94A4AFF
			Debug.Log(formatStr);
		}
		else if (c == E_ColorType.Over)
		{
			formatStr = "<color=#FFA662FF> =====" + str + "=====</color>";
			Debug.Log(formatStr); //2F5283FF   
		}
		
		if(isRecord)
			sb.Append(str+"\n");
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="f"></param>
	/// <param name="precision"></param>精度
	/// <returns></returns>
	public static string Float2String(float f,int precision=0)
    {
        #region   
        #endregion
        if (precision < 0)
			precision = 0;
		return f.ToString("f" + precision);
	}
	public static string Float2Percent(float f, int precision = 0)
	{
		if (precision < 0)
			precision = 0;
		return (f * 100).ToString("f" + precision)+"%";
	}

	public static float Float2Float(float f ,int precision=0)
    {
		if (precision < 0)
			precision = 0;

		return float.Parse(Float2String(f, precision));
	}
	public static void PrintLog()
    {
		if(!isRecord)
        {
			Log("record is closed !!!", E_ColorType.Init);
			return;
        }
		string path = Application.dataPath;
		FileStream fs = null;
		if (!File.Exists(path + "/log"+DFHelper.GetFormatDateTime()+".txt"))
		{
			fs = File.Create(path + "/data.txt");
		}
		else
			fs = File.Open(path + "/data.txt",FileMode.OpenOrCreate);
		StreamWriter sw = new StreamWriter(fs);
		sw.Write(sb.ToString());
		sw.Close();
        //StringWriter sw = new StringWriter(sb);
        //File.WriteAllText(path,sb.toString());
        Log("write Over", E_ColorType.Over);
		isRecord = false;
    }
	
}


