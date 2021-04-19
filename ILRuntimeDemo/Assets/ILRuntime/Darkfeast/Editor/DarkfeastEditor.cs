#define Occasional
//#define SpineAndTween

//#define Special
//#define DinosaurSelect
//#define Ferr

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
#if Special
using System.CodeDom;
using System.Reflection;
using System.CodeDom.Compiler;
#endif
using System;
using UnityEngine.Rendering;
using UnityEngine.Events;

#if SpineAndTween
using Spine.Unity;
#endif

public class DarkfeastEditor : MonoBehaviour
{
	const string path = @"Darkfeast/";
	const string pathDefault = @"Darkfeast/default/";
	const string pathShotcut = @"Darkfeast/shotcut/";

	const string pathOccasional = @"Darkfeast/occasional/";
	const string pathSpecial = @"Darkfeast/special/";
	const string pathSpecialSpine = @"Darkfeast/spineTween/spine/";
    const string pathSpecialTween = @"Darkfeast/spineTween/tween/";
	const string pathDinosaur = @"Darkfeast/dinosaur/";
	const string pathFerr = @"Darkfeast/ferr/";


	[MenuItem(pathDefault + "CreateFolder(创建默认文件夹)", false,2)]
	public static void CreateNesscessaryFolder()
	{
		if (!Directory.Exists(Application.dataPath + @"\App"))
		{
			Directory.CreateDirectory(Application.dataPath + @"\App");
		}
		else
		{
			Darkfeast.Log("exit app", E_ColorType.Over);
		}
		string[] paths = new string[] { @"\Materials", @"\MyTools", @"\Prefabs", @"\Textures", @"\MyTools\Editor", @"\Scripts" };  //@"\MyTools\Editor",
		for (int i = 0; i < paths.Length; i++)
		{
			if (!Directory.Exists(Application.dataPath + @"\App" + paths[i]))
			{
				Directory.CreateDirectory(Application.dataPath + @"\App" + paths[i]);
				Darkfeast.Log("create " + paths[i], E_ColorType.Init);
			}
			else
			{
				Darkfeast.Log("exit " + paths[i], E_ColorType.Over);
			}
		}
		if(!Directory.Exists(Application.dataPath + @"\Resources"))
        {
			Directory.CreateDirectory(Application.dataPath + @"\Resources");
			Darkfeast.Log("create Resources", E_ColorType.Init);
		}
		else
        {
			Darkfeast.Log("exit Resources", E_ColorType.Over);
		}

		if (!Directory.Exists(Application.dataPath + @"\Scenes"))
		{
			Directory.CreateDirectory(Application.dataPath + @"\Scenes");
			Darkfeast.Log("create Scenes", E_ColorType.Init);
		}
		else
		{
			Darkfeast.Log("exit Scenes", E_ColorType.Over);
		}

		AssetDatabase.Refresh();
		Darkfeast.Log("complete " + Application.dataPath, E_ColorType.UI);
	}

#if Special
	[MenuItem(pathSpecial + "precision(精度)", false, 1)]
	public static void ChangePrecision()
	{
		var allTransfomrs = Resources.FindObjectsOfTypeAll(typeof(Transform));
		Darkfeast.Log("length  " + allTransfomrs.Length);
		int i = 1;
		foreach (var v in allTransfomrs)
		{
			Darkfeast.Log(i + " - " + v.name);

			Transform trans = (v as Transform).GetComponent<Transform>();
			if (trans)   //31.4   17.25     -50
			{
				Vector3 vecPos = trans.localPosition;
				trans.localPosition = ResetVec(vecPos);
				Vector3 vecRot = new Vector3(trans.localEulerAngles.x, trans.localEulerAngles.y, trans.localEulerAngles.z);
				trans.localRotation = Quaternion.Euler(ResetVec(vecRot));
				Vector3 vecSca = trans.localScale;
				trans.localScale = ResetVec(vecSca);
			}
			else
			{
				Darkfeast.Log("err  " + i + "   " + v.name);
			}
			i++;
		}
	}
	public static Vector3 ResetVec(Vector3 vec)
	{

		float x = float.Parse((string.Format("{0:N2}", vec.x)));
		float y = float.Parse((string.Format("{0:N2}", vec.y)));
		float z;
		if (vec.z != 0.01f) //vec3 精度损失 0.01f  0.0
		{
			//Darkfeast.Log("z0 " + vec.z);
			z = float.Parse((string.Format("{0:N2}", vec.z)));
		}
		else
		{
			//Darkfeast.Log("z " + vec.z);
			z = vec.z;
			//Darkfeast.Log("z2 " + vec.z);
		}
		Vector3 res = new Vector3(x, y, z);
		Darkfeast.Log("res  " + res);
		return res;

	}
	[MenuItem(pathSpecial + "Generate cs", false,5)]
	public static void BuildClass()
	{
		string className = "DarkF";
		CodeCompileUnit unit = new CodeCompileUnit();

		CodeNamespace myNamespace = new CodeNamespace("Joh.Test");


		myNamespace.Imports.Add(new CodeNamespaceImport("System"));

		myNamespace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
		CodeTypeDeclaration myClass = new CodeTypeDeclaration(className);

		myClass.IsClass = true;
		myClass.TypeAttributes = TypeAttributes.Public;// | TypeAttributes.Sealed;
		myNamespace.Types.Add(myClass);

		unit.Namespaces.Add(myNamespace);


		CodeMemberField field = new CodeMemberField(typeof(System.String), "str");

		field.Attributes = MemberAttributes.Private;

		myClass.Members.Add(field);

		CodeMemberProperty property = new CodeMemberProperty();

		property.Attributes = MemberAttributes.Public | MemberAttributes.Final;

		property.Name = "Str";
		property.HasGet = true;

		property.HasSet = true;

		property.Type = new CodeTypeReference(typeof(System.String));
		property.Comments.Add(new CodeCommentStatement("this is Str"));

		property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "str")));

		property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "str"), new CodePropertySetValueReferenceExpression()));

		myClass.Members.Add(property);
		CodeMemberMethod method = new CodeMemberMethod();
		method.Name = "Function";

		method.Attributes = MemberAttributes.Public | MemberAttributes.Final;

		method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "number"));

		method.ReturnType = new CodeTypeReference(typeof(int));


		method.Statements.Add(new CodeSnippetStatement(" return number+1; "));
		myClass.Members.Add(method);


		CodeConstructor constructor = new CodeConstructor();

		constructor.Attributes = MemberAttributes.Public;

		myClass.Members.Add(constructor);



		//添加特特性

		//myClass.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(SerializeField))));
		CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

		CodeGeneratorOptions options = new CodeGeneratorOptions();

		//代码风格:大括号的样式{}

		options.BracingStyle = "C";



		//是否在字段、属性、方法之间添加空白行

		options.BlankLinesBetweenMembers = true;

		//输出文件路径

		string outputFile = Application.dataPath + "/App/Scripts/" + className + ".cs";

		//保存
		System.IO.StreamWriter sw = new System.IO.StreamWriter(outputFile);

		provider.GenerateCodeFromCompileUnit(unit, sw, options);
		sw.Flush();
		sw.Close();

		//为指定的代码文档对象模型(CodeDOM) 编译单元生成代码并将其发送到指定的文本编写器，使用指定的选项。(官方解释)

		//将自定义代码编译器(代码内容)、和代码格式写入到sw中

		Darkfeast.Log("finish build csharp");

		AssetDatabase.Refresh();

	}

	private static int buffSize = 1024;

	[MenuItem(pathSpecial + "mergeFile",false,6)]
	public static void MergeFile()
	{
		int ThreadNum = 5;
		string[] FileNames = new string[ThreadNum];

		int bufferSize = 512;
		string downFileNamePath = Application.dataPath + "/App/Pre";
		byte[] bytes = new byte[bufferSize];
		FileStream fs = new FileStream(downFileNamePath, FileMode.Create);
		FileStream fsTemp = null;

		for (int i = 0; i < ThreadNum; i++)
		{
			fsTemp = new FileStream(FileNames[i], FileMode.Open);
			while (true)
			{

				buffSize = fsTemp.Read(bytes, 0, bufferSize);
				if (buffSize > 0)
					fs.Write(bytes, 0, buffSize);
				else
					break;
			}
			fsTemp.Close();
		}
		fs.Close();
	}

	[MenuItem(pathSpecial + "RenameFontCard", false, 11)]
	public static void RenameFontCard()
	{
		string path = Application.dataPath + "/Card";
		for (int i = 1; i <= 18; i++)
		{
			string fullPath = path + "/theme" + i;

			for (int j = 1; j <= 5; j++)
			{
				string path3 = fullPath + "/" + j;

				if (!Directory.Exists(path3))
				{
					Darkfeast.Log("not exist  " + path3);
					continue;
				}

				string[] paths = Directory.GetFiles(path3, "*.png");
				foreach (string p in paths)
				{
					if (p.Contains("_"))
						continue;
					Darkfeast.Log("path  " + p);

					string sourcePath = p.Substring(0, p.Length - 5);
					string index = p.Substring(p.Length - 5, 1);

					sourcePath += i + "_" + j + "_" + index + ".png";
					//File.Copy(p,)
					Darkfeast.Log("sour " + sourcePath, E_ColorType.UI);

					File.Copy(p, sourcePath);
					File.Delete(p);
				}
			}
		}

	}

#endif

#if Occasional
	[MenuItem(pathOccasional + "SingleCreatePrefab", false, 100)]
	public static void SingleCreatePrefab()
	{
		//----------------------------
		//foreach (var g in Selection.gameObjects)
		//{
		//	Transform trs = g.transform;
		//	PrefabUtility.CreatePrefab(@"Assets/App/Prefabs/2test1/" + trs.name + ".prefab", g);
		//}
		//--------------------------------
		string path = "Assets/App/Prefabs/2test1/";
		GameObject[] gos = Selection.gameObjects;
		for (int i = 0; i < gos.Length; i++)
		{
			PrefabUtility.CreatePrefab(path + gos[i].name + ".prefab", gos[i], ReplacePrefabOptions.Default);
		}

		Darkfeast.Log("finished...", E_ColorType.Over);
	}

	[MenuItem(pathOccasional + "Grid9", false, 102)]
	public static void Grid9()
	{
		UnityEngine.Object[] objs = Selection.objects;
		foreach (UnityEngine.Object obj in objs)
		{
			TextureImporterSettings imp = new TextureImporterSettings();
			imp.textureType = TextureImporterType.Sprite;
			imp.spriteMeshType = SpriteMeshType.FullRect;
			imp.spriteMode = (int)SpriteImportMode.Single;
			imp.spritePixelsPerUnit = 100;
			imp.spriteExtrude = 1;
			imp.spriteGenerateFallbackPhysicsShape = true;
			//imp.spriteBorder= new Vector4(60, 120, 60, 120);
			imp.spriteBorder = new Vector4(25, 25, 25, 25);

			imp.sRGBTexture = true;
			imp.alphaSource = TextureImporterAlphaSource.FromInput;
			imp.alphaIsTransparency = true;
			imp.wrapMode = TextureWrapMode.Clamp;
			imp.filterMode = FilterMode.Bilinear;
			imp.aniso = 1;
			TextureImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(obj)) as TextureImporter;

			importer.SetTextureSettings(imp);
		}
	}

	[MenuItem(pathOccasional + "CancelParticleLoop",false,103)]
	public static void CancelParticleLoop()
    {
		GameObject[] gos= Selection.gameObjects;
		foreach(var v in gos)
        {
			FindType<ParticleSystem>(v.transform,t=> {
				//if (t == null)
				//{
				//	Darkfeast.Log("t  is null");
				//	return;
				//}
				//Darkfeast.Log(t);
				ParticleSystem.MainModule main= t.main;
				main.loop = false;
			});
        }
    }

	[MenuItem(pathOccasional + "GenerateSpawn", false, 88)]
	public static void GenerateSpawn()
	{
		GameObject go = Selection.activeGameObject;
		if (go == null) return;
		GameObject spawn = new GameObject("spawn");
		spawn.tag = DFConfig.DFTag.SpawnBoldPlayer;
		spawn.transform.parent = go.transform;
		spawn.transform.SetAsFirstSibling();
		spawn.transform.localPosition = Vector3.zero;
		Selection.activeGameObject = spawn.gameObject;
	}

	[MenuItem(pathOccasional + "ModifySortingGroup", false, 1300)]
	static void ModifySortingGroup()
	{
		List<string> listFilter = new List<string>() {
			"mainCard", //1
			"subCard1",
			"subCard2",
			"subCard3",
			"subCard4",

			"mask", //3
			"maskPk",

			"lock", //5

		};
		Transform root = Selection.activeTransform;
		if (root != null)
		{
			FindType<Renderer>(root, 0, (t, o) => {
				t.sortingOrder += o;
				if (listFilter.Contains(t.transform.name))
				{
					if (t.transform.name == "mask" || t.transform.name == "maskPk")
						t.sortingOrder += 5;
					else if (t.transform.name == "lock")
						t.sortingOrder += 10;
					else
						t.sortingOrder += 1;
				}
				Darkfeast.Log((t.sortingOrder) + "   " + PrintP(t.gameObject));
				SortingGroup sg = t.GetComponent<SortingGroup>();
				if (sg != null)
					DestroyImmediate(sg);
			});
		}
	}
#endif

	static void FindG(Transform parent, UnityAction<Transform> act = null)
	{
		if (parent.childCount > 0)
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				//Darkfeast.Log(parent.GetChild(i).name);
				Transform child = parent.GetChild(i);


				FindG(parent.GetChild(i), act);
			}
		}
		else
		{
			act.Invoke(parent);
		}
	}
	static void FindType<T>(Transform root ,Action<T> action=null) where T: Component
	{
		if (root.childCount>0)
        {
			for(int i=0;i< root.childCount;i++)
            {
				FindType<T>(root.GetChild(i),action);
            }
        }
		T targetT = root.GetComponent<T>();
        if (targetT==null)
        {
			//Darkfeast.Log("not exist: ["+typeof(T)+"] in ["+ root +"]    __path: "+ AssetDatabase.GetAssetPath(root), E_ColorType.Err);
			return;
		}
		action.Invoke(targetT);
	}
	static void FindType<T>(Transform root, int sortOrder = 0, Action<T, int> action = null) where T : Component
	{
		if (root.childCount > 0)
		{
			SortingGroup sgRoot = root.GetComponent<SortingGroup>();
			int sortId = 0;
			if (sgRoot != null)
			{
				sortId = sgRoot.sortingOrder;
			}
			for (int i = 0; i < root.childCount; i++)
			{

				FindType<T>(root.GetChild(i), sortOrder + sortId, action);
			}
			if (sgRoot != null)
			{
				DestroyImmediate(sgRoot);
			}
		}
		T targetT = root.GetComponent<T>();
		if (targetT != null)
		{
			if (action != null)
				action.Invoke(targetT, sortOrder);
		}
		SortingGroup sg = root.GetComponent<SortingGroup>();
		if (sg != null)
			DestroyImmediate(sg);
	}
	static string PrintP(GameObject go)
	{
		List<string> listPath = new List<string>();
		listPath.Add(go.transform.name);
		Transform parent = go.transform.parent;
		while (parent)
		{
			listPath.Add(parent.name);
			parent = parent.parent;
		}

		string path = "";
		for (int i = listPath.Count - 1; i >= 0; i--)
		{
			path += listPath[i] + "/";
		}

		path = path.Substring(0, path.Length - 1);
		return path;
	}


	[MenuItem(pathShotcut+ "SetActiveState &v", false, 83)]
	public static void SetActive()
	{
		GameObject[] gos = Selection.gameObjects;


		foreach (GameObject g in gos)
		{
			if (g.activeSelf)
			{
				g.SetActive(false);
			}
			else
			{
				g.SetActive(true);
			}
		}
	}

	[MenuItem(pathShotcut + "BreakPrefab #c", false, 84)]
	public static void BreakPrefab()
	{
		UnityEngine.Object[] objs = Selection.objects;
		foreach (UnityEngine.Object obj in objs)
		{
#if UNITY_2017
			PrefabUtility.DisconnectPrefabInstance(obj);
#elif UNITY_2019_1_OR_NEWER
			GameObject g = obj as GameObject;
			PrefabUtility.UnpackPrefabInstance(g,PrefabUnpackMode.Completely,InteractionMode.UserAction);
#endif
		}
	}

	[MenuItem(pathShotcut + "PrintPath #p", false, 87)]
	public static void PrintPath()
	{
		GameObject go = Selection.activeGameObject;
		string path = PrintP(go);
		GUIUtility.systemCopyBuffer = "GameObject.Find(\""+ path+"\").transform;";
		Darkfeast.Log("path:   " + path + " ");
	}

	[MenuItem(pathShotcut + "PrintPath2 #a", false, 88)]
	public static void PrintPath2()
	{
		PrintPath();
	}

#if SpineAndTween

	[MenuItem(pathSpecialSpine + "GenerateJellyEff", false, 1009)]
	public static void GenerateJellyEff()
	{
		GameObject[] gos = Selection.gameObjects;

		DFAddJellys(gos[0]);

		Darkfeast.Log("finished...", E_ColorType.Over);
	}

	[MenuItem(pathSpecialSpine + "RecoverJellyEff", false, 1010)]
	public static void RecoverJellyEff()
	{
		GameObject[] gos = Selection.gameObjects;

		//Darkfeast.Log([1]);

		DFRemoveJellys(gos[0]);

		Darkfeast.Log("finished...", E_ColorType.Over);
	}


	[MenuItem(pathSpecialSpine + "GenerateJellyEffByTarget/Default", false, 1011)]
	public static void GenerateJellyEffByTargetDefault()
	{
		GameObject[] gos = Selection.gameObjects;
		DFAddJellys(gos[0]);

		Darkfeast.Log("finished...", E_ColorType.Over);
	}

	[MenuItem(pathSpecialSpine + "GenerateJellyEffByTarget/Center", false, 1012)]
	public static void GenerateJellyEffByTargetCenter()
	{
		GameObject[] gos = Selection.gameObjects;
		DFAddJellys(gos[0], E_JellyType.Center);

		Darkfeast.Log("finished...", E_ColorType.Over);
	}

	[MenuItem(pathSpecialSpine + "GenerateJellyEffByTarget/Bot", false, 1013)]
	public static void GenerateJellyEffByTargetBot()
	{
		GameObject[] gos = Selection.gameObjects;
		DFAddJellys(gos[0], E_JellyType.Bot);

		Darkfeast.Log("finished...", E_ColorType.Over);
	}

	[MenuItem(pathSpecialSpine + "GenerateJellyEffByTarget/Top", false, 1014)]
	public static void GenerateJellyEffByTargetTop()
	{
		GameObject[] gos = Selection.gameObjects;
		DFAddJellys(gos[0], E_JellyType.Top);
		Darkfeast.Log("finished...", E_ColorType.Over);
	}

	[MenuItem(pathSpecialSpine + "GenerateJellyEffByTarget/SpecialBot", false, 1015)]
	public static void GenerateJellyEffByTargetSpecialBot()
	{
		GameObject[] gos = Selection.gameObjects;
		DFAddJellys(gos[0], E_JellyType.Special);
		Darkfeast.Log("finished...", E_ColorType.Over);
	}

	[MenuItem(pathSpecialSpine + "GeneraeJellyAuto", false, 1016)]
	public static void GeneraeJellyAuto()
    {
		GameObject root = GameObject.Find("themes");
		for(int i=1;i<=18;i++)
        {
			Transform island = root.transform.Find("isLand" + i);
			if (island == null)
				continue;

			Transform d = island.Find("decorationInteract_d");
			if (d != null)
            {
				DFRemoveJellys(d.gameObject);
				DFAddJellys(d.gameObject, E_JellyType.Default);
			}
			
			else
				Darkfeast.Log("island " + i + "  " + "d is null");

			Transform b = island.Find("decorationInteract_b");
			if (b != null)
            {
				DFRemoveJellys(b.gameObject);
				DFAddJellys(b.gameObject, E_JellyType.Bot);
			}
				
			else
				Darkfeast.Log("island " + i + "  " + "b is null");

			Transform s = island.Find("decorationInteract_s");
			if (s != null)
            {
				DFRemoveJellys(s.gameObject);
				DFAddJellys(s.gameObject, E_JellyType.Special);
			}
			else
				Darkfeast.Log("island " + i + "  " + "s is null");
		}
    }


	[MenuItem(pathSpecialSpine + "Weight/SetSpineAnimWeight", false, 1100)]
	public static void SetSpineAnimWeight()
	{
		GameObject go = Selection.activeGameObject;
		SkeletonAnimation skeleAnim = go.GetComponent<SkeletonAnimation>();
		if (skeleAnim == null)
			return;
		Spine.ExposedList<Spine.Animation> listAnim = skeleAnim.state.Data.SkeletonData.Animations;
		DFSpineLoopCmp loop = go.GetComponent<DFSpineLoopCmp>();
		if (loop == null)
			loop = go.AddComponent<DFSpineLoopCmp>();

		loop.listName = new List<string>();
		loop.listWeight = new List<DFSpineLoopCmp.Weight>();
		int id = 0;
		foreach (var v in listAnim)
		{
			loop.listName.Add(v.Name);
			DFSpineLoopCmp.Weight weight = new DFSpineLoopCmp.Weight();
			weight.id = id;
			weight.weight = 10;
			loop.listWeight.Add(weight);
			id++;
		}
	}

	[MenuItem(pathSpecialSpine + "InitAnim/AddAnimGameObject", false, 1200)]
	public static void AddAnimGameObject()
	{
		GameObject go = Selection.activeGameObject;
		GameObject goEmp = new GameObject("decorationInteract_s");
		goEmp.AddComponent<SortingGroup>().sortingOrder = 40;
		goEmp.transform.parent = go.transform;
		goEmp.transform.localPosition = Vector3.zero;
		GameObject goAnim = new GameObject("anim");
		goAnim.transform.parent = goEmp.transform;
		goAnim.transform.localPosition = Vector3.zero;
		SkeletonAnimation anim = goAnim.AddComponent<SkeletonAnimation>();
	}

	[MenuItem(pathSpecialSpine + "InitAnim/SetAnimIdleLoop", false, 1201)]
	public static void SetAnimIdleLoop()
	{
		GameObject go = Selection.activeGameObject;
		FindType<SkeletonAnimation>(go.transform,(anim)=> {
			if (anim.state == null)
				return;
			anim.AnimationName = "idle";
			//anim.state.SetAnimation(0, "idle", true);
			Darkfeast.Log("set " + anim.name + " idle");
			anim.loop = true;
		});
		Darkfeast.Log("SetAnimIdleLoop finish...", E_ColorType.Over);
	}

	public static void AddTweenT<T>() where T : MonoBehaviour
	{
		GameObject[] gos = Selection.gameObjects;

		foreach (GameObject go in gos)
		{
			//Component[] coms= go.GetComponents<Component>();
			//SerializedObject so = new SerializedObject(go);
			//SerializedProperty soProp= so.FindProperty("m_Compoment");
			//int c = 0;
			//for(int i=0;i<coms.Length;i++)
			//         {
			//	soProp.DeleteArrayElementAtIndex(i - c);
			//	Darkfeast.Log("clear " + go.name);
			//	c++;
			//         }
			//if(c>0)
			//         {
			//	so.ApplyModifiedProperties();
			//         }

			//我用的unity2019.1.12，这个版本修改了这个方法，有一个自带的删除方法，
			//可能是这个GameObjectUtility.RemoveMonoBehavioursWithMissingScript，
			//int count = 0;
			//for (int i=0;i<coms.Length;i++)
			//         {
			//	if (coms[i] == null)
			//             {
			//		DestroyImmediate(coms[i]);
			//		count++;
			//	}
			//         }
			//Darkfeast.Log("delete valid script " + count, E_ColorType.UI);
			T df = go.GetComponent<T>();
			if (df != null)
				DestroyImmediate(df);
			go.AddComponent<T>();
		}
	}
	[MenuItem(pathSpecialTween + "AddTween/Position/Yoyo/X", false, 1020)]
	public static void AddTweenPositionYoyoX()
	{
		AddTweenT<DFTweenPositionYoyoXCmp>();
	}
	[MenuItem(pathSpecialTween + "AddTween/Position/Yoyo/Y", false, 1021)]
	public static void AddTweenPositionYoyoY()
	{
		AddTweenT<DFTweenPositionYoyoXCmp>();
	}
	[MenuItem(pathSpecialTween + "AddTween/Position/Repeat/X", false, 1022)]
	public static void AddTweenPositionRepeatX()
	{
		AddTweenT<DFTweenPositionRepeatXCmp>();
	}
	[MenuItem(pathSpecialTween + "AddTween/Position/Repeat/Y", false, 1023)]
	public static void AddTweenPositionRepeatY()
	{
		AddTweenT<DFTweenPositionRepeatYCmp>();
	}

	[MenuItem(pathSpecialTween + "AddTween/Alpha/Yoyo", false, 1030)]
	public static void AddTweenAlphaYoyo()
	{
		AddTweenT<DFTweenAlphaYoyoCmp>();
	}
	[MenuItem(pathSpecialTween + "AddTween/Alpha/Repeat", false, 1031)]
	public static void AddTweenAlphaRepeat()
	{
		AddTweenT<DFTweenAlphaRepeatCmp>();
	}
	[MenuItem(pathSpecialTween + "AddTween/Scale/Yoyo", false, 1040)]
	public static void AddTweenScaleYoyo()
	{
		AddTweenT<DFTweenScaleYoyoCmp>();
	}
	[MenuItem(pathSpecialTween + "AddTween/Scale/Repeat", false, 1041)]
	public static void AddTweenScaleRepeat()
	{
		AddTweenT<DFTweenScaleRepeatCmp>();
	}

	public enum E_JellyType
	{
		Default,
		Center,
		Bot,
		Top,
		Special,
	}
	static void DFAddJellys(GameObject rootGo, E_JellyType etype = E_JellyType.Default)
	{
		List<Transform> trsList = new List<Transform>();
		for (int i = 0; i < rootGo.transform.childCount; i++)
		{
			trsList.Add(rootGo.transform.GetChild(i));
		}
		foreach (var v in trsList)
		{
			DFAddJelly(v.gameObject, etype);
		}
	}
	public static void DFAddJelly(GameObject go, E_JellyType e_jelly = E_JellyType.Default)
	{
		if (go.name.Contains("Root_"))
		{
			Darkfeast.Log(go.name + " has exist", E_ColorType.Err);
			return;
		}

		BoxCollider2D box2d = go.GetComponent<BoxCollider2D>();
		if (box2d == null)
			box2d = go.AddComponent<BoxCollider2D>();

		box2d.size = box2d.size * Mathf.Abs(go.transform.localScale.x);
		//string[] splitStr = go.name.Split('_');
		string str;
		float offsetY;

		switch (e_jelly)
		{
			case E_JellyType.Top:
				str = "t";
				offsetY = -box2d.size.y / 2;
				break;
			case E_JellyType.Bot:
				str = "b";
				offsetY = box2d.size.y / 2;
				break;
			case E_JellyType.Special:
				str = "s";
				offsetY = 0;
				break;
			case E_JellyType.Default:
				str = "d";
				offsetY = box2d.size.y / 6;
				break;
			default:
				str = "c";
				offsetY = 0;
				break;
		}
		Transform trsParent = go.transform.parent;
		GameObject p = new GameObject(go.name + "Root_" + str);
		if (trsParent != null)
		{
			p.transform.parent = trsParent;
			p.transform.localPosition = go.transform.localPosition;
		}
		else
		{
			p.transform.position = go.transform.position;
		}
		go.transform.parent = p.transform;
		go.transform.localPosition += new Vector3(0, offsetY, 0);
		p.transform.localPosition = new Vector3(p.transform.localPosition.x, p.transform.localPosition.y - offsetY, p.transform.localPosition.z);
		BoxCollider2D box2dN = p.AddComponent<BoxCollider2D>();
		box2dN.size = box2d.size;
		box2dN.offset = new Vector2(box2d.offset.x, box2d.offset.y + offsetY);

		if (e_jelly == E_JellyType.Special)
			box2dN.offset = new Vector2(box2dN.offset.x * go.transform.localScale.x, box2dN.offset.y * go.transform.localScale.y);
		//if (go.transform.localScale.x<1)
		//      {
		//          box2dN.offset = new Vector2(box2dN.offset.x * go.transform.localScale.x, box2dN.offset.y * go.transform.localScale.y);
		//      }
		//else
		//      {
		//	//box2dN.offset = new Vector2(box2dN.offset.x / go.transform.localScale.x, box2dN.offset.y / go.transform.localScale.y);
		//}

		DestroyImmediate(box2d); //编辑器下用imme
								 //Destroy(box2d);
								 //p.AddComponent<DarkfeastTweenJellyEvent>().AddJellyEvent(p);
		p.AddComponent<DFTweenJellyCmp>();
	}

	public static void DFRemoveJellys(GameObject rootGo)
	{
		List<Transform> trsList = new List<Transform>();
		for (int i = 0; i < rootGo.transform.childCount; i++)
		{
			trsList.Add(rootGo.transform.GetChild(i));
		}
		foreach (var v in trsList)
		{
			DFRemoveJelly(v);
		}
	}
	public static void DFRemoveJelly(Transform go)
	{
		string[] strs = go.name.Split(new string[] { "Root" }, StringSplitOptions.None);
		if (strs.Length != 2)
			return;
		string[] strs2 = strs[1].Split('_');
		if (strs2.Length != 2)
			return;
		Transform child = go.GetChild(0);
		Transform parent = go.parent;
		BoxCollider2D box2d = go.GetComponent<BoxCollider2D>();
		if (box2d == null)
		{
			DFTweenJellyCmp df = go.GetComponent<DFTweenJellyCmp>();
			if (df != null)
				DestroyImmediate(df);
			return;
		}

		string str = "d";
		float offsetY = box2d.size.y / 6;

		switch (strs2[1])
		{
			case "t":
				str = "t";
				offsetY = -box2d.size.y / 2;
				break;
			case "b":
				str = "b";
				offsetY = box2d.size.y / 2;
				break;
			case "s":
				str = "s";
				offsetY = 0;
				break;
			default:
				break;
		}
		child.parent = parent;
		child.localPosition = go.localPosition + new Vector3(0, offsetY, 0);
		DestroyImmediate(go.gameObject);
	}
	//d ：1/6
	//c : center
	//t
	//b
	//添加时 判断当前方法的类型  然后自动加后缀
	//移除时 根据后缀还原
	//树的图片  名字要带有tree  点击时，判断物体名字是否包含tree,   来播放对应的音效

#endif


#if DinosaurSelect

	[MenuItem(pathDinosaur + "ResetIslandPos", false, 1200)]
	public static void ResetIslandPos()
	{
		float islandDis = 24.05f;
		//float transDis= 12
		string islandName = "isLand";
		List<Transform> listIsland = new List<Transform>();
		for (int i = 5; i <= 18; i++)
		{
			GameObject islandGo = GameObject.Find(islandName + i);
			if (islandGo == null)
				Darkfeast.Log("land  " + i + "  is null", E_ColorType.Err);
			listIsland.Add(islandGo.transform);

		}
		Transform isLand4 = GameObject.Find("isLand4").transform;

		listIsland[0].position = new Vector3(isLand4.position.x, 0, 0) + new Vector3(islandDis, 0, 0);

		for (int i = 6; i <= 18; i++)
		{
			listIsland[i - 5].position = listIsland[i - 6].position + new Vector3(islandDis, 0, 0);
		}

	}

	[MenuItem(pathDinosaur + "GenerateTrans", false, 1201)]
	public static void GenerateTrans()
	{
		float yScale = 28.2f;
		string path = Application.dataPath + "/App/Textures/0select/trans";
		Transform transGo = GameObject.Find("Trans").transform;
		string[] paths = Directory.GetFiles(path, "*.png");

		Dictionary<string, string> dictPath = new Dictionary<string, string>();
		List<string> listPath = new List<string>();

		for (int i = 0; i < paths.Length; i++)
		{
			int index = paths[i].IndexOf("Assets");
			string realPath = paths[i].Substring(index, paths[i].Length - index);
			paths[i] = realPath;
		}
		foreach (var v in paths)
		{
			string[] str = v.Split('色');
			string key = str[1].Split('.')[0];
			dictPath.Add(key, v);
		}

		foreach (var v in dictPath)
		{
			//Darkfeast.Log(v.Key+"   "+v.Value);
		}

		for (int i = 1; i < 18; i++)
		{
			string v;
			if (dictPath.TryGetValue(i.ToString(), out v))
				listPath.Add(v);
		}

		for (int i = 2; i <= 17; i++)
		{
            GameObject go = new GameObject("t" + i);
            go.transform.parent = transGo;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(1, yScale, 1);

            SpriteRenderer sp = go.AddComponent<SpriteRenderer>();
            //SpriteRenderer sp = transGo.Find("t" + i).GetComponent<SpriteRenderer>();
			sp.sortingOrder = -50;
			Sprite s = AssetDatabase.LoadAssetAtPath(listPath[i - 1], typeof(Sprite)) as Sprite;
			sp.sprite = s;
		}
	}
	[MenuItem(pathDinosaur + "ResetTransPos", false, 1202)]
	public static void ResetTransPos()
	{
		float islandDis = 24.05f;
		//float transDis= 12
		string islandName = "isLand";
		List<Transform> listIsland = new List<Transform>();
		List<Transform> listTrans = new List<Transform>();
		for (int i = 1; i <= 18; i++)
		{
			GameObject islandGo = GameObject.Find(islandName + i);
			if (islandGo == null)
				Darkfeast.Log("land  " + i + "  is null", E_ColorType.Err);
			listIsland.Add(islandGo.transform);

		}
		for (int i = 1; i <= 17; i++)
		{
			GameObject islandGo = GameObject.Find("Trans/t" + i);
			if (islandGo == null)
				Darkfeast.Log("trans  " + i + "  is null", E_ColorType.Err);
			listTrans.Add(islandGo.transform);
		}

		for (int i = 1; i < 17; i++)
		{
			listTrans[i].position = (listIsland[i + 1].position + listIsland[i].position) / 2;
		}

		for (int i = 6; i <= 18; i++)
		{
			listIsland[i - 5].position = listIsland[i - 6].position + new Vector3(islandDis, 0, 0);
		}

	}

	[MenuItem(pathDinosaur + "FindRoadLine", false, 1250)]
	public static void FindRoadLine()
	{
		string pathCard = "Assets/App/Textures/0select/Card/theme";
		string pathButtonBg = "Assets/App/Textures/0select/岛";
		string pathMask = "Assets/App/Textures/0select/common/mask/";
		GameObject root = GameObject.Find("themes");

		UnityEngine.GameObject obj = AssetDatabase.LoadAssetAtPath("Assets/App/Prefabs/0select/origin/LevelNode.prefab", typeof(UnityEngine.GameObject)) as GameObject;


		Sprite spMask = AssetDatabase.LoadAssetAtPath(pathMask + "mask.png", typeof(Sprite)) as Sprite;
		Sprite spMaskPk = AssetDatabase.LoadAssetAtPath(pathMask + "maskPk.png", typeof(Sprite)) as Sprite;
		Sprite spMaskLock = AssetDatabase.LoadAssetAtPath(pathMask + "lock.png", typeof(Sprite)) as Sprite;

		//GameObject go2 = PrefabUtility.InstantiatePrefab(obj) as GameObject;
		//PrefabUtility.DisconnectPrefabInstance(go2);
		//go.name = i + "_" + j;
		//return;

		for (int i = 1; i <= 18; i++)
		{
			Transform island = root.transform.Find("isLand" + i + "/roadLine");

			for (int j = 0; j < 6; j++)
			{
				Transform road = island.GetChild(j);
				if (road.childCount > 0)
				{
					for (int k = 0; k < road.childCount; k++)
					{
						Darkfeast.Log("delete " + road.name + "  " + k + "  child  " + road.GetChild(k).name);
						DestroyImmediate(road.GetChild(k).gameObject);
					}
				}
				//Darkfeast.Log(road.name,E_ColorType.UI);
				GameObject go = PrefabUtility.InstantiatePrefab(obj) as GameObject;
				PrefabUtility.DisconnectPrefabInstance(go);
				go.name = i + "_" + (j + 1);
				go.transform.parent = road;
				go.transform.localPosition = Vector3.zero;
				go.AddComponent<LevelUtil>();

				SpriteRenderer srBg = go.transform.Find("bg").GetComponent<SpriteRenderer>();
				SpriteRenderer srMain = go.transform.Find("mainCard").GetComponent<SpriteRenderer>();
				SpriteRenderer srSub1 = go.transform.Find("subCard1").GetComponent<SpriteRenderer>();
				SpriteRenderer srSub2 = go.transform.Find("subCard2").GetComponent<SpriteRenderer>();
				SpriteRenderer srSub3 = go.transform.Find("subCard3").GetComponent<SpriteRenderer>();
				SpriteRenderer srSub4 = go.transform.Find("subCard4").GetComponent<SpriteRenderer>();
				SpriteRenderer srMask = go.transform.Find("mask").GetComponent<SpriteRenderer>();
				SpriteRenderer srMaskPk = go.transform.Find("maskPk").GetComponent<SpriteRenderer>();
				SpriteRenderer srLock = go.transform.Find("lock").GetComponent<SpriteRenderer>();

				Sprite spBg;
				Sprite spMain;
				Sprite spSub1;
				Sprite spSub2;
				Sprite spSub3;
				Sprite spSub4;

				if (j == 5)
				{
					srMain.gameObject.SetActive(false);
					srSub1.gameObject.SetActive(false);
					srSub2.gameObject.SetActive(false);
					srSub3.gameObject.SetActive(false);
					srSub4.gameObject.SetActive(false);
					srMask.gameObject.SetActive(false);
					spBg = AssetDatabase.LoadAssetAtPath(pathButtonBg + i + "/pk按钮.png", typeof(Sprite)) as Sprite;
					srBg.sprite = spBg;
					srMaskPk.sprite = spMaskPk;
					srLock.sprite = spMaskLock;
				}
				else
				{
					srMaskPk.gameObject.SetActive(false);

					spBg = AssetDatabase.LoadAssetAtPath(pathButtonBg + i + "/按钮.png", typeof(Sprite)) as Sprite;
					srBg.sprite = spBg;

					spMain = AssetDatabase.LoadAssetAtPath(pathCard + i + "/" + (j + 1) + "/" + i + "_" + (j + 1) + ".png", typeof(Sprite)) as Sprite;
					spSub1 = AssetDatabase.LoadAssetAtPath(pathCard + i + "/" + (j + 1) + "/" + i + "_" + (j + 1) + "_1.png", typeof(Sprite)) as Sprite;
					spSub2 = AssetDatabase.LoadAssetAtPath(pathCard + i + "/" + (j + 1) + "/" + i + "_" + (j + 1) + "_2.png", typeof(Sprite)) as Sprite;
					spSub3 = AssetDatabase.LoadAssetAtPath(pathCard + i + "/" + (j + 1) + "/" + i + "_" + (j + 1) + "_3.png", typeof(Sprite)) as Sprite;
					spSub4 = AssetDatabase.LoadAssetAtPath(pathCard + i + "/" + (j + 1) + "/" + i + "_" + (j + 1) + "_4.png", typeof(Sprite)) as Sprite;

					srMain.sprite = spMain;
					srSub1.sprite = spSub1;
					srSub2.sprite = spSub2;
					srSub3.sprite = spSub3;
					srSub4.sprite = spSub4;

					srMask.sprite = spMask;
					srLock.sprite = spMaskLock;
				}
			}
		}
	}

#endif

#if Ferr

	[MenuItem(pathFerr + "1 CreatePoint", false, 5000)]
	public static void CreatePoint()
	{
		Darkfeast.Log("创建root后 按下shift+leftMouse 创建点 最后CreateFerr");
		//Camera cam = GameObject.Find("Camera").GetComponent<Camera>();


		GameObject parent = GameObject.Find("root");
		if (parent == null)
		{
			parent = new GameObject("root");
			parent.AddComponent<Root>();
		}
		parent.transform.position = Vector3.zero;
		Selection.activeGameObject = parent;
		//UnityEngine.Debug.DrawRay(r.origin, r.direction*10000, Color.cyan,1);
		//g.transform.localPosition = cam.ScreenToWorldPoint(Input.mousePosition);
		//g.transform.position = cam.

		//Ray r = cam.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + cam.pixelHeight));
		//Vector3 mousePos = r.origin;//mousepos向量保存射线的来源

		//GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//Vector3 aligned = new Vector3(mousePos.x,mousePos.y,0.0f);

		//obj.transform.position = aligned;
		////注册撤消操作
		//Undo.RegisterCreatedObjectUndo(obj, "Create" + obj.name);
	}

	[MenuItem(pathFerr + "2 CreateFerr", false, 5002)]
	public static void CreateFerr()
	{
		GameObject go = new GameObject("Ferr");
		Ferr2DT_PathTerrain ferr = go.AddComponent<Ferr2DT_PathTerrain>();

		Transform root = GameObject.Find("root").transform;
		int count = root.childCount;
		ferr.AddPoint(new Vector2(-50, -50));
		ferr.AddPoint(new Vector2(100, -50), 0);

		for (int i = 0; i < count; i++)
		{
			ferr.AddPoint(root.GetChild(i).position, ferr.PathData.GetPoints().Count);
		}


	}
	[MenuItem(pathFerr + "ModifyOffset", false, 5004)]
	public static void ModifyOffset()
	{
		GameObject root = GameObject.Find("root");
		if (root == null)
			return;

		float offsetXY = 0.5f;

		int count = root.transform.childCount;
		List<Vector3> listPos = new List<Vector3>();
		for (int i = 0; i < count; i++)
		{
			listPos.Add(root.transform.GetChild(i).transform.position);
		}

		for (int i = 0; i < listPos.Count - 1; i++)
		{
			Vector3 next = listPos[i + 1];
			Vector3 before = listPos[i];
			float x = next.x;
			float y = next.y;
			if (next.x - before.x != 0 && next.x - before.x < offsetXY)
			{
				x = before.x;
			}
			if (next.y - before.y != 0 && next.y - before.y < offsetXY)
			{
				y = before.y;
			}
			listPos[i + 1] = new Vector3(x, y, listPos[i + 1].z);
		}

		for (int i = 0; i < count; i++)
		{
			root.transform.GetChild(i).position = listPos[i];
		}
		Darkfeast.Log("finished", E_ColorType.UI);
	}

	[MenuItem(pathFerr + "ResetFerrZ", false, 5005)]
	public static void ResetFerrZ()
	{
		int count = 0;
		GameObject root = Selection.activeGameObject;
		FindType<Ferr2DT_PathTerrain>(root.transform, (ferr) => {
			ferr.fillZ = 0;
			count++;
			Darkfeast.Log(ferr.name + "  has modify", E_ColorType.UI);
		});
		Darkfeast.Log("finish " + count);
	}

#endif

}
[CustomEditor(typeof(Root))]
public class PointLine : Editor
{
	void OnSceneGUI()
	{
		if (Event.current.shift)
		{
			Ray r = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
			Vector3 worldPt = r.GetPoint(0);

			worldPt = new Vector3(worldPt.x, worldPt.y, 0);

			if (Handles.Button(worldPt, SceneView.lastActiveSceneView.camera.transform.rotation, 10, 10,
					Root.EmptyCap
				))
			{
				GameObject parent = GameObject.Find("root");
				if (parent == null)
				{
					parent = new GameObject("root");
					parent.AddComponent<Root>();
				}

				GameObject g = new GameObject("point");
				g.transform.parent = parent.transform;
				g.transform.position = worldPt;
			}
		}
	}
}

public class Root : MonoBehaviour
{
	public static void EmptyCap(int aControlID, Vector3 aPosition, Quaternion aRotation, float aSize, EventType aEvent)
	{
		if (aEvent == EventType.Layout)
		{
			HandleUtility.AddControl(aControlID, HandleUtility.DistanceToRectangle(aPosition, aRotation, aSize));
		}
	}
}