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

        //当按下出售物品按钮执行出售物品函数
        GetControl<Button>("ButtonUse").onClick.AddListener(UseItem);

        //监听当前使用该道具的将军
        EventCenter.GetInstance().AddEventListener<General>("ToggleChanged", GetCurrentGeneral);

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
        itemData = GameDataMgr.GetInstance().GetItemInfo(itemInfo.id);
        //使用道具表中的数据
        //图标
        //通过道具ID得到道具表中的数据信息后，就可以得到对应的道具ID用的图标是什么
        GetControl<Image>("ImageIcon").sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + itemData.Icon);
        //名称
        GetControl<TMP_Text>("TextName").text = itemData.Name;
        //数量，此处显示玩家道具列表中该道具的数量
        GetControl<TMP_Text>("TextNumber").text = info.number.ToString();
        //售价
        GetControl<TMP_Text>("TextPrice").text = (itemData.Price / 2).ToString();
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

    /// <summary>
    /// 使用道具时的确认窗口
    /// </summary>
    public void UseItem()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound_pc01-ItemSelect", false);
        itemData = GameDataMgr.GetInstance().GetItemInfo(itemInfo.id);
        ConditionCheck();
        if (itemInfo.number > 0 && okToUse)
        {
            //显示确认出售窗口
            UIManager.GetInstance().ShowPanel<UI_SellConfirmPanel>("SellConfirmPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("是否确认使用该物品?");
                //添加确认购买的回调函数
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
                    panel.InitInfo("无法使用该物品");
                });
            }
            else
            {
                UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
                {
                    panel.InitInfo("所选武将已掌握该技能");
                });
            }

        }
    }

    /// <summary>
    /// 检查确认窗口条件
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
    /// 确认使用的函数
    /// </summary>
    /// <param name="confirmed"></param>
    private void OnUseConfirmed(bool confirmed)
    {
        if (confirmed)
        {

            //从玩家库存中减少
            itemInfo.number -= 1;
            //如果玩家的库存量为0
            if (itemInfo.number <= 0)
            {
                //移除该物品的类
                GameDataMgr.GetInstance().PlayerDataInfo.AllItems.Remove(itemInfo);
                if (itemData.Type == "ActiveSkillBook")
                    GameDataMgr.GetInstance().PlayerDataInfo.activeSkillBooks.Remove(itemInfo);
                if (itemData.Type == "PassiveSkillBook")
                    GameDataMgr.GetInstance().PlayerDataInfo.passiveSkillBooks.Remove(itemInfo);
                if (itemData.Type == "Material")
                    GameDataMgr.GetInstance().PlayerDataInfo.Materials.Remove(itemInfo);

                EventCenter.GetInstance().EventTrigger("PlayerItemNumberChangeInTroop");
            }

            //更新面板中的库存数量
            UpdateInfo(itemInfo);
            
            //如果该物品是主动技能书
            if (itemData.Type == "ActiveSkillBook")
            {
                //增加主动技能
                GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].PossessedActiveSkills.Add(itemData.SkillName);
                //刷新
                EventCenter.GetInstance().EventTrigger("ToggleChanged", CurrentGeneral);


            }
            //如果是被动技能书
            else if (itemData.Type == "PassiveSkillBook")
            {

                //增加被动技能
                GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].PossessedPassiveSkills.Add(itemData.SkillName);
                //刷新
                EventCenter.GetInstance().EventTrigger("ToggleChanged", CurrentGeneral);


            }
            else
            {
                //如果是物资
                switch (itemData.SkillName)
                {
                    //虎符
                    case "Tiger Seal":

                        //增加血量及上限
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Hp += 100;
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].CurrentHp += 100;
                       
                        break;
                    
                    //尉缭子
                    case "Wei Liao Zi":

                        //增加力量
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Strength += 2;
                        GameDataMgr.GetInstance().PlayerDataInfo.AttributeChangeConfirm(CurrentGeneral.GeneralKey, 2, 0, 0);


                        break;

                    //孙子兵法     
                    case "SunTzu's Art of War":

                        //增加统率
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].LeaderShip += 2;
                        GameDataMgr.GetInstance().PlayerDataInfo.AttributeChangeConfirm(CurrentGeneral.GeneralKey, 0, 2, 0);


                        break;

                    //六韬
                    case "Liu Tao":
                        //增加智谋
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Wisdom += 2;
                        GameDataMgr.GetInstance().PlayerDataInfo.AttributeChangeConfirm(CurrentGeneral.GeneralKey, 0, 0, 2);
                        break;

                    //印绶
                    case "Official Seal":
                       
                        //还原属性点分配状况
                        //还原属性
                        int strength = GameDataMgr.GetInstance().GetGeneralInfo(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].GeneralID).Strength - GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Strength;
                        int leaderShip = GameDataMgr.GetInstance().GetGeneralInfo(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].GeneralID).LeaderShip - GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].LeaderShip;
                        int wisdom =  GameDataMgr.GetInstance().GetGeneralInfo(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].GeneralID).Wisdom - GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Wisdom;
                        //将属性设置为一开始的属性
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Strength = GameDataMgr.GetInstance().GetGeneralInfo(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].GeneralID).Strength;
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].LeaderShip = GameDataMgr.GetInstance().GetGeneralInfo(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].GeneralID).LeaderShip;
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Wisdom = GameDataMgr.GetInstance().GetGeneralInfo(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].GeneralID).Wisdom;

                        //将差值发送事件给DataMgr，确认二级属性变化
                        EventCenter.GetInstance().EventTrigger("AttributeChangeConfirmed", CurrentGeneral.GeneralKey, strength, leaderShip, wisdom);
                        //还原可分配属性点
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].MaxAttributePoints = 3 * GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Level;
                        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].UnassignedAttributePoints = 3 * (GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey].Level - 1);
                        break;
                }
                //刷新页面
                EventCenter.GetInstance().EventTrigger("ToggleChanged", GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[CurrentGeneral.GeneralKey]);
                //保存数据
                EventCenter.GetInstance().EventTrigger("SavePlayerInfo");

            }

        }
    }
}
