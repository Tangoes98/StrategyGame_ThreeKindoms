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

        //当按下出售物品按钮执行出售物品函数
        GetControl<Button>("ButtonSell").onClick.AddListener(SellItem);

        //Tips的鼠标移入监听
        ///////////////////////////////////////////////////////////////////////////////////////////////
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
        /////////////////////////////////////////////////////////////////////////////////////////////////

    }


    public void MouseEnterItemCell(BaseEventData data)
    {
        if (itemInfo == null)
            return;

        //显示面板
        UIManager.GetInstance().ShowPanel<UI_TipsPanel>("TipsPanel", E_UI_Layer.Top, (panel) =>
        {
            //异步加载结束后，设置位置信息
            //更新信息
            panel.InitInfo(itemInfo);
            //更新位置
            panel.transform.position = GetControl<Image>("ImageIcon").transform.position;
        });

    }
    public void MouseExitItemCell(BaseEventData data)
    {
        if (itemInfo == null)
            return;

        //隐藏面板
        UIManager.GetInstance().HidePanel("TipsPanel");
    }

    /// <summary>
    /// 道具格子对象
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(PlayerItemInfo info)
    {
        this._itemInfo = info;
        //根据道具信息的数据，更新格子对象
        //通过得到玩家道具列表中的id来得到整个道具信息
        Item itemData = GameDataMgr.GetInstance().GetItemInfo(info.id);
        //使用道具表中的数据
        //图标
        //通过道具ID得到道具表中的数据信息后，就可以得到对应的道具ID用的图标是什么
        GetControl<Image>("ImageIcon").sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + itemData.Icon);
        //名称
        GetControl<TMP_Text>("TextName").text = itemData.Name;
        //数量，此处显示玩家道具列表中该道具的数量
        GetControl<TMP_Text>("TextNumber").text = info.number.ToString();
        //售价
        GetControl<TMP_Text>("TextPrice").text = (itemData.Price/2).ToString();
    }

    ////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 更新玩家道具中该道具的剩余数量
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
            //显示确认出售窗口
            UIManager.GetInstance().ShowPanel<UI_SellConfirmPanel>("SellConfirmPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("是否确认出售该物品?");
                //添加确认购买的回调函数
                panel.onConfirm += OnSellConfirmed;
            });
            return;
        }
        else
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("您持有的该物品数量不足，无法出售");
            });
        }
    }



    private void OnSellConfirmed(bool confirmed)
    {
        if (confirmed)
        {
            //将该道具添加到商店的道具库存
            Item itemData = GameDataMgr.GetInstance().GetItemInfo(itemInfo.id);
            itemData.Number += 1;
            //从玩家库存中减少
            itemInfo.number -= 1;
            //如果玩家的库存量为0
            if(itemInfo.number <= 0)
            {
                //移除该物品的类
                GameDataMgr.GetInstance().PlayerDataInfo.AllItems.Remove(itemInfo);
                if(itemData.Type == "ActiveSkillBook")
                    GameDataMgr.GetInstance().PlayerDataInfo.activeSkillBooks.Remove(itemInfo);
                if (itemData.Type == "PassiveSkillBook")
                    GameDataMgr.GetInstance().PlayerDataInfo.passiveSkillBooks.Remove(itemInfo);
                if (itemData.Type == "Material")
                    GameDataMgr.GetInstance().PlayerDataInfo.Materials.Remove(itemInfo);
                
                //刷新玩家当前停留的背包页签
                //得到BagPanel
                GameObject BagPanel = GameObject.FindWithTag("BagPanel");
                UI_BagPanel BagScript = BagPanel.GetComponent<UI_BagPanel>();
                //得到当前BagPanel处于的页签
                int currentBagType = BagScript.currentType;
                EventCenter.GetInstance().EventTrigger("PlayerItemNumberChange", currentBagType);
            }

            //更新面板中的库存数量
            UpdateInfo(itemInfo);

            //售卖成功，获得等于价格一半的钱数
            //四舍五入道具价值的一半
            float Money = Mathf.Round(itemData.Price/2);
            int roundedMoney = Mathf.RoundToInt(Money);
            //通过事件监听触发钱的改变
            EventCenter.GetInstance().EventTrigger("MoneyChange", roundedMoney);

            //刷新玩家当前停留的背包页签
            //得到ShopPanel
            GameObject ShopPanel = GameObject.FindWithTag("ShopPanel");
            UI_ShopPanel Script = ShopPanel.GetComponent<UI_ShopPanel>();
            //得到当前ShopPanel处于的页签
            int currentShopType = Script.currentType;
            //发送事件更新改页签的信息
            EventCenter.GetInstance().EventTrigger("ShopItemNumberChange", currentShopType);

        }

    }
}
