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

        //�����¹ؿ���ťʱ����ʾ��Ӧ��Ϣ
        GetControl<Button>("ButtonMainLevel").onClick.AddListener(showMainLevel1Panel);
        GetControl<Button>("ButtonSubLevel").onClick.AddListener(showSubLevel1Panel);
        GetControl<Button>("ButtonToNextChapter").onClick.AddListener(ToNextChapter);

        //Icon������������
        ///////////////////////////////////////////////////////////////////////////////////////////////
        //����������������Ƴ����¼������д���
        EventTrigger triggerMain = GetControl<Button>("ButtonMainLevel").gameObject.AddComponent<EventTrigger>();
        EventTrigger triggerSub = GetControl<Button>("ButtonSubLevel").gameObject.AddComponent<EventTrigger>();

        //����һ����������¼������
        EventTrigger.Entry enterMain = new EventTrigger.Entry();
        enterMain.eventID = EventTriggerType.PointerEnter;
        enterMain.callback.AddListener(MouseEnterMainButton);

        //����һ������Ƴ����¼������
        EventTrigger.Entry exitMain = new EventTrigger.Entry();
        exitMain.eventID = EventTriggerType.PointerExit;
        exitMain.callback.AddListener(MouseExitMainButton);


        //����һ����������¼������
        EventTrigger.Entry enterSub = new EventTrigger.Entry();
        enterSub.eventID = EventTriggerType.PointerEnter;
        enterSub.callback.AddListener(MouseEnterSubButton);

        //����һ������Ƴ����¼������
        EventTrigger.Entry exitSub = new EventTrigger.Entry();
        exitSub.eventID = EventTriggerType.PointerExit;
        exitSub.callback.AddListener(MouseExitSubButton);


        triggerMain.triggers.Add(enterMain);
        triggerMain.triggers.Add(exitMain);

        triggerSub.triggers.Add(enterSub);
        triggerSub.triggers.Add(exitSub);
        /////////////////////////////////////////////////////////////////////////////////////////////////

        //�����¼�������������Щ���߹ؿ������
        EventCenter.GetInstance().AddEventListener("MainLevel1IsFinished", () =>
        {
            MainLevel1IsFinished = true;
            GameDataMgr.GetInstance().PlayerDataInfo.C2_MainLevel1IsFinished = true;
            imageLevelIconMain1.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/FinishedLevelIcon");
        });

        //�����¼�������������Щ֧�߹ؿ������
        EventCenter.GetInstance().AddEventListener("SubLevel1IsFinished", () =>
        {
            SubLevel1IsFinished = true;
            GameDataMgr.GetInstance().PlayerDataInfo.C2_SubLevel1IsFinished = true;
            imageLevelIconSub1.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/FinishedLevelIcon");
        });
    }

    private void Start()
    {
        //��ȡ��ҵĹؿ���ɽ��ȣ��Դ������ùؿ�������
        MainLevel1IsFinished = GameDataMgr.GetInstance().PlayerDataInfo.C2_MainLevel1IsFinished;
        SubLevel1IsFinished = GameDataMgr.GetInstance().PlayerDataInfo.C2_SubLevel1IsFinished;
    }

    private void Update()
    {
        //������������ɣ���ť����ʾ��ɫ
        ChangeButtonsColor();
    }


    public override void HideMe()
    {
        base.HideMe();
        //ɾ��ʱ�Ƴ�����
        EventCenter.GetInstance().RemoveEventListener("MainLevel1IsFinished", () =>
        {
        });

        EventCenter.GetInstance().RemoveEventListener("SubLevel1IsFinished", () =>
        {
        });
    }

    /// <summary>
    /// ����������߹ؿ�ѡ��
    /// </summary>
    /// <param name="data"></param>
    public void MouseEnterMainButton(BaseEventData data)
    {
        imageLevelIconMain1.gameObject.SetActive(true);
        imageLevelIconMain1.rectTransform.anchoredPosition = new Vector3(-107, -72, 0);
    }

    /// <summary>
    /// ����Ƴ����߹ؿ�ѡ��
    /// </summary>
    /// <param name="data"></param>
    public void MouseExitMainButton(BaseEventData data)
    {
        imageLevelIconMain1.gameObject.SetActive(false);
    }

    /// <summary>
    /// �������֧�߹ؿ�ѡ��
    /// </summary>
    /// <param name="data"></param>
    public void MouseEnterSubButton(BaseEventData data)
    {
        imageLevelIconSub1.gameObject.SetActive(true);
        imageLevelIconSub1.rectTransform.anchoredPosition = new Vector3(-615, -220, 0);
    }

    /// <summary>
    /// ����Ƴ�֧�߹ؿ�ѡ��
    /// </summary>
    /// <param name="data"></param>
    public void MouseExitSubButton(BaseEventData data)
    {
        imageLevelIconSub1.gameObject.SetActive(false);
    }

    private IEnumerator DelayedTriggerUpdateLevelContent_Main1()
    {
        yield return null; // �ȴ���һ֡
        EventCenter.GetInstance().EventTrigger<string>("LevelContentInfoUpdate", "C2_MainLevel1");
    }

    private IEnumerator DelayedTriggerUpdateLevelContent_Sub1()
    {
        yield return null; // �ȴ���һ֡
        EventCenter.GetInstance().EventTrigger<string>("LevelContentInfoUpdate", "C2_SubLevel1");
    }


    /// <summary>
    /// ��ʾ���߹ؿ�1�������������
    /// </summary>
    public void showMainLevel1Panel()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);

        UIManager.GetInstance().ShowPanel<UI_LevelConfirmPanel>("LevelConfirmPanel", E_UI_Layer.Mid);
        StartCoroutine(DelayedTriggerUpdateLevelContent_Main1());
    }

    /// <summary>
    /// ��ʾ֧�߹ؿ�1�������������
    /// </summary>
    public void showSubLevel1Panel()
    {

        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);

        UIManager.GetInstance().ShowPanel<UI_LevelConfirmPanel>("LevelConfirmPanel", E_UI_Layer.Mid);
        StartCoroutine(DelayedTriggerUpdateLevelContent_Sub1());
    }

    /// <summary>
    /// ������һ�µĺ���
    /// </summary>
    public void ToNextChapter()
    {

        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);

        //����Ѿ������Ҫս�������Խ�����һ�½�
        if (MainLevel1IsFinished)
        {
            //����������δ��
            
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
            //������ʾ����
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("��Ҫս��δ��ɣ��޷�������һ�½�");
            });
        }
    }
    
    /// <summary>
    /// ��������������½ں󣬸ı䰴ť��ɫ��ʾ
    /// </summary>
    public void ChangeButtonsColor()
    {
        if (MainLevel1IsFinished)
        {
            Color newColor = Color.red;
            ColorBlock colorBlock = buttonToNextLevel.colors;
            colorBlock.normalColor = newColor;
            buttonToNextLevel.colors = colorBlock;
            //��ʽ����ĳ�����Ĵ��룬�ı�Ŀ�갴ť��ͼ��
            //buttonToNextLevel.image.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/FinishedLevelIcon");
        }
    }
}
