using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public enum E_ShopType_Type
{
    All,
    ActiveSkillBook,
    PassiveSkillBook,
    Material
}
public class UI_ShopPanel : BasePanel
{

    public Transform content;
    //存储列表用于切换页签时清空
    private List<UI_ShopItemCell> list = new List<UI_ShopItemCell>();
    public int currentType;

    void Start()
    {
        //为Toggle添加事件监听。触发数据更新
        GetControl<Toggle>("ToggleAll").onValueChanged.AddListener(ToggleValueChange);
        GetControl<Toggle>("ToggleASB").onValueChanged.AddListener(ToggleValueChange);
        GetControl<Toggle>("TogglePSB").onValueChanged.AddListener(ToggleValueChange);
        GetControl<Toggle>("ToggleMaterial").onValueChanged.AddListener(ToggleValueChange);

        //事件监听，当玩家出售成功时刷新页面
        EventCenter.GetInstance().AddEventListener<int>("ShopItemNumberChange", UpdateType);
    }

    public override void ShowMe()
    {
        base.ShowMe();
        //默认显示全部
        ChangeType(E_Bag_Type.All);
    }
    public override void HideMe()
    {
        base.HideMe();
        EventCenter.GetInstance().RemoveEventListener<int>("ShopItemNumberChange", UpdateType);
    }
    /// <summary>
    /// 页签切换
    /// </summary>
    /// <param name="value"></param>
    private void ToggleValueChange(bool value)
    {
        if (GetControl<Toggle>("ToggleAll").isOn)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_ignition04_SwitchToggle", false);
            ChangeType(E_Bag_Type.All);
            currentType = 0;
        }
        else if (GetControl<Toggle>("ToggleASB").isOn)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_ignition04_SwitchToggle", false);
            ChangeType(E_Bag_Type.ActiveSkillBook);
            currentType = 1;
        }
        else if (GetControl<Toggle>("TogglePSB").isOn)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_ignition04_SwitchToggle", false);
            ChangeType(E_Bag_Type.PassiveSkillBook);
            currentType = 2;
        }
        else if (GetControl<Toggle>("ToggleMaterial").isOn)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_ignition04_SwitchToggle", false);
            ChangeType(E_Bag_Type.Material);
            currentType = 3;
        }
    }

    public void UpdateType(int currentType)
    {
        if (currentType == 0)
            ChangeType(E_Bag_Type.All);
        else if (currentType == 1)
            ChangeType(E_Bag_Type.ActiveSkillBook);
        else if (currentType == 2)
            ChangeType(E_Bag_Type.PassiveSkillBook);
        else if (currentType == 3)
            ChangeType(E_Bag_Type.Material);
    }

    /// <summary>
    /// 将字典转换为list存储
    /// </summary>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public List<Item> ConvertDictionaryToList(Dictionary<int, Item> dictionary)
    {
        List<Item> list = new List<Item>(dictionary.Values);
        return list;
    }
    /// <summary>
    /// 遍历list把数据存在新的list里的方法
    /// </summary>
    /// <param name="originalList"></param>
    /// <param name="targetValue"></param>
    /// <returns></returns>
    public List<Item> FilterList(List<Item> originalList, string targetValue)
    {
        List<Item> filteredList = new List<Item>();
        foreach (Item item in originalList)
        {
            if (item.Type == targetValue)
            {
                filteredList.Add(item);
            }
        }
        return filteredList;
    }

    /// <summary>
    /// 页签切换显示不同的道具
    /// </summary>
    /// <param name="type"></param>
    private void ChangeType(E_Bag_Type type)
    {
        List<Item> AllItems = ConvertDictionaryToList(GameDataMgr.GetInstance().itemInfoDic);
        List<Item> tempInfo = new List<Item>();

        int currentChapter = GameDataMgr.GetInstance().PlayerDataInfo.currentChapter;
        switch (type)
        {
            case E_Bag_Type.All:
                tempInfo = AllItems.Where(item => item.OpenChapter <= currentChapter).ToList();
                break;
            case E_Bag_Type.ActiveSkillBook:
                tempInfo = FilterList(AllItems, "ActiveSkillBook").Where(item => item.OpenChapter <= currentChapter).ToList();
                break;
            case E_Bag_Type.PassiveSkillBook:
                tempInfo = FilterList(AllItems, "PassiveSkillBook").Where(item => item.OpenChapter <= currentChapter).ToList();
                break;
            case E_Bag_Type.Material:
                tempInfo = FilterList(AllItems, "Material").Where(item => item.OpenChapter <= currentChapter).ToList();
                break;
        }


        // 更新内容
        for (int i = 0; i < list.Count; ++i)
        {
            Destroy(list[i].gameObject);
        }
        list.Clear();

        for (int i = 0; i < tempInfo.Count; ++i)
        {
            UI_ShopItemCell cell = ResourceManager.GetInstance().Load<GameObject>("UI/ShopItemCell").GetComponent<UI_ShopItemCell>();
            cell.InitInfo(tempInfo[i]);
            cell.transform.SetParent(content, false);
            list.Add(cell);
       
        }
    }
}
