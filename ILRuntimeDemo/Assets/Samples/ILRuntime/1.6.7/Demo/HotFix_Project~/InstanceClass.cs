using System;
using System.Collections.Generic;

namespace HotFix_Project
{
    public class InstanceClass
    {
        private int id;

        public InstanceClass()
        {
            Darkfeast.Log("0 ctor");
            Darkfeast.Log("!!! InstanceClass::InstanceClass()");
            this.id = 0;
        }

        public InstanceClass(int id)
        {
            Darkfeast.Log("!!! InstanceClass::InstanceClass() id = " + id);
            this.id = id;
        }

        public int ID
        {
            get { return id; }
        }

        // static method
        public static void StaticFunTest()
        {
            Darkfeast.Log("!!! InstanceClass.StaticFunTest()");
        }

        //public static void StaticFunTest2(int a)
        //{
        //    UnityEngine.Debug.Log("!!! InstanceClass.StaticFunTest2(int), a=" + a);
        //}
        public static void StaticFunTest2(float a)
        {
            Darkfeast.Log("!!! InstanceClass.StaticFunTest2(float), a=" + a);
        }

        public static void GenericMethod<T>(T a)
        {
            Darkfeast.Log("!!! InstanceClass.GenericMethod(), a=" + a);
        }

        public void RefOutMethod(int addition, out List<int> lst, ref int val)
        {
            val = val + addition + id;
            lst = new List<int>();
            lst.Add(id);
        }
    }


}
