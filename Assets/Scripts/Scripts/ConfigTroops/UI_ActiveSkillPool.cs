using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ActiveSkillPool : BasePanel
{
    public Transform content;
    //存储列表用于切换页签时清空
    private List<UI_ActiveSkillCell> list = new List<UI_ActiveSkillCell>();
    private General currentGeneral;


    public override void ShowMe()
    {
        base.ShowMe();
        OnToggleChanged(GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral);
        //每当切换Toggle就刷新武将拥有的被动技能栏位
        EventCenter.GetInstance().AddEventListener<General>("ToggleChanged", OnToggleChanged);
    }

    public override void HideMe()
    {
        base.HideMe();
        EventCenter.GetInstance().RemoveEventListener<General>("ToggleChanged", OnToggleChanged);
    }

    private void OnToggleChanged(General selectedGeneral)
    {
        currentGeneral = selectedGeneral;
        ShowGeneralOwnedActiveSkillCells();
    }

    /// <summary>
    /// 在被动技能池中显示所有当前武将拥有的被动技能
    /// </summary>
    private void ShowGeneralOwnedActiveSkillCells()
    {
        List<string> tempInfo = currentGeneral.PossessedActiveSkills;

        // 更新内容
        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i] != null && list[i].gameObject != null)
            {
                Destroy(list[i].gameObject);
            }
        }
        list.Clear();

        for (int i = 0; i < tempInfo.Count; ++i)
        {
            UI_ActiveSkillCell cell = ResourceManager.GetInstance().Load<GameObject>("UI/ActiveSkillCell").GetComponent<UI_ActiveSkillCell>();
            cell.InitInfo(tempInfo[i]);
            cell.type = E_ActiveSkillCell_Type.ActiveSkillPools;
            cell.transform.SetParent(content, false);
            list.Add(cell);
            cell.index = 0;
        };
    }

}
