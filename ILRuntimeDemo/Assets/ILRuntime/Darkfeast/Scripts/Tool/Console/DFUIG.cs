using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DFUIG : MonoBehaviour
{
    GameObject ui_downTip;
    Text ui_Tip;
    Slider ui_Progress;
    Text ui_Speed;
    Button btn_clear;
    Button btn_ShowOrHide;

    private void Start()
    {
        InitUI();

        Application.logMessageReceived += ShowTips;
    }
    void InitUI()
    {
        Sprite sp = Resources.Load<Sprite>("Textures/color");
        Sprite mask = Resources.Load<Sprite>("Textures/mask68");

        GameObject canvasGo = new GameObject("Canvas");
        Canvas canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGo.AddComponent<CanvasScaler>();
        canvasGo.AddComponent<GraphicRaycaster>();

        GameObject scrollViewGo = new GameObject("Scroll View");
        scrollViewGo.transform.parent = canvasGo.transform;
        ScrollRect scrollRect = scrollViewGo.AddComponent<ScrollRect>();
        scrollRect.horizontal = false;

        Image scrollViewImg = scrollViewGo.AddComponent<Image>();
        scrollViewImg.sprite = sp;
        //scrollViewImg.color = new Color(0.5f, 0.5f, 0.5f, 0.55f); //
        scrollViewImg.color = new Color(0.33f, 0.3f, 0.3f, 0.91f); //

        //RectTransform rectScrollView = scrollViewGo.GetComponent<RectTransform>();
        RectTransform rectScrollView = SetRect(scrollViewGo, 0.5f, 0.5f, 0.5f, 1, 0.5f, 1);
        rectScrollView.sizeDelta = new Vector2(820, 1410);
        rectScrollView.localPosition = new Vector2(0, 160);   // Vector2.zero; PosY= Y - canvas.PosY  

        GameObject viewportGo = new GameObject("Viewport");
        Image viewportImg = viewportGo.AddComponent<Image>();
        viewportImg.sprite = sp; //mask
        //viewPortImg.type = Image.Type.Sliced;
        //viewPortImg.fillCenter = true;

        Mask viewPortMask = viewportGo.AddComponent<Mask>();
        viewPortMask.showMaskGraphic = false;

        viewportGo.transform.parent = scrollViewGo.transform;
        viewportGo.transform.localPosition = Vector3.zero;

        scrollRect.viewport = SetRect(viewportGo, 0.5f, 1, 0, 0, 1, 1, 0, 0, 0, 0);

        GameObject contentGo = new GameObject("Content");
        contentGo.transform.parent = viewportGo.transform;
        contentGo.transform.localPosition = Vector3.zero;
        scrollRect.content = SetRect(contentGo, 0.5f, 1, 0, 0, 1, 1, 0, -900, 0, 0);

        GameObject tipGo = new GameObject("Tip");
        tipGo.transform.parent = contentGo.transform;
        tipGo.transform.localPosition = Vector3.zero;
        Text textTip = tipGo.AddComponent<Text>();
        SetRect(tipGo, 0.5f, 1f, 0, 0, 1, 1, 0, 0, 0, -60);
        textTip.text = "---------------------------------------------------------------------------------";

        textTip.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textTip.fontSize = 30;


        GameObject downTipGo = new GameObject("downTip");
        downTipGo.transform.parent = canvasGo.transform;

        RectTransform rectDownTip = SetRect(downTipGo, 0.5f, 0.5f, 0.5f, 1, 0.5f, 1f);
        downTipGo.transform.localPosition = new Vector3(0, 818, 0);
        //downTipGo.transform.localPosition = Vector3.zero;

        GameObject downGo = new GameObject("down");
        Text downText = downGo.AddComponent<Text>();
        downGo.transform.parent = downTipGo.transform;
        downGo.transform.localPosition = new Vector3(0, 0, 0);
        downText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        downText.fontSize = 26;
        downText.text = "下载进度:";
        downText.color = new Color(0.6f, 1, 0.1f);
        RectTransform rectDown = SetRect(downGo, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f);
        rectDown.sizeDelta = new Vector2(127, 34);
        rectDown.localPosition = new Vector3(-320, -1.7f, 0);


        GameObject speedGo = new GameObject("speed");
        Text speedText = speedGo.AddComponent<Text>();
        speedGo.transform.parent = downTipGo.transform;
        speedGo.transform.localPosition = new Vector3(0, 0, 0);
        speedText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        speedText.fontSize = 27;
        speedText.color = new Color(0, 1, 0.84f);
        RectTransform rectSpeed = SetRect(speedGo, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f);
        rectSpeed.sizeDelta = new Vector2(130, 30);
        rectSpeed.localPosition = new Vector3(332, 0, 0);


        GameObject downProgressGo = new GameObject("downProgress");
        downProgressGo.transform.parent = downTipGo.transform;
        //downProgressGo.transform.localPosition = Vector3.zero;
        Slider downProgressSlider = downProgressGo.AddComponent<Slider>();
        RectTransform rectDownProgress = downProgressGo.GetComponent<RectTransform>();
        rectDownProgress.sizeDelta = new Vector2(520, 54);
        rectDownProgress.localPosition = new Vector3(0, 0, 0);

        GameObject backGroundGo = new GameObject("background");
        backGroundGo.transform.parent = downProgressGo.transform;
        Image backGroundImg = backGroundGo.AddComponent<Image>();
        backGroundImg.sprite = sp;
        SetRect(backGroundGo, 0.5f, 0.5f, 0, 0.25f, 1, 0.75f, 0, 0, 0, 0);

        GameObject fillGo = new GameObject("fill");
        fillGo.transform.parent = backGroundGo.transform;
        Image fillImg = fillGo.AddComponent<Image>();
        fillImg.sprite = sp;
        fillImg.color = new Color(0.02f, 1, 0.11f, 1);
        RectTransform rectFill = fillGo.GetComponent<RectTransform>();
        downProgressSlider.targetGraphic = fillImg;
        downProgressSlider.fillRect = rectFill;
        SetRect(fillGo, 0.5f, 0.5f, 0, 0, 1, 1);//必须位于 downProgressSlider.fillRect = rectFill;之后
        downProgressSlider.value = 0.001f;

        GameObject systemGo = new GameObject("EventSystem");
        systemGo.AddComponent<EventSystem>();
        systemGo.AddComponent<StandaloneInputModule>();
        systemGo.AddComponent<BaseInput>();

        GameObject btnClear = new GameObject("btnClose");
        btnClear.AddComponent<Image>();
        btnClear.SetParent(canvasGo.transform);
        //btnClear.
        //SetRect(btnClear, 0.5f, 0.5f, 0.5f, 1, 0.5f,1);
        RectTransform rectBtnClear= SetRect(btnClear);
        rectBtnClear.sizeDelta = new Vector2(150, 60);
        rectBtnClear.localPosition = new Vector3(-220, -680, 0);

        Button btC= btnClear.AddComponent<Button>();
        btC.onClick.AddListener(() => {
            ui_Tip.text = "";
        });

        GameObject btnShow = new GameObject("btnShow");
        btnShow.AddComponent<Image>();
        btnShow.SetParent(canvasGo.transform);
        //btnClear.
        //SetRect(btnClear, 0.5f, 0.5f, 0.5f, 1, 0.5f,1);
        RectTransform rectBtnShow = SetRect(btnShow);
        rectBtnShow.sizeDelta = new Vector2(150, 60);
        rectBtnShow.localPosition = new Vector3(220, -680, 0);

        Button btS = btnShow.AddComponent<Button>();
        btS.onClick.AddListener(() => {
            scrollViewGo.State(!scrollViewGo.activeSelf);
        });


        ui_Tip = textTip;

        ui_Progress = downProgressSlider;
        ui_Speed = GameObject.Find("Canvas/downTip/speed").GetComponent<Text>();
        ui_downTip = downTipGo;
        ui_downTip.SetActive(false);
        ui_Progress = downProgressSlider;

        scrollViewGo.State(false);
    }

    RectTransform SetRect(GameObject go, float pivotX = 0.5f, float pivotY = 0.5f, float anchorMinX = 0.5f, float anchorMinY = 0.5f, float anchorMaxX = 0.5f, float anchorMaxY = 0.5f, float offsetMinX = 0, float offsetMinY = 0, float offsetMaxX = 0, float offsetMaxY = 0)
    {

        RectTransform rect = go.GetComponent<RectTransform>();
        if (rect == null)
            rect = go.AddComponent<RectTransform>();
        rect.pivot = new Vector2(pivotX, pivotY);
        rect.anchorMin = new Vector2(anchorMinX, anchorMinY);
        rect.anchorMax = new Vector2(anchorMaxX, anchorMaxY);
        rect.offsetMin = new Vector2(offsetMinX, offsetMinY);
        rect.offsetMax = new Vector2(offsetMaxX, offsetMaxY);
        return rect;
    }

    void ShowTips(string msg, string stackTrace, LogType type)
    {
        ui_Tip.text += msg + "\r\n";
        //tips = msg;
        //tips += "\r\n";
        Text t = null;

        //t.alignment = TextAnchor.MiddleCenter;
        //t.fontSize = 3;
        RectTransform rec = null;
        //rec.rect.siz
        //rec.sizeDelta
        //rec.pivot = new Vector2(0.5f, 0.5f);
        //rec.anchorMin = new Vector2(0, 0);
        //rec.anchorMax = new Vector2(0.5f, 0.5f);
        //rec.offsetMin =  Vector2.zero;
        //rec.offsetMax =  Vector2.zero;

    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= ShowTips;
    }
}
