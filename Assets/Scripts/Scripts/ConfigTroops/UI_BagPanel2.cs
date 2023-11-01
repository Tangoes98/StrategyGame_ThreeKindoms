using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

public enum E_Bag_Type2
{
    All,
    ActiveSkillBook,
    PassiveSkillBook,
    Material
}


public class UI_BagPanel2 : BasePanel
{
    //用于绑定content
    public Transform content;
    //存储列表用于切换页签时清空
    private List<UI_ItemCellInConfigTroop> list = new List<UI_ItemCellInConfigTroop>();
    public int currentType;

    void Start()
    {
        //为Toggle添加事件监听。触发数据更新
        GetControl<Toggle>("ToggleAll").onValueChanged.AddListener(ToggleValueChange);
        GetControl<Toggle>("ToggleASB").onValueChanged.AddListener(ToggleValueChange);
        GetControl<Toggle>("TogglePSB").onValueChanged.AddListener(ToggleValueChange);
        GetControl<Toggle>("ToggleMaterial").onValueChanged.AddListener(ToggleValueChange);

        //事件监听，当玩家购买成功时刷新页面
        EventCenter.GetInstance().AddEventListener("PlayerItemNumberChangeInTroop", UpdateType);
    }

    public override void ShowMe()
    {
        base.ShowMe();
        ChangeType(E_Bag_Type2.All);
    }

    public override void HideMe()
    {
        base.HideMe();
        EventCenter.GetInstance().RemoveEventListener("PlayerItemNumberChangeInTroop", UpdateType);
    }

    private void ToggleValueChange(bool value)
    {
        if (GetControl<Toggle>("ToggleAll").isOn)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_ignition04_SwitchToggle", false);
            ChangeType(E_Bag_Type2.All);
            currentType = 0;
        }
        else if (GetControl<Toggle>("ToggleASB").isOn)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_ignition04_SwitchToggle", false);
            ChangeType(E_Bag_Type2.ActiveSkillBook);
            currentType = 1;
        }
        else if (GetControl<Toggle>("TogglePSB").isOn)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_ignition04_SwitchToggle", false);
            ChangeType(E_Bag_Type2.PassiveSkillBook);
            currentType = 2;
        }
        else if (GetControl<Toggle>("ToggleMaterial").isOn)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_ignition04_SwitchToggle", false);
            ChangeType(E_Bag_Type2.Material);
            currentType = 3;
        }
    }

    public void UpdateType()
    {
        if (currentType == 0)
            ChangeType(E_Bag_Type2.All);
        else if (currentType == 1)
            ChangeType(E_Bag_Type2.ActiveSkillBook);
        else if (currentType == 2)
            ChangeType(E_Bag_Type2.PassiveSkillBook);
        else if (currentType == 3)
            ChangeType(E_Bag_Type2.Material);
    }

    /// <summary>
    /// 页签切换功能
    /// </summary>
    /// <param name="type"></param>
    private void ChangeType(E_Bag_Type2 type)
    {
        // 默认展示所有道具
        List<PlayerItemInfo> tempInfo = GameDataMgr.GetInstance().PlayerDataInfo.AllItems;

        // 使用临时列表存储需要删除的元素
        List<PlayerItemInfo> itemsToRemove = new List<PlayerItemInfo>();

        // 找到需要删除的元素
        foreach (PlayerItemInfo iteminfo in tempInfo)
        {
            if (iteminfo.number == 0)
            {
                itemsToRemove.Add(iteminfo);
            }
        }

        // 从AllItems中删除需要删除的元素
        foreach (PlayerItemInfo itemToRemove in itemsToRemove)
        {
            tempInfo.Remove(itemToRemove);
        }

        // 同时从Materials中删除满足条件的元素
        GameDataMgr.GetInstance().PlayerDataInfo.Materials.RemoveAll(item => item.number == 0);


        switch (type)
        {
            case E_Bag_Type2.All:
                tempInfo = GameDataMgr.GetInstance().PlayerDataInfo.AllItems;
                break;
            case E_Bag_Type2.ActiveSkillBook:
                tempInfo = GameDataMgr.GetInstance().PlayerDataInfo.activeSkillBooks;
                break;
            case E_Bag_Type2.PassiveSkillBook:
                tempInfo = GameDataMgr.GetInstance().PlayerDataInfo.passiveSkillBooks;
                break;
            case E_Bag_Type2.Material:
                tempInfo = GameDataMgr.GetInstance().PlayerDataInfo.Materials;
                break;
        }

        //更新内容
        //先删除之前的格子
        for (int i = 0; i < list.Count; ++i)
            Destroy(list[i].gameObject);
        list.Clear();
        //再更新现在的格子
        //动态创建ItemCell预设体，把它存入List，方便下一次更新时候删除
        for (int i = 0; i < tempInfo.Count; ++i)
        {
            //实例化格子预设体，并得到它的脚本
            UI_ItemCellInConfigTroop cell = ResourceManager.GetInstance().Load<GameObject>("UI/ItemCellInConfigTroop").GetComponent<UI_ItemCellInConfigTroop>();
            //设置父对象为content
            //cell.transform.parent = content;
            cell.transform.SetParent(content);
            //初始化数据，调用Cell脚本上的初始化函数
            cell.InitInfo(tempInfo[i]);
            //把cell存进list
            list.Add(cell);
        }
    }

}
