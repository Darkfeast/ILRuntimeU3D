using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DFConfig{

	public const string ver = "t20201225_6"; //格式： t+Data+version(当天测试)
	public const bool Print = true; //是否打印

	//public const bool Debug = true;
	public static bool KillMode = false;
	public static float LimitX = 16; //测试对战边界
	public static float timeScale = 10; //快速测试
	public static bool reportDrag = false;
	public static bool reportHistogramDrag = false;
	public static bool purch = false;
	public static int freeLinkId = 2; //连词岛屿免费开放id;
	public static int freeLinkTheme = 1; //连词岛屿免费开放id;
	public static int freePkMechaIndex = 5; //机甲免费开放索引;
	public static int freePkItemIndex = 2; //pk免费开放索引;
	public static bool tipShowFinger; //答题时显示引导
	public static bool levelUnitHasClick; //
	public static bool pkUnitHasClick; //

	public static List<string> fliterList = new List<string>(new string[] {
		"3_2_1",
		"3_2_4",
		"3_5_2",
        //"3_2_3",
		"3_5_3",
		"4_1_1",
		"4_3_1",
		"5_1_3",
		"5_2_3",
		"8_3_2",
		"9_2_1",
		"9_4_1"
	});
	public static string dir = "2|32";
	public class PlayerPrefabKey
    {
		public const string currentNpc = "currentNpc"; //pk  boss 波数
		public const string pkRecords = "pkRecords"; //pk 模式答题记录
		public const string recordProgress = "recordProgress"; //记录当前解锁id
		public const string recordProgressBoss = "recordProgressBoss"; //记录当前boss解锁id
		public const string recordProgressPk = "recordProgressPk"; //记录当前解锁topic
		public const string recordProgressPkUnLock = "recordProgressPkUnLock"; //是否解锁pk选择界面
		public const string recordProgressPkTrophyUnLock = "recordProgressPkTrophyUnLock"; //是否解锁pk选择界面奖杯
		public const string recordData = "recordData"; //记录答题正确率
		public const string linkPos = "linkPos"; //link界面位置
		public const string pkPos = "pkPos"; //pk界面位置
		public const string lastLevelId = "lastLevelId"; //选择界面上次位置
		public const string levelPassed = "levelPassed"; //通关
		public const string nextIsland = "nextIsland"; //下一个岛

		public const string reportUnlock = "reportUnlock"; //报告解锁
		public const string purch = "purch";// 购买

	}
	public class SceneName
    {
		public const string entry = "entry"; //首页
		public const string main = "main";  //岛屿
		public const string pk = "pk";      //pk选择
		public const string pkMecha = "pkMecha";      //pk选择
		public const string title = "title"; //标题
		public const string report = "report"; //报告
	}

	public class ValueConfig
    {
		public const float rightRate=0.6f; //题库正确率
    }

	public class DFTag
	{
		public const string Untagged = "Untagged";
		public const string Player = "Player";
		public const string Enemy = "Enemy";
		public const string SpawnBoldPlayer = "SpawnBoldPlayer";
		public const string SpawnBoldEnemy = "SpawnBoldEnemy";
		public const string UmbrellaPlayer = "UmbrellaPlayer";
		public const string UmbrellaEnemy = "UmbrellaEnemy";
	}

	public class ReferencePlayer
    {
		//public static MotorCtrl motorCtrl;
    }
}
