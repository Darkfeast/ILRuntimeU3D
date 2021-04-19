using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DFTagHelper<T> {
    public static T GetComponentType(string tag)
    {
        GameObject go= GameObject.FindWithTag(tag);
        if(go==null)
            Darkfeast.Log("find tag [" + tag + "]type " + typeof(T) + "  gameObject is null", E_ColorType.Err);
        T t= go.GetComponent<T>();
        if (t == null)
            Darkfeast.Log("find tag [" + tag + "]" + "  find  type " + typeof(T) + "  is null", E_ColorType.Err);
        return t;
    }
}

