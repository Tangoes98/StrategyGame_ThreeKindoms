using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_ActiveSkillChute : BasePanel
{
    public Transform content;
    private List<UI_ActiveSkillCell> list = new List<UI_ActiveSkillCell>();
    private General currentGeneral;
    private Dictionary<int, string> currentOccupiedSkill;
    private bool IsFirstRun;

    public override void ShowMe()
    {
        base.ShowMe();
        IsFirstRun = true;
        currentOccupiedSkill = new Dictionary<int, string>();

        OnToggleChanged(GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral);
        //ÿ���л�Toggle��ˢ���佫ӵ�е�����������λ
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
        ShowGeneralOwnedActiveSkillChutes();
    }

    private void ShowGeneralOwnedActiveSkillChutes()
    {

        IsFirstRun = true;
        currentOccupiedSkill.Clear();

        // ���֮ǰ���б�
        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i] != null && list[i].gameObject != null)
            {
                Destroy(list[i].gameObject);
            }
        }
        list.Clear();

        // ��󱻶����ܲ�λΪ3
        if (currentGeneral.ActiveSkillCount > 6)
        {
            currentGeneral.ActiveSkillCount = 6;
        }

        // ���ݲ�λ�������ɲ�λ
        for (int i = 0; i < currentGeneral.ActiveSkillCount; ++i)
        {
            UI_ActiveSkillCell cell = ResourceManager.GetInstance().Load<GameObject>("UI/ActiveSkillCell").GetComponent<UI_ActiveSkillCell>();
            cell.InitInfoChute();
            cell.type = E_ActiveSkillCell_Type.ActiveSkillChute;
            cell.transform.SetParent(content, false);
            list.Add(cell);
            //����˳������cell�ı��
            cell.index = i;

        }
        // �����ǰ�佫���Ѿ�װ���ı�������
        if (currentGeneral.CurrentSelectedActiveSkills.Count >= 1 && IsFirstRun)
        {
            for (int u = 0; u < currentGeneral.CurrentSelectedActiveSkills.Count; ++u)
            {
                //������װ���ļ����������¶�Ӧ�Ĳ�λ
                StartSetChute(u, currentGeneral.CurrentSelectedActiveSkills[u]);
                IsFirstRun = false;
            }
        }
    }


    /// <summary>
    /// ��������±������ܲ�λ������
    /// </summary>
    /// <param name="index"></param>
    /// <param name="currentSkill"></param>
    public void StartSetChute(int index, string currentSkill)
    {
        IsFirstRun = true;

        // ��鵱ǰ���õ�currentSkill�Ƿ������е���ͬ�������ͬ��ֱ�ӷ���
        if (currentOccupiedSkill.ContainsValue(currentSkill))
        {
            return;
        }

        // ���µ�ǰ���õ�currentSkill����䵽chuteλ��
        currentOccupiedSkill.Add(index, currentSkill);
        list[index].InitInfo(currentSkill);
    }


    public void UpdateChute(int index, string currentSkill)
    {

        //�����ռ�ݵļ��ܲ۲�������ǰ�ļ�ֵ��Ҳ����˵��Ӧindex��λ��û�м���
        if (!currentOccupiedSkill.ContainsKey(index) && !currentOccupiedSkill.ContainsValue(currentSkill))
        {
            //�����λ�÷��ü��ܲ��Ұѵ�ǰ��index��Ϊ��ֵ�洢����װ������
            list[index].InitInfo(currentSkill);
            //������װ������
            currentOccupiedSkill.Add(index, currentSkill);
            //����Ӧ�Ľ��������������
            foreach (General ganeral in GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral.Values)
            {
                //�������ID�͵�ǰ����һ��
                if (currentGeneral.GeneralID == ganeral.GeneralID)
                {
                    ganeral.CurrentSelectedActiveSkills.Add(currentSkill);
                    //��������
                    GameDataMgr.GetInstance().SavePlayerInfo();
                }
            }
        }
        //��ռ�ݵļ��ܲ۰�����ǰ�ļ�ֵ�������ݲ�һ����Ҳ����˵��Ӧ��indexλ������������
        else if (currentOccupiedSkill.ContainsKey(index) && !currentOccupiedSkill.ContainsValue(currentSkill))
        {
            //�������λ����Ϣ��ɾ����Ӧ����װ�������б�ļ���ֵ
            ClearChute(index, currentOccupiedSkill[index]);
            //�����λ�÷��ü��ܲ��Ұѵ�ǰ��index��Ϊ��ֵ�洢����װ������
            list[index].InitInfo(currentSkill);
            //������װ������
            currentOccupiedSkill.Add(index, currentSkill);
            //����Ӧ�Ľ��������������
            foreach (General ganeral in GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral.Values)
            {
                //�������ID�͵�ǰ����һ��
                if (currentGeneral.GeneralID == ganeral.GeneralID)
                {
                    ganeral.CurrentSelectedActiveSkills.Add(currentSkill);
                    //��������
                    GameDataMgr.GetInstance().SavePlayerInfo();
                }
            }
        }

        //��ռ�ݵļ��ܲ�û�м��ܵ�����λ�ð���ͬ���ļ���
        else if (!currentOccupiedSkill.ContainsKey(index) && currentOccupiedSkill.ContainsValue(currentSkill))
        {
            // ����һ����ʱ�б����ڴ洢Ҫɾ���ļ�
            List<int> keysToRemove = new List<int>();

            // �����ֵ��еļ�ֵ�ԣ��ҵ�����ָ��ֵ�ļ�����������ӵ���ʱ�б���
            foreach (var kvp in currentOccupiedSkill)
            {
                if (kvp.Value == currentSkill)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            // ������ʱ�б����ֵ���ɾ����Ӧ�ļ�ֵ��
            foreach (int key in keysToRemove)
            {
                ClearChute(key, currentSkill);
            }

            //�����λ�÷��ü��ܲ��Ұѵ�ǰ��index��Ϊ��ֵ�洢����װ������
            list[index].InitInfo(currentSkill);
            //������װ������
            currentOccupiedSkill.Add(index, currentSkill);
            //����Ӧ�Ľ��������������
            foreach (General ganeral in GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral.Values)
            {
                //�������ID�͵�ǰ����һ��
                if (currentGeneral.GeneralID == ganeral.GeneralID)
                {
                    ganeral.CurrentSelectedActiveSkills.Add(currentSkill);
                    //��������
                    GameDataMgr.GetInstance().SavePlayerInfo();

                }
            }
        }//��ֵ�����ݶ�һ����˵�����λ���Ѿ�����һ���ļ���
        else if (currentOccupiedSkill.ContainsKey(index) && currentOccupiedSkill.ContainsValue(currentSkill))
        {
            Debug.Log("�Ѵ���ͬ������");
            return;
        }
    }

    public void ClearChute(int index, string currentSkill)
    {
        // ���ָ����λ�ļ�����Ϣ
        list[index].InitInfoChute();

        currentOccupiedSkill.Remove(index);
        // ���¶�Ӧ�����ļ����б�
        foreach (General general in GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral.Values)
        {
            if (currentGeneral.GeneralID == general.GeneralID)
            {
                if (general.CurrentSelectedActiveSkills.Contains(currentSkill))
                {
                    general.CurrentSelectedActiveSkills.Remove(currentSkill);
                }
            }
        }

        // ��������
        GameDataMgr.GetInstance().SavePlayerInfo();
    }
}