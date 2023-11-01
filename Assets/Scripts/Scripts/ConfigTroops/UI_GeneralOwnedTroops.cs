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
    //存储列表用于切换页签时清空
    private List<UI_TroopCell> list = new List<UI_TroopCell>();
    private General currentGeneral;


    public override void ShowMe()
    {
        base.ShowMe();

        //每当切换Toggle就刷新武将拥有的所拥有兵种
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
    /// 将字典转换为list存储
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
            UI_TroopCell cell = ResourceManager.GetInstance().Load<GameObject>("UI/TroopCell").GetComponent<UI_TroopCell>();
            cell.InitInfo(tempInfo[i]);
            cell.transform.SetParent(content, false);
            list.Add(cell);

        };
    }
}
