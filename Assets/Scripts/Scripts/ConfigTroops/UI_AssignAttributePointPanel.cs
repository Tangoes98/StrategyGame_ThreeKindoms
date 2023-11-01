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
    //����string�洢MainPanel����Ľ���key
    public string generalKey;
    //��¼��������
    public string attributeType;
    //ÿ�ε�����Ӷ��ٵ����Ե�
    private int number;
    //��¼�ѷ���ĸ�������
    private int AlreadyAssignedPointCountS;
    private int AlreadyAssignedPointCountL;
    private int AlreadyAssignedPointCountW;
    //��¼����ʱ�佫ԭ�еĸ�������
    private int GeneralOriginalAttributeS;
    private int GeneralOriginalAttributeL;
    private int GeneralOriginalAttributeW;

    //��¼�ý���δ��������Ե���
    private int UnassignedAttributePoint;
    private int maxAttributePoint;

    //��¼�Ƿ�ȷ�Ϸ�����
    private bool isConfirm;

    // Start is called before the first frame update
    void Start()
    {
        isConfirm = false;
        //��ʼ�ѷ������Ե�Ϊ0
        AlreadyAssignedPointCountS = 0;
        AlreadyAssignedPointCountL = 0;
        AlreadyAssignedPointCountW = 0;
        //������ԭ��������
        GeneralOriginalAttributeS = GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].Strength;
        GeneralOriginalAttributeL = GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].LeaderShip;
        GeneralOriginalAttributeW = GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].Wisdom;
        //�õ�������ť�Ŀؼ�
        GetControl<Button>("ButtonIncreaseS").onClick.AddListener(AssignAttributePointIncreaseStrength);
        GetControl<Button>("ButtonIncreaseL").onClick.AddListener(AssignAttributePointIncreaseLeaderShip);
        GetControl<Button>("ButtonIncreaseW").onClick.AddListener(AssignAttributePointIncreaseWisdom);
        GetControl<Button>("ButtonDecreaseS").onClick.AddListener(AssignAttributePointDecreaseStrength);
        GetControl<Button>("ButtonDecreaseL").onClick.AddListener(AssignAttributePointDecreaseLeaderShip);
        GetControl<Button>("ButtonDecreaseW").onClick.AddListener(AssignAttributePointDecreaseWisdom);

        GetControl<Button>("ButtonConfirm").onClick.AddListener(ConfirmChange);
        GetControl<Button>("ButtonWithdraw").onClick.AddListener(WithDraw);

        //�����ʱ��ʾ������ʾͼ��
        Transform Transform1 = this.transform.Find("ImageLevelUpTip");
        Image imageComponent1 = Transform1.GetComponent<Image>();
        imageComponent1.enabled = true;
    }

    public override void ShowMe()
    {
        base.ShowMe();
        //�õ����������ȼ�����£�����δ�������Ե�����
        maxAttributePoint = GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].MaxAttributePoints;
        //�����佫����
        EventCenter.GetInstance().AddEventListener<string,int>("GeneralLevelUp", setMaxAttributePoint);
    }

    public override void HideMe()
    {
        base.HideMe();
        //���û��ȷ�Ϸ��䣬���������ʱ�������з���
        if (!isConfirm)
        {
            WithDraw();
        }
        //�رռ���
        EventCenter.GetInstance().RemoveEventListener<string, int>("GeneralLevelUp", setMaxAttributePoint);
    }

    private void Update()
    {
        //��ʱ�õ���ǰ�ɷ�������Ե�
        UnassignedAttributePoint = GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].UnassignedAttributePoints;

        //��ʱ����ʣ��ɷ������Ե����ʾ
        UpdatePointTip();
    }

    /// <summary>
    /// ����ʱ�����������δ�������Ե�ĺ���
    /// </summary>
    /// <param name="genenralKey"></param>
    /// <param name="Exp"></param>
    public void setMaxAttributePoint(string genenralKey,int Exp)
    {
        //ÿ�����������δ�������Ե�����+3
        maxAttributePoint += 3;
        //���ı�����ֵ�洢
        GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].MaxAttributePoints = maxAttributePoint;
    }


    /// <summary>
    /// ���¿ɷ������Ե���ʾͼ��ĺ���
    /// </summary>
    public void UpdatePointTip()
    {
        //����ɷ������Ϊ0
        if (UnassignedAttributePoint == 0)
        {
            //����ͼ��
            Transform Transform = this.transform.Find("ImageUnAssignedPoint");
            Image imageComponent = Transform.GetComponent<Image>();
            TMP_Text textComponent = transform.Find("ImageUnAssignedPoint").GetComponentInChildren<TMP_Text>();
            imageComponent.enabled = false;
            textComponent.enabled = false;
        }
        else
        {
            //��ʾͼ��
            Transform Transform = this.transform.Find("ImageUnAssignedPoint");
            Image imageComponent = Transform.GetComponent<Image>();
            TMP_Text textComponent = transform.Find("ImageUnAssignedPoint").GetComponentInChildren<TMP_Text>();
            imageComponent.enabled = true;
            textComponent.enabled = true;
        }
    }

    /// <summary>
    /// ��ҵ��ȷ�Ϸ������Ե��ִ�еĺ���
    /// </summary>
    public void ConfirmChange()
    {
        if(UnassignedAttributePoint == 0)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_pc01-ItemSelect", false);
            //�ر����ʱ������ʾ������ʾͼ��
            Transform Transform1 = this.transform.Find("ImageLevelUpTip");
            Image imageComponent1 = Transform1.GetComponent<Image>();
            imageComponent1.enabled = false;

            //�����¼���MainPanel��ȷ�Ϸ������
            EventCenter.GetInstance().EventTrigger("ChangeConfirmed");
            //�����¼���DataMgr��ȷ�϶������Ա仯
            EventCenter.GetInstance().EventTrigger("AttributeChangeConfirmed", generalKey, AlreadyAssignedPointCountS, AlreadyAssignedPointCountL, AlreadyAssignedPointCountW);
            //�Ѿ�ȷ�ϣ�����Ϊtrue
            isConfirm = true;
            //ȷ�Ϻ󣬻�ԭ���佫�����ɷ������
            GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].MaxAttributePoints = 3;
            //�������
            UIManager.GetInstance().HidePanel("AssignAttributePointPanel");
        }
        else
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            //δ�����Լ����������
            Debug.Log("���л�δ��������Ե㣬��������ȷ��");
        }

    }

    /// <summary>
    /// �������Ե����ĺ��������ݷ����ڸ������ϵĴ���������ִ����Ӧ�����ļ�ȥ����ĺ�������ɳ���
    /// </summary>
    public void WithDraw()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
        //��������µĴ���
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
       

        //�����ͳ�ʵĴ���
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
   

        //�������ı�Ĵ���
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
    /// ������
    /// </summary>
    public void AssignAttributePointIncreaseStrength()
    {
        //�ڵ�һ�����Ϸ���ĵ���С��δ����ĵ�����δ����ĵ�������0
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
    /// ��ͳ��
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
    /// ����ı
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
    /// ������
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
    /// ��ͳ��
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
    /// ����ı
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
