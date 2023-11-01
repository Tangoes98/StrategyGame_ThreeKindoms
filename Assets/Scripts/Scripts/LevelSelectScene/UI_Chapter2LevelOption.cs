using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Chapter2LevelOption : BasePanel
{
    public Image imageLevelIconMain1;
    public Image imageLevelIconSub1;

    public Button buttonToNextLevel;

    bool MainLevel1IsFinished;
    bool SubLevel1IsFinished;

    protected override void Awake()
    {
        base.Awake();

        //当按下关卡按钮时候显示对应信息
        GetControl<Button>("ButtonMainLevel").onClick.AddListener(showMainLevel1Panel);
        GetControl<Button>("ButtonSubLevel").onClick.AddListener(showSubLevel1Panel);
        GetControl<Button>("ButtonToNextChapter").onClick.AddListener(ToNextChapter);

        //Icon的鼠标移入监听
        ///////////////////////////////////////////////////////////////////////////////////////////////
        //监听鼠标移入和鼠标移出的事件，进行处理
        EventTrigger triggerMain = GetControl<Button>("ButtonMainLevel").gameObject.AddComponent<EventTrigger>();
        EventTrigger triggerSub = GetControl<Button>("ButtonSubLevel").gameObject.AddComponent<EventTrigger>();

        //申明一个鼠标进入的事件类对象
        EventTrigger.Entry enterMain = new EventTrigger.Entry();
        enterMain.eventID = EventTriggerType.PointerEnter;
        enterMain.callback.AddListener(MouseEnterMainButton);

        //申明一个鼠标移出的事件类对象
        EventTrigger.Entry exitMain = new EventTrigger.Entry();
        exitMain.eventID = EventTriggerType.PointerExit;
        exitMain.callback.AddListener(MouseExitMainButton);


        //申明一个鼠标进入的事件类对象
        EventTrigger.Entry enterSub = new EventTrigger.Entry();
        enterSub.eventID = EventTriggerType.PointerEnter;
        enterSub.callback.AddListener(MouseEnterSubButton);

        //申明一个鼠标移出的事件类对象
        EventTrigger.Entry exitSub = new EventTrigger.Entry();
        exitSub.eventID = EventTriggerType.PointerExit;
        exitSub.callback.AddListener(MouseExitSubButton);


        triggerMain.triggers.Add(enterMain);
        triggerMain.triggers.Add(exitMain);

        triggerSub.triggers.Add(enterSub);
        triggerSub.triggers.Add(exitSub);
        /////////////////////////////////////////////////////////////////////////////////////////////////

        //开启事件监听，监听哪些主线关卡完成了
        EventCenter.GetInstance().AddEventListener("MainLevel1IsFinished", () =>
        {
            MainLevel1IsFinished = true;
            GameDataMgr.GetInstance().PlayerDataInfo.C2_MainLevel1IsFinished = true;
            imageLevelIconMain1.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/FinishedLevelIcon");
        });

        //开启事件监听，监听哪些支线关卡完成了
        EventCenter.GetInstance().AddEventListener("SubLevel1IsFinished", () =>
        {
            SubLevel1IsFinished = true;
            GameDataMgr.GetInstance().PlayerDataInfo.C2_SubLevel1IsFinished = true;
            imageLevelIconSub1.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/FinishedLevelIcon");
        });
    }

    private void Start()
    {
        //读取玩家的关卡完成进度，以此来设置关卡完成情况
        MainLevel1IsFinished = GameDataMgr.GetInstance().PlayerDataInfo.C2_MainLevel1IsFinished;
        SubLevel1IsFinished = GameDataMgr.GetInstance().PlayerDataInfo.C2_SubLevel1IsFinished;
    }

    private void Update()
    {
        //当主线任务完成，按钮会提示变色
        ChangeButtonsColor();
    }


    public override void HideMe()
    {
        base.HideMe();
        //删除时移除监听
        EventCenter.GetInstance().RemoveEventListener("MainLevel1IsFinished", () =>
        {
        });

        EventCenter.GetInstance().RemoveEventListener("SubLevel1IsFinished", () =>
        {
        });
    }

    /// <summary>
    /// 鼠标移入主线关卡选项
    /// </summary>
    /// <param name="data"></param>
    public void MouseEnterMainButton(BaseEventData data)
    {
        imageLevelIconMain1.gameObject.SetActive(true);
        imageLevelIconMain1.rectTransform.anchoredPosition = new Vector3(-107, -72, 0);
    }

    /// <summary>
    /// 鼠标移出主线关卡选项
    /// </summary>
    /// <param name="data"></param>
    public void MouseExitMainButton(BaseEventData data)
    {
        imageLevelIconMain1.gameObject.SetActive(false);
    }

    /// <summary>
    /// 鼠标移入支线关卡选项
    /// </summary>
    /// <param name="data"></param>
    public void MouseEnterSubButton(BaseEventData data)
    {
        imageLevelIconSub1.gameObject.SetActive(true);
        imageLevelIconSub1.rectTransform.anchoredPosition = new Vector3(-615, -220, 0);
    }

    /// <summary>
    /// 鼠标移出支线关卡选项
    /// </summary>
    /// <param name="data"></param>
    public void MouseExitSubButton(BaseEventData data)
    {
        imageLevelIconSub1.gameObject.SetActive(false);
    }

    private IEnumerator DelayedTriggerUpdateLevelContent_Main1()
    {
        yield return null; // 等待下一帧
        EventCenter.GetInstance().EventTrigger<string>("LevelContentInfoUpdate", "C2_MainLevel1");
    }

    private IEnumerator DelayedTriggerUpdateLevelContent_Sub1()
    {
        yield return null; // 等待下一帧
        EventCenter.GetInstance().EventTrigger<string>("LevelContentInfoUpdate", "C2_SubLevel1");
    }


    /// <summary>
    /// 显示主线关卡1的任务描述面板
    /// </summary>
    public void showMainLevel1Panel()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);

        UIManager.GetInstance().ShowPanel<UI_LevelConfirmPanel>("LevelConfirmPanel", E_UI_Layer.Mid);
        StartCoroutine(DelayedTriggerUpdateLevelContent_Main1());
    }

    /// <summary>
    /// 显示支线关卡1的任务描述面板
    /// </summary>
    public void showSubLevel1Panel()
    {

        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);

        UIManager.GetInstance().ShowPanel<UI_LevelConfirmPanel>("LevelConfirmPanel", E_UI_Layer.Mid);
        StartCoroutine(DelayedTriggerUpdateLevelContent_Sub1());
    }

    /// <summary>
    /// 进入下一章的函数
    /// </summary>
    public void ToNextChapter()
    {

        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);

        //如果已经完成主要战场，可以进入下一章节
        if (MainLevel1IsFinished)
        {
            //第三章内容未定
            
            /*
            SceneMgr.GetInstance().LoadSceneAsynByIndex(9, () =>
            {
                UIManager.GetInstance().HidePanel("Chapter1Options");
                UIManager.GetInstance().HidePanel("LevelSelectPanel");

                MusicMgr.GetInstance().PlaySound("maou_se_system48-start", false);
            });*/
        }
        else
        {
            //弹出提示窗口
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("主要战场未完成，无法进入下一章节");
            });
        }
    }
    
    /// <summary>
    /// 当完成所有主线章节后，改变按钮颜色提示
    /// </summary>
    public void ChangeButtonsColor()
    {
        if (MainLevel1IsFinished)
        {
            Color newColor = Color.red;
            ColorBlock colorBlock = buttonToNextLevel.colors;
            colorBlock.normalColor = newColor;
            buttonToNextLevel.colors = colorBlock;
            //正式版更改成下面的代码，改变目标按钮的图标
            //buttonToNextLevel.image.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/FinishedLevelIcon");
        }
    }
}
