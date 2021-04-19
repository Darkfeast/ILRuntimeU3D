using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DFExtendHelper
{ 

}
public static class ExtendTrans
{
	public static void State(this Transform trs, bool state)
	{
		trs.gameObject.SetActive(state);
	}
	public static void State(this GameObject go, bool state)
	{
		State(go.transform, state);
	}


	public static void SetParent(this Transform trs, Transform parent)
	{
		trs.parent = parent;
		trs.localPosition = Vector3.zero;
	}
	public static void SetParent(this Transform trs, GameObject parent)
	{
		SetParent(trs, parent.transform);
	}
	public static void SetParent(this GameObject go, Transform parent)
	{
		SetParent(go.transform, parent);
	}
	public static void SetParent(this GameObject go, GameObject parent)
	{
		SetParent(go.transform, parent);
	}
}
public static class ExtendList
{
	//打印列表内容
	public static void Print<T>(this List<T> list,bool sort=false ,UnityAction<T> act=null, E_ColorType ecolor = E_ColorType.Init)
	{
		if(sort)
        {
			list.Sort((a, b) => {
				return a.ToString().CompareTo(b.ToString());
			});
        }
		Darkfeast.Log("Print-listCount " + list.Count, E_ColorType.UI);
		foreach (var v in list)
		{
			Darkfeast.Log(v, ecolor);
			act?.Invoke(v);
		}
		Darkfeast.Log("---------------------" + list.Count, E_ColorType.Over);
	}

	//获取列表中的字符串并拼成一行
	public static string GetOneLine(this List<string> list,E_ColorType ecolor = E_ColorType.Init)
    {
		StringBuilder sb = new StringBuilder();
		foreach(var v in list)
        {
			sb.Append(v+"#");
        }
		return sb.ToString(0,sb.Length-1);
    }
	//public static void ToLower<T>(this List<T> list, E_ColorType ecolor = E_ColorType.Init)
	//   {
	//	Darkfeast.Log("ToLower-listCount " + list.Count, E_ColorType.UI);
	//	for(int i=0;i<list.Count;i++)
	//	{
	//		if(typeof(T)==typeof(string))
	//           {
	//			string vv = list[i] as string;

	//			T t= vv.ToLower() as T;
	//			list[i] = t;
	//		}
	//	}
	//	Darkfeast.Log("---------------------" + list.Count, E_ColorType.Over);
	//}
}

public static class ExtendDict
{
	public static void Print(this Dictionary<object,object> dict, E_ColorType ecolor = E_ColorType.Init)
    {
		Darkfeast.Log("Print-dictCount " + dict.Count, E_ColorType.UI);
		foreach (var v in dict)
		{
			Darkfeast.Log("k "+v.Key+"  v "+v.Value, ecolor);
		}
		Darkfeast.Log("---------------------" + dict.Count, E_ColorType.Over);
	}
}


	 
