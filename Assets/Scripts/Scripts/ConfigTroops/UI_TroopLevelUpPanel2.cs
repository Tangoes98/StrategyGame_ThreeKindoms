using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_TroopLevelUpPanel2 : BasePanel
{

    //���ڰ�content1
    public Transform content1;
    //���ڰ�content2
    public Transform content2;

    //�洢�б������л�ҳǩʱ���
    private List<UI_ItemNeedCell> list = new List<UI_ItemNeedCell>();
    private List<UI_ItemNeedCell> list2 = new List<UI_ItemNeedCell>();
    
    //�洢��������������Ϣ
    public TroopLevelUp NexttroopLevelUpInfo1;
    public TroopLevelUp NexttroopLevelUpInfo2;
    
    //���ֵ�洢����������������Ϣ
    Dictionary<int, int> materialIDNumberMap = new Dictionary<int, int>();
    Dictionary<int, int> materialIDNumberMap2 = new Dictionary<int, int>();
    
    //��int�洢������������Ʒ�������������������Ʒ�����������Ա�
    int ItemConditionNumber1;
    int ItemConditionNumber2;

    //��list�����ItemNeedCell����ĵ��ߣ���Ҫ�������
    List<PlayerItemInfo> playerMaterialInfo1;
    List<int> UnlockNeedNumber1;
    List<PlayerItemInfo> playerMaterialInfo2;
    List<int> UnlockNeedNumber2;

    //�Ѿ���ɽ������֣�����λ1������λ2�������ֺ�Ϊtrue
    private bool alreadyUnlockNextTroop1;
    private bool alreadyUnlockNextTroop2;

    //�ж��Ƿ������������������
    bool nextTroop1CanBeUnlocked;
    bool nextTroop2CanBeUnlocked;

    //����һ��������λ�������ñ���
    public UI_TroopCellInPanel SelectTroop;
    public UI_TroopCellInPanel NextTroop;
    public UI_TroopCellInPanel NextTroop2;

    //�洢�����ͱ��ֵ���Ϣ
    private General SelectedGeneral;
    private Troop SelectedTroop;
    //����Ŀ�����1
    private Troop NextTroopInfo;
    //����Ŀ�����2
    private Troop NextTroop2Info;

    //�洢�Ӽ���gameObject
    //��λ1
    private GameObject NextTroopCellInPanel1;
    private GameObject ScrollView1;
    private GameObject ButtonUnlock1;
    //��λ2
    private GameObject NextTroopCellInPanel2;
    private GameObject ScrollView2;
    private GameObject ButtonUnlock2;

    public override void ShowMe()
    {
        base.ShowMe();
/*
        //Ĭ��boolȫ��Ϊfalse
        alreadyUnlockNextTroop1 = false;
        alreadyUnlockNextTroop2 = false;
        nextTroop1CanBeUnlocked = false;
        nextTroop2CanBeUnlocked = false;*/


        //��ʼ�¼��������������±��������¼�
        EventCenter.GetInstance().AddEventListener<General, Troop>("UpdateTroopTree", SetInfo);
        //��������Ƿ������������ֵ�����
        EventCenter.GetInstance().AddEventListener<int,PlayerItemInfo,int>("PlayerHaveEnoughItem", AddItemConditionNumber);
    }

    public override void HideMe()
    {
        base.HideMe();
        //����б���ֹ�´ν���ʱ��������
        playerMaterialInfo1.Clear();
        UnlockNeedNumber1.Clear();
        playerMaterialInfo2.Clear();
        UnlockNeedNumber2.Clear();

        //�Ƴ���ؼ���
        EventCenter.GetInstance().RemoveEventListener<General, Troop>("UpdateTroopTree", SetInfo);
        EventCenter.GetInstance().RemoveEventListener<int, PlayerItemInfo, int>("PlayerHaveEnoughItem", AddItemConditionNumber);
        
    }


    /// <summary>
    /// �洢��MainPanel�������Ϣ����������߼��ĺ���
    /// </summary>
    /// <param name="currentSelectedGeneral"></param>
    /// <param name="currentSelectedTroop"></param>
    private void SetInfo(General currentSelectedGeneral, Troop currentSelectedTroop)
    {

        //����紫��Ľ����ͱ�����Ϣ�洢����
        SelectedGeneral = currentSelectedGeneral;
        SelectedTroop = currentSelectedTroop;
        //��ʼ��list
        playerMaterialInfo1 = new List<PlayerItemInfo>();
        UnlockNeedNumber1 = new List<int>();
        playerMaterialInfo2 = new List<PlayerItemInfo>();
        UnlockNeedNumber2 = new List<int>();

        //ִ���ҵ�������Ӽ�object
        FindChildObjects();
        //�����ǰ��������������·�߷�֧
        if (SelectedTroop.TroopNextLevelID.Count > 1)
        {
            //���õ�һ���������
            InitTroopTree2();
            //����������ť�ļ���
            GetControl<Button>("ButtonUnlock1").onClick.AddListener(ShowUnlockPanel1);
            GetControl<Button>("ButtonUnlock2").onClick.AddListener(ShowUnlockPanel2);


            if (SelectedGeneral.GeneralOwnedTroop.ContainsKey(NextTroopInfo.TroopIcon))
            {
                alreadyUnlockNextTroop1 = true;
            }

            if (SelectedGeneral.GeneralOwnedTroop.ContainsKey(NextTroop2Info.TroopIcon))
            {
                alreadyUnlockNextTroop2 = true;
            }


            //�����������
            if (nextTroop1CanBeUnlocked && !alreadyUnlockNextTroop1)
            {
                //����ť���ָ���Ϊ�ɽ���
                ChangeTextInChildren("ButtonUnlock1", "�ɽ���");
            }
            else if (alreadyUnlockNextTroop1)
            {
                //����ť���ָ���Ϊ�ѽ���
                ChangeTextInChildren("ButtonUnlock1", "�ѽ���");
            }
            else
            {
                //����ť���ָ���Ϊ���ɽ���
                ChangeTextInChildren("ButtonUnlock1", "���ɽ���");
            }

            if (nextTroop2CanBeUnlocked && !alreadyUnlockNextTroop2)
            {
                //����ť���ָ���Ϊ�ɽ���
                ChangeTextInChildren("ButtonUnlock2", "�ɽ���");
            }
            else if (alreadyUnlockNextTroop2)
            {
                //����ť���ָ���Ϊ�ѽ���
                ChangeTextInChildren("ButtonUnlock2", "�ѽ���");
            }
            else
            {
                //����ť���ָ���Ϊ���ɽ���
                ChangeTextInChildren("ButtonUnlock2", "���ɽ���");
            }
        }
        else
        {
            //��֮���õڶ����������
            InitTroopTree1();
            //����һ����ť�ļ���
            GetControl<Button>("ButtonUnlock1").onClick.AddListener(ShowUnlockPanel1);

            if (SelectedGeneral.GeneralOwnedTroop.ContainsValue(NextTroopInfo))
            {
                alreadyUnlockNextTroop1 = true;
            }

            if (nextTroop1CanBeUnlocked && !alreadyUnlockNextTroop1)
            {
                //����ť���ָ���Ϊ�ɽ���
                ChangeTextInChildren("ButtonUnlock1", "�ɽ���");
            }
            else if (alreadyUnlockNextTroop1)
            {
                //����ť���ָ���Ϊ�ѽ���
                ChangeTextInChildren("ButtonUnlock1", "�ѽ���");
            }
            else
            {
                //����ť���ָ���Ϊ���ɽ���
                ChangeTextInChildren("ButtonUnlock1", "���ɽ���");
            }
        }

    }
    /// <summary>
    /// ��һ��������ã���ǰ����������������֧��ʱ������
    /// </summary>
    private void InitTroopTree2()
    {
        SelectTroop.InitInfo(SelectedTroop);
        GetNextTroopInTroopTree2();
        SetGameObjectsIn2();
        NextTroop.InitInfo(NextTroopInfo);
        NextTroop2.InitInfo(NextTroop2Info);

        ShowItemNeedCell(materialIDNumberMap,content1, list,1);
        ShowItemNeedCell(materialIDNumberMap2,content2, list2,2);

        CheckUnlockCondition(ItemConditionNumber1, materialIDNumberMap.Count, ref nextTroop1CanBeUnlocked);
        CheckUnlockCondition(ItemConditionNumber2, materialIDNumberMap2.Count, ref nextTroop2CanBeUnlocked);

     

    }
    /// <summary>
    /// �ڶ���������ã���ǰ������һ��������֧��ʱ������
    /// </summary>
    private void InitTroopTree1()
    {
        SelectTroop.InitInfo(SelectedTroop);
        GetNextTroopInTroopTree1();
        SetGameObjectsIn1();
        NextTroop.InitInfo(NextTroopInfo);

        ShowItemNeedCell(materialIDNumberMap,content1, list,1);

        CheckUnlockCondition(ItemConditionNumber1, materialIDNumberMap.Count, ref nextTroop1CanBeUnlocked);

    }

    /// <summary>
    /// ������������֧�����������֧�ı�����Ϣ������������ϵ���Ϣ
    /// </summary>
    private void GetNextTroopInTroopTree2()
    {
        List<int> list = SelectedTroop.TroopNextLevelID;
        NextTroopInfo = GameDataMgr.GetInstance().GetTroopInfo(list[0]);
        NextTroop2Info = GameDataMgr.GetInstance().GetTroopInfo(list[1]);
        NexttroopLevelUpInfo1 = GameDataMgr.GetInstance().GetTroopLevelUpInfo(NextTroopInfo.TroopID);
        NexttroopLevelUpInfo2 = GameDataMgr.GetInstance().GetTroopLevelUpInfo(NextTroop2Info.TroopID);
        materialIDNumberMap = SeperateMaterialsInfo(NexttroopLevelUpInfo1);
        materialIDNumberMap2 = SeperateMaterialsInfo(NexttroopLevelUpInfo2);
    }

    /// <summary>
    /// ֻ��һ��������֧��ֻ��Ҫ���һ����֧�ı�����Ϣ������������ϵ���Ϣ
    /// </summary>
    private void GetNextTroopInTroopTree1()
    {
        List<int> list = SelectedTroop.TroopNextLevelID;
        NextTroopInfo = GameDataMgr.GetInstance().GetTroopInfo(list[0]);
        NexttroopLevelUpInfo1 = GameDataMgr.GetInstance().GetTroopLevelUpInfo(NextTroopInfo.TroopID);
        materialIDNumberMap = SeperateMaterialsInfo(NexttroopLevelUpInfo1);
    }

    /// <summary>
    /// �ҵ����е��Ӽ�gameObject�ĺ���
    /// </summary>
    private void FindChildObjects()
    {
        NextTroopCellInPanel1 = transform.Find("NextTroopCellInPanel1")?.gameObject;
        ScrollView1 = transform.Find("ScrollView1")?.gameObject;
        ButtonUnlock1 = transform.Find("ButtonUnlock1")?.gameObject;

        NextTroopCellInPanel2 = transform.Find("NextTroopCellInPanel2")?.gameObject;
        ScrollView2 = transform.Find("ScrollView2")?.gameObject;
        ButtonUnlock2 = transform.Find("ButtonUnlock2")?.gameObject;
    }

    /// <summary>
    /// �ڶ�����������£�����objects��λ��
    /// </summary>
    private void SetGameObjectsIn1()
    {
        NextTroopCellInPanel2.SetActive(false);
        ScrollView2.SetActive(false);
        ButtonUnlock2.SetActive(false);

        NextTroopCellInPanel1.transform.localPosition = new Vector3(845, 166, 0);
        ScrollView1.transform.localPosition = new Vector3(512, 166, 0);
        ButtonUnlock1.transform.localPosition = new Vector3(727, 166, 0);
    }

    /// <summary>
    /// ��һ����������£�2��֧������objects��λ��
    /// </summary>
    private void SetGameObjectsIn2()
    {
        NextTroopCellInPanel2.SetActive(true);
        ScrollView2.SetActive(true);
        ButtonUnlock2.SetActive(true);

        NextTroopCellInPanel1.transform.localPosition = new Vector3(845, 247, 0);
        ScrollView1.transform.localPosition = new Vector3(512, 247, 0);
        ButtonUnlock1.transform.localPosition = new Vector3(727, 247, 0);
    }

    /// <summary>
    /// �������������ʱ�����ݰ��մ洢���ֵ��У���ֵΪ�������ʵ�id��ֵΪ�������ʵ�����
    /// </summary>
    /// <param name="info">�����������ʱ�����</param>
    /// <returns></returns>
    private Dictionary<int,int> SeperateMaterialsInfo(TroopLevelUp info)
    {
        Dictionary<int, int> materialIDNumberMap = new Dictionary<int, int>();
        for (int i = 0; i < info.MaterialID.Count; i++)
        {
            int materialID = info.MaterialID[i];
            int number = info.Number[i];

            materialIDNumberMap[materialID] = number;

            foreach (KeyValuePair<int, int> kvp in materialIDNumberMap)
            {
                int key = kvp.Key;
                int value = kvp.Value;

            }
        }
        return materialIDNumberMap;
    }

    /// <summary>
    /// �����ֵ����������cell
    /// </summary>
    /// <param name="materialIDNumberMap">����Ĵ洢�������������ֵ�</param>
    /// <param name="content">scrollView��content</param>
    /// <param name="list">�洢��list�������</param>
    /// <param name="index">������λ���</param>
    private void ShowItemNeedCell(Dictionary<int,int> materialIDNumberMap,Transform content, List<UI_ItemNeedCell> list,int index)
    {
        // ��������
        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i].gameObject != null)
            {
                Destroy(list[i].gameObject);
            }
        }
        list.Clear();

        foreach (KeyValuePair<int, int> info in materialIDNumberMap)
        {
            int key = info.Key;
            int value = info.Value;
            UI_ItemNeedCell cell = ResourceManager.GetInstance().Load<GameObject>("UI/ItemNeedCell").GetComponent<UI_ItemNeedCell>();
            cell.InitInfo(key, value,index);
            cell.transform.SetParent(content, false);
            list.Add(cell);
        }
    }

    /// <summary>
    /// �����������Ƿ����㣬������趨Ϊ��
    /// </summary>
    /// <param name="ItemConditionNumber">��������Ҫ���������������</param>
    /// <param name="ItemTypeNumber">������Ҫ��������������</param>
    /// <param name="NextTroopCanBeUnLocked">�����Ƿ�����</param>
    /// <returns></returns>
    private bool CheckUnlockCondition(int ItemConditionNumber,int ItemTypeNumber, ref bool NextTroopCanBeUnLocked)
    {
        if (ItemConditionNumber >= ItemTypeNumber)
        {
            NextTroopCanBeUnLocked = true;
            return NextTroopCanBeUnLocked;
        }
        else
        {
            NextTroopCanBeUnLocked = false;
            return NextTroopCanBeUnLocked;
        }
    }

    /// <summary>
    /// ÿ������������cell���ᴫ��һ����Ӧ����
    /// </summary>
    /// <param name="index">cell���ڵı�����λ�ı��</param>
    /// <param name="playerMaterial">�����ӵ�е��������</param>
    /// <param name="NeedNumber">��������</param>
    private void AddItemConditionNumber(int index, PlayerItemInfo playerMaterial, int NeedNumber)
    { 
        if(index == 1)
        {
            ItemConditionNumber1 += 1;

            playerMaterialInfo1.Add(playerMaterial);
            UnlockNeedNumber1.Add(NeedNumber);
        }
        else if (index == 2)
        {
            ItemConditionNumber2 += 1;

            playerMaterialInfo2.Add(playerMaterial);
            UnlockNeedNumber2.Add(NeedNumber);
        }

    }

    /// <summary>
    /// ���������ť1��ִ�еĺ���
    /// </summary>
    private void ShowUnlockPanel1()
    {
        if (nextTroop1CanBeUnlocked && !alreadyUnlockNextTroop1)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_pc01-ItemSelect", false);
            foreach (General general in GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral.Values)
            {
               if(SelectedGeneral.GeneralID== general.GeneralID)
                {
                    general.GeneralOwnedTroop.Add(NextTroopInfo.TroopIcon, NextTroopInfo);
                }
            }

            EventCenter.GetInstance().EventTrigger<General>("ToggleChanged",SelectedGeneral);
            ChangeTextInChildren("ButtonUnlock1", "�ѽ���");
            DecreaseMaterial1();

            alreadyUnlockNextTroop1 = true;
            ShowItemNeedCell(materialIDNumberMap, content1, list, 1);

            EventCenter.GetInstance().EventTrigger("SavePlayerInfo");

            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("���ɹ������ñ���");
            });
        }
        else if (alreadyUnlockNextTroop1)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("���ѽ����ñ���");
            });
        }
        else
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("������Դ����");
            });
        }

    }

    /// <summary>
    /// ���������ť2��ִ�еĺ���
    /// </summary>
    private void ShowUnlockPanel2()
    {
        if (nextTroop2CanBeUnlocked && !alreadyUnlockNextTroop2)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_pc01-ItemSelect", false);
            foreach (General general in GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral.Values)
            {
                if (SelectedGeneral.GeneralID == general.GeneralID)
                {
                    general.GeneralOwnedTroop.Add(NextTroop2Info.TroopIcon, NextTroop2Info);
                }
            }
            EventCenter.GetInstance().EventTrigger<General>("ToggleChanged", SelectedGeneral);
            ChangeTextInChildren("ButtonUnlock2", "�ѽ���");
            DecreaseMaterial2();

            alreadyUnlockNextTroop2 = true;
            ShowItemNeedCell(materialIDNumberMap2, content2, list2, 2);

            EventCenter.GetInstance().EventTrigger("SavePlayerInfo");

            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("���ɹ������ñ���");
            });
        }
        else if (alreadyUnlockNextTroop2)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("���ѽ����ñ���");
            });
        }
        else
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("������Դ����");
            });
        }

    }
    /// <summary>
    /// ��������ɺ���ٵ���
    /// </summary>
    private void DecreaseMaterial1()
    {
        if (playerMaterialInfo1.Count != UnlockNeedNumber1.Count)
        {
            return;
        }

        // ��needNumber��PlayerItemInfo��number�м�ȥ
        for (int i = 0; i < playerMaterialInfo1.Count; i++)
        {
            playerMaterialInfo1[i].number -= UnlockNeedNumber1[i];

            foreach (PlayerItemInfo playerItem in GameDataMgr.GetInstance().PlayerDataInfo.AllItems)
            {
                if (playerItem.id == playerMaterialInfo1[i].id)
                {
                    playerItem.number = playerMaterialInfo1[i].number;
                }
            }
        }

       
    }
    /// <summary>
    /// ��������ɺ���ٵ���
    /// </summary>
    private void DecreaseMaterial2()
    {
        if (playerMaterialInfo2.Count != UnlockNeedNumber2.Count)
        {
            return;
        }

        // ��needNumber��PlayerItemInfo��number�м�ȥ
        for (int i = 0; i < playerMaterialInfo2.Count; i++)
        {
            playerMaterialInfo2[i].number -= UnlockNeedNumber2[i];

            foreach (PlayerItemInfo playerItem in GameDataMgr.GetInstance().PlayerDataInfo.AllItems)
            {
                if (playerItem.id == playerMaterialInfo2[i].id)
                {
                    playerItem.number = playerMaterialInfo2[i].number;
                }
            }
        }

    }

    /// <summary>
    ///����������ƻ�����������������ı� 
    /// </summary>
    /// <param name="ComponentName">�����</param>
    /// <param name="NewSprite">��Ҫ�ĳɵ��ı�</param>
    public void ChangeTextInChildren(string ComponentName, string NewText)
    {
        Transform target = transform.Find(ComponentName);
        if (target != null)
        {
            // ���Ի�ȡ�Ӷ����ϵ�Text���
            TMP_Text textComponent = target.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
            {
                // �޸��ı�
                textComponent.text = NewText;
            }
        }
    }
}
