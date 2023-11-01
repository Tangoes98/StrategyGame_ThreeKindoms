using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Image = UnityEngine.UI.Image;

public class UI_ConfigPanel : BasePanel
{
    //声明循环音效用来获得前一个场景未删除的音效
    public AudioSource loopSound;

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
        //更新基础信息

        ChangeDate();

        //GetControl<TMP_Text>("ShowMoney").text = "金钱：" + GameDataMgr.GetInstance().PlayerDataInfo.money.ToString() + "贯";

    }

    public override void HideMe()
    {
        base.HideMe();
        //关闭面板时保存数据
        GameDataMgr.GetInstance().SavePlayerInfo();
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
            //点击进入军备物资页面
            case "ManageSupplies":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("ManageSupplies", AfterLoadFunctions);
                break;
            //点击进入关卡选择页面
            case "ChooseLevel":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("LevelSelectScene", AfterLoadFunctions);
                break;
            //点击打开菜单面板
            case "OpenMenu":
                if(!MenuIsOpen)
                {
                    MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
                    UIManager.GetInstance().ShowPanel<UI_GameMenu>("MenuPanel", E_UI_Layer.Top);
                    MenuIsOpen = true;
                }
             
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
        UIManager.GetInstance().HidePanel("ConfigPanel");
        Clear();
        MenuIsOpen = false;
    }

    public void ChangeDate()
    {
        string date = GameDataMgr.GetInstance().PlayerDataInfo.date;
        if (date == "184年秋,汉光和七年")
        {
            GetControl<Image>("ImageShowDate").sprite = ResourceManager.GetInstance().Load<Sprite>("UIAssets/BY_B_Time_184");
        }
    }

    //清空缓存池/事件中心
    public void Clear()
    {
        PoolManager.GetInstance().Clear();
    }
}
