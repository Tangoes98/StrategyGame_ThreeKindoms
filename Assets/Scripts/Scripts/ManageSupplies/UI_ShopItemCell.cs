using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UI_ShopItemCell : BasePanel
{
    private Item shopCellInfo;
    protected override void Awake()
    {
        base.Awake();
        //�����¹�����Ʒ��ťִ�й�����Ʒ����
        GetControl<Button>("ButtonBuy").onClick.AddListener(BuyItem);

        /////////////////////////////////////////////////////////////////////////
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
        ///////////////////////////////////////////////////////////////////////////
    }

    public void MouseEnterItemCell(BaseEventData data)
    {
        if (shopCellInfo == null)
            return;
        //��ʾ���
        UIManager.GetInstance().ShowPanel<UI_ShopTipsPanel>("ShopTipsPanel", E_UI_Layer.Top, (panel) =>
        {
            //�첽���ؽ���������λ����Ϣ
            //������Ϣ
            panel.InitInfo(shopCellInfo);
            //����λ��
            panel.transform.position = GetControl<Image>("ImageIcon").transform.position;
        });
    }
    public void MouseExitItemCell(BaseEventData data)
    {
        if (shopCellInfo == null)
            return;
        //�������
        UIManager.GetInstance().HidePanel("ShopTipsPanel");
    }

    /// <summary>
    /// ��ʼ����Ʒ
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(Item info)
    {
        this.shopCellInfo = info;

        //����id�õ�������Ϣ
        Item itemData = GameDataMgr.GetInstance().GetItemInfo(info.ID);
        //����ͼ��
        GetControl<Image>("ImageIcon").sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + itemData.Icon);
        //����
        GetControl<TMP_Text>("TextName").text = itemData.Name;
        //�۸�
        GetControl<TMP_Text>("TextPrice").text = itemData.Price.ToString();
        //��������
        GetControl<TMP_Text>("TextNumber").text = itemData.Number.ToString();  
    }

    /// <summary>
    /// �����̵��иõ��ߵ�ʣ������
    /// </summary>
    /// <param name="info"></param>
    public void UpdateInfo(Item info)
    {
        GetControl<TMP_Text>("TextNumber").text = shopCellInfo.Number.ToString();
    }

    /// <summary>
    /// �ص��������ȴ�����ȷ�������ȷ�Ϲ����ִ��
    /// </summary>
    /// <param name="confirmed"></param>
    private void OnBuyConfirmed(bool confirmed)
    {
        if (confirmed)
        {
            //���õ�����ӵ���ҵĵ��߿�
            GameDataMgr.GetInstance().PlayerDataInfo.AddItemForPlayer(GameDataMgr.GetInstance().PlayerDataInfo.AddPlayerItemInfo(shopCellInfo.ID, 1));
            //���̵����м���
            shopCellInfo.Number -= 1;
            //��������еĿ������
            UpdateInfo(shopCellInfo);

            //����ɹ������ٵ��ڼ۸��Ǯ��
            //ͨ���¼���������Ǯ�ĸı�
            EventCenter.GetInstance().EventTrigger("MoneyChange", -shopCellInfo.Price);

            //ˢ����ҵ�ǰͣ���ı���ҳǩ
            //�õ�BagPanel
            GameObject BagPanel = GameObject.FindWithTag("BagPanel");
            UI_BagPanel Script = BagPanel.GetComponent<UI_BagPanel>();
            //�õ���ǰBagPanel���ڵ�ҳǩ
            int currentBagType = Script.currentType;
            //�����¼����¸�ҳǩ����Ϣ
            EventCenter.GetInstance().EventTrigger("PlayerItemNumberChange", currentBagType);

        }

    }

    /// <summary>
    /// ������Ʒ�ĺ���
    /// </summary>
    public void BuyItem()
    {
        
        //�����ҵ�Ǯ�����ڵ�ǰ��Ʒ�ļ۸�
        if (GameDataMgr.GetInstance().PlayerDataInfo.money >= shopCellInfo.Price && shopCellInfo.Number > 0)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_pc01-ItemSelect", false);
            //��ʾȷ�Ϲ��򴰿�
            UIManager.GetInstance().ShowPanel<UI_BuyConfirmPanel>("ConfirmPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("�Ƿ�ȷ�Ϲ������Ʒ?");
                //���ȷ�Ϲ���Ļص�����
                panel.onConfirm += OnBuyConfirmed;
            });
            return;
           
        }
        //��������Ǯ���̵������������
        else if (GameDataMgr.GetInstance().PlayerDataInfo.money > shopCellInfo.Price && shopCellInfo.Number <= 0)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("����Ʒ�������㣬�޷�����");
            });
        }

        else
        //���Ǯ����
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("�Ҿ����Ѳ����Թ������Ʒ");
            });
        }
    }
}
