using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemCell : BasePanel
{
    private PlayerItemInfo _itemInfo;

    public PlayerItemInfo itemInfo
    {
        get { return _itemInfo; }
    }

    protected override void Awake()
    {
        base.Awake();

        //�����³�����Ʒ��ťִ�г�����Ʒ����
        GetControl<Button>("ButtonSell").onClick.AddListener(SellItem);

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
        Item itemData = GameDataMgr.GetInstance().GetItemInfo(info.id);
        //ʹ�õ��߱��е�����
        //ͼ��
        //ͨ������ID�õ����߱��е�������Ϣ�󣬾Ϳ��Եõ���Ӧ�ĵ���ID�õ�ͼ����ʲô
        GetControl<Image>("ImageIcon").sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + itemData.Icon);
        //����
        GetControl<TMP_Text>("TextName").text = itemData.Name;
        //�������˴���ʾ��ҵ����б��иõ��ߵ�����
        GetControl<TMP_Text>("TextNumber").text = info.number.ToString();
        //�ۼ�
        GetControl<TMP_Text>("TextPrice").text = (itemData.Price/2).ToString();
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

    public void SellItem()
    {
        if (itemInfo.number > 0)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_pc01-ItemSelect", false);
            //��ʾȷ�ϳ��۴���
            UIManager.GetInstance().ShowPanel<UI_SellConfirmPanel>("SellConfirmPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("�Ƿ�ȷ�ϳ��۸���Ʒ?");
                //���ȷ�Ϲ���Ļص�����
                panel.onConfirm += OnSellConfirmed;
            });
            return;
        }
        else
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("�����еĸ���Ʒ�������㣬�޷�����");
            });
        }
    }



    private void OnSellConfirmed(bool confirmed)
    {
        if (confirmed)
        {
            //���õ�����ӵ��̵�ĵ��߿��
            Item itemData = GameDataMgr.GetInstance().GetItemInfo(itemInfo.id);
            itemData.Number += 1;
            //����ҿ���м���
            itemInfo.number -= 1;
            //�����ҵĿ����Ϊ0
            if(itemInfo.number <= 0)
            {
                //�Ƴ�����Ʒ����
                GameDataMgr.GetInstance().PlayerDataInfo.AllItems.Remove(itemInfo);
                if(itemData.Type == "ActiveSkillBook")
                    GameDataMgr.GetInstance().PlayerDataInfo.activeSkillBooks.Remove(itemInfo);
                if (itemData.Type == "PassiveSkillBook")
                    GameDataMgr.GetInstance().PlayerDataInfo.passiveSkillBooks.Remove(itemInfo);
                if (itemData.Type == "Material")
                    GameDataMgr.GetInstance().PlayerDataInfo.Materials.Remove(itemInfo);
                
                //ˢ����ҵ�ǰͣ���ı���ҳǩ
                //�õ�BagPanel
                GameObject BagPanel = GameObject.FindWithTag("BagPanel");
                UI_BagPanel BagScript = BagPanel.GetComponent<UI_BagPanel>();
                //�õ���ǰBagPanel���ڵ�ҳǩ
                int currentBagType = BagScript.currentType;
                EventCenter.GetInstance().EventTrigger("PlayerItemNumberChange", currentBagType);
            }

            //��������еĿ������
            UpdateInfo(itemInfo);

            //�����ɹ�����õ��ڼ۸�һ���Ǯ��
            //����������߼�ֵ��һ��
            float Money = Mathf.Round(itemData.Price/2);
            int roundedMoney = Mathf.RoundToInt(Money);
            //ͨ���¼���������Ǯ�ĸı�
            EventCenter.GetInstance().EventTrigger("MoneyChange", roundedMoney);

            //ˢ����ҵ�ǰͣ���ı���ҳǩ
            //�õ�ShopPanel
            GameObject ShopPanel = GameObject.FindWithTag("ShopPanel");
            UI_ShopPanel Script = ShopPanel.GetComponent<UI_ShopPanel>();
            //�õ���ǰShopPanel���ڵ�ҳǩ
            int currentShopType = Script.currentType;
            //�����¼����¸�ҳǩ����Ϣ
            EventCenter.GetInstance().EventTrigger("ShopItemNumberChange", currentShopType);

        }

    }
}
