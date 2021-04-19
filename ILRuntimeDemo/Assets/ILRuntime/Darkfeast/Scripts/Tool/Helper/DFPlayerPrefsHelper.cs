using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DFPlayerPrefsHelper{

    public static void SetPos(string key,Vector3 pos)
    {
        string x = Darkfeast.Float2String(pos.x,2);
        string y = Darkfeast.Float2String(pos.y, 2);
        string z = Darkfeast.Float2String(pos.z, 2);
        string p = x + "|" + y + "|" + z;
        PlayerPrefs.SetString(key, p);
    }
    public static Vector3 GetPos(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string pos= PlayerPrefs.GetString(key);
            string[] strs = pos.Split('|');
            float x = float.Parse(strs[0]);
            float y = float.Parse(strs[1]);
            float z = float.Parse(strs[2]);
            return new Vector3(x, y, z);
        }
        else
            return Vector3.zero;
    }
}
