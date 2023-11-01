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
    /// ��ʼ��Troop��Ϣ
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(int infoID,int infoNumber,int index)
    {
        NeedNumber = infoNumber;
        //���ݵ�����Ϣ�����ݣ����¸��Ӷ���
        //ͨ���õ���ҵ����б��е�id���õ�����������Ϣ
        Item ItemData = GameDataMgr.GetInstance().GetItemInfo(infoID);
        //ʹ�õ��߱��е�����
        //ͼ��
        //ͨ������ID�õ����߱��е�������Ϣ�󣬾Ϳ��Եõ���Ӧ�ĵ���ID�õ�ͼ����ʲô
        GetControl<Image>("ImageIcon").sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + ItemData.Icon);
        //����
        GetControl<TMP_Text>("TextMaterialName").text = ItemData.Name;
        
        //������������
        foreach (PlayerItemInfo playerItem in GameDataMgr.GetInstance().PlayerDataInfo.Materials)
        {
            //������ʵ�ID�ʹ����ID��ͬ
            if (playerItem.id == infoID)
            {
                //���ݸ���������ҵ��߿��е�ID������ҵ��߿��еĸõ���
                playerMaterial = playerItem;
                //ͨ���õ��ߵ�ID�������ӵ�еĸõ�������
                playerCurrentNumber = playerItem.number;
            }
        }

        //����
        GetControl<TMP_Text>("TextNumber").text = "������:" + playerCurrentNumber + "/������:" + NeedNumber;
        //�����ҵĳ��������ڽ����ñ��ֵ�������
        if (playerCurrentNumber >= NeedNumber)
        {
            //�����¼���ͬʱ���͸ñ�����λ������λ�ã���ҵ��߿��еĸõ��ߣ�������Ҫ�ĵ�������
            EventCenter.GetInstance().EventTrigger("PlayerHaveEnoughItem",index, playerMaterial, NeedNumber);
        }
    }
}
