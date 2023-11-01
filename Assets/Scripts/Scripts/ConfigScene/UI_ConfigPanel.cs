using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Image = UnityEngine.UI.Image;

public class UI_ConfigPanel : BasePanel
{
    //����ѭ����Ч�������ǰһ������δɾ������Ч
    public AudioSource loopSound;

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
        //���»�����Ϣ

        ChangeDate();

        //GetControl<TMP_Text>("ShowMoney").text = "��Ǯ��" + GameDataMgr.GetInstance().PlayerDataInfo.money.ToString() + "��";

    }

    public override void HideMe()
    {
        base.HideMe();
        //�ر����ʱ��������
        GameDataMgr.GetInstance().SavePlayerInfo();
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
            //��������������ҳ��
            case "ManageSupplies":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("ManageSupplies", AfterLoadFunctions);
                break;
            //�������ؿ�ѡ��ҳ��
            case "ChooseLevel":
                UIManager.GetInstance().HidePanel("MenuPanel");
                SceneMgr.GetInstance().LoadSceneAsyn("LevelSelectScene", AfterLoadFunctions);
                break;
            //����򿪲˵����
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


    //ִ���л���������Ҫִ�еĺ���
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
        if (date == "184����,���������")
        {
            GetControl<Image>("ImageShowDate").sprite = ResourceManager.GetInstance().Load<Sprite>("UIAssets/BY_B_Time_184");
        }
    }

    //��ջ����/�¼�����
    public void Clear()
    {
        PoolManager.GetInstance().Clear();
    }
}
