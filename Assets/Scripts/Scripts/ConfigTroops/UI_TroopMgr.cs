using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TroopMgr : BaseManager<UI_TroopMgr>
{
    //当前拖动的军队格子
    private UI_TroopCell currentDragTroop;
    //当前所在的军队格子
    private UI_TroopCell currentStayTroop;

    //当前选中军队的图标
    private Image currentDragTroopImg;
    //bool判断军队是否在拖动
    private bool isDraging = false;
    ///////////////////////////////////////////////////////////////////////

    //当前拖动的被动技能格子
    private UI_PassiveSkillCell currentDragPassiveSkillCell;
    //当前所在的被动技能格子
    private UI_PassiveSkillCell currentStayPassiveSkillCell;

    //当前选中的被动技能图标
    private Image currentDragPassiveSkillImg;
    //bool判断被动技能是否在拖动
    private bool isPScellDraging = false;
    //设置被动技能栏位编号
    public int setPassiveIndex;

    /// /////////////////////////////////////////////////////////////////////

    //当前拖动的被动技能格子
    private UI_ActiveSkillCell currentDragActiveSkillCell;
    //当前所在的被动技能格子
    private UI_ActiveSkillCell currentStayActiveSkillCell;

    //当前选中的被动技能图标
    private Image currentDragActiveSkillImg;
    //bool判断被动技能是否在拖动
    private bool isAScellDraging = false;
    //设置主动技能栏位编号
    public int setActiveIndex;

    /// <summary>
    /// 初始化所有事件监听
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
    /// 移除所有事件监听
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
    /// 替换兵种的函数
    /// </summary>
    /// <param name="troopCell"></param>
    public void ChangeTroop(UI_TroopCell troopCell)
    {
        //存在进入的格子，且这个格子是兵种栏位
        if(currentStayTroop != null && currentStayTroop.type == E_TroopCell_Type.TroopChute)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound06_Change", false);
            //兵种替换
            GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral.currentSelectTroop = troopCell.currentTroop;

            //更新面板
            UIManager.GetInstance().GetPanel<UI_ConfigTroopsPanelMain>("ConfigTroopsPanelMain").UpdateSelectTroop();

            //保存数据
            GameDataMgr.GetInstance().SavePlayerInfo();
        }
    }

    /// <summary>
    /// 鼠标进入部队格子
    /// </summary>
    /// <param name="troopCell"></param>
    private void MouseEnterTroopCell(UI_TroopCell troopCell)
    {
        //记录拖动过程中进入的格子
        if (isDraging)
        {
            //如果正在拖动，那么拖动中的cell就是当前选择的cell
            currentStayTroop = troopCell;
            return;
        }

    }

    /// <summary>
    /// 鼠标离开部队格子
    /// </summary>
    /// <param name="troopCell"></param>
    private void MouseExitTroopCell(UI_TroopCell troopCell)
    {
        //置空拖动过程中离开的格子
        if (isDraging)
        {
            currentStayTroop = null;
            return;
        }

    }

    /// <summary>
    /// 鼠标开始拖动部队格子
    /// </summary>
    /// <param name="troopCell"></param>
    private void BeginDragTroopCell(UI_TroopCell troopCell)
    {
        isDraging = true;
        //记录当前选中的格子
        currentDragTroop = troopCell;

        //创建图片，表示当前拖动的兵种的图标
        PoolManager.GetInstance().GetObject("UI/imageTroopIcon", (obj) =>
        {
            currentDragTroopImg = obj.GetComponent<Image>();
            currentDragTroopImg.sprite = troopCell.imageTroopIcon.sprite;

            //设置父对象并改变缩放大小
            currentDragTroopImg.transform.SetParent(UIManager.GetInstance().canvas);
            currentDragTroopImg.transform.localScale = Vector3.one;

            //如果异步加载结束，拖动已经结束，直接放入缓存池
            if (!isDraging)
            {
                PoolManager.GetInstance().PushObject(currentDragTroopImg.name, currentDragTroopImg.gameObject);
                currentDragTroopImg = null;
            }

        });
    }

    /// <summary>
    /// 鼠标拖动中部队格子
    /// </summary>
    /// <param name="eventData"></param>
    private void OnDragTroopCell(BaseEventData eventData)
    {
        //更新位置
        //把鼠标位置转换到UI相关位置，让图标跟随鼠标移动
        if(currentDragTroopImg == null)
            return;
        Vector2 localPosition;
        //用于坐标转换的API
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            UIManager.GetInstance().canvas, //希望得到坐标结果对象的父对象
            (eventData as PointerEventData).position, //鼠标位置
            (eventData as PointerEventData).pressEventCamera, //UI摄像机
            out localPosition);
        currentDragTroopImg.transform.localPosition = localPosition;
    }

    /// <summary>
    /// 鼠标拖动完毕
    /// </summary>
    /// <param name="troopCell"></param>
    private void EndDragTroopCell(UI_TroopCell troopCell)
    {
        isDraging=false;

        //结束拖动时，置空格子信息
        currentDragTroop = null;

        //切换兵种
        ChangeTroop(troopCell);

        //结束拖动，移除图片
        if (currentDragTroopImg == null)
            return;
        PoolManager.GetInstance().PushObject(currentDragTroopImg.name, currentDragTroopImg.gameObject);

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 替换被动技能的函数
    /// </summary>
    /// <param name="passiveSkillCell"></param>
    public void ChangePassiveSkill(UI_PassiveSkillCell passiveSkillCell)
    {

        //存在进入的格子，且这个格子是被动技能栏位
        if (currentStayPassiveSkillCell != null && currentStayPassiveSkillCell.type == E_PassiveSkillCell_Type.PassiveSkillChute)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound06_Change", false);
            setPassiveIndex = currentStayPassiveSkillCell.index;
            //更新面板
            UIManager.GetInstance().GetPanel<UI_PassiveSkillChute>("PassiveSkillsScrollView").UpdateChute(setPassiveIndex,passiveSkillCell.currentSkill);

        }
        else
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound05_takeDown", false);
            setPassiveIndex = currentDragPassiveSkillCell.index;
            //更新面板
            UIManager.GetInstance().GetPanel<UI_PassiveSkillChute>("PassiveSkillsScrollView").ClearChute(setPassiveIndex, passiveSkillCell.currentSkill);

        }
    }

    /// <summary>
    /// 鼠标进入被动技能格子
    /// </summary>
    /// <param name="passiveSkillCell"></param>
    private void MouseEnterPSCell(UI_PassiveSkillCell passiveSkillCell)
    {
        //记录拖动过程中进入的格子
        if (isPScellDraging)
        {
            //如果正在拖动，那么拖动中的cell就是当前选择的cell
            currentStayPassiveSkillCell = passiveSkillCell;
            return;
        }

    }

    /// <summary>
    /// 鼠标离开被动技能格子
    /// </summary>
    /// <param name="passiveSkillCell"></param>
    private void MouseExitPSCell(UI_PassiveSkillCell passiveSkillCell)
    {
        //置空拖动过程中离开的格子
        if (isPScellDraging)
        {
            currentStayPassiveSkillCell = null;
            return;
        }

    }

    /// <summary>
    /// 鼠标开始拖动被动技能格子
    /// </summary>
    /// <param name="passiveSkillCell"></param>
    private void BeginDragPSCell(UI_PassiveSkillCell passiveSkillCell)
    {
        isPScellDraging = true;
        //记录当前选中的格子
        currentDragPassiveSkillCell = passiveSkillCell;

        //创建图片，表示当前拖动的兵种的图标
        PoolManager.GetInstance().GetObject("UI/ImageSkillIcon", (obj) =>
        {
            currentDragPassiveSkillImg = obj.GetComponent<Image>();
            currentDragPassiveSkillImg.sprite = passiveSkillCell.imageSkillIcon.sprite;

            //设置父对象并改变缩放大小
            currentDragPassiveSkillImg.transform.SetParent(UIManager.GetInstance().canvas);
            currentDragPassiveSkillImg.transform.localScale = Vector3.one;

            //如果异步加载结束，拖动已经结束，直接放入缓存池
            if (!isPScellDraging)
            {
                PoolManager.GetInstance().PushObject(currentDragPassiveSkillImg.name, currentDragPassiveSkillImg.gameObject);
                currentDragPassiveSkillImg = null;
            }

        });
    }

    /// <summary>
    /// 鼠标拖动中被动技能格子
    /// </summary>
    /// <param name="eventData"></param>
    private void OnDragPSCell(BaseEventData eventData)
    {
        //更新位置
        //把鼠标位置转换到UI相关位置，让图标跟随鼠标移动
        if (currentDragPassiveSkillImg == null)
            return;
        Vector2 localPosition;
        //用于坐标转换的API
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            UIManager.GetInstance().canvas, //希望得到坐标结果对象的父对象
            (eventData as PointerEventData).position, //鼠标位置
            (eventData as PointerEventData).pressEventCamera, //UI摄像机
            out localPosition);
        currentDragPassiveSkillImg.transform.localPosition = localPosition;
    }

    /// <summary>
    /// 鼠标结束拖动被动技能格子
    /// </summary>
    /// <param name="passiveSkillCell"></param>
    private void EndDragPSCell(UI_PassiveSkillCell passiveSkillCell)
    {
        isPScellDraging = false;

        //切换技能
        ChangePassiveSkill(passiveSkillCell);
        //结束拖动时，置空格子信息
        currentDragPassiveSkillCell = null;

        //结束拖动，移除图片
        if (currentDragPassiveSkillImg == null)
            return;
        PoolManager.GetInstance().PushObject(currentDragPassiveSkillImg.name, currentDragPassiveSkillImg.gameObject);

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 替换主动技能的函数
    /// </summary>
    /// <param name="activeSkillCell"></param>
    public void ChangeActiveSkill(UI_ActiveSkillCell activeSkillCell)
    {

        //存在进入的格子，且这个格子是被动技能栏位
        if (currentStayActiveSkillCell != null && currentStayActiveSkillCell.type == E_ActiveSkillCell_Type.ActiveSkillChute)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound06_Change", false);
            setActiveIndex = currentStayActiveSkillCell.index;
            //更新面板
            UIManager.GetInstance().GetPanel<UI_ActiveSkillChute>("ActiveSkillScrollView").UpdateChute(setActiveIndex, activeSkillCell.currentSkill);
            //Debug.Log(setActiveIndex);
        }
        else
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound05_takeDown", false);
            setActiveIndex = currentDragActiveSkillCell.index;
            //更新面板
            UIManager.GetInstance().GetPanel<UI_ActiveSkillChute>("ActiveSkillScrollView").ClearChute(setActiveIndex, activeSkillCell.currentSkill);
            //Debug.Log(setActiveIndex);
        }
    }

    /// <summary>
    /// 鼠标进入主动技能格子
    /// </summary>
    /// <param name="activeSkillCell"></param>
    private void MouseEnterASCell(UI_ActiveSkillCell activeSkillCell)
    {
        //记录拖动过程中进入的格子
        if (isAScellDraging)
        {
            //如果正在拖动，那么拖动中的cell就是当前选择的cell
            currentStayActiveSkillCell = activeSkillCell;
            return;
        }

    }

    /// <summary>
    /// 鼠标离开主动技能格子
    /// </summary>
    /// <param name="activeSkillCell"></param>
    private void MouseExitASCell(UI_ActiveSkillCell activeSkillCell)
    {
        //置空拖动过程中离开的格子
        if (isAScellDraging)
        {
            currentStayActiveSkillCell = null;
            return;
        }

    }

    /// <summary>
    /// 鼠标开始拖动主动技能格子
    /// </summary>
    /// <param name="activeSkillCell"></param>
    private void BeginDragASCell(UI_ActiveSkillCell activeSkillCell)
    {
        isAScellDraging = true;
        //记录当前选中的格子
        currentDragActiveSkillCell = activeSkillCell;

        //创建图片，表示当前拖动的兵种的图标
        PoolManager.GetInstance().GetObject("UI/ImageSkillIcon", (obj) =>
        {
            currentDragActiveSkillImg = obj.GetComponent<Image>();
            currentDragActiveSkillImg.sprite = activeSkillCell.imageSkillIcon.sprite;

            //设置父对象并改变缩放大小
            currentDragActiveSkillImg.transform.SetParent(UIManager.GetInstance().canvas);
            currentDragActiveSkillImg.transform.localScale = Vector3.one;

            //如果异步加载结束，拖动已经结束，直接放入缓存池
            if (!isAScellDraging)
            {
                PoolManager.GetInstance().PushObject(currentDragActiveSkillImg.name, currentDragActiveSkillImg.gameObject);
                currentDragActiveSkillImg = null;
            }

        });
    }

    /// <summary>
    /// 鼠标拖动中主动技能格子
    /// </summary>
    /// <param name="eventData"></param>
    private void OnDragASCell(BaseEventData eventData)
    {
        //更新位置
        //把鼠标位置转换到UI相关位置，让图标跟随鼠标移动
        if (currentDragActiveSkillImg == null)
            return;
        Vector2 localPosition;
        //用于坐标转换的API
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            UIManager.GetInstance().canvas, //希望得到坐标结果对象的父对象
            (eventData as PointerEventData).position, //鼠标位置
            (eventData as PointerEventData).pressEventCamera, //UI摄像机
            out localPosition);
        currentDragActiveSkillImg.transform.localPosition = localPosition;
    }

    /// <summary>
    /// 鼠标结束拖动主动技能格子
    /// </summary>
    /// <param name="activeSkillCell"></param>
    private void EndDragASCell(UI_ActiveSkillCell activeSkillCell)
    {
        isPScellDraging = false;

        //切换技能
        ChangeActiveSkill(activeSkillCell);
        //结束拖动时，置空格子信息
        currentDragActiveSkillCell = null;

        //结束拖动，移除图片
        if (currentDragActiveSkillImg == null)
            return;
        PoolManager.GetInstance().PushObject(currentDragActiveSkillImg.name, currentDragActiveSkillImg.gameObject);

    }
}
