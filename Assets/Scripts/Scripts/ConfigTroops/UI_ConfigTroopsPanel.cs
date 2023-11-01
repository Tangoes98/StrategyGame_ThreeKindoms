using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ConfigTroopsPanel : BasePanel
{
    //判断鼠标点击次数
    public bool MenuIsOpen = false;

    protected override void Awake()
    {
        //父类的awake，初始化信息
        base.Awake();
        MenuIsOpen = false;
    }

    protected override void OnClick(string buttonName)
    {
        //在这里通过名字判断哪个按钮被点击了,然后直接在这里处理逻辑
        switch (buttonName)
        {
            //点击进入军备物资页面
            case "ManageSupplies":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("ManageSupplies", AfterLoadFunctions);
                break;
            //点击进入军营页面
            case "ReturnConfig":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("ConfigurationUnit", AfterLoadFunctions);
                break;
            /*//点击进入关卡选择页面
            case "ChooseLevel":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("LevelSelectScene", AfterLoadFunctions);
                break;*/
            //点击打开菜单面板
            case "OpenMenu":

                if (!MenuIsOpen)
                {
                    MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
                    UIManager.GetInstance().ShowPanel<UI_GameMenu>("MenuPanel", E_UI_Layer.System);
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
        PoolManager.GetInstance().Clear();

        UIManager.GetInstance().HidePanel("HintPanel");
        UIManager.GetInstance().HidePanel("SellConfirmPanel");
        UIManager.GetInstance().HidePanel("ActiveSkillPool");
        UIManager.GetInstance().HidePanel("ActiveSkillScrollView");
        UIManager.GetInstance().HidePanel("PassiveSkillsPool");
        UIManager.GetInstance().HidePanel("PassiveSkillsScrollView");
        UIManager.GetInstance().HidePanel("TroopLevelUpPanel2");
        UIManager.GetInstance().HidePanel("BagPanel2");
        UIManager.GetInstance().HidePanel("TroopSelectPanel");
        UIManager.GetInstance().HidePanel("ConfigTroopsPanel");
        UIManager.GetInstance().HidePanel("ConfigTroopsPanelMain");
        UIManager.GetInstance().HidePanel("AssignAttributePointPanel");
        
        MenuIsOpen = false;
    }

}
