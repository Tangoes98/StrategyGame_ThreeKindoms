using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_ConfigTroopsPanelMain : BasePanel
{
    //设置Toggle的父级，方便添加ToggleGroup
    public Transform SwitchGeneralButtons;
    //使用字典存储所有生成的toggle
    private Dictionary<string, Toggle> toggleDict = new Dictionary<string, Toggle>();

    //Toggle之间的间距
    public float ToggleSpacing;
    //使用list存储Toggle方便清除
    private List<GameObject> generatedToggles = new List<GameObject>();
    //Toggle相关变量
    private int currentStartIndex = 0;
    private int maxTogglesPerPage = 6;
    private int dictionaryCount;

    //使用dictionary存储玩家拥有的武将信息
    private Dictionary<string, General> PlayerOwnedGenerals;
    //申明一个General类存储当前选中的将军的数据
    public General currentSelectedGeneral;
    //申明一个int存储当前剩余的未分配属性点
    private int UnassignedAttributePoint;

    //使用bool表示当前分配属性面板的开关状态
    private bool AssignAttributePointPanelIsOpen = false;
    //表示是否确认更改属性点
    private bool ChangeConfirmed;

    //申明一个兵种栏位用来放置兵种
    public UI_TroopCell SelectTroop;
    //当前选择的兵种
    public Troop currentSelectTroop;

    //显示兵种树的条件
    private bool showTroopTree;
    private bool showBagPanel2;

    public override void ShowMe()
    {
        base.ShowMe();
        //打开面板时就根据玩家拥有的武将生成toggle
        GenerateTogglesFromDictionary(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral);

        //开启TroopMgr中的事件监听，监听拖动相关的事件
        UI_TroopMgr.GetInstance().InitListener();

        //添加事件监听，监听属性分配面板传来的事件
        EventCenter.GetInstance().AddEventListener("ChangeConfirmed", () =>
        {
            //如果确认属性分配，设置bool为true
            ChangeConfirmed = true;
        });
    }

    private void Start()
    {
        //设置确认更改属性点bool为false
        ChangeConfirmed = false;
   
        //点击左右Button切换Toggle显示，显示更多将军
        GetControl<Button>("ButtonShowNextToggle").onClick.AddListener(ShowNextPage);
        GetControl<Button>("ButtonShowPreviousToggle").onClick.AddListener(ShowPreviousPage);

        showTroopTree = false;
        GetControl<Button>("ButtonShowLevelUpPanel").onClick.AddListener(ShowTroopTree);
        GetControl<Button>("ButtonShowBag").onClick.AddListener(ShowBagPanel2);
    }

    private void Update()
    {
        //游戏进行中在Update中检测哪个Toggle被选中了
        GetSelectedToggle();
       
    }

    public override void HideMe()
    {
        base.HideMe();
        //关闭TroopMgr中的监听
        UI_TroopMgr.GetInstance().RemoveListener();

        //面板销毁时移除事件监听
        EventCenter.GetInstance().RemoveEventListener("ChangeConfirmed", () =>
        {

        });
    }

    /// <summary>
    /// 检测哪个Toggle被选中，执行选中这个Toggle需要执行的逻辑
    /// </summary>
    /// <returns></returns>
    public Toggle GetSelectedToggle()
    {
        foreach (var pair in toggleDict)
        {
            Toggle toggle = pair.Value;
            if (toggle.isOn)
            {
                currentSelectedGeneral = PlayerOwnedGenerals[toggle.name];

                GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral = currentSelectedGeneral;
/*
                Debug.Log(currentSelectedGeneral.GeneralName);
                Debug.Log(GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral.GeneralName);
                Debug.Log(GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral.currentSelectTroop.TroopName);
*/
                //设置默认选择的兵种为武将的默认兵种
                currentSelectTroop = PlayerOwnedGenerals[toggle.name].currentSelectTroop;

                Debug.Log(currentSelectedGeneral.GeneralName);

                //Debug.Log(toggle.name);
                ChangeImage("ImageGeneral", ResourceManager.GetInstance().Load<Sprite>("Sprites/" + toggle.name));
                //武将基本属性
                ChangeText("TextGeneralLevel", "等级：" + PlayerOwnedGenerals[toggle.name].Level.ToString());
                ChangeText("TextGeneralName", PlayerOwnedGenerals[toggle.name].GeneralName);
                //经验值相关逻辑
                ChangeText("TextEXP", "经验值：" + PlayerOwnedGenerals[toggle.name].CurrentEXP + "/" + PlayerOwnedGenerals[toggle.name].MaxEXP);
                //这里的使用情景应该是使用经验道具，临时升级的情况，如果当前经验值大于升级所需要的经验值,就升级
                if (PlayerOwnedGenerals[toggle.name].CurrentEXP >= PlayerOwnedGenerals[toggle.name].MaxEXP)
                {
                    //触发升级的事件监听,更新玩家数据中的武将数据，输入参数依次为武将名，剩余经验
                    EventCenter.GetInstance().EventTrigger("GeneralLevelUp", toggle.name, PlayerOwnedGenerals[toggle.name].CurrentEXP - PlayerOwnedGenerals[toggle.name].MaxEXP);
                    //得到升级时获得的可分配属性点，然后显示未分配属性点的提示
                    UnassignedAttributePoint = PlayerOwnedGenerals[toggle.name].UnassignedAttributePoints;
                    UpdateToggles(generatedToggles);

                    //升级后更新经验条
                    EventCenter.GetInstance().EventTrigger("GeneralExpChange", currentSelectedGeneral);

                    //如果升级，就显示分配属性面板
                    //在分配属性面板中处理属性分配，以及隐藏提示等
                    if (!AssignAttributePointPanelIsOpen)
                    {
                        UIManager.GetInstance().ShowPanel<UI_AssignAttributePointPanel>("AssignAttributePointPanel", E_UI_Layer.Top, (panel) =>
                        {
                            panel.generalKey = toggle.name;
                        });
                    }
                    //如果分配属性面板已经打开，且还没有完成分配，且此时当前经验值大于升级所需要的经验值
                    else if (AssignAttributePointPanelIsOpen)
                    {
                        //什么也不做
                    }
               ////////////////////////////////////////////////////////////////////////////////////

                         //分配属性面板的bool设置为true
                         AssignAttributePointPanelIsOpen = true;

                }

                //当分配属性面板是显示状态，且已经确认完成属性分配
                if (AssignAttributePointPanelIsOpen && ChangeConfirmed && PlayerOwnedGenerals[toggle.name].UnassignedAttributePoints == 0)
                {

                    //分配属性面板的bool设置为false
                    AssignAttributePointPanelIsOpen = false;
                    //更新Toggle上的提示，隐藏提示的显示
                    UpdateToggles(generatedToggles);
                    //设置确认更改属性点bool为false
                    ChangeConfirmed = false;
                }
              



                //当玩家未分配点数大于0，且此时分配属性面板没有打开
                if (PlayerOwnedGenerals[toggle.name].UnassignedAttributePoints > 0 && !AssignAttributePointPanelIsOpen)
                {
                    //打开分配属性面板
                    UIManager.GetInstance().ShowPanel<UI_AssignAttributePointPanel>("AssignAttributePointPanel", E_UI_Layer.Top, (panel) =>
                    {
                        //将panel的Key传入分配属性面板
                        panel.generalKey = toggle.name;
                    });
                    //分配属性面板的bool设置为true
                    AssignAttributePointPanelIsOpen = true;
                }





                ChangeText("TextHp", "兵力值(HP)：" + PlayerOwnedGenerals[toggle.name].CurrentHp + "/" + PlayerOwnedGenerals[toggle.name].Hp);

                ChangeText("TextStrength", "武勇：" + PlayerOwnedGenerals[toggle.name].Strength);

                ChangeText("TextLeaderShip", "统率：" + PlayerOwnedGenerals[toggle.name].LeaderShip);

                ChangeText("TextWisdom", "智谋：" + PlayerOwnedGenerals[toggle.name].Wisdom);

                ChangeText("TextAttack", "攻击力：" + PlayerOwnedGenerals[toggle.name].Attack);

                ChangeText("TextDefense", "防御力：" + PlayerOwnedGenerals[toggle.name].Defense);

                ChangeText("TextCriticalChance", "暴击率：" + (PlayerOwnedGenerals[toggle.name].CriticalRate * 100f).ToString("0.00") + "%");

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                //更新当前装备的兵种栏位上的兵种
                UpdateSelectTroop();

                //兵种属性
                ChangeText("TextSelectTroop", "兵种：" + currentSelectTroop.TroopName);

                ChangeText("TextMorale", "士气值：" + currentSelectTroop.Morale);
                ChangeText("TextMobility", "机动力：" + currentSelectTroop.Movement);
                ChangeText("TextTroopInfoDetail",  currentSelectTroop.TroopFeature);
                //差一个图片，对应每个兵种的详情，正式版做成图片添加进来
                //ChangeImage("ImageTroopInfo", ResourceManager.GetInstance().Load<Sprite>("Sprites/" + currentSelectTroop.TroopID));

                ChangeText("TextTroopSkill", currentSelectTroop.TroopSkill);
                GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[toggle.name].TroopSkill = currentSelectTroop.TroopSkill;
                ChangeImage("ImageTroopSkillIcon", ResourceManager.GetInstance().Load<Sprite>("Sprites/" + currentSelectTroop.TroopSkill + "Icon"));
                //OpenTroopLevelUpPanelFunc();
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //主动技能
                //被动技能




                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //增加经验值时随时记录经验变化
                EventCenter.GetInstance().EventTrigger("GeneralExpChange", currentSelectedGeneral);
                //需要一个功能，在弹出属性分配面板后AssignAttributePointPanelIsOpen为true，且ChangeConfirmed为false的情况下，既升级完成前，不得使用任何增加经验值的功能，使用就撤销并且弹出提示，“您需要完成升级后的属性分配才能继续使用经验值道具”.
                //需要嗑药时增加经验值的功能

                return toggle;
            }
        }
        return null;
    }

    /// <summary>
    /// 打开兵种树面板的函数
    /// </summary>
    public void OpenTroopLevelUpPanelFunc()
    {
        //打开当前所选兵种的兵种树
        if (showTroopTree)
        {

            GameObject button = GameObject.Find("ButtonShowLevelUpPanel");
            if (button != null)
            {
                Button buttonComponent = button.GetComponent<Button>();
                TMP_Text buttonText = buttonComponent.GetComponentInChildren<TMP_Text>();
                buttonText.text = "关闭兵种树";
            }

            UIManager.GetInstance().ShowPanel<UI_TroopLevelUpPanel2>("TroopLevelUpPanel2", E_UI_Layer.Top);
            StartCoroutine(DelayedAccessSetTroopAndGeneral());
       
        }
        else
        {

            GameObject button = GameObject.Find("ButtonShowLevelUpPanel");
            if (button != null)
            {
                Button buttonComponent = button.GetComponent<Button>();
                TMP_Text buttonText = buttonComponent.GetComponentInChildren<TMP_Text>();
                buttonText.text = "打开兵种树";
            }

            UIManager.GetInstance().HidePanel("TroopLevelUpPanel2");
        }
    }


    /// <summary>
    /// 打开背包面板的函数
    /// </summary>
    public void OpenBagPanel2Func()
    {
        //打开当前所选兵种的兵种树
        if (showBagPanel2)
        {

            GameObject button = GameObject.Find("ButtonShowBag");
            if (button != null)
            {
                Button buttonComponent = button.GetComponent<Button>();
                TMP_Text buttonText = buttonComponent.GetComponentInChildren<TMP_Text>();
                buttonText.text = "关闭面板";
            }

            UIManager.GetInstance().ShowPanel<UI_BagPanel2>("BagPanel2", E_UI_Layer.Top);
            StartCoroutine(DelayedAccessToCurrentSelectedGeneral());

        }
        else
        {

            GameObject button = GameObject.Find("ButtonShowBag");
            if (button != null)
            {
                Button buttonComponent = button.GetComponent<Button>();
                TMP_Text buttonText = buttonComponent.GetComponentInChildren<TMP_Text>();
                buttonText.text = "使用道具";
            }

            UIManager.GetInstance().HidePanel("BagPanel2");
        }
    }


    /// <summary>
    /// 点击按钮改变打开兵种树面板的条件,打开所选武将的兵种树面板,传入当前所选的兵种和将军的数据
    /// </summary>
    public void ShowTroopTree()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
        showTroopTree = !showTroopTree;
        OpenTroopLevelUpPanelFunc();
    }

    public void ShowBagPanel2()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
        showBagPanel2 = !showBagPanel2;
        OpenBagPanel2Func();
    }


    /// <summary>
    /// 更新当前选择的兵种
    /// </summary>
    public void UpdateSelectTroop()
    {
        SelectTroop.InitInfo(currentSelectTroop);
    }

    /// <summary>
    /// 当Toggle切换就发送事件
    /// </summary>
    /// <param name="value"></param>
    public void OnToggleValueChanged(bool value)
    {
        if (value)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_switch01", false);
            StartCoroutine(DelayedAccessToCurrentSelectedGeneral());
            StartCoroutine(DelayedAccessSetTroopAndGeneral());
            currentSelectedGeneral = GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral;
        }
    }

    /// <summary>
    /// 协程目的是等currentSelectedGeneral切换好再发送
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayedAccessToCurrentSelectedGeneral()
    {
        yield return null; // 等待下一帧
        EventCenter.GetInstance().EventTrigger("ToggleChanged", currentSelectedGeneral);
    }

    /// <summary>
    /// 协程目的是等currentSelectedGeneral切换好再发送将军和兵种相关信息
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayedAccessSetTroopAndGeneral()
    {
        yield return null; // 等待下一帧
        EventCenter.GetInstance().EventTrigger("UpdateTroopTree", currentSelectedGeneral, currentSelectTroop);
    }

    /// <summary>
    ///根据组件名称获得组件，更改上面的图片 
    /// </summary>
    /// <param name="ComponentName">组件名</param>
    /// <param name="NewSprite">想要加载的图片</param>
    public void ChangeImage(string ComponentName, Sprite NewSprite)
    {
        Transform target = transform.Find(ComponentName);
        if (target != null)
        {
            //尝试获取子对象上的Image组件
            Image imageComponent = target.GetComponent<Image>();
            if (imageComponent != null)
            {
                // 修改图片
                imageComponent.sprite = NewSprite;
            }
        }
    }

    /// <summary>
    ///根据组件名称获得组件，更改上面的文本 
    /// </summary>
    /// <param name="ComponentName">组件名</param>
    /// <param name="NewSprite">想要改成的文本</param>
    public void ChangeText(string ComponentName, string NewText)
    {
        Transform target = transform.Find(ComponentName);
        if (target != null)
        {
            // 尝试获取子对象上的Text组件
            TMP_Text textComponent = target.GetComponent<TMP_Text>();
            if (textComponent != null)
            {
                // 修改文本
                textComponent.text = NewText;
            }
        }
    }

    /// <summary>
    ///根据玩家的将军数量生成Toggle 
    /// </summary>
    /// <param name="dictionary"></param>
    public void GenerateTogglesFromDictionary(Dictionary<string, General> dictionary)
    {
        //清除之前剩余的Toggle
        ClearGeneratedToggles();
        //设定两个Toggle之间的距离
        ToggleSpacing = 180f;
        dictionaryCount = dictionary.Count;
        //将传入的代表玩家武将数据的dictionary储存起来
        PlayerOwnedGenerals = dictionary;

        //设定字典编号范围
        int endIndex = currentStartIndex + Mathf.Min(dictionaryCount, maxTogglesPerPage) - 1;
        if (endIndex >= dictionaryCount)
        {
            endIndex -= dictionaryCount;
        }
        //设定Toggle位置
        float offset = -((float)(maxTogglesPerPage - 1) * ToggleSpacing) / 2f;

        for (int i = 0; i < Mathf.Min(dictionaryCount, maxTogglesPerPage); i++)
        {
            int indexInDictionary = (currentStartIndex + i) % dictionaryCount;

            KeyValuePair<string, General> pair = dictionary.ElementAt(indexInDictionary);

            //获得Toggle的GameObject和上面的组件Toggle
            GameObject toggleObject = Instantiate(ResourceManager.GetInstance().Load<GameObject>("UI/ToggleGeneralPage"), SwitchGeneralButtons);
            Toggle toggle = toggleObject.GetComponent<Toggle>();

            //获得父级的Group并将Toggle绑定上去
            ToggleGroup toggleGroup = toggleObject.GetComponentInParent<ToggleGroup>();
            toggle.group = toggleGroup;


            //将pair.key存储下来，方便根据Key从字典中调用信息
            toggleObject.name = pair.Key;
            UnassignedAttributePoint = PlayerOwnedGenerals[toggleObject.name].UnassignedAttributePoints;

            //根据Key的名字改变Toggle子物体图片上的头像
            //sprite中的头像需要以Key的名字加Head来命名，如LiuBeiHead
            Transform imageHeadTransform = toggleObject.transform.Find("ImageHead");
            if (imageHeadTransform != null)
            {
                Image imageHead = imageHeadTransform.GetComponent<Image>();
                if (imageHead != null)
                {
                    imageHead.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + pair.Key + "Head");
                }
            }
            //根据Key的名字改变Toggle上的头像
            Image toggleImage = toggle.GetComponent<Image>();
            toggleImage.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + pair.Key + "Head");
            //根据Key对应的值的武将名字改变Toggle文本的名字
            TMP_Text TextToggleGeneralName = toggle.GetComponentInChildren<TMP_Text>();
            TextToggleGeneralName.text = pair.Value.GeneralName;
            //将toggle设置在父级上
            toggleObject.transform.SetParent(SwitchGeneralButtons);

            //设置Toggle的位置
            toggleObject.transform.localPosition = new Vector3(offset + i * ToggleSpacing, 450f, 0f);
            //将Toggle加入Toggle字典，以名字为键key方便查找调用
            toggleDict.Add(toggleObject.name, toggle);
            //将Toggle加入待清除list
            generatedToggles.Add(toggleObject);

            //进游戏时显示所拥有的武将是否有未分配的属性点
            UpdateToggles(generatedToggles);
        }

        //重新注册事件监听
        RegisterToggleListeners();
    }

    private void RegisterToggleListeners()
    {
        foreach (var toggle in toggleDict.Values)
        {
            toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    /// <summary>
    /// 当玩家有未分配属性点时显示提示的函数
    /// </summary>
    /// <param name="toggleList"></param>
    public void UpdateToggles(List<GameObject> toggleList)
    {
        foreach (GameObject generatedToggle in toggleList)
        {
            UnassignedAttributePoint = PlayerOwnedGenerals[generatedToggle.name].UnassignedAttributePoints;
            //需要一个方法更新这个数据
            Transform childTransform = generatedToggle.transform.Find("ImagePointHint");
            if (childTransform != null)
            {
                Image imageComponent = childTransform.GetComponent<Image>();
                if (imageComponent != null)
                {
                    imageComponent.enabled = false;
                    if (UnassignedAttributePoint > 0)
                    {
                        imageComponent.enabled = true;
                    }
                    else
                    {
                        imageComponent.enabled = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 清除之前的Toggle的函数
    /// </summary>
    public void ClearGeneratedToggles()
    {
        foreach (GameObject toggleObject in generatedToggles)
        {
            Destroy(toggleObject);
        }
        generatedToggles.Clear();
        toggleDict.Clear();
    }
    /// <summary>
    /// 点击下一页显示下一个的Toggle
    /// </summary>
    public void ShowNextPage()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound_switch01", false);
        currentStartIndex += 1;

        if (currentStartIndex >= dictionaryCount)
        {
            currentStartIndex = 0;
        }

        GenerateTogglesFromDictionary(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral);

        UpdateToggles(generatedToggles);
    }
    /// <summary>
    /// 点击上一页显示上一个的Toggle
    /// </summary>
    public void ShowPreviousPage()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound_switch01", false);
        currentStartIndex -= 1;

        if (currentStartIndex < 0)
        {
            currentStartIndex = dictionaryCount - 1;
        }

        GenerateTogglesFromDictionary(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral);

        UpdateToggles(generatedToggles);
    }
}
