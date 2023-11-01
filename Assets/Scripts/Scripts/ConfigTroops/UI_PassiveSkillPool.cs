using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PassiveSkillPool : BasePanel
{
    public Transform content;
    //�洢�б������л�ҳǩʱ���
    private List<UI_PassiveSkillCell> list = new List<UI_PassiveSkillCell>();
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
        ShowGeneralOwnedPassiveSkillCells();
    }

    /// <summary>
    /// �ڱ������ܳ�����ʾ���е�ǰ�佫ӵ�еı�������
    /// </summary>
    private void ShowGeneralOwnedPassiveSkillCells()
    {
        List<string> tempInfo = currentGeneral.PossessedPassiveSkills;

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
            UI_PassiveSkillCell cell = ResourceManager.GetInstance().Load<GameObject>("UI/PassiveSkillCell").GetComponent<UI_PassiveSkillCell>();
            cell.InitInfo(tempInfo[i]);
            cell.type = E_PassiveSkillCell_Type.PassiveSkillPools;
            cell.transform.SetParent(content, false);
            list.Add(cell);
            cell.index = 0;
        };
    }

}
