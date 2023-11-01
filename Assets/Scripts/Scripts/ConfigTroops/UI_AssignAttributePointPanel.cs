using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class UI_AssignAttributePointPanel : BasePanel
{
    //申明string存储MainPanel传入的将军key
    public string generalKey;
    //记录属性类型
    public string attributeType;
    //每次点击增加多少点属性点
    private int number;
    //记录已分配的各种属性
    private int AlreadyAssignedPointCountS;
    private int AlreadyAssignedPointCountL;
    private int AlreadyAssignedPointCountW;
    //记录升级时武将原有的各自属性
    private int GeneralOriginalAttributeS;
    private int GeneralOriginalAttributeL;
    private int GeneralOriginalAttributeW;

    //记录该将军未分配的属性点数
    private int UnassignedAttributePoint;
    private int maxAttributePoint;

    //记录是否确认分配了
    private bool isConfirm;

    // Start is called before the first frame update
    void Start()
    {
        isConfirm = false;
        //起始已分配属性点为0
        AlreadyAssignedPointCountS = 0;
        AlreadyAssignedPointCountL = 0;
        AlreadyAssignedPointCountW = 0;
        //获得玩家原本的属性
        GeneralOriginalAttributeS = GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].Strength;
        GeneralOriginalAttributeL = GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].LeaderShip;
        GeneralOriginalAttributeW = GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].Wisdom;
        //得到各个按钮的控件
        GetControl<Button>("ButtonIncreaseS").onClick.AddListener(AssignAttributePointIncreaseStrength);
        GetControl<Button>("ButtonIncreaseL").onClick.AddListener(AssignAttributePointIncreaseLeaderShip);
        GetControl<Button>("ButtonIncreaseW").onClick.AddListener(AssignAttributePointIncreaseWisdom);
        GetControl<Button>("ButtonDecreaseS").onClick.AddListener(AssignAttributePointDecreaseStrength);
        GetControl<Button>("ButtonDecreaseL").onClick.AddListener(AssignAttributePointDecreaseLeaderShip);
        GetControl<Button>("ButtonDecreaseW").onClick.AddListener(AssignAttributePointDecreaseWisdom);

        GetControl<Button>("ButtonConfirm").onClick.AddListener(ConfirmChange);
        GetControl<Button>("ButtonWithdraw").onClick.AddListener(WithDraw);

        //打开面板时显示升级提示图标
        Transform Transform1 = this.transform.Find("ImageLevelUpTip");
        Image imageComponent1 = Transform1.GetComponent<Image>();
        imageComponent1.enabled = true;
    }

    public override void ShowMe()
    {
        base.ShowMe();
        //得到连续提升等级情况下，最大的未分配属性点数量
        maxAttributePoint = GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].MaxAttributePoints;
        //监听武将升级
        EventCenter.GetInstance().AddEventListener<string,int>("GeneralLevelUp", setMaxAttributePoint);
    }

    public override void HideMe()
    {
        base.HideMe();
        //如果没有确认分配，则隐藏面板时撤回所有分配
        if (!isConfirm)
        {
            WithDraw();
        }
        //关闭监听
        EventCenter.GetInstance().RemoveEventListener<string, int>("GeneralLevelUp", setMaxAttributePoint);
    }

    private void Update()
    {
        //随时得到当前可分配的属性点
        UnassignedAttributePoint = GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].UnassignedAttributePoints;

        //随时更新剩余可分配属性点的提示
        UpdatePointTip();
    }

    /// <summary>
    /// 升级时重新设置最大未分配属性点的函数
    /// </summary>
    /// <param name="genenralKey"></param>
    /// <param name="Exp"></param>
    public void setMaxAttributePoint(string genenralKey,int Exp)
    {
        //每次升级，最大未分配属性点数量+3
        maxAttributePoint += 3;
        //将改变后的数值存储
        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].MaxAttributePoints = maxAttributePoint;
    }


    /// <summary>
    /// 更新可分配属性点提示图标的函数
    /// </summary>
    public void UpdatePointTip()
    {
        //如果可分配点数为0
        if (UnassignedAttributePoint == 0)
        {
            //隐藏图标
            Transform Transform = this.transform.Find("ImageUnAssignedPoint");
            Image imageComponent = Transform.GetComponent<Image>();
            TMP_Text textComponent = transform.Find("ImageUnAssignedPoint").GetComponentInChildren<TMP_Text>();
            imageComponent.enabled = false;
            textComponent.enabled = false;
        }
        else
        {
            //显示图标
            Transform Transform = this.transform.Find("ImageUnAssignedPoint");
            Image imageComponent = Transform.GetComponent<Image>();
            TMP_Text textComponent = transform.Find("ImageUnAssignedPoint").GetComponentInChildren<TMP_Text>();
            imageComponent.enabled = true;
            textComponent.enabled = true;
        }
    }

    /// <summary>
    /// 玩家点击确认分配属性点后执行的函数
    /// </summary>
    public void ConfirmChange()
    {
        if(UnassignedAttributePoint == 0)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_pc01-ItemSelect", false);
            //关闭面板时隐藏显示升级提示图标
            Transform Transform1 = this.transform.Find("ImageLevelUpTip");
            Image imageComponent1 = Transform1.GetComponent<Image>();
            imageComponent1.enabled = false;

            //发送事件给MainPanel，确认分配完成
            EventCenter.GetInstance().EventTrigger("ChangeConfirmed");
            //发送事件给DataMgr，确认二级属性变化
            EventCenter.GetInstance().EventTrigger("AttributeChangeConfirmed", generalKey, AlreadyAssignedPointCountS, AlreadyAssignedPointCountL, AlreadyAssignedPointCountW);
            //已经确认，设置为true
            isConfirm = true;
            //确认后，还原该武将的最大可分配点数
            GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].MaxAttributePoints = 3;
            //隐藏面板
            UIManager.GetInstance().HidePanel("AssignAttributePointPanel");
        }
        else
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            //未来可以加入提升面板
            Debug.Log("您有还未分配的属性点，请分配后再确认");
        }

    }

    /// <summary>
    /// 撤回属性点分配的函数，根据分配在该属性上的次数，依次执行相应次数的减去分配的函数，达成撤回
    /// </summary>
    public void WithDraw()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
        //分配给武勇的次数
        if (AlreadyAssignedPointCountS > 0)
        {
            for (int i = 0; i < AlreadyAssignedPointCountS + maxAttributePoint-1; i++)
            {
                AssignAttributePointDecreaseStrength();
            }
        }
        else if(AlreadyAssignedPointCountS < 0)
        {
            for (int i = 0; i < Math.Abs(AlreadyAssignedPointCountS) + maxAttributePoint-1; i++)
            {
                AssignAttributePointIncreaseStrength();
            }
        }
       

        //分配给统率的次数
        if (AlreadyAssignedPointCountL > 0)
        {
            for (int i = 0; i < AlreadyAssignedPointCountL + maxAttributePoint-1; i++)
            {
                AssignAttributePointDecreaseLeaderShip();
            }
        }
        else if (AlreadyAssignedPointCountL < 0)
        {
            for (int i = 0; i < Math.Abs(AlreadyAssignedPointCountL) + maxAttributePoint-1; i++)
            {
                AssignAttributePointIncreaseLeaderShip();
            }
        }
   

        //分配给智谋的次数
        if (AlreadyAssignedPointCountW > 0)
        {
            for (int i = 0; i < AlreadyAssignedPointCountW + maxAttributePoint-1; i++)
            {
                AssignAttributePointDecreaseWisdom();
            }
        }
        else if (AlreadyAssignedPointCountW < 0)
        {
            for (int i = 0; i < Math.Abs(AlreadyAssignedPointCountW) + maxAttributePoint-1; i++)
            {
                AssignAttributePointIncreaseWisdom();
            }
        }

    }

    /// <summary>
    /// 加武勇
    /// </summary>
    public void AssignAttributePointIncreaseStrength()
    {
        //在单一属性上分配的点数小于未分配的点数，未分配的点数大于0
        if (UnassignedAttributePoint <= maxAttributePoint - AlreadyAssignedPointCountS && UnassignedAttributePoint > 0)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            Debug.Log("2");
            number = 1;
            AlreadyAssignedPointCountS += 1;
            attributeType = "Strength";
            EventCenter.GetInstance().EventTrigger("AssignAttributePoint", generalKey, attributeType, number);
            //GameDataMgr.GetInstance().PlayerDataInfo.AssignAttributePoint(generalKey, attributeType, number);
        }
    }

    /// <summary>
    /// 加统率
    /// </summary>
    public void AssignAttributePointIncreaseLeaderShip()
    {
        if (UnassignedAttributePoint <= maxAttributePoint - AlreadyAssignedPointCountL && UnassignedAttributePoint > 0)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            number = 1;
            AlreadyAssignedPointCountL += 1;
            attributeType = "LeaderShip";

            EventCenter.GetInstance().EventTrigger("AssignAttributePoint", generalKey, attributeType, number);
        }
    }

    /// <summary>
    /// 加智谋
    /// </summary>
    public void AssignAttributePointIncreaseWisdom()
    {
        if (UnassignedAttributePoint <= maxAttributePoint - AlreadyAssignedPointCountW && UnassignedAttributePoint > 0)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            number = 1;
            AlreadyAssignedPointCountW += 1;
            attributeType = "Wisdom";

            EventCenter.GetInstance().EventTrigger("AssignAttributePoint", generalKey, attributeType, number);
        }
    }

    /// <summary>
    /// 减武勇
    /// </summary>
    public void AssignAttributePointDecreaseStrength()
    {
        if (AlreadyAssignedPointCountS > 0 &&  GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].Strength >= GeneralOriginalAttributeS)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            number = -1;
            AlreadyAssignedPointCountS -= 1;
            attributeType = "Strength";

            EventCenter.GetInstance().EventTrigger("AssignAttributePoint", generalKey, attributeType, number);
        }
    }

    /// <summary>
    /// 减统率
    /// </summary>
    public void AssignAttributePointDecreaseLeaderShip()
    {
        if (AlreadyAssignedPointCountL > 0 && GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].LeaderShip >= GeneralOriginalAttributeL)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            number = -1;
            AlreadyAssignedPointCountL -= 1;
            attributeType = "LeaderShip";

            EventCenter.GetInstance().EventTrigger("AssignAttributePoint", generalKey, attributeType, number);
        }
    }

    /// <summary>
    /// 减智谋
    /// </summary>
    public void AssignAttributePointDecreaseWisdom()
    {
        if (AlreadyAssignedPointCountW > 0 && GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].Wisdom >= GeneralOriginalAttributeW)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            number = -1;
            AlreadyAssignedPointCountW -= 1;
            attributeType = "Wisdom";

            EventCenter.GetInstance().EventTrigger("AssignAttributePoint", generalKey, attributeType, number);
        }
    }
}
