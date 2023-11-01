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
        //当按下购买物品按钮执行购买物品函数
        GetControl<Button>("ButtonBuy").onClick.AddListener(BuyItem);

        /////////////////////////////////////////////////////////////////////////
        //监听鼠标移入和鼠标移出的事件，进行处理
        EventTrigger trigger = GetControl<Image>("ImageIcon").gameObject.AddComponent<EventTrigger>();

        //申明一个鼠标进入的事件类对象
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener(MouseEnterItemCell);

        //申明一个鼠标移出的事件类对象
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
        //显示面板
        UIManager.GetInstance().ShowPanel<UI_ShopTipsPanel>("ShopTipsPanel", E_UI_Layer.Top, (panel) =>
        {
            //异步加载结束后，设置位置信息
            //更新信息
            panel.InitInfo(shopCellInfo);
            //更新位置
            panel.transform.position = GetControl<Image>("ImageIcon").transform.position;
        });
    }
    public void MouseExitItemCell(BaseEventData data)
    {
        if (shopCellInfo == null)
            return;
        //隐藏面板
        UIManager.GetInstance().HidePanel("ShopTipsPanel");
    }

    /// <summary>
    /// 初始化商品
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(Item info)
    {
        this.shopCellInfo = info;

        //根据id得到道具信息
        Item itemData = GameDataMgr.GetInstance().GetItemInfo(info.ID);
        //道具图标
        GetControl<Image>("ImageIcon").sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + itemData.Icon);
        //名字
        GetControl<TMP_Text>("TextName").text = itemData.Name;
        //价格
        GetControl<TMP_Text>("TextPrice").text = itemData.Price.ToString();
        //售卖数量
        GetControl<TMP_Text>("TextNumber").text = itemData.Number.ToString();  
    }

    /// <summary>
    /// 更新商店中该道具的剩余数量
    /// </summary>
    /// <param name="info"></param>
    public void UpdateInfo(Item info)
    {
        GetControl<TMP_Text>("TextNumber").text = shopCellInfo.Number.ToString();
    }

    /// <summary>
    /// 回调函数，等待购买确认面板中确认购买后执行
    /// </summary>
    /// <param name="confirmed"></param>
    private void OnBuyConfirmed(bool confirmed)
    {
        if (confirmed)
        {
            //将该道具添加到玩家的道具库
            GameDataMgr.GetInstance().PlayerDataInfo.AddItemForPlayer(GameDataMgr.GetInstance().PlayerDataInfo.AddPlayerItemInfo(shopCellInfo.ID, 1));
            //从商店库存中减少
            shopCellInfo.Number -= 1;
            //更新面板中的库存数量
            UpdateInfo(shopCellInfo);

            //购买成功，减少等于价格的钱数
            //通过事件监听触发钱的改变
            EventCenter.GetInstance().EventTrigger("MoneyChange", -shopCellInfo.Price);

            //刷新玩家当前停留的背包页签
            //得到BagPanel
            GameObject BagPanel = GameObject.FindWithTag("BagPanel");
            UI_BagPanel Script = BagPanel.GetComponent<UI_BagPanel>();
            //得到当前BagPanel处于的页签
            int currentBagType = Script.currentType;
            //发送事件更新改页签的信息
            EventCenter.GetInstance().EventTrigger("PlayerItemNumberChange", currentBagType);

        }

    }

    /// <summary>
    /// 购买物品的函数
    /// </summary>
    public void BuyItem()
    {
        
        //如果玩家的钱数大于当前物品的价格
        if (GameDataMgr.GetInstance().PlayerDataInfo.money >= shopCellInfo.Price && shopCellInfo.Number > 0)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_pc01-ItemSelect", false);
            //显示确认购买窗口
            UIManager.GetInstance().ShowPanel<UI_BuyConfirmPanel>("ConfirmPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("是否确认购买该物品?");
                //添加确认购买的回调函数
                panel.onConfirm += OnBuyConfirmed;
            });
            return;
           
        }
        //如果玩家有钱但商店道具数量不足
        else if (GameDataMgr.GetInstance().PlayerDataInfo.money > shopCellInfo.Price && shopCellInfo.Number <= 0)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("该物品数量不足，无法购买");
            });
        }

        else
        //玩家钱不够
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("我军军费不足以购买该物品");
            });
        }
    }
}
