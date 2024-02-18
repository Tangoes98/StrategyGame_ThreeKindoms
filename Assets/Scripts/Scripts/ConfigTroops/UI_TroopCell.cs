using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///�������ֱ������� 
/// </summary>
public enum E_TroopCell_Type
{
    //������λ����MainPanel��
    TroopChute,
    //���ֳأ��ڽ��������еı���panel��ı���
    TroopPools    
}


public class UI_TroopCell : BasePanel
{
    //���浱ǰѡ��ı�����Ϣ
    public Troop currentTroop;

    //����TroopCellMgr����
    public Image imageTroop;
    public Image imageTroopIcon;

    public E_TroopCell_Type type = E_TroopCell_Type.TroopPools;

    protected override void Awake()
    {
        base.Awake();
        imageTroop = GetControl<Image>("ImageTroop");
        imageTroopIcon = GetControl<Image>("ImageTroopIcon");
        /////////////////////////////////////////////////////////////////////////
        //����������������Ƴ����¼������д���
        EventTrigger trigger = imageTroop.gameObject.AddComponent<EventTrigger>();

        //����һ����������¼������
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener(MouseEnterTroopCell);
        trigger.triggers.Add(enter);

        //����һ������Ƴ����¼������
        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener(MouseExitTroopCell);
        trigger.triggers.Add(exit);

        //��������϶����¼������
        //��ʼ�϶�
        EventTrigger.Entry beginDrag = new EventTrigger.Entry();
        beginDrag.eventID = EventTriggerType.BeginDrag;
        beginDrag.callback.AddListener(BeginDragTroopCell);
        trigger.triggers.Add(beginDrag);
        //�϶���
        EventTrigger.Entry onDrag = new EventTrigger.Entry();
        onDrag.eventID = EventTriggerType.Drag;
        onDrag.callback.AddListener(OnDragTroopCell);
        trigger.triggers.Add(onDrag);
        //�϶�����
        EventTrigger.Entry endDrag = new EventTrigger.Entry();
        endDrag.eventID = EventTriggerType.EndDrag;
        endDrag.callback.AddListener(EndDragTroopCell);
        trigger.triggers.Add(endDrag);
    }

    //�����¼���������ͼ��
    public void MouseEnterTroopCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_TroopCell>("MouseEnterTroopCell", this);
    }

    //�����¼�������Ƴ�ͼ��
    public void MouseExitTroopCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_TroopCell>("MouseExitTroopCell", this);
    }

    //�����¼�����ʼ�϶�
    public void BeginDragTroopCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_TroopCell>("BeginDragTroopCell", this);
    }

    //�����¼����϶���
    public void OnDragTroopCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<BaseEventData>("OnDragTroopCell", data);
    }

    //�����¼����϶����
    public void EndDragTroopCell(BaseEventData data)
    {
        EventCenter.GetInstance().EventTrigger<UI_TroopCell>("EndDragTroopCell", this);
    }

    /// <summary>
    /// ��ʼ��Troop��Ϣ
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(Troop info)
    {
        this.currentTroop = info;
        //���ݵ�����Ϣ�����ݣ����¸��Ӷ���
        //ͨ���õ���ҵ����б��е�id���õ�����������Ϣ
        Troop troopData = GameDataMgr.GetInstance().GetTroopInfo(info.TroopID);
        //ʹ�õ��߱��е�����
        //ͼ��
        //ͨ������ID�õ����߱��е�������Ϣ�󣬾Ϳ��Եõ���Ӧ�ĵ���ID�õ�ͼ����ʲô
        imageTroopIcon.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + troopData.TroopIcon);
        //����
        GetControl<TMP_Text>("TextTroopName").text = troopData.TroopName;
        
    }
}
