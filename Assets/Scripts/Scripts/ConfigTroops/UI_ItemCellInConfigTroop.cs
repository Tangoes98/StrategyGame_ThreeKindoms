using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemCellInConfigTroop : BasePanel
{
    private PlayerItemInfo _itemInfo;
    private General CurrentGeneral;
    private bool okToUse;
    private Item itemData;

    public PlayerItemInfo itemInfo
    {
        get { return _itemInfo; }
    }

    protected override void Awake()
    {
        base.Awake();

        //�����³�����Ʒ��ťִ�г�����Ʒ����
        GetControl<Button>("ButtonUse").onClick.AddListener(UseItem);

        //������ǰʹ�øõ��ߵĽ���
        EventCenter.GetInstance().AddEventListener<General>("ToggleChanged", GetCurrentGeneral);

        //Tips������������
        ///////////////////////////////////////////////////////////////////////////////////////////////
        //����������������Ƴ����¼������д���
        EventTrigger trigger = GetControl<Image>("ImageIcon").gameObject.AddComponent<EventTrigger>();

        //����һ����������¼������
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener(MouseEnterItemCell);

        //����һ������Ƴ����¼������
        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener(MouseExitItemCell);

        trigger.triggers.Add(enter);
        trigger.triggers.Add(exit);
        /////////////////////////////////////////////////////////////////////////////////////////////////
    }

    public override void HideMe()
    {
        base.HideMe();
        EventCenter.GetInstance().RemoveEventListener<General>("ToggleChanged", GetCurrentGeneral);
    }

    public void GetCurrentGeneral(General general)
    {
        CurrentGeneral = general;
    }


    public void MouseEnterItemCell(BaseEventData data)
    {
        if (itemInfo == null)
            return;

        //��ʾ���
        UIManager.GetInstance().ShowPanel<UI_TipsPanel>("TipsPanel", E_UI_Layer.Top, (panel) =>
        {
            //�첽���ؽ���������λ����Ϣ
            //������Ϣ
            panel.InitInfo(itemInfo);
            //����λ��
            panel.transform.position = GetControl<Image>("ImageIcon").transform.position;
        });

    }
    public void MouseExitItemCell(BaseEventData data)
    {
        if (itemInfo == null)
            return;

        //�������
        UIManager.GetInstance().HidePanel("TipsPanel");
    }

    /// <summary>
    /// ���߸��Ӷ���
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(PlayerItemInfo info)
    {
        this._itemInfo = info;
        //���ݵ�����Ϣ�����ݣ����¸��Ӷ���
        //ͨ���õ���ҵ����б��е�id���õ�����������Ϣ
        itemData = GameDataMgr.GetInstance().GetItemInfo(itemInfo.id);
        //ʹ�õ��߱��е�����
        //ͼ��
        //ͨ������ID�õ����߱��е�������Ϣ�󣬾Ϳ��Եõ���Ӧ�ĵ���ID�õ�ͼ����ʲô
        GetControl<Image>("ImageIcon").sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + itemData.Icon);
        //����
        GetControl<TMP_Text>("TextName").text = itemData.Name;
        //�������˴���ʾ��ҵ����б��иõ��ߵ�����
        GetControl<TMP_Text>("TextNumber").text = info.number.ToString();
        //�ۼ�
        GetControl<TMP_Text>("TextPrice").text = (itemData.Price / 2).ToString();
    }

    ////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// ������ҵ����иõ��ߵ�ʣ������
    /// </summary>
    /// <param name="info"></param>
    public void UpdateInfo(PlayerItemInfo info)
    {
        GetControl<TMP_Text>("TextNumber").text = itemInfo.number.ToString();
    }

    /// <summary>
    /// ʹ�õ���ʱ��ȷ�ϴ���
    /// </summary>
    public void UseItem()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound_pc01-ItemSelect", false);
        itemData = GameDataMgr.GetInstance().GetItemInfo(itemInfo.id);
        ConditionCheck();
        if (itemInfo.number > 0 && okToUse)
        {
            //��ʾȷ�ϳ��۴���
            UIManager.GetInstance().ShowPanel<UI_SellConfirmPanel>("SellConfirmPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("�Ƿ�ȷ��ʹ�ø���Ʒ?");
                //���ȷ�Ϲ���Ļص�����
                panel.onConfirm += OnUseConfirmed;
                okToUse = false;
            });
            return;
        }
        else
        {
            if (itemData.Type == "Material")
            {

                UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
                {
                    panel.InitInfo("�޷�ʹ�ø���Ʒ");
                });
            }
            else
            {
                UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
                {
                    panel.InitInfo("��ѡ�佫�����ոü���");
                });
            }

        }
    }

    /// <summary>
    /// ���ȷ�ϴ�������
    /// </summary>
    public void ConditionCheck()
    {
        if (itemData.Type == "Material")
        {
            okToUse = false;
            if (itemData.SkillName == "Tiger Seal" || itemData.SkillName == "Wei Liao Zi" || itemData.SkillName == "SunTzu's Art of War" || itemData.SkillName == "Liu Tao" || itemData.SkillName == "Official Seal")
            {
                okToUse = true;
            }
        }
        else if (itemData.Type == "ActiveSkillBook")
        {
            okToUse = !CurrentGeneral.PossessedActiveSkills.Contains(itemData.SkillName);
        }
        else if (itemData.Type == "PassiveSkillBook")
        {
            okToUse = !CurrentGeneral.PossessedPassiveSkills.Contains(itemData.SkillName);
        }

    }

    /// <summary>
    /// ȷ��ʹ�õĺ���
    /// </summary>
    /// <param name="confirmed"></param>
    private void OnUseConfirmed(bool confirmed)
    {
        if (confirmed)
        {

            //����ҿ���м���
            itemInfo.number -= 1;
            //�����ҵĿ����Ϊ0
            if (itemInfo.number <= 0)
            {
                //�Ƴ�����Ʒ����
                GameDataMgr.GetInstance().PlayerDataInfo.AllItems.Remove(itemInfo);
                if (itemData.Type == "ActiveSkillBook")
                    GameDataMgr.GetInstance().PlayerDataInfo.activeSkillBooks.Remove(itemInfo);
                if (itemData.Type == "PassiveSkillBook")
                    GameDataMgr.GetInstance().PlayerDataInfo.passiveSkillBooks.Remove(itemInfo);
                if (itemData.Type == "Material")
                    GameDataMgr.GetInstance().PlayerDataInfo.Materials.Remove(itemInfo);

                EventCenter.GetInstance().EventTrigger("PlayerItemNumberChangeInTroop");
            }

            //��������еĿ������
            UpdateInfo(itemInfo);
            
            //�������Ʒ������������
            if (itemData.Type == "ActiveSkillBook")
            {
                //������������
                GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].PossessedActiveSkills.Add(itemData.SkillName);
                //ˢ��
                EventCenter.GetInstance().EventTrigger("ToggleChanged", CurrentGeneral);


            }
            //����Ǳ���������
            else if (itemData.Type == "PassiveSkillBook")
            {

                //���ӱ�������
                GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].PossessedPassiveSkills.Add(itemData.SkillName);
                //ˢ��
                EventCenter.GetInstance().EventTrigger("ToggleChanged", CurrentGeneral);


            }
            else
            {
                //���������
                switch (itemData.SkillName)
                {
                    //����
                    case "Tiger Seal":

                        //����Ѫ��������
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Hp += 100;
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].CurrentHp += 100;
                       
                        break;
                    
                    //ξ����
                    case "Wei Liao Zi":

                        //��������
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Strength += 2;
                        GameDataMgr.GetInstance().PlayerDataInfo.AttributeChangeConfirm(CurrentGeneral.GeneralKey, 2, 0, 0);


                        break;

                    //���ӱ���     
                    case "SunTzu's Art of War":

                        //����ͳ��
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].LeaderShip += 2;
                        GameDataMgr.GetInstance().PlayerDataInfo.AttributeChangeConfirm(CurrentGeneral.GeneralKey, 0, 2, 0);


                        break;

                    //���
                    case "Liu Tao":
                        //������ı
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Wisdom += 2;
                        GameDataMgr.GetInstance().PlayerDataInfo.AttributeChangeConfirm(CurrentGeneral.GeneralKey, 0, 0, 2);
                        break;

                    //ӡ�
                    case "Official Seal":
                       
                        //��ԭ���Ե����״��
                        //��ԭ����
                        int strength = GameDataMgr.GetInstance().GetGeneralInfo(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].GeneralID).Strength - GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Strength;
                        int leaderShip = GameDataMgr.GetInstance().GetGeneralInfo(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].GeneralID).LeaderShip - GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].LeaderShip;
                        int wisdom =  GameDataMgr.GetInstance().GetGeneralInfo(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].GeneralID).Wisdom - GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Wisdom;
                        //����������Ϊһ��ʼ������
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Strength = GameDataMgr.GetInstance().GetGeneralInfo(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].GeneralID).Strength;
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].LeaderShip = GameDataMgr.GetInstance().GetGeneralInfo(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].GeneralID).LeaderShip;
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Wisdom = GameDataMgr.GetInstance().GetGeneralInfo(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].GeneralID).Wisdom;

                        //����ֵ�����¼���DataMgr��ȷ�϶������Ա仯
                        EventCenter.GetInstance().EventTrigger("AttributeChangeConfirmed", CurrentGeneral.GeneralKey, strength, leaderShip, wisdom);
                        //��ԭ�ɷ������Ե�
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].MaxAttributePoints = 3 * GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Level;
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].UnassignedAttributePoints = 3 * (GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Level - 1);
                        break;
                }
                //ˢ��ҳ��
                EventCenter.GetInstance().EventTrigger("ToggleChanged", GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey]);
                //��������
                EventCenter.GetInstance().EventTrigger("SavePlayerInfo");

            }

        }
    }
}
