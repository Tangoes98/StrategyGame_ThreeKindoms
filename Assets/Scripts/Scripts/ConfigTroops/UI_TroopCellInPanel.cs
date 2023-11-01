using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TroopCellInPanel : BasePanel
{
    /// <summary>
    /// 初始化Troop信息
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(Troop info)
    {
        //使用道具表中的数据
        //图标
        //通过道具ID得到道具表中的数据信息后，就可以得到对应的道具ID用的图标是什么
        GetControl<Image>("ImageTroopIcon").sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + info.TroopIcon);
        //名称
        GetControl<TMP_Text>("TextTroopName").text = info.TroopName;

    }
}
