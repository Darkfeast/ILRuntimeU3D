using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace HotFix_Project
{
    class PlayerInfo
    {

        Image img_bg;
        Transform trs_bg;

        public void Init()
        {
            Darkfeast.Log("PlayerInfo Init");

            trs_bg = new GameObject("img_bg").transform;
            img_bg= trs_bg.gameObject.AddComponent<Image>();

        }
    }
}
