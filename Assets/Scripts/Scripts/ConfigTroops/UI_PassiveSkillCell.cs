using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///�������ּ�����λ���� 
/// </summary>
public enum E_PassiveSkillCell_Type
{
    //������λ����MainPanel��
    PassiveSkillChute,
    //���ܳ�
    PassiveSkillPools
}


public class UI_PassiveSkillCell : BasePanel
{
    //���浱ǰѡ��ļ�����Ϣ
    public string currentSkill;

    //����TroopCellMgr����
    public Image imageSkill;
    public Image imageSkillIcon;

    public E_PassiveSkillCell_Type type;

    //��int��¼��ǰ���ӵı��
    public int index;

    protected override void Awake()
    {
        base.Awake();
        imageSkill = GetControl<Image>("ImageSkill");
        imageSkillIcon = GetControl<Image>("ImageSkillIcon");

        /////////////////////////////////////////////////////////////////////////
        //����������������Ƴ����¼������д���
        EventTrigger trigger = imageSkillIcon.gameObject.AddComponent<EventTrigger>();

        //����һ����������¼������
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener(MouseEnterPSCell);
        trigger.triggers.Add(enter);

        //����һ������Ƴ����¼������
        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener(MouseExitPSCell);
        trigger.triggers.Add(exit);

        //��������϶����¼������
        //��ʼ�϶�
        EventTrigger.Entry beginDrag = new EventTrigger.Entry();
        beginDrag.eventID = EventTriggerType.BeginDrag;
        beginDrag.callback.AddListener(BeginDragPSCell);
        trigger.triggers.Add(beginDrag);
        //�϶���
        EventTrigger.Entry onDrag = new EventTrigger.Entry();
        onDrag.eventID = EventTriggerType.Drag;
        onDrag.callback.AddListener(OnDragPSCell);
        trigger.triggers.Add(onDrag);
        //�϶�����
        EventTrigger.Entry endDrag = new EventTrigger.Entry();
        endDrag.eventID = EventTriggerType.EndDrag;
        endDrag.callback.AddListener(EndDragPSCell);
        trigger.triggers.Add(endDrag);
    }

    //�����¼���������ͼ��
    public void MouseEnterPSCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_PassiveSkillCell>("MouseEnterPSCell", this);
    }

    //�����¼�������Ƴ�ͼ��
    public void MouseExitPSCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_PassiveSkillCell>("MouseExitPSCell", this);
    }

    //�����¼�����ʼ�϶�
    public void BeginDragPSCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_PassiveSkillCell>("BeginDragPSCell", this);
    }

    //�����¼����϶���
    public void OnDragPSCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<BaseEventData>("OnDragPSCell", data);
    }

    //�����¼����϶����
    public void EndDragPSCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_PassiveSkillCell>("EndDragPSCell", this);
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
