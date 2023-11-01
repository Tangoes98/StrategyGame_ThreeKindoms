using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SellConfirmPanel : BasePanel
{
    //定义事件
    public event Action<bool> onConfirm;

    // Start is called before the first frame update
    void Start()
    {
        GetControl<Button>("ButtonYes").onClick.AddListener(() =>
        {
            //需替换成新的售出音效
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            //确认购买，传递true；
            ConfirmSell(true);
        });

        GetControl<Button>("ButtonNo").onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            //取消购买，传递false
            ConfirmSell(false);
        });

    }

    private void ConfirmSell(bool confirmed)
    {
        // 触发事件或调用委托，将确认结果传递给回调函数
        onConfirm?.Invoke(confirmed);
        //传递完毕后，隐藏面板
        UIManager.GetInstance().HidePanel("SellConfirmPanel");
    }

    /// <summary>
    /// 初始化文本信息
    /// </summary>
    /// <param name="content"></param>
    public void InitInfo(string content)
    {
        GetControl<TMP_Text>("TextContent").text = content;
    }
}
