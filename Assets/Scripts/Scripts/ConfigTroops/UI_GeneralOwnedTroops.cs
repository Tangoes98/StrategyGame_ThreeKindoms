using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
public class UI_GeneralOwnedTroops : BasePanel
{

    public Transform content;
    //�洢�б������л�ҳǩʱ���
    private List<UI_TroopCell> list = new List<UI_TroopCell>();
    private General currentGeneral;


    public override void ShowMe()
    {
        base.ShowMe();

        //ÿ���л�Toggle��ˢ���佫ӵ�е���ӵ�б���
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
        ShowGeneralOwnedTroops();
    }

    /// <summary>
    /// ���ֵ�ת��Ϊlist�洢
    /// </summary>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public List<Troop> ConvertDictionaryToList(Dictionary<string, Troop> dictionary)
    {
        List<Troop> list = new List<Troop>(dictionary.Values);
        return list;
    }

    private void ShowGeneralOwnedTroops()
    {
        //Debug.Log(general.GeneralOwnedTroop.Count);
        List<Troop> tempInfo = ConvertDictionaryToList(currentGeneral.GeneralOwnedTroop);

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
            UI_TroopCell cell = ResourceManager.GetInstance().Load<GameObject>("UI/TroopCell").GetComponent<UI_TroopCell>();
            cell.InitInfo(tempInfo[i]);
            cell.transform.SetParent(content, false);
            list.Add(cell);

        };
    }
}
