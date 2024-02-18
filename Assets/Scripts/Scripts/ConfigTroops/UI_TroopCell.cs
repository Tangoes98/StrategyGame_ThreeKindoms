using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///设置两种兵种类型 
/// </summary>
public enum E_TroopCell_Type
{
    //兵种栏位，在MainPanel里
    TroopChute,
    //兵种池，在将军所持有的兵种panel里的兵种
    TroopPools    
}


public class UI_TroopCell : BasePanel
{
    //储存当前选择的兵种信息
    public Troop currentTroop;

    //方便TroopCellMgr调用
    public Image imageTroop;
    public Image imageTroopIcon;

    public E_TroopCell_Type type = E_TroopCell_Type.TroopPools;

    protected override void Awake()
    {
        base.Awake();
        imageTroop = GetControl<Image>("ImageTroop");
        imageTroopIcon = GetControl<Image>("ImageTroopIcon");
        /////////////////////////////////////////////////////////////////////////
        //监听鼠标移入和鼠标移出的事件，进行处理
        EventTrigger trigger = imageTroop.gameObject.AddComponent<EventTrigger>();

        //申明一个鼠标进入的事件类对象
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener(MouseEnterTroopCell);
        trigger.triggers.Add(enter);

        //申明一个鼠标移出的事件类对象
        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener(MouseExitTroopCell);
        trigger.triggers.Add(exit);

        //申明鼠标拖动的事件类对象
        //开始拖动
        EventTrigger.Entry beginDrag = new EventTrigger.Entry();
        beginDrag.eventID = EventTriggerType.BeginDrag;
        beginDrag.callback.AddListener(BeginDragTroopCell);
        trigger.triggers.Add(beginDrag);
        //拖动中
        EventTrigger.Entry onDrag = new EventTrigger.Entry();
        onDrag.eventID = EventTriggerType.Drag;
        onDrag.callback.AddListener(OnDragTroopCell);
        trigger.triggers.Add(onDrag);
        //拖动结束
        EventTrigger.Entry endDrag = new EventTrigger.Entry();
        endDrag.eventID = EventTriggerType.EndDrag;
        endDrag.callback.AddListener(EndDragTroopCell);
        trigger.triggers.Add(endDrag);
    }

    //发送事件，鼠标进入图标
    public void MouseEnterTroopCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_TroopCell>("MouseEnterTroopCell", this);
    }

    //发送事件，鼠标移出图标
    public void MouseExitTroopCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_TroopCell>("MouseExitTroopCell", this);
    }

    //发送事件，开始拖动
    public void BeginDragTroopCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_TroopCell>("BeginDragTroopCell", this);
    }

    //发送事件，拖动中
    public void OnDragTroopCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<BaseEventData>("OnDragTroopCell", data);
    }

    //发送事件，拖动完成
    public void EndDragTroopCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_TroopCell>("EndDragTroopCell", this);
    }

    /// <summary>
    /// 初始化Troop信息
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(Troop info)
    {
        this.currentTroop = info;
        //根据道具信息的数据，更新格子对象
        //通过得到玩家道具列表中的id来得到整个道具信息
        Troop troopData = GameDataMgr.GetInstance().GetTroopInfo(info.TroopID);
        //使用道具表中的数据
        //图标
        //通过道具ID得到道具表中的数据信息后，就可以得到对应的道具ID用的图标是什么
        imageTroopIcon.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + troopData.TroopIcon);
        //名称
        GetControl<TMP_Text>("TextTroopName").text = troopData.TroopName;
        
    }
}
