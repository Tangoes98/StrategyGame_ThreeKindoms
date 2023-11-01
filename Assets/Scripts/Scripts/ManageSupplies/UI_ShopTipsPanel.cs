using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UI_ShopTipsPanel : BasePanel
{
    //�洢������ֵ�������
    private int ItemNumberOfPlayer;

    /// <summary>
    /// ��ʼ��Tips�����Ϣ
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(Item info)
    {
        //���ݵ�����Ϣ�����ݣ����¸��Ӷ���
        //ͨ���õ����е����б��е�id���õ�����������Ϣ
        Item itemData = GameDataMgr.GetInstance().GetItemInfo(info.ID);
        //�����ӵ�е����е�����Ϣ�в��ң����е��ߵ�id���ڵ�ǰ��ʾ��tips�ĵ���id
        PlayerItemInfo playerItem = GameDataMgr.GetInstance().PlayerDataInfo.AllItems.Find(item => item.id == info.ID);
        //���û�ҵ���˵�����û�е�ǰid�ĵ���
        if(playerItem == null)
        {
            ItemNumberOfPlayer = 0;
        }
        else
        {
            ItemNumberOfPlayer = playerItem.number;
        }
        //ʹ�õ��߱��е�����
        //ͼ��
        //ͨ������ID�õ����߱��е�������Ϣ�󣬾Ϳ��Եõ���Ӧ�ĵ���ID�õ�ͼ����ʲô
        GetControl<Image>("ImageIcon").sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + itemData.Icon);
        //����
        GetControl<TMP_Text>("TextName").text = "���ƣ�" + itemData.Name;
        //�������˴���ʾ��ҵ����б��иõ��ߵ�����
        GetControl<TMP_Text>("TextNumber").text = "�ѳ���������" + ItemNumberOfPlayer.ToString();
        //�ۼ�
        GetControl<TMP_Text>("TextContent").text = itemData.Tips;
    }
    
}
