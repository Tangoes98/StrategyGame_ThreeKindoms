using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HintPanel : BasePanel
{

    // Start is called before the first frame update
    void Start()
    {
        GetControl<Button>("ButtonYes").onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            UIManager.GetInstance().HidePanel("HintPanel");
        });
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