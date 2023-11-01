using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class UI_PassiveSkillChute : BasePanel
{

    public Transform content;
    //存储列表用于切换页签时清空
    private List<UI_PassiveSkillCell> list = new List<UI_PassiveSkillCell>();
    private General currentGeneral;
    //检测是否有重复的技能占据了两个槽位
    private Dictionary<int, string> currentOccupiedSkill;
    private bool IsFirstRun;


    public override void ShowMe()
    {
        base.ShowMe();
        IsFirstRun = true;
        currentOccupiedSkill = new Dictionary<int, string>();

        OnToggleChanged(GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral);
        //每当切换Toggle就刷新武将拥有的被动技能栏位
        EventCenter.GetInstance().AddEventListener<General>("ToggleChanged", OnToggleChanged);

        currentGeneral = GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral;
    }

    public override void HideMe()
    {
        base.HideMe();
        EventCenter.GetInstance().RemoveEventListener<General>("ToggleChanged", OnToggleChanged);
        currentOccupiedSkill.Clear();
    }

    private void OnToggleChanged(General selectedGeneral)
    {
        currentGeneral = selectedGeneral;
        ShowGeneralOwnedPassiveSkillChutes();
    }

    /// <summary>
    /// 显示技能槽位
    /// </summary>
    private void ShowGeneralOwnedPassiveSkillChutes()
    {

        IsFirstRun = true;
        currentOccupiedSkill.Clear();


        // 更新内容时清除之前的列表
        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i] != null && list[i].gameObject != null)
            {
                Destroy(list[i].gameObject);
            }
        }
        list.Clear();

        //最大被动技能槽位为3
        if(currentGeneral.PassiveSkillCount > 3)
        {
            currentGeneral.PassiveSkillCount = 3;
        }


        //根据槽位数量生成槽位
        for (int i = 0; i < currentGeneral.PassiveSkillCount; ++i)
        {
            UI_PassiveSkillCell cell = ResourceManager.GetInstance().Load<GameObject>("UI/PassiveSkillCell").GetComponent<UI_PassiveSkillCell>();
            cell.InitInfoChute();
            cell.type = E_PassiveSkillCell_Type.PassiveSkillChute;
            cell.transform.SetParent(content, false);
            list.Add(cell);
            //按照顺序设置cell的编号
            cell.index = i; 
        }
        //如果当前武将有已经装备的被动技能
        if (currentGeneral.CurrentSelectedPassiveSkills.Count >= 1 && IsFirstRun)
        {

            for (int u = 0; u < currentGeneral.CurrentSelectedPassiveSkills.Count; ++u)
            {
                //根据已装备的技能数量更新对应的槽位
                StartSetChute(u, currentGeneral.CurrentSelectedPassiveSkills[u]);
                IsFirstRun = false;
            }
        }
    }

    /// <summary>
    /// 读档后更新被动技能槽位的内容
    /// </summary>
    /// <param name="index"></param>
    /// <param name="currentSkill"></param>
    public void StartSetChute(int index, string currentSkill)
    {
        // 检查当前放置的currentSkill是否与已有的相同，如果相同则直接返回
        if (currentOccupiedSkill.ContainsValue(currentSkill))
        {
            return;
        }

        // 更新当前放置的currentSkill并填充到chute位置
        currentOccupiedSkill.Add(index, currentSkill);
        list[index].InitInfo(currentSkill);
    }



    public void UpdateChute(int index, string currentSkill)
    {

        //如果已占据的技能槽不包含当前的键值，也就是说对应index的位置没有技能
        if (!currentOccupiedSkill.ContainsKey(index) && !currentOccupiedSkill.ContainsValue(currentSkill))
        {
            //在这个位置放置技能并且把当前的index作为键值存储到已装备技能
            list[index].InitInfo(currentSkill);
            //加入已装备技能
            currentOccupiedSkill.Add(index, currentSkill);
            //给对应的将军增加这个技能
            foreach (General ganeral in GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral.Values)
            {
                //如果将军ID和当前将军一致
                if (currentGeneral.GeneralID == ganeral.GeneralID)
                {
                    ganeral.CurrentSelectedPassiveSkills.Add(currentSkill);
                    //保存数据
                    GameDataMgr.GetInstance().SavePlayerInfo();
                }
            }
        }
        //已占据的技能槽包含当前的键值，但内容不一样，也就是说对应的index位置是其它技能
        else if (currentOccupiedSkill.ContainsKey(index) && !currentOccupiedSkill.ContainsValue(currentSkill))
        {
            //清除该栏位的信息，删除对应的已装备技能列表的键和值
            ClearChute(index, currentOccupiedSkill[index]);
            //在这个位置放置技能并且把当前的index作为键值存储到已装备技能
            list[index].InitInfo(currentSkill);
            //加入已装备技能
            currentOccupiedSkill.Add(index, currentSkill);
            //给对应的将军增加这个技能
            foreach (General ganeral in GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral.Values)
            {
                //如果将军ID和当前将军一致
                if (currentGeneral.GeneralID == ganeral.GeneralID)
                {
                    ganeral.CurrentSelectedPassiveSkills.Add(currentSkill);
                    //保存数据
                    GameDataMgr.GetInstance().SavePlayerInfo();
                }
            }
        }

        //已占据的技能槽没有技能但其它位置包含同样的技能
        else if (!currentOccupiedSkill.ContainsKey(index) && currentOccupiedSkill.ContainsValue(currentSkill))
        {
            // 创建一个临时列表用于存储要删除的键
            List<int> keysToRemove = new List<int>();

            // 遍历字典中的键值对，找到包含指定值的键，并将其添加到临时列表中
            foreach (var kvp in currentOccupiedSkill)
            {
                if (kvp.Value == currentSkill)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            // 遍历临时列表，从字典中删除对应的键值对
            foreach (int key in keysToRemove)
            {
                ClearChute(key, currentSkill);
            }

            //在这个位置放置技能并且把当前的index作为键值存储到已装备技能
            list[index].InitInfo(currentSkill);
            //加入已装备技能
            currentOccupiedSkill.Add(index, currentSkill);
            //给对应的将军增加这个技能
            foreach (General ganeral in GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral.Values)
            {
                //如果将军ID和当前将军一致
                if (currentGeneral.GeneralID == ganeral.GeneralID)
                {
                    ganeral.CurrentSelectedPassiveSkills.Add(currentSkill);
                    //保存数据
                    GameDataMgr.GetInstance().SavePlayerInfo();

                }
            }
        }//键值和内容都一样，说明这个位置已经存在一样的技能
        else if (currentOccupiedSkill.ContainsKey(index) && currentOccupiedSkill.ContainsValue(currentSkill))
        {
            Debug.Log("已存在同样技能");
            return;
        }
    }

    public void ClearChute(int index, string currentSkill)
    {
        // 清除指定槽位的技能信息
        list[index].InitInfoChute();

        currentOccupiedSkill.Remove(index);
      
        // 更新对应将军的技能列表
        foreach (General general in GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral.Values)
        {
            if (currentGeneral.GeneralID == general.GeneralID)
            {
                if (general.CurrentSelectedPassiveSkills.Contains(currentSkill))
                {
                    general.CurrentSelectedPassiveSkills.Remove(currentSkill);
                }
            }
        }
        // 保存数据
        GameDataMgr.GetInstance().SavePlayerInfo();
    }
}

