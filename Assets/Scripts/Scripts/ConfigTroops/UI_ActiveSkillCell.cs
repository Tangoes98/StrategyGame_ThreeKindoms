using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///�������ּ�����λ���� 
/// </summary>
public enum E_ActiveSkillCell_Type
{
    //������λ����MainPanel��
    ActiveSkillChute,
    //���ܳ�
    ActiveSkillPools
}

public class UI_ActiveSkillCell : BasePanel
{
    //���浱ǰѡ��ļ�����Ϣ
    public string currentSkill;

    //����TroopCellMgr����
    public Image imageSkillIcon;

    public E_ActiveSkillCell_Type type;

    //��int��¼��ǰ���ӵı��
    public int index;

    protected override void Awake()
    {
        base.Awake();
        imageSkillIcon = GetControl<Image>("ImageSkillIcon");

        /////////////////////////////////////////////////////////////////////////
        //����������������Ƴ����¼������д���
        EventTrigger trigger = imageSkillIcon.gameObject.AddComponent<EventTrigger>();

        //����һ����������¼������
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener(MouseEnterASCell);
        trigger.triggers.Add(enter);

        //����һ������Ƴ����¼������
        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener(MouseExitASCell);
        trigger.triggers.Add(exit);

        //��������϶����¼������
        //��ʼ�϶�
        EventTrigger.Entry beginDrag = new EventTrigger.Entry();
        beginDrag.eventID = EventTriggerType.BeginDrag;
        beginDrag.callback.AddListener(BeginDragASCell);
        trigger.triggers.Add(beginDrag);
        //�϶���
        EventTrigger.Entry onDrag = new EventTrigger.Entry();
        onDrag.eventID = EventTriggerType.Drag;
        onDrag.callback.AddListener(OnDragASCell);
        trigger.triggers.Add(onDrag);
        //�϶�����
        EventTrigger.Entry endDrag = new EventTrigger.Entry();
        endDrag.eventID = EventTriggerType.EndDrag;
        endDrag.callback.AddListener(EndDragASCell);
        trigger.triggers.Add(endDrag);
    }

    //�����¼���������ͼ��
    public void MouseEnterASCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_ActiveSkillCell>("MouseEnterASCell", this);
    }

    //�����¼�������Ƴ�ͼ��
    public void MouseExitASCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_ActiveSkillCell>("MouseExitASCell", this);
    }

    //�����¼�����ʼ�϶�
    public void BeginDragASCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_ActiveSkillCell>("BeginDragASCell", this);
    }

    //�����¼����϶���
    public void OnDragASCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<BaseEventData>("OnDragASCell", data);
    }

    //�����¼����϶����
    public void EndDragASCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_ActiveSkillCell>("EndDragASCell", this);
    }

    /// <summary>
    /// ��ʼ��λ�ڱ������ܳ��еĸ�����Ϣ
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(string info)
    {
        this.currentSkill = info;

        //ͼ��
        imageSkillIcon.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + info + "Icon");
        //����
        GetControl<TMP_Text>("TextName").text = info;

    }

    /// <summary>
    /// ��ʼ��λ�ڱ������ܲ�λ�еĸ�����Ϣ
    /// </summary>
    public void InitInfoChute()
    {
        //ͼ�꣬ͨ��ͼ��
        imageSkillIcon.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/PassiveSkillIconChute");
        //���ƣ��̶�����
        GetControl<TMP_Text>("TextName").text = "����������λ";
    }
}

