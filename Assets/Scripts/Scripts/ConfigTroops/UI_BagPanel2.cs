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
    //���ڰ�content
    public Transform content;
    //�洢�б������л�ҳǩʱ���
    private List<UI_ItemCellInConfigTroop> list = new List<UI_ItemCellInConfigTroop>();
    public int currentType;

    void Start()
    {
        //ΪToggle����¼��������������ݸ���
        GetControl<Toggle>("ToggleAll").onValueChanged.AddListener(ToggleValueChange);
        GetControl<Toggle>("ToggleASB").onValueChanged.AddListener(ToggleValueChange);
        GetControl<Toggle>("TogglePSB").onValueChanged.AddListener(ToggleValueChange);
        GetControl<Toggle>("ToggleMaterial").onValueChanged.AddListener(ToggleValueChange);

        //�¼�����������ҹ���ɹ�ʱˢ��ҳ��
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
    /// ҳǩ�л�����
    /// </summary>
    /// <param name="type"></param>
    private void ChangeType(E_Bag_Type2 type)
    {
        // Ĭ��չʾ���е���
        List<PlayerItemInfo> tempInfo = GameDataMgr.GetInstance().PlayerDataInfo.AllItems;

        // ʹ����ʱ�б�洢��Ҫɾ����Ԫ��
        List<PlayerItemInfo> itemsToRemove = new List<PlayerItemInfo>();

        // �ҵ���Ҫɾ����Ԫ��
        foreach (PlayerItemInfo iteminfo in tempInfo)
        {
            if (iteminfo.number == 0)
            {
                itemsToRemove.Add(iteminfo);
            }
        }

        // ��AllItems��ɾ����Ҫɾ����Ԫ��
        foreach (PlayerItemInfo itemToRemove in itemsToRemove)
        {
            tempInfo.Remove(itemToRemove);
        }

        // ͬʱ��Materials��ɾ������������Ԫ��
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

        //��������
        //��ɾ��֮ǰ�ĸ���
        for (int i = 0; i < list.Count; ++i)
            Destroy(list[i].gameObject);
        list.Clear();
        //�ٸ������ڵĸ���
        //��̬����ItemCellԤ���壬��������List��������һ�θ���ʱ��ɾ��
        for (int i = 0; i < tempInfo.Count; ++i)
        {
            //ʵ��������Ԥ���壬���õ����Ľű�
            UI_ItemCellInConfigTroop cell = ResourceManager.GetInstance().Load<GameObject>("UI/ItemCellInConfigTroop").GetComponent<UI_ItemCellInConfigTroop>();
            //���ø�����Ϊcontent
            //cell.transform.parent = content;
            cell.transform.SetParent(content);
            //��ʼ�����ݣ�����Cell�ű��ϵĳ�ʼ������
            cell.InitInfo(tempInfo[i]);
            //��cell���list
            list.Add(cell);
        }
    }

}
