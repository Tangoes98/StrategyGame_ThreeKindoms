using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class IntroManager_chapter2 : MonoBehaviour
{
    //用于检测鼠标是否位于按钮所在区域
    private bool hitButton;
    //按钮区域
    public Rect ButtonArea;

    //检测MenuPanel是否开启
    private bool MenuPanelIsOpen;

    //背景
    public SpriteRenderer backGround;
    //角色对话文本
    public TMP_Text contentText;

    //保存当前的对话ID索引值
    public int dialogIndex = 0;

    //为了方便调用，利用字典装载数据集合中的数据
    private Dictionary<int, IntroScript> dialogdic = new Dictionary<int, IntroScript>();

    //判断是否可以切换环境/循环音效的条件
    public bool canSend;
    //当前对话ID索引值，用于判断是否可以切换环境
    public int currentdialogIndex;

    public float currentBGMValue = 0.3f;

    private void Awake()
    {

        //监听菜单是否打开，一旦打开就将其设置为true
        MenuPanelIsOpen = false;
        EventCenter.GetInstance().AddEventListener("MenuIsOpen", () =>
        {
            MenuPanelIsOpen = true;
        });
        EventCenter.GetInstance().AddEventListener("MenuIsClose", () =>
        {
            MenuPanelIsOpen = false;
        });


        UIManager.GetInstance().ShowPanel<UI_DiaPanel>("DiaPanel", E_UI_Layer.Bot);
    }
    // Start is called before the first frame update
    void Start()
    {

        //开始时播放背景音乐
        MusicMgr.GetInstance().PlayBkMusic("Ancestral Spirits");
        MusicMgr.GetInstance().ChangeBKValue(0.3f);
        //开启输入检测
        InputMgr.GetInstance().StartOrEndCheck(true);
        //该对象死亡时移除监听即可停止相关功能
        EventCenter.GetInstance().AddEventListener<KeyCode>("KeyButtonDown", CheckInputDown);
        
        //开始时自动解析Json文件
        ParseData();

        //初始化设定
        canSend = true;
        currentdialogIndex = dialogIndex;

    }
    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("KeyButtonDown", CheckInputDown);
        EventCenter.GetInstance().RemoveEventListener("MenuIsOpen", () => { });
        EventCenter.GetInstance().RemoveEventListener("MenuIsClose", () => { });
        InputMgr.GetInstance().StartOrEndCheck(false);
    }

    // Update is called once per frame
    void Update()
    {
        checkButtonHit();
        //检查，当进行到特定位置时，切换场景和BGM
        /*CheckEnviroment("Home", 0);
        CheckEnviroment("Camp", 6);
        CheckEnviroment("Home", 12);*/
        //持续显示和更新对话
        ShowDialogue();
    }
    /// <summary>
    /// 解析Json文件
    /// </summary>
    public void ParseData()
    {
        //读取并解析Json文件
        IntroScripts dialogues = new IntroScripts();
        dialogues = JsonMgr.Instance.LoadData<IntroScripts>("IntroScript/introScript2");
        //将数据集合按照ID号分别放入
        for (int i = 0; i < dialogues.introScripts.Count; i++)
        {
            dialogdic.Add(dialogues.introScripts[i].DialogueID, dialogues.introScripts[i]);
        }

    }

    /// <summary>
    /// 根据对话ID，得到详细信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IntroScript GetInFo(int id)
    {
        if (dialogdic.ContainsKey(id))
            return dialogdic[id];
        return null;
    }

    /// <summary>
    /// 更新文本
    /// </summary>
    /// <param name="ID">Json文件中的ID索引</param>
    /// <param name="content">对话内容</param>
    public void UpdateText(int ID, string content)
    {
        contentText.text = content;
    }
    /// <summary>
    /// 更新立绘
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="name"></param>
    /// <param name="position"></param>


    /// <summary>
    /// 显示文本和立绘
    /// </summary>
    public void ShowDialogue()
    {
        //当存在索引编号时，更新文本和立绘
        if (dialogdic.ContainsKey(dialogIndex))
        {
            UpdateText(GetInFo(dialogIndex).DialogueID, GetInFo(dialogIndex).Content);
        }

    }

    /// <summary>
    /// 改变背景图片
    /// </summary>
    /// <param name="name"></param>
    public void ChangeBackGround(string name)
    {
        backGround.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + name);
    }

    /// <summary>
    /// 输入检测
    /// </summary>
    /// <param name="key"></param>
    private void CheckInputDown(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Mouse0:
                //鼠标点击音效
                MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
                PressMouse();
                break;
        }
    }


    /// <summary>
    /// 清除缓存，目前未调用
    /// </summary>
    public void Clear()
    {
        PoolManager.GetInstance().Clear();
        EventCenter.GetInstance().Clear();
    }

    /// <summary>
    /// 点击鼠标左键后执行的逻辑
    /// </summary>
    private void PressMouse()
    {
        if (!hitButton && !MenuPanelIsOpen)
        {
            //点击鼠标会使对话进行下去
            dialogIndex = GetInFo(dialogIndex).JumpToID;
            currentdialogIndex = dialogIndex;

            if (dialogIndex == dialogdic.Count)
            {
                SceneMgr.GetInstance().LoadSceneAsyn("Story2", () =>
                {
                    //UIManager.GetInstance().HidePanel("DiaPanel");
                                    //Clear();

                });
            }
        }
            // 在这里添加判断，如果满足条件，逐步减小音量值
            if (dialogIndex >= dialogdic.Count - 6)
        {
           
            // 逐步减小音量值，不低于0.1
            float targetValue = Mathf.Max(0.1f, currentBGMValue - 0.05f);
            MusicMgr.GetInstance().ChangeBKValue(targetValue);
            currentBGMValue = targetValue;
        }

    }



    /// <summary>
    /// 检测什么时候切换环境
    /// </summary>
    /// <param name="Environment">//环境名称</param>
    /// <param name="changeID">当处于该ID时切换</param>
    public void CheckEnviroment(string Environment, int changeID)
    {
        //判断是否能够切换环境的条件
        if (canSend && currentdialogIndex == changeID)
        {
            //ChangeEnvironment(Environment);
            canSend = false;
            currentdialogIndex++;
            if (currentdialogIndex > dialogIndex)
            {
                canSend = true;
            }
        }
    }
    /// <summary>
    /// 检测鼠标是否位于按钮所在区域
    /// </summary>
    private void checkButtonHit()
    {
        Vector3 MousePosition = Input.mousePosition;
        if (ButtonArea.Contains(MousePosition))
        {
            hitButton = true;
        }
        else
        {
            hitButton = false;
        }
    }
}
