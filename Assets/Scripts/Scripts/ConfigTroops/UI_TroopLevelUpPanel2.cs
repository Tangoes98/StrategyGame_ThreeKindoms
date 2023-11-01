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

    //用于绑定content1
    public Transform content1;
    //用于绑定content2
    public Transform content2;

    //存储列表用于切换页签时清空
    private List<UI_ItemNeedCell> list = new List<UI_ItemNeedCell>();
    private List<UI_ItemNeedCell> list2 = new List<UI_ItemNeedCell>();
    
    //存储升级所需物资信息
    public TroopLevelUp NexttroopLevelUpInfo1;
    public TroopLevelUp NexttroopLevelUpInfo2;
    
    //用字典存储解析的所需物资信息
    Dictionary<int, int> materialIDNumberMap = new Dictionary<int, int>();
    Dictionary<int, int> materialIDNumberMap2 = new Dictionary<int, int>();
    
    //用int存储满足条件的物品种类数量，和需求的物品种类数量做对比
    int ItemConditionNumber1;
    int ItemConditionNumber2;

    //用list储存从ItemNeedCell传入的道具，和要求的数量
    List<PlayerItemInfo> playerMaterialInfo1;
    List<int> UnlockNeedNumber1;
    List<PlayerItemInfo> playerMaterialInfo2;
    List<int> UnlockNeedNumber2;

    //已经完成解锁兵种，当栏位1或者栏位2解锁兵种后为true
    private bool alreadyUnlockNextTroop1;
    private bool alreadyUnlockNextTroop2;

    //判断是否满足物资需求的条件
    bool nextTroop1CanBeUnlocked;
    bool nextTroop2CanBeUnlocked;

    //申明一个兵种栏位用来放置兵种
    public UI_TroopCellInPanel SelectTroop;
    public UI_TroopCellInPanel NextTroop;
    public UI_TroopCellInPanel NextTroop2;

    //存储将军和兵种的信息
    private General SelectedGeneral;
    private Troop SelectedTroop;
    //升级目标兵种1
    private Troop NextTroopInfo;
    //升级目标兵种2
    private Troop NextTroop2Info;

    //存储子级的gameObject
    //栏位1
    private GameObject NextTroopCellInPanel1;
    private GameObject ScrollView1;
    private GameObject ButtonUnlock1;
    //栏位2
    private GameObject NextTroopCellInPanel2;
    private GameObject ScrollView2;
    private GameObject ButtonUnlock2;

    public override void ShowMe()
    {
        base.ShowMe();
/*
        //默认bool全部为false
        alreadyUnlockNextTroop1 = false;
        alreadyUnlockNextTroop2 = false;
        nextTroop1CanBeUnlocked = false;
        nextTroop2CanBeUnlocked = false;*/


        //开始事件监听，监听更新兵种树的事件
        EventCenter.GetInstance().AddEventListener<General, Troop>("UpdateTroopTree", SetInfo);
        //监听玩家是否满足升级兵种的条件
        EventCenter.GetInstance().AddEventListener<int,PlayerItemInfo,int>("PlayerHaveEnoughItem", AddItemConditionNumber);
    }

    public override void HideMe()
    {
        base.HideMe();
        //清空列表，防止下次进入时出现问题
        playerMaterialInfo1.Clear();
        UnlockNeedNumber1.Clear();
        playerMaterialInfo2.Clear();
        UnlockNeedNumber2.Clear();

        //移除相关监听
        EventCenter.GetInstance().RemoveEventListener<General, Troop>("UpdateTroopTree", SetInfo);
        EventCenter.GetInstance().RemoveEventListener<int, PlayerItemInfo, int>("PlayerHaveEnoughItem", AddItemConditionNumber);
        
    }


    /// <summary>
    /// 存储从MainPanel传入的信息并处理各种逻辑的函数
    /// </summary>
    /// <param name="currentSelectedGeneral"></param>
    /// <param name="currentSelectedTroop"></param>
    private void SetInfo(General currentSelectedGeneral, Troop currentSelectedTroop)
    {

        //将外界传入的将军和兵种信息存储起来
        SelectedGeneral = currentSelectedGeneral;
        SelectedTroop = currentSelectedTroop;
        //初始化list
        playerMaterialInfo1 = new List<PlayerItemInfo>();
        UnlockNeedNumber1 = new List<int>();
        playerMaterialInfo2 = new List<PlayerItemInfo>();
        UnlockNeedNumber2 = new List<int>();

        //执行找到自身的子级object
        FindChildObjects();
        //如果当前兵种有两个升级路线分支
        if (SelectedTroop.TroopNextLevelID.Count > 1)
        {
            //启用第一种面板设置
            InitTroopTree2();
            //启动两个按钮的监听
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


            //如果满足条件
            if (nextTroop1CanBeUnlocked && !alreadyUnlockNextTroop1)
            {
                //将按钮文字更改为可解锁
                ChangeTextInChildren("ButtonUnlock1", "可解锁");
            }
            else if (alreadyUnlockNextTroop1)
            {
                //将按钮文字更改为已解锁
                ChangeTextInChildren("ButtonUnlock1", "已解锁");
            }
            else
            {
                //将按钮文字更改为不可解锁
                ChangeTextInChildren("ButtonUnlock1", "不可解锁");
            }

            if (nextTroop2CanBeUnlocked && !alreadyUnlockNextTroop2)
            {
                //将按钮文字更改为可解锁
                ChangeTextInChildren("ButtonUnlock2", "可解锁");
            }
            else if (alreadyUnlockNextTroop2)
            {
                //将按钮文字更改为已解锁
                ChangeTextInChildren("ButtonUnlock2", "已解锁");
            }
            else
            {
                //将按钮文字更改为不可解锁
                ChangeTextInChildren("ButtonUnlock2", "不可解锁");
            }
        }
        else
        {
            //反之启用第二种面板设置
            InitTroopTree1();
            //启动一个按钮的监听
            GetControl<Button>("ButtonUnlock1").onClick.AddListener(ShowUnlockPanel1);

            if (SelectedGeneral.GeneralOwnedTroop.ContainsValue(NextTroopInfo))
            {
                alreadyUnlockNextTroop1 = true;
            }

            if (nextTroop1CanBeUnlocked && !alreadyUnlockNextTroop1)
            {
                //将按钮文字更改为可解锁
                ChangeTextInChildren("ButtonUnlock1", "可解锁");
            }
            else if (alreadyUnlockNextTroop1)
            {
                //将按钮文字更改为已解锁
                ChangeTextInChildren("ButtonUnlock1", "已解锁");
            }
            else
            {
                //将按钮文字更改为不可解锁
                ChangeTextInChildren("ButtonUnlock1", "不可解锁");
            }
        }

    }
    /// <summary>
    /// 第一种面板设置，当前兵种有两个升级分支的时候启用
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
    /// 第二种面板设置，当前兵种有一个升级分支的时候启用
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
    /// 有两个升级分支，获得两个分支的兵种信息和升级所需材料的信息
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
    /// 只有一个升级分支，只需要获得一个分支的兵种信息和升级所需材料的信息
    /// </summary>
    private void GetNextTroopInTroopTree1()
    {
        List<int> list = SelectedTroop.TroopNextLevelID;
        NextTroopInfo = GameDataMgr.GetInstance().GetTroopInfo(list[0]);
        NexttroopLevelUpInfo1 = GameDataMgr.GetInstance().GetTroopLevelUpInfo(NextTroopInfo.TroopID);
        materialIDNumberMap = SeperateMaterialsInfo(NexttroopLevelUpInfo1);
    }

    /// <summary>
    /// 找到所有的子级gameObject的函数
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
    /// 第二种面板设置下，设置objects的位置
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
    /// 第一种面板设置下，2分支，设置objects的位置
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
    /// 将兵种升级物资表的内容按照存储到字典中，键值为所需物资的id，值为所需物资的数量
    /// </summary>
    /// <param name="info">兵种升级物资表数据</param>
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
    /// 根据字典的数量生成cell
    /// </summary>
    /// <param name="materialIDNumberMap">传入的存储兵种升级数据字典</param>
    /// <param name="content">scrollView的content</param>
    /// <param name="list">存储的list便于清除</param>
    /// <param name="index">兵种栏位编号</param>
    private void ShowItemNeedCell(Dictionary<int,int> materialIDNumberMap,Transform content, List<UI_ItemNeedCell> list,int index)
    {
        // 更新内容
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
    /// 检测解锁条件是否满足，满足就设定为真
    /// </summary>
    /// <param name="ItemConditionNumber">满足升级要求的物资种类数量</param>
    /// <param name="ItemTypeNumber">升级需要的物资种类数量</param>
    /// <param name="NextTroopCanBeUnLocked">条件是否满足</param>
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
    /// 每个满足条件的cell都会传入一次相应数据
    /// </summary>
    /// <param name="index">cell所在的兵种栏位的编号</param>
    /// <param name="playerMaterial">玩家所拥有的需求道具</param>
    /// <param name="NeedNumber">需求数量</param>
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
    /// 点击解锁按钮1后执行的函数
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
            ChangeTextInChildren("ButtonUnlock1", "已解锁");
            DecreaseMaterial1();

            alreadyUnlockNextTroop1 = true;
            ShowItemNeedCell(materialIDNumberMap, content1, list, 1);

            EventCenter.GetInstance().EventTrigger("SavePlayerInfo");

            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("您成功解锁该兵种");
            });
        }
        else if (alreadyUnlockNextTroop1)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("您已解锁该兵种");
            });
        }
        else
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("您的资源不足");
            });
        }

    }

    /// <summary>
    /// 点击解锁按钮2后执行的函数
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
            ChangeTextInChildren("ButtonUnlock2", "已解锁");
            DecreaseMaterial2();

            alreadyUnlockNextTroop2 = true;
            ShowItemNeedCell(materialIDNumberMap2, content2, list2, 2);

            EventCenter.GetInstance().EventTrigger("SavePlayerInfo");

            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("您成功解锁该兵种");
            });
        }
        else if (alreadyUnlockNextTroop2)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("您已解锁该兵种");
            });
        }
        else
        {
            MusicMgr.GetInstance().PlaySound("maou_se_onepoint05-false", false);
            UIManager.GetInstance().ShowPanel<UI_HintPanel>("HintPanel", E_UI_Layer.System, (panel) =>
            {
                panel.InitInfo("您的资源不足");
            });
        }

    }
    /// <summary>
    /// 当解锁完成后减少道具
    /// </summary>
    private void DecreaseMaterial1()
    {
        if (playerMaterialInfo1.Count != UnlockNeedNumber1.Count)
        {
            return;
        }

        // 将needNumber从PlayerItemInfo的number中减去
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
    /// 当解锁完成后减少道具
    /// </summary>
    private void DecreaseMaterial2()
    {
        if (playerMaterialInfo2.Count != UnlockNeedNumber2.Count)
        {
            return;
        }

        // 将needNumber从PlayerItemInfo的number中减去
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
    ///根据组件名称获得组件，更改上面的文本 
    /// </summary>
    /// <param name="ComponentName">组件名</param>
    /// <param name="NewSprite">想要改成的文本</param>
    public void ChangeTextInChildren(string ComponentName, string NewText)
    {
        Transform target = transform.Find(ComponentName);
        if (target != null)
        {
            // 尝试获取子对象上的Text组件
            TMP_Text textComponent = target.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
            {
                // 修改文本
                textComponent.text = NewText;
            }
        }
    }
}
