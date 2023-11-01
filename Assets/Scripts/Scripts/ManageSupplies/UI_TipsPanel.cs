using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 道具装备的详细信息面板
/// </summary>
public class UI_TipsPanel : BasePanel
{
    /// <summary>
    /// 初始化Tips面板信息
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(PlayerItemInfo info)
    {
        //根据道具信息的数据，更新格子对象
        //通过得到玩家道具列表中的id来得到整个道具信息
        Item itemData = GameDataMgr.GetInstance().GetItemInfo(info.id);
        //使用道具表中的数据
        //图标
        //通过道具ID得到道具表中的数据信息后，就可以得到对应的道具ID用的图标是什么
        GetControl<Image>("ImageIcon").sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + itemData.Icon);
        //名称
        GetControl<TMP_Text>("TextName").text = "名称：" + itemData.Name;
        //数量，此处显示玩家道具列表中该道具的数量
        GetControl<TMP_Text>("TextNumber").text = "持有数量：" + info.number.ToString();
        //售价
        GetControl<TMP_Text>("TextContent").text = itemData.Tips;
    }
}
