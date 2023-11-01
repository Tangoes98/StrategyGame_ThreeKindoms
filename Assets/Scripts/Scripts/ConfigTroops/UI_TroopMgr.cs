using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TroopMgr : BaseManager<UI_TroopMgr>
{
    //��ǰ�϶��ľ��Ӹ���
    private UI_TroopCell currentDragTroop;
    //��ǰ���ڵľ��Ӹ���
    private UI_TroopCell currentStayTroop;

    //��ǰѡ�о��ӵ�ͼ��
    private Image currentDragTroopImg;
    //bool�жϾ����Ƿ����϶�
    private bool isDraging = false;
    ///////////////////////////////////////////////////////////////////////

    //��ǰ�϶��ı������ܸ���
    private UI_PassiveSkillCell currentDragPassiveSkillCell;
    //��ǰ���ڵı������ܸ���
    private UI_PassiveSkillCell currentStayPassiveSkillCell;

    //��ǰѡ�еı�������ͼ��
    private Image currentDragPassiveSkillImg;
    //bool�жϱ��������Ƿ����϶�
    private bool isPScellDraging = false;
    //���ñ���������λ���
    public int setPassiveIndex;

    /// /////////////////////////////////////////////////////////////////////

    //��ǰ�϶��ı������ܸ���
    private UI_ActiveSkillCell currentDragActiveSkillCell;
    //��ǰ���ڵı������ܸ���
    private UI_ActiveSkillCell currentStayActiveSkillCell;

    //��ǰѡ�еı�������ͼ��
    private Image currentDragActiveSkillImg;
    //bool�жϱ��������Ƿ����϶�
    private bool isAScellDraging = false;
    //��������������λ���
    public int setActiveIndex;

    /// <summary>
    /// ��ʼ�������¼�����
    /// </summary>
    public void InitListener()
    {
        EventCenter.GetInstance().AddEventListener<UI_TroopCell>("MouseEnterTroopCell", MouseEnterTroopCell);
        EventCenter.GetInstance().AddEventListener<UI_TroopCell>("MouseExitTroopCell", MouseExitTroopCell);
        EventCenter.GetInstance().AddEventListener<UI_TroopCell>("BeginDragTroopCell", BeginDragTroopCell);
        EventCenter.GetInstance().AddEventListener<BaseEventData>("OnDragTroopCell", OnDragTroopCell);
        EventCenter.GetInstance().AddEventListener<UI_TroopCell>("EndDragTroopCell", EndDragTroopCell);

        EventCenter.GetInstance().AddEventListener<UI_PassiveSkillCell>("MouseEnterPSCell", MouseEnterPSCell);
        EventCenter.GetInstance().AddEventListener<UI_PassiveSkillCell>("MouseExitPSCell", MouseExitPSCell);
        EventCenter.GetInstance().AddEventListener<UI_PassiveSkillCell>("BeginDragPSCell", BeginDragPSCell);
        EventCenter.GetInstance().AddEventListener<BaseEventData>("OnDragPSCell", OnDragPSCell);
        EventCenter.GetInstance().AddEventListener<UI_PassiveSkillCell>("EndDragPSCell", EndDragPSCell);

        EventCenter.GetInstance().AddEventListener<UI_ActiveSkillCell>("MouseEnterASCell", MouseEnterASCell);
        EventCenter.GetInstance().AddEventListener<UI_ActiveSkillCell>("MouseExitASCell", MouseExitASCell);
        EventCenter.GetInstance().AddEventListener<UI_ActiveSkillCell>("BeginDragASCell", BeginDragASCell);
        EventCenter.GetInstance().AddEventListener<BaseEventData>("OnDragASCell", OnDragASCell);
        EventCenter.GetInstance().AddEventListener<UI_ActiveSkillCell>("EndDragASCell", EndDragASCell);
    }
    /// <summary>
    /// �Ƴ������¼�����
    /// </summary>
    public void RemoveListener()
    {
        EventCenter.GetInstance().RemoveEventListener<UI_TroopCell>("MouseEnterTroopCell", MouseEnterTroopCell);
        EventCenter.GetInstance().RemoveEventListener<UI_TroopCell>("MouseExitTroopCell", MouseExitTroopCell);
        EventCenter.GetInstance().RemoveEventListener<UI_TroopCell>("BeginDragTroopCell", BeginDragTroopCell);
        EventCenter.GetInstance().RemoveEventListener<BaseEventData>("OnDragTroopCell", OnDragTroopCell);
        EventCenter.GetInstance().RemoveEventListener<UI_TroopCell>("EndDragTroopCell", EndDragTroopCell);

        EventCenter.GetInstance().RemoveEventListener<UI_PassiveSkillCell>("MouseEnterPSCell", MouseEnterPSCell);
        EventCenter.GetInstance().RemoveEventListener<UI_PassiveSkillCell>("MouseExitPSCell", MouseExitPSCell);
        EventCenter.GetInstance().RemoveEventListener<UI_PassiveSkillCell>("BeginDragPSCell", BeginDragPSCell);
        EventCenter.GetInstance().RemoveEventListener<BaseEventData>("OnDragPSCell", OnDragPSCell);
        EventCenter.GetInstance().RemoveEventListener<UI_PassiveSkillCell>("EndDragPSCell", EndDragPSCell);

        EventCenter.GetInstance().RemoveEventListener<UI_ActiveSkillCell>("MouseEnterASCell", MouseEnterASCell);
        EventCenter.GetInstance().RemoveEventListener<UI_ActiveSkillCell>("MouseExitASCell", MouseExitASCell);
        EventCenter.GetInstance().RemoveEventListener<UI_ActiveSkillCell>("BeginDragASCell", BeginDragASCell);
        EventCenter.GetInstance().RemoveEventListener<BaseEventData>("OnDragASCell", OnDragASCell);
        EventCenter.GetInstance().RemoveEventListener<UI_ActiveSkillCell>("EndDragASCell", EndDragASCell);
    }

    /// <summary>
    /// �滻���ֵĺ���
    /// </summary>
    /// <param name="troopCell"></param>
    public void ChangeTroop(UI_TroopCell troopCell)
    {
        //���ڽ���ĸ��ӣ�����������Ǳ�����λ
        if(currentStayTroop != null && currentStayTroop.type == E_TroopCell_Type.TroopChute)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound06_Change", false);
            //�����滻
            GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral.currentSelectTroop = troopCell.currentTroop;

            //�������
            UIManager.GetInstance().GetPanel<UI_ConfigTroopsPanelMain>("ConfigTroopsPanelMain").UpdateSelectTroop();

            //��������
            GameDataMgr.GetInstance().SavePlayerInfo();
        }
    }

    /// <summary>
    /// �����벿�Ӹ���
    /// </summary>
    /// <param name="troopCell"></param>
    private void MouseEnterTroopCell(UI_TroopCell troopCell)
    {
        //��¼�϶������н���ĸ���
        if (isDraging)
        {
            //��������϶�����ô�϶��е�cell���ǵ�ǰѡ���cell
            currentStayTroop = troopCell;
            return;
        }

    }

    /// <summary>
    /// ����뿪���Ӹ���
    /// </summary>
    /// <param name="troopCell"></param>
    private void MouseExitTroopCell(UI_TroopCell troopCell)
    {
        //�ÿ��϶��������뿪�ĸ���
        if (isDraging)
        {
            currentStayTroop = null;
            return;
        }

    }

    /// <summary>
    /// ��꿪ʼ�϶����Ӹ���
    /// </summary>
    /// <param name="troopCell"></param>
    private void BeginDragTroopCell(UI_TroopCell troopCell)
    {
        isDraging = true;
        //��¼��ǰѡ�еĸ���
        currentDragTroop = troopCell;

        //����ͼƬ����ʾ��ǰ�϶��ı��ֵ�ͼ��
        PoolManager.GetInstance().GetObject("UI/imageTroopIcon", (obj) =>
        {
            currentDragTroopImg = obj.GetComponent<Image>();
            currentDragTroopImg.sprite = troopCell.imageTroopIcon.sprite;

            //���ø����󲢸ı����Ŵ�С
            currentDragTroopImg.transform.SetParent(UIManager.GetInstance().canvas);
            currentDragTroopImg.transform.localScale = Vector3.one;

            //����첽���ؽ������϶��Ѿ�������ֱ�ӷ��뻺���
            if (!isDraging)
            {
                PoolManager.GetInstance().PushObject(currentDragTroopImg.name, currentDragTroopImg.gameObject);
                currentDragTroopImg = null;
            }

        });
    }

    /// <summary>
    /// ����϶��в��Ӹ���
    /// </summary>
    /// <param name="eventData"></param>
    private void OnDragTroopCell(BaseEventData eventData)
    {
        //����λ��
        //�����λ��ת����UI���λ�ã���ͼ���������ƶ�
        if(currentDragTroopImg == null)
            return;
        Vector2 localPosition;
        //��������ת����API
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            UIManager.GetInstance().canvas, //ϣ���õ�����������ĸ�����
            (eventData as PointerEventData).position, //���λ��
            (eventData as PointerEventData).pressEventCamera, //UI�����
            out localPosition);
        currentDragTroopImg.transform.localPosition = localPosition;
    }

    /// <summary>
    /// ����϶����
    /// </summary>
    /// <param name="troopCell"></param>
    private void EndDragTroopCell(UI_TroopCell troopCell)
    {
        isDraging=false;

        //�����϶�ʱ���ÿո�����Ϣ
        currentDragTroop = null;

        //�л�����
        ChangeTroop(troopCell);

        //�����϶����Ƴ�ͼƬ
        if (currentDragTroopImg == null)
            return;
        PoolManager.GetInstance().PushObject(currentDragTroopImg.name, currentDragTroopImg.gameObject);

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// �滻�������ܵĺ���
    /// </summary>
    /// <param name="passiveSkillCell"></param>
    public void ChangePassiveSkill(UI_PassiveSkillCell passiveSkillCell)
    {

        //���ڽ���ĸ��ӣ�����������Ǳ���������λ
        if (currentStayPassiveSkillCell != null && currentStayPassiveSkillCell.type == E_PassiveSkillCell_Type.PassiveSkillChute)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound06_Change", false);
            setPassiveIndex = currentStayPassiveSkillCell.index;
            //�������
            UIManager.GetInstance().GetPanel<UI_PassiveSkillChute>("PassiveSkillsScrollView").UpdateChute(setPassiveIndex,passiveSkillCell.currentSkill);

        }
        else
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound05_takeDown", false);
            setPassiveIndex = currentDragPassiveSkillCell.index;
            //�������
            UIManager.GetInstance().GetPanel<UI_PassiveSkillChute>("PassiveSkillsScrollView").ClearChute(setPassiveIndex, passiveSkillCell.currentSkill);

        }
    }

    /// <summary>
    /// �����뱻�����ܸ���
    /// </summary>
    /// <param name="passiveSkillCell"></param>
    private void MouseEnterPSCell(UI_PassiveSkillCell passiveSkillCell)
    {
        //��¼�϶������н���ĸ���
        if (isPScellDraging)
        {
            //��������϶�����ô�϶��е�cell���ǵ�ǰѡ���cell
            currentStayPassiveSkillCell = passiveSkillCell;
            return;
        }

    }

    /// <summary>
    /// ����뿪�������ܸ���
    /// </summary>
    /// <param name="passiveSkillCell"></param>
    private void MouseExitPSCell(UI_PassiveSkillCell passiveSkillCell)
    {
        //�ÿ��϶��������뿪�ĸ���
        if (isPScellDraging)
        {
            currentStayPassiveSkillCell = null;
            return;
        }

    }

    /// <summary>
    /// ��꿪ʼ�϶��������ܸ���
    /// </summary>
    /// <param name="passiveSkillCell"></param>
    private void BeginDragPSCell(UI_PassiveSkillCell passiveSkillCell)
    {
        isPScellDraging = true;
        //��¼��ǰѡ�еĸ���
        currentDragPassiveSkillCell = passiveSkillCell;

        //����ͼƬ����ʾ��ǰ�϶��ı��ֵ�ͼ��
        PoolManager.GetInstance().GetObject("UI/ImageSkillIcon", (obj) =>
        {
            currentDragPassiveSkillImg = obj.GetComponent<Image>();
            currentDragPassiveSkillImg.sprite = passiveSkillCell.imageSkillIcon.sprite;

            //���ø����󲢸ı����Ŵ�С
            currentDragPassiveSkillImg.transform.SetParent(UIManager.GetInstance().canvas);
            currentDragPassiveSkillImg.transform.localScale = Vector3.one;

            //����첽���ؽ������϶��Ѿ�������ֱ�ӷ��뻺���
            if (!isPScellDraging)
            {
                PoolManager.GetInstance().PushObject(currentDragPassiveSkillImg.name, currentDragPassiveSkillImg.gameObject);
                currentDragPassiveSkillImg = null;
            }

        });
    }

    /// <summary>
    /// ����϶��б������ܸ���
    /// </summary>
    /// <param name="eventData"></param>
    private void OnDragPSCell(BaseEventData eventData)
    {
        //����λ��
        //�����λ��ת����UI���λ�ã���ͼ���������ƶ�
        if (currentDragPassiveSkillImg == null)
            return;
        Vector2 localPosition;
        //��������ת����API
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            UIManager.GetInstance().canvas, //ϣ���õ�����������ĸ�����
            (eventData as PointerEventData).position, //���λ��
            (eventData as PointerEventData).pressEventCamera, //UI�����
            out localPosition);
        currentDragPassiveSkillImg.transform.localPosition = localPosition;
    }

    /// <summary>
    /// �������϶��������ܸ���
    /// </summary>
    /// <param name="passiveSkillCell"></param>
    private void EndDragPSCell(UI_PassiveSkillCell passiveSkillCell)
    {
        isPScellDraging = false;

        //�л�����
        ChangePassiveSkill(passiveSkillCell);
        //�����϶�ʱ���ÿո�����Ϣ
        currentDragPassiveSkillCell = null;

        //�����϶����Ƴ�ͼƬ
        if (currentDragPassiveSkillImg == null)
            return;
        PoolManager.GetInstance().PushObject(currentDragPassiveSkillImg.name, currentDragPassiveSkillImg.gameObject);

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// �滻�������ܵĺ���
    /// </summary>
    /// <param name="activeSkillCell"></param>
    public void ChangeActiveSkill(UI_ActiveSkillCell activeSkillCell)
    {

        //���ڽ���ĸ��ӣ�����������Ǳ���������λ
        if (currentStayActiveSkillCell != null && currentStayActiveSkillCell.type == E_ActiveSkillCell_Type.ActiveSkillChute)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound06_Change", false);
            setActiveIndex = currentStayActiveSkillCell.index;
            //�������
            UIManager.GetInstance().GetPanel<UI_ActiveSkillChute>("ActiveSkillScrollView").UpdateChute(setActiveIndex, activeSkillCell.currentSkill);
            //Debug.Log(setActiveIndex);
        }
        else
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound05_takeDown", false);
            setActiveIndex = currentDragActiveSkillCell.index;
            //�������
            UIManager.GetInstance().GetPanel<UI_ActiveSkillChute>("ActiveSkillScrollView").ClearChute(setActiveIndex, activeSkillCell.currentSkill);
            //Debug.Log(setActiveIndex);
        }
    }

    /// <summary>
    /// �������������ܸ���
    /// </summary>
    /// <param name="activeSkillCell"></param>
    private void MouseEnterASCell(UI_ActiveSkillCell activeSkillCell)
    {
        //��¼�϶������н���ĸ���
        if (isAScellDraging)
        {
            //��������϶�����ô�϶��е�cell���ǵ�ǰѡ���cell
            currentStayActiveSkillCell = activeSkillCell;
            return;
        }

    }

    /// <summary>
    /// ����뿪�������ܸ���
    /// </summary>
    /// <param name="activeSkillCell"></param>
    private void MouseExitASCell(UI_ActiveSkillCell activeSkillCell)
    {
        //�ÿ��϶��������뿪�ĸ���
        if (isAScellDraging)
        {
            currentStayActiveSkillCell = null;
            return;
        }

    }

    /// <summary>
    /// ��꿪ʼ�϶��������ܸ���
    /// </summary>
    /// <param name="activeSkillCell"></param>
    private void BeginDragASCell(UI_ActiveSkillCell activeSkillCell)
    {
        isAScellDraging = true;
        //��¼��ǰѡ�еĸ���
        currentDragActiveSkillCell = activeSkillCell;

        //����ͼƬ����ʾ��ǰ�϶��ı��ֵ�ͼ��
        PoolManager.GetInstance().GetObject("UI/ImageSkillIcon", (obj) =>
        {
            currentDragActiveSkillImg = obj.GetComponent<Image>();
            currentDragActiveSkillImg.sprite = activeSkillCell.imageSkillIcon.sprite;

            //���ø����󲢸ı����Ŵ�С
            currentDragActiveSkillImg.transform.SetParent(UIManager.GetInstance().canvas);
            currentDragActiveSkillImg.transform.localScale = Vector3.one;

            //����첽���ؽ������϶��Ѿ�������ֱ�ӷ��뻺���
            if (!isAScellDraging)
            {
                PoolManager.GetInstance().PushObject(currentDragActiveSkillImg.name, currentDragActiveSkillImg.gameObject);
                currentDragActiveSkillImg = null;
            }

        });
    }

    /// <summary>
    /// ����϶����������ܸ���
    /// </summary>
    /// <param name="eventData"></param>
    private void OnDragASCell(BaseEventData eventData)
    {
        //����λ��
        //�����λ��ת����UI���λ�ã���ͼ���������ƶ�
        if (currentDragActiveSkillImg == null)
            return;
        Vector2 localPosition;
        //��������ת����API
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            UIManager.GetInstance().canvas, //ϣ���õ�����������ĸ�����
            (eventData as PointerEventData).position, //���λ��
            (eventData as PointerEventData).pressEventCamera, //UI�����
            out localPosition);
        currentDragActiveSkillImg.transform.localPosition = localPosition;
    }

    /// <summary>
    /// �������϶��������ܸ���
    /// </summary>
    /// <param name="activeSkillCell"></param>
    private void EndDragASCell(UI_ActiveSkillCell activeSkillCell)
    {
        isPScellDraging = false;

        //�л�����
        ChangeActiveSkill(activeSkillCell);
        //�����϶�ʱ���ÿո�����Ϣ
        currentDragActiveSkillCell = null;

        //�����϶����Ƴ�ͼƬ
        if (currentDragActiveSkillImg == null)
            return;
        PoolManager.GetInstance().PushObject(currentDragActiveSkillImg.name, currentDragActiveSkillImg.gameObject);

    }
}
