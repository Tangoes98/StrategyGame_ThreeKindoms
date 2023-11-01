using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///设置两种技能栏位类型 
/// </summary>
public enum E_PassiveSkillCell_Type
{
    //技能栏位，在MainPanel里
    PassiveSkillChute,
    //技能池
    PassiveSkillPools
}


public class UI_PassiveSkillCell : BasePanel
{
    //储存当前选择的技能信息
    public string currentSkill;

    //方便TroopCellMgr调用
    public Image imageSkill;
    public Image imageSkillIcon;

    public E_PassiveSkillCell_Type type;

    //用int记录当前格子的编号
    public int index;

    protected override void Awake()
    {
        base.Awake();
        imageSkill = GetControl<Image>("ImageSkill");
        imageSkillIcon = GetControl<Image>("ImageSkillIcon");

        /////////////////////////////////////////////////////////////////////////
        //监听鼠标移入和鼠标移出的事件，进行处理
        EventTrigger trigger = imageSkillIcon.gameObject.AddComponent<EventTrigger>();

        //申明一个鼠标进入的事件类对象
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener(MouseEnterPSCell);
        trigger.triggers.Add(enter);

        //申明一个鼠标移出的事件类对象
        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener(MouseExitPSCell);
        trigger.triggers.Add(exit);

        //申明鼠标拖动的事件类对象
        //开始拖动
        EventTrigger.Entry beginDrag = new EventTrigger.Entry();
        beginDrag.eventID = EventTriggerType.BeginDrag;
        beginDrag.callback.AddListener(BeginDragPSCell);
        trigger.triggers.Add(beginDrag);
        //拖动中
        EventTrigger.Entry onDrag = new EventTrigger.Entry();
        onDrag.eventID = EventTriggerType.Drag;
        onDrag.callback.AddListener(OnDragPSCell);
        trigger.triggers.Add(onDrag);
        //拖动结束
        EventTrigger.Entry endDrag = new EventTrigger.Entry();
        endDrag.eventID = EventTriggerType.EndDrag;
        endDrag.callback.AddListener(EndDragPSCell);
        trigger.triggers.Add(endDrag);
    }

    //发送事件，鼠标进入图标
    public void MouseEnterPSCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_PassiveSkillCell>("MouseEnterPSCell", this);
    }

    //发送事件，鼠标移出图标
    public void MouseExitPSCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_PassiveSkillCell>("MouseExitPSCell", this);
    }

    //发送事件，开始拖动
    public void BeginDragPSCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_PassiveSkillCell>("BeginDragPSCell", this);
    }

    //发送事件，拖动中
    public void OnDragPSCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<BaseEventData>("OnDragPSCell", data);
    }

    //发送事件，拖动完成
    public void EndDragPSCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_PassiveSkillCell>("EndDragPSCell", this);
    }

    /// <summary>
    /// 初始化位于被动技能池中的格子信息
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(string info)
    {
        this.currentSkill = info;

        //图标
        imageSkillIcon.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + info + "Icon");
        //名称
        GetControl<TMP_Text>("TextName").text = info;

    }

    /// <summary>
    /// 初始化位于被动技能槽位中的格子信息
    /// </summary>
    public void InitInfoChute()
    {
        //图标，通用图标
        imageSkillIcon.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/PassiveSkillIconChute");
        //名称，固定名称
        GetControl<TMP_Text>("TextName").text = "被动技能栏位";
    }
}
