using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ȷ�����
/// </summary>
public class UI_BuyConfirmPanel : BasePanel
{
    //�����¼�
    public event Action<bool> onConfirm;

    // Start is called before the first frame update
    void Start()
    {
        GetControl<Button>("ButtonYes").onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_Buy", false);
            //ȷ�Ϲ��򣬴���true��
            ConfirmBuy(true);
        });

        GetControl<Button>("ButtonNo").onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            //ȡ�����򣬴���false
            ConfirmBuy(false);
        });

    }

    private void ConfirmBuy(bool confirmed)
    {
        // �����¼������ί�У���ȷ�Ͻ�����ݸ��ص�����
        onConfirm?.Invoke(confirmed);
        //������Ϻ��������
        UIManager.GetInstance().HidePanel("ConfirmPanel");
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
