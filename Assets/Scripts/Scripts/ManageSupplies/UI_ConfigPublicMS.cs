using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ConfigPublicMS : BasePanel
{
    //�ж����������
    public bool MenuIsOpen = false;

    protected override void Awake()
    {
        //�����awake����ʼ����Ϣ
        base.Awake();
        MenuIsOpen = false;
    }

    public override void ShowMe()
    {
        base.ShowMe();
        //��һ��ִ�У���ʼ���������
        GetControl<TMP_Text>("ShowDate").text = GameDataMgr.GetInstance().PlayerDataInfo.date;
        GetControl<TMP_Text>("ShowMoney").text = "��Ǯ��" + GameDataMgr.GetInstance().PlayerDataInfo.money.ToString() + "��";
        
        //��ʼ�����¼���һ��Ǯ�������仯�˾͸�����������
        EventCenter.GetInstance().AddEventListener<int>("MoneyChange", UpdatePanel);
    }

    public override void HideMe()
    {
        base.HideMe();
        //�ر����ʱ��������
        GameDataMgr.GetInstance().SavePlayerInfo();
        //�Ƴ�����
        EventCenter.GetInstance().RemoveEventListener<int>("MoneyChange", UpdatePanel);
    }

    /// <summary>
    /// ˢ�����ں͵�ǰ��Ǯ�������ⲿ����
    /// </summary>
    public void UpdatePanel(int money)
    {
        GetControl<TMP_Text>("ShowMoney").text = "��Ǯ��" + GameDataMgr.GetInstance().PlayerDataInfo.money.ToString() + "��";
    }

    protected override void OnClick(string buttonName)
    {
        //������ͨ�������ж��ĸ���ť�������,Ȼ��ֱ�������ﴦ���߼�
        switch (buttonName)
        {
            //��������佫����ҳ��
            case "ConfigTroops":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("ConfigTroops", AfterLoadFunctions);
                break;
            //��������Ӫҳ��
            case "ReturnConfig":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("ConfigurationUnit", AfterLoadFunctions);
                break;
            //�������ؿ�ѡ��ҳ��
            case "ChooseLevel":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("LevelSelectScene", AfterLoadFunctions);
                break;
            //����򿪲˵����
            case "OpenMenu":

                if (!MenuIsOpen)
                {
                    MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
                    UIManager.GetInstance().ShowPanel<UI_GameMenu>("MenuPanel", E_UI_Layer.Top);
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

        UIManager.GetInstance().HidePanel("HintPanel");
        UIManager.GetInstance().HidePanel("ConfirmPanel");
        UIManager.GetInstance().HidePanel("SellConfirmPanel");
        UIManager.GetInstance().HidePanel("ManageSuppliesPanel");
        UIManager.GetInstance().HidePanel("BagPanel");
        UIManager.GetInstance().HidePanel("ShopPanel");
        MenuIsOpen = false;
    }
}

