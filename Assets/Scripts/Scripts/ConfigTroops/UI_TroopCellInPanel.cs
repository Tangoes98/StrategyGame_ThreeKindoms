using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TroopCellInPanel : BasePanel
{
    /// <summary>
    /// ��ʼ��Troop��Ϣ
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(Troop info)
    {
        //ʹ�õ��߱��е�����
        //ͼ��
        //ͨ������ID�õ����߱��е�������Ϣ�󣬾Ϳ��Եõ���Ӧ�ĵ���ID�õ�ͼ����ʲô
        GetControl<Image>("ImageTroopIcon").sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + info.TroopIcon);
        //����
        GetControl<TMP_Text>("TextTroopName").text = info.TroopName;

    }
}
