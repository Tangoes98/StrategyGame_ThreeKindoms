using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LevelSelectPanel : BasePanel
{
    //�ж����������
    public bool MenuIsOpen = false;

    protected override void Awake()
    {
        //�����awake����ʼ����Ϣ
        base.Awake();
        MenuIsOpen = false;
    }

    protected override void OnClick(string buttonName)
    {
        //������ͨ�������ж��ĸ���ť�������,Ȼ��ֱ�������ﴦ���߼�
        switch (buttonName)
        {
            //��������������ҳ��
            case "ManageSupplies":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("ManageSupplies", AfterLoadFunctions);
                break;
            //��������Ӫҳ��
            case "ReturnConfig":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("ConfigurationUnit", AfterLoadFunctions);
                break;
            //��������佫����ҳ��
            case "ConfigTroops":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("ConfigTroops", AfterLoadFunctions);
                break;
            //����򿪲˵����
            case "OpenMenu":

                if (!MenuIsOpen)
                {
                    MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
                    UIManager.GetInstance().ShowPanel<UI_GameMenu>("MenuPanel", E_UI_Layer.System);
                    MenuIsOpen = true;
                }
                //������Ϊż�����ڶ��ε�����ر����
                if (MenuIsOpen)
                {
                    MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
                    UIManager.GetInstance().HidePanel("MenuPanel");
                    MenuIsOpen = false;
                }
                break;
        }
    }

    //ִ���л���������Ҫִ�еĺ���
    public void AfterLoadFunctions()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
        PoolManager.GetInstance().Clear();


        UIManager.GetInstance().HidePanel("HintPanel");
        UIManager.GetInstance().HidePanel("LevelConfirmPanel");
        UIManager.GetInstance().HidePanel("Chapter1Options");
        UIManager.GetInstance().HidePanel("Chapter2Options");
        UIManager.GetInstance().HidePanel("LevelSelectPanel");
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
