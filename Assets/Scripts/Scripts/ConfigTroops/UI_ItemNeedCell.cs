using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemNeedCell : BasePanel
{
    int playerCurrentNumber;
    int NeedNumber;
    PlayerItemInfo playerMaterial;
    /// <summary>
    /// 初始化Troop信息
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(int infoID,int infoNumber,int index)
    {
        NeedNumber = infoNumber;
        //根据道具信息的数据，更新格子对象
        //通过得到玩家道具列表中的id来得到整个道具信息
        Item ItemData = GameDataMgr.GetInstance().GetItemInfo(infoID);
        //使用道具表中的数据
        //图标
        //通过道具ID得到道具表中的数据信息后，就可以得到对应的道具ID用的图标是什么
        GetControl<Image>("ImageIcon").sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + ItemData.Icon);
        //名称
        GetControl<TMP_Text>("TextMaterialName").text = ItemData.Name;
        
        //遍历所有物资
        foreach (PlayerItemInfo playerItem in GameDataMgr.GetInstance().PlayerDataInfo.Materials)
        {
            //如果物资的ID和传入的ID相同
            if (playerItem.id == infoID)
            {
                //根据该物资在玩家道具库中的ID储存玩家道具库中的该道具
                playerMaterial = playerItem;
                //通过该道具的ID储存玩家拥有的该道具数量
                playerCurrentNumber = playerItem.number;
            }
        }

        //数量
        GetControl<TMP_Text>("TextNumber").text = "持有量:" + playerCurrentNumber + "/需求量:" + NeedNumber;
        //如果玩家的持有量大于解锁该兵种的需求量
        if (playerCurrentNumber >= NeedNumber)
        {
            //发送事件，同时发送该兵种栏位所处的位置，玩家道具库中的该道具，解锁需要的道具数量
            EventCenter.GetInstance().EventTrigger("PlayerHaveEnoughItem",index, playerMaterial, NeedNumber);
        }
    }
}
