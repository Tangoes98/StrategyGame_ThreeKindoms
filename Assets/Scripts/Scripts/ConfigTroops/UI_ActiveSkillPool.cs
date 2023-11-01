using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ActiveSkillPool : BasePanel
{
    public Transform content;
    //�洢�б������л�ҳǩʱ���
    private List<UI_ActiveSkillCell> list = new List<UI_ActiveSkillCell>();
    private General currentGeneral;


    public override void ShowMe()
    {
        base.ShowMe();
        OnToggleChanged(GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral);
        //ÿ���л�Toggle��ˢ���佫ӵ�еı���������λ
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
    /// �ڱ������ܳ�����ʾ���е�ǰ�佫ӵ�еı�������
    /// </summary>
    private void ShowGeneralOwnedActiveSkillCells()
    {
        List<string> tempInfo = currentGeneral.PossessedActiveSkills;

        // ��������
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
