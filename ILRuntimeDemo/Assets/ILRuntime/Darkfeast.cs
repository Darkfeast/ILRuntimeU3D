// ========================================================
// CreateTime：2020/07/07 17:50:34
// 版 本：v 1.0
// ========================================================
using UnityEngine;

public enum E_ColorType
{
	Init,
	Normal,
	UI,
	Temp,
	Err,
	Over
}
public class Darkfeast
{
	static Application.LogCallback logCallback;
	static bool openLogTrace;
	public static void Log(object str, E_ColorType c = E_ColorType.Normal )
	{
		//if (!DarkfeastConfig.Debug) return;
		
		if (c == E_ColorType.Init)
			Debug.Log("<color=cyan>" + str + "</color>");
		else if (c == E_ColorType.Normal)
			Debug.Log("<color=#00c0ff>" + str + "</color>");
		else if (c == E_ColorType.UI)//#00FF21a0
		{
			Debug.Log("<color=#91EC17FF>>>>>>> " + str + "  -----------------</color>");
		}
		else if (c == E_ColorType.Temp)
		{
			Debug.Log("<color=magenta>>>>>>>> " + str + "  -----------------</color>");
		}
		else if (c == E_ColorType.Err)
		{
			Debug.Log("<color=#C94A4AFF>-----------" + str + "</color>");
		}
		else if (c == E_ColorType.Over)
		{
			Debug.Log("<color=#FFA662FF> =====" + str + "=====</color>"); //2F5283FF   
		}
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="f"></param>
	/// <param name="precision"></param>精度
	/// <returns></returns>
	/// 
	public static void LogTraceState(bool openLogTrace=false)
    {
		if (openLogTrace && !Darkfeast.openLogTrace)
		{
			Darkfeast.openLogTrace = openLogTrace;
			Application.logMessageReceived+=LogTrace;
			Darkfeast.Log("Open", E_ColorType.Temp);
		}
		//if (openLogTrace && Darkfeast.openLogTrace)
		//{
		//	Darkfeast.openLogTrace = !openLogTrace;
		//	logCallback = null;
		//}
	}
	static void LogTrace(string str,string trace , LogType t)
    {
		Log(str + " ### \n" + trace, E_ColorType.UI);
    }
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
}


