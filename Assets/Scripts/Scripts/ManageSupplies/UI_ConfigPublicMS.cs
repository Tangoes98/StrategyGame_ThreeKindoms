using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ConfigPublicMS : BasePanel
{
    //判断鼠标点击次数
    public bool MenuIsOpen = false;

    protected override void Awake()
    {
        //父类的awake，初始化信息
        base.Awake();
        MenuIsOpen = false;
    }

    public override void ShowMe()
    {
        base.ShowMe();
        //第一次执行，初始化面板数据
        GetControl<TMP_Text>("ShowDate").text = GameDataMgr.GetInstance().PlayerDataInfo.date;
        GetControl<TMP_Text>("ShowMoney").text = "金钱：" + GameDataMgr.GetInstance().PlayerDataInfo.money.ToString() + "贯";
        
        //开始监听事件，一旦钱的数量变化了就更新面板的数据
        EventCenter.GetInstance().AddEventListener<int>("MoneyChange", UpdatePanel);
    }

    public override void HideMe()
    {
        base.HideMe();
        //关闭面板时保存数据
        GameDataMgr.GetInstance().SavePlayerInfo();
        //移除监听
        EventCenter.GetInstance().RemoveEventListener<int>("MoneyChange", UpdatePanel);
    }

    /// <summary>
    /// 刷新日期和当前金钱，方便外部调用
    /// </summary>
    public void UpdatePanel(int money)
    {
        GetControl<TMP_Text>("ShowMoney").text = "金钱：" + GameDataMgr.GetInstance().PlayerDataInfo.money.ToString() + "贯";
    }

    protected override void OnClick(string buttonName)
    {
        //在这里通过名字判断哪个按钮被点击了,然后直接在这里处理逻辑
        switch (buttonName)
        {
            //点击进入武将整备页面
            case "ConfigTroops":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("ConfigTroops", AfterLoadFunctions);
                break;
            //点击进入军营页面
            case "ReturnConfig":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("ConfigurationUnit", AfterLoadFunctions);
                break;
            //点击进入关卡选择页面
            case "ChooseLevel":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("LevelSelectScene", AfterLoadFunctions);
                break;
            //点击打开菜单面板
            case "OpenMenu":

                if (!MenuIsOpen)
                {
                    MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
                    UIManager.GetInstance().ShowPanel<UI_GameMenu>("MenuPanel", E_UI_Layer.Top);
                    MenuIsOpen = true;
                }
                //当次数为偶数，第二次点击，关闭面板
                if (MenuIsOpen)
                {
                    MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
                    UIManager.GetInstance().HidePanel("MenuPanel");
                    MenuIsOpen = false;
                }
                break;
        }
    }

    //执行切换场景后需要执行的函数
    public void AfterLoadFunctions()
    {

        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);

        UIManager.GetInstance().HidePanel("HintPanel");
        UIManager.GetInstance().HidePanel("ConfirmPanel");
        UIManager.GetInstance().HidePanel("SellConfirmPanel");
        UIManager.GetInstance().HidePanel("ManageSuppliesPanel");
        UIManager.GetInstance().HidePanel("BagPanel");
        UIManager.GetInstance().HidePanel("ShopPanel");
        MenuIsOpen = false;
    }
}

