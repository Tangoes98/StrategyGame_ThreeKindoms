using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ����װ������ϸ��Ϣ���
/// </summary>
public class UI_TipsPanel : BasePanel
{
    /// <summary>
    /// ��ʼ��Tips�����Ϣ
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(PlayerItemInfo info)
    {
        //���ݵ�����Ϣ�����ݣ����¸��Ӷ���
        //ͨ���õ���ҵ����б��е�id���õ�����������Ϣ
        Item itemData = GameDataMgr.GetInstance().GetItemInfo(info.id);
        //ʹ�õ��߱��е�����
        //ͼ��
        //ͨ������ID�õ����߱��е�������Ϣ�󣬾Ϳ��Եõ���Ӧ�ĵ���ID�õ�ͼ����ʲô
        GetControl<Image>("ImageIcon").sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + itemData.Icon);
        //����
        GetControl<TMP_Text>("TextName").text = "���ƣ�" + itemData.Name;
        //�������˴���ʾ��ҵ����б��иõ��ߵ�����
        GetControl<TMP_Text>("TextNumber").text = "����������" + info.number.ToString();
        //�ۼ�
        GetControl<TMP_Text>("TextContent").text = itemData.Tips;
    }
}
