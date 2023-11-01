using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SellConfirmPanel : BasePanel
{
    //�����¼�
    public event Action<bool> onConfirm;

    // Start is called before the first frame update
    void Start()
    {
        GetControl<Button>("ButtonYes").onClick.AddListener(() =>
        {
            //���滻���µ��۳���Ч
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            //ȷ�Ϲ��򣬴���true��
            ConfirmSell(true);
        });

        GetControl<Button>("ButtonNo").onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            //ȡ�����򣬴���false
            ConfirmSell(false);
        });

    }

    private void ConfirmSell(bool confirmed)
    {
        // �����¼������ί�У���ȷ�Ͻ�����ݸ��ص�����
        onConfirm?.Invoke(confirmed);
        //������Ϻ��������
        UIManager.GetInstance().HidePanel("SellConfirmPanel");
    }

    /// <summary>
    /// ��ʼ���ı���Ϣ
    /// </summary>
    /// <param name="content"></param>
    public void InitInfo(string content)
    {
        GetControl<TMP_Text>("TextContent").text = content;
    }
}
