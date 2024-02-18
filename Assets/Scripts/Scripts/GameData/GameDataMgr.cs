using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GameDataMgr : BaseManager<GameDataMgr>
{
    //字典存储玩家从Json读取到的物品数据
    public Dictionary<int, Item> itemInfoDic = new Dictionary<int, Item>();
    //字典存储玩家从Json读取到的武将数据
    public Dictionary<int, General> generalInfoDic = new Dictionary<int, General>();
    //字典存储玩家从Json读取到的兵种数据
    public Dictionary<int, Troop> troopInfoDic = new Dictionary<int, Troop>();
    //从字典存储玩家从Json读取到的经验值表
    public Dictionary<int, EXP> expInfodic = new Dictionary<int, EXP>();
    //从字典存储玩家从Json读取到的兵种升级所需物资表
    public Dictionary<int, TroopLevelUp> troopLevelUpInfodic = new Dictionary<int, TroopLevelUp>();

    //玩家的存储路径
    private static string PlayerInfoSaveAdress = Application.persistentDataPath + "/PlayerSaveData.json";

    //玩家的数据信息
    public PlayerInfo PlayerDataInfo;

    public bool isParseDataExecuted = false;

    /// <summary>
    /// 解析存储数据的Json信息
    /// </summary>
    public void ParseData()
    {

        //读取并解析Json文件//物品信息
        ItemInfo itemInfos = JsonMgr.Instance.LoadData<ItemInfo>("ItemInfo");
        //将数据集合按照ID号分别放入
        for (int i = 0; i < itemInfos.itemInfo.Count; ++i)
        {
            itemInfoDic.Add(itemInfos.itemInfo[i].ID, itemInfos.itemInfo[i]);
        }

        //读取并解析Json文件//将军信息
        GeneralInfo generalInfos = JsonMgr.Instance.LoadData<GeneralInfo>("GeneralInfo");
        //将数据集合按照ID号分别放入
        for (int i = 0; i < generalInfos.generalInfo.Count; ++i)
        {
            generalInfoDic.Add(generalInfos.generalInfo[i].GeneralID, generalInfos.generalInfo[i]);
        }

        //读取并解析Json文件//部队信息
        TroopInfo troopInfos = JsonMgr.Instance.LoadData<TroopInfo>("TroopInfo");
        //将数据集合按照ID号分别放入
        for (int i = 0; i < troopInfos.troopInfo.Count; ++i)
        {
            troopInfoDic.Add(troopInfos.troopInfo[i].TroopID, troopInfos.troopInfo[i]);
        }

        //读取并解析Json文件//经验值信息
        EXPInfo expInfos = JsonMgr.Instance.LoadData<EXPInfo>("EXPInfo");
        //将数据集合按照ID号分别放入
        for (int i = 0; i < expInfos.expInfo.Count; ++i)
        {
            expInfodic.Add(expInfos.expInfo[i].TargetLevel, expInfos.expInfo[i]);
        }

        //读取并解析Json文件//经验值信息
        TroopLevelUpInfo troopLevelUpInfos = JsonMgr.Instance.LoadData<TroopLevelUpInfo>("TroopLevelUpInfo");
        //将数据集合按照ID号分别放入
        for (int i = 0; i < troopLevelUpInfos.troopLevelUpInfo.Count; ++i)
        {
            troopLevelUpInfodic.Add(troopLevelUpInfos.troopLevelUpInfo[i].TroopID, troopLevelUpInfos.troopLevelUpInfo[i]);
        }

        //没有玩家数据时，初始化一个默认数据
        if (PlayerDataInfo == null)
        {
            PlayerDataInfo = new PlayerInfo();
            //存储它
            JsonMgr.Instance.SaveData(PlayerDataInfo, "PlayerSaveData");
        }
        //File.WriteAllBytes(PlayerInfoSaveAdress, Encoding.UTF8.GetBytes(xxx));

        ////////////////////////事件监听，监听贯穿整个游戏的数据变化///////////////////////////////////////////////////////////
        ///这些监听不能删除，每多执行一次ParseData就会导致玩家的存档变化出问题，一定要杜绝
        //通过事件监听监听钱是否有变化
        EventCenter.GetInstance().AddEventListener<int>("MoneyChange", ChangeMoney);
        //通过事件监听等级是否有变化
        EventCenter.GetInstance().AddEventListener<string, int>("GeneralLevelUp", GeneralLevelUp);
        //通过事件监听属性点是否有变化
        EventCenter.GetInstance().AddEventListener<string, string, int>("AssignAttributePoint", AssignAttributePoint);
        //通过事件监听是否存档
        EventCenter.GetInstance().AddEventListener("SavePlayerInfo", SavePlayerInfo);
        //通过事件监听二级属性根据属性点变化       
        EventCenter.GetInstance().AddEventListener<string,int,int,int>("AttributeChangeConfirmed", AttributeChangeConfirm);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //执行完后，将当前ParseData执行状态调整为已经执行过
        isParseDataExecuted = true;
    }
    /// <summary>
    /// 方便外界使用GameDataMgr调用金钱改变
    /// </summary>
    /// <param name="money"></param>
    private void ChangeMoney(int money)
    {
        PlayerDataInfo.ChangeMoney(money);
        //减少钱就存储数据
        EventCenter.GetInstance().EventTrigger("SavePlayerInfo");
    }

    /// <summary>
    /// 方便外界使用GameDataMgr调用二级属性改变
    /// </summary>
    /// <param name="generalKey"></param>
    private void AttributeChangeConfirm(string generalKey,int AlreadyAssignedPointCountS,int AlreadyAssignedPointCountL,int AlreadyAssignedPointCountW)
    {
        PlayerDataInfo.AttributeChangeConfirm(generalKey, AlreadyAssignedPointCountS, AlreadyAssignedPointCountL, AlreadyAssignedPointCountW);
        //属性改变，存储数据
        EventCenter.GetInstance().EventTrigger("SavePlayerInfo");
    }

    /// <summary>
    /// 方便外界使用GameDataMgr调用等级改变
    /// </summary>
    /// <param name="generalKey"></param>
    /// <param name="excessEXP"></param>
    private void GeneralLevelUp(string generalKey, int excessEXP)
    {

        PlayerDataInfo.GeneralLevelUp(generalKey, excessEXP);
        //升级就存储数据
        EventCenter.GetInstance().EventTrigger("SavePlayerInfo");
    }
    /// <summary>
    /// 方便外界使用GameDataMgr调用将军属性点改变
    /// </summary>
    /// <param name="generalKey"></param>
    /// <param name="attributeType"></param>
    /// <param name="number"></param>
    private void AssignAttributePoint(string generalKey, string attributeType, int number)
    {
        PlayerDataInfo.AssignAttributePoint(generalKey, attributeType, number);
        //分配属性点就存储数据
        EventCenter.GetInstance().EventTrigger("SavePlayerInfo");
    }

    /// <summary>
    /// 方便外界使用GameDataMgr调用存档
    /// 保存玩家信息----以后将其作为自动存档/快速存档
    /// </summary>
    public void SavePlayerInfo()
    {
        JsonMgr.Instance.SaveData(PlayerDataInfo, "PlayerSaveData");
    }


    /// <summary>
    /// 读取玩家信息----以后将其作为快速读取最近存档
    /// </summary>
    public void LoadPlayerInfo()
    {
        if (File.Exists(PlayerInfoSaveAdress))
        {
            //读取作为存档的Json文件
            PlayerDataInfo = JsonMgr.Instance.LoadData<PlayerInfo>("PlayerSaveData");
        }
    }


    //未来需要一个手动存档的方式
    //未来需要一个手动读档的方式

    /// <summary>
    /// 根据道具ID获得道具信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Item GetItemInfo(int id)
    {
        if (itemInfoDic.ContainsKey(id))
            return itemInfoDic[id];
        return null;
    }
    /// <summary>
    /// 根据武将ID获得武将信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public General GetGeneralInfo(int id)
    {
        if (generalInfoDic.ContainsKey(id))
            return generalInfoDic[id];
        return null;
    }
    /// <summary>
    /// 根据部队信息获得部队信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Troop GetTroopInfo(int id)
    {
        if (troopInfoDic.ContainsKey(id))
            return troopInfoDic[id];
        return null;
    }

    public TroopLevelUp GetTroopLevelUpInfo(int id)
    {
        if (troopLevelUpInfodic.ContainsKey(id))
            return troopLevelUpInfodic[id];
        return null;
    }
}

/// <summary>
/// 玩家的所有道具和资源
/// </summary>
public class PlayerInfo
{
    public string date;
    public int money;
    public int currentChapter;
    public string currentPhase;
    public Dictionary<string, General> PlayerOwnedGeneral;

    public List<PlayerItemInfo> AllItems;
    public List<PlayerItemInfo> Materials;
    public List<PlayerItemInfo> activeSkillBooks;
    public List<PlayerItemInfo> passiveSkillBooks;
    public General currentSelectedGeneral;

    public bool C1_MainLevel1IsFinished;
    public bool C1_SubLevel1IsFinished;

    public bool C2_MainLevel1IsFinished;
    public bool C2_SubLevel1IsFinished;

    public PlayerInfo()
    {
        //起始年份，以后在故事阶段更新年份
        date = "184年秋,汉光和七年";
        //玩家起始金钱
        money = 1000;
        //玩家起始章节，一般在Stroy场景更改
        currentChapter = 0;
        //玩家起始阶段，一般每章开头处于剧情阶段，每次进入新的场景，就更新为相应的阶段
        currentPhase = "Story0";

        AllItems = new List<PlayerItemInfo>();
        activeSkillBooks = new List<PlayerItemInfo>();
        passiveSkillBooks = new List<PlayerItemInfo>();
        Materials = new List<PlayerItemInfo>();

        //记录关卡通关情况
        C1_MainLevel1IsFinished = false;
        C1_SubLevel1IsFinished = false;

        C2_MainLevel1IsFinished = false;
        C2_SubLevel1IsFinished = false;


        //////////////////////////////////////////////////////////////////////////////////////////////
        //想给玩家添加道具就在这里操作，以下都是测试用的/////////////////////////
        //activeSkillBooks.Add(AddPlayerItemInfo(1, 2));
        //第一个参数是物品ID，第二个参数是添加数量
        AddItemForPlayer(AddPlayerItemInfo(1, 2));
        AddItemForPlayer(AddPlayerItemInfo(50, 3));
        AddItemForPlayer(AddPlayerItemInfo(66, 5));
        AddItemForPlayer(AddPlayerItemInfo(68, 3));
        /////////////////////////////////////////////////////////////////////

        PlayerOwnedGeneral = new Dictionary<string, General>();

        //给玩家添加初始将军，刘关张
        //通过GetGeneralInfo输入将军的Id得到存储在全部将军字典中的将军，然后存储为玩家的将军
        //添加刘备
        //先利用将军ID使用保存在构造函数中(Json中读取的数据)的值初始化一个将军
        //因为刘备的ID是0，所以传入0，因为Json中以0为起始值
        General LiuBei = new General(0);
        //将初始化后的将军添加到玩家已拥有的将军字典，并用英文名作为键，在导入头像等美术素材时可以直接使用键来导入
        PlayerOwnedGeneral.Add("LiuBei",LiuBei);

        currentSelectedGeneral = PlayerOwnedGeneral["LiuBei"];
        //////////////////////直接添加技能的方法，解锁技能或者使用技能书时使用////////////////////////////////////////////////////
        PlayerOwnedGeneral["LiuBei"].PossessedActiveSkills.Add("Arson Fire");

        //添加关羽，同刘备
        General GuanYu = new General(1);
        PlayerOwnedGeneral.Add("GuanYu", GuanYu);


        PlayerOwnedGeneral["GuanYu"].PossessedPassiveSkills.Add("Good Surrounder");
        PlayerOwnedGeneral["GuanYu"].PossessedPassiveSkills.Add("Good Collaborator");

        //添加张飞，同刘备
        General ZhangFei = new General(2);
        PlayerOwnedGeneral.Add("ZhangFei", ZhangFei);
    }

    /// <summary>
    /// 分配属性点的函数
    /// </summary>
    /// <param name="generalKey"></param>
    /// <param name="attributeType"></param>
    /// <param name="number"></param>
    public void AssignAttributePoint(string generalKey, string attributeType, int number)
    {
        //每次分配对应属性加number
        switch (attributeType)
        {
            case "Strength":
                PlayerOwnedGeneral[generalKey].Strength += number;
                break;

            case "LeaderShip":
                PlayerOwnedGeneral[generalKey].LeaderShip += number;
                break;

            case "Wisdom":
                PlayerOwnedGeneral[generalKey].Wisdom += number;
                break;

        }
        //每次分配武将的未分配属性点也要减去number
        PlayerOwnedGeneral[generalKey].UnassignedAttributePoints -= number;
    }
    /// <summary>
    /// 确认属性分配后玩家的二级属性改变
    /// </summary>
    /// <param name="generalKey"></param>
    public void AttributeChangeConfirm(string generalKey,int AlreadyAssignedPointCountS, int AlreadyAssignedPointCountL, int AlreadyAssignedPointCountW)
    {
        //每次分配属性后更新该武将的相关信息
        
        //武勇带来的属性变化
        //攻击力增加
        PlayerOwnedGeneral[generalKey].Attack += AlreadyAssignedPointCountS * 10;
        PlayerOwnedGeneral[generalKey].CriticalRate += 0.01 * AlreadyAssignedPointCountS;

        //统率带来的属性变化
        //统率带来的防御值增加
        PlayerOwnedGeneral[generalKey].Defense += AlreadyAssignedPointCountL * 10;
        //武将等级兵力调整值增加
        PlayerOwnedGeneral[generalKey].LevelHealthAdjustment = 20 + PlayerOwnedGeneral[generalKey].LeaderShip * 2;
        //武将统率造成的兵力值增加
        //升级统率获得的生命值增加量为，生命值统率调整值*增加的统率
        PlayerOwnedGeneral[generalKey].Hp += AlreadyAssignedPointCountL * PlayerOwnedGeneral[generalKey].LeaderShipHealthAdjustment;
        PlayerOwnedGeneral[generalKey].CurrentHp = PlayerOwnedGeneral[generalKey].Hp;

        //智谋带来的属性变化
        //智谋伤害增加
        PlayerOwnedGeneral[generalKey].StratageDamage += AlreadyAssignedPointCountW * 10;
        //智谋带来的可携带主动技能数量的增加
        PlayerOwnedGeneral[generalKey].ActiveSkillCount = (PlayerOwnedGeneral[generalKey].Wisdom / 6) + 1;


        EventCenter.GetInstance().EventTrigger<General>("ToggleChanged", PlayerOwnedGeneral[generalKey]);
    }

    /// <summary>
    /// 将领升级的函数
    /// </summary>
    /// <param name="GeneralKey">将领的key值</param>
    public void GeneralLevelUp(string generalKey, int excessEXP)
    {
        //当前等级加1
        PlayerOwnedGeneral[generalKey].Level += 1;
        //等级提升获得3点可分配属性点
        PlayerOwnedGeneral[generalKey].UnassignedAttributePoints += 3;
        //等级经验值上限提高（需求经验值增加）
        PlayerOwnedGeneral[generalKey].MaxEXP = GameDataMgr.GetInstance().expInfodic[PlayerOwnedGeneral[generalKey].Level + 1].NeedEXP;
        //清空当前经验值并加上升级时的剩余经验
        PlayerOwnedGeneral[generalKey].CurrentEXP = excessEXP;
        
        //等级提升带来的HP上限提升
        PlayerOwnedGeneral[generalKey].Hp += GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].LevelHealthAdjustment;
        //统率带来的生命值成长值为20（生命值统率调整值）
        PlayerOwnedGeneral[generalKey].LeaderShipHealthAdjustment = 20;
        PlayerOwnedGeneral[generalKey].CurrentHp = PlayerOwnedGeneral[generalKey].Hp;
        //等级带来的攻击力提升
        PlayerOwnedGeneral[generalKey].Attack += PlayerOwnedGeneral[generalKey].LevelAttackAdjustment;
        //等级带来的被动技能数量增加
        PlayerOwnedGeneral[generalKey].PassiveSkillCount = (PlayerOwnedGeneral[generalKey].Level / 5) + 1;

        EventCenter.GetInstance().EventTrigger<General>("ToggleChanged", PlayerOwnedGeneral[generalKey]);
    }


    /// <summary>
    /// 改变玩家当前钱数的函数
    /// </summary>
    /// <param name="money">加钱或者减钱的数量</param>
    public void ChangeMoney(int money)
    {
        //判断玩家的钱够不够减去，避免出现负数
        //如果传入的mony小于0，意味着玩家在执行花钱的动作
        if (money < 0 && this.money < money)
            return;
        //改变玩家的钱
        this.money += money;
    }

    /// <summary>
    /// 添加玩家道具列表到玩家信息中已经存在的三种类型的道具中
    /// </summary>
    /// <param name="info"></param>
    public void AddItemForPlayer(PlayerItemInfo info)
    {
        //根据玩家道具列表的id得到物品
        Item item = GameDataMgr.GetInstance().GetItemInfo(info.id);

        //默认自动添加到AllItems中
        //查找AllItems里是否已经存在相同的道具
        PlayerItemInfo existingItem = AllItems.Find(itemInfo => itemInfo.id == info.id);
        if (existingItem != null)
        {
            //如果已经存在该对象，则在所有物品里不改变它的数量，在它所属的类别内更改即可
        }
        else
        {
            // 否则，将新的PlayerItemInfo对象添加到AllItems中
            AllItems.Add(info);
        }

        //然后根据类别添加到各个子类中
        switch (item.Type)
        {
            //主动技能书
            case "ActiveSkillBook":
                PlayerItemInfo existingASB = activeSkillBooks.Find(itemInfo => itemInfo.id == info.id);
                if (existingASB != null)
                {
                    // 如果已存在相同的PlayerItemInfo对象，则只修改数量
                    existingASB.number += 1;
                }
                else
                {
                    // 否则，将新的PlayerItemInfo对象添加到ActiveSkillBook中
                    activeSkillBooks.Add(info);
                }
                break;
            //被动技能书
            case "PassiveSkillBook":
                PlayerItemInfo existingPSB = passiveSkillBooks.Find(itemInfo => itemInfo.id == info.id);
                if (existingPSB != null)
                {
                    // 如果已存在相同的PlayerItemInfo对象，则只修改数量
                    existingPSB.number += 1;
                }
                else
                {
                    // 否则，将新的PlayerItemInfo对象添加到PassiveSkillBook中
                    passiveSkillBooks.Add(info);
                }
                break;
            //物资
            case "Material":
                PlayerItemInfo existingMaterial = Materials.Find(itemInfo => itemInfo.id == info.id);
                if (existingMaterial != null)
                {
                    // 如果已存在相同的PlayerItemInfo对象，则只修改数量
                    existingMaterial.number += 1;
                }
                else
                {
                    // 否则，将新的PlayerItemInfo对象添加到Material中
                    Materials.Add(info);
                }
                break;
        }
    }

    /// <summary>
    /// 为玩家道具列表添加指定id道具的函数
    /// </summary>
    /// <param name="id">道具ID</param>
    /// <param name="number">道具数量</param>
    /// <returns></returns>
    public PlayerItemInfo AddPlayerItemInfo(int id, int number)
    {
        PlayerItemInfo itemA = new PlayerItemInfo();
        itemA.id = id;
        itemA.number = number;
        return itemA;
    }
}



/// <summary>
/// 玩家拥有的道具信息
/// </summary>
public class PlayerItemInfo
{
    public int id;
    public int number;
}

//用于读取Json中所有道具配置信息的数据结构
public class ItemInfo
{
    public List<Item> itemInfo;
}

//装载道具每个数据的类，名字必须与Json文件键值对应
[System.Serializable]
public class Item
{
    public int ID;
    public string Name;
    public string Icon;
    public string Type;
    public int Price;
    public string Tips;
    public int OpenChapter;
    public int Number;
    public string SkillName;
}

//用于读取Json中所有将领配置信息的数据结构
public class GeneralInfo
{
    public List<General> generalInfo;
}

/// <summary>
///将领的基本信息 
/// </summary>

[System.Serializable]
public class General
{
    public int GeneralID;
    public string GeneralName;
    public string GeneralKey;
    public int Level;
    public int CurrentEXP;
    public int MaxEXP;
    public int Strength;
    public int LeaderShip;
    public int Wisdom;
    public int Attack;
    public int Defense;
    public int Hp;
    public int LevelHealthAdjustment;
    public int LevelAttackAdjustment;
    public int LeaderShipHealthAdjustment;
    public int CurrentHp;
    public int StratageDamage;
    public double CriticalRate;
    public string CurrentActiveSkills;
    public string CurrentPassiveSkills;
    public int ActiveSkillCount;
    public int PassiveSkillCount;
    public int UnassignedAttributePoints;
    public int MaxAttributePoints;
    public string TroopSkill;

    public Dictionary<string, Troop> GeneralOwnedTroop;
    public Troop currentSelectTroop;
    
    public List<string> PossessedActiveSkills;
    public List<string> PossessedPassiveSkills;

    public List<string> CurrentSelectedActiveSkills;
    public List<string> CurrentSelectedPassiveSkills;

    /// <summary>
    /// General的无参构造函数，使用有参构造函数前必须建立无参构造函数
    /// </summary>
    public General()
    {
        GeneralID = 0;  // 默认的 GeneralID
        GeneralName = "Default Name";  // 默认的 GeneralName
        GeneralKey = "DefaultKey";
        Level = 1;  // 默认的 Level
        CurrentEXP = 0;
        MaxEXP = 0;
        Strength = 0;
        LeaderShip = 0;
        Wisdom = 0;
        Attack = 0;
        Defense = 0;
        Hp = 0;
        LevelHealthAdjustment = 0;
        LevelAttackAdjustment = 0;

        LeaderShipHealthAdjustment = 0;
        CurrentHp = 0;
        StratageDamage = 0;
        CriticalRate = 0;
        CurrentActiveSkills = "Default";
        CurrentPassiveSkills = "Default";
        ActiveSkillCount = 0;
        PassiveSkillCount = 0;
        UnassignedAttributePoints = 0;
        MaxAttributePoints = 3;
        TroopSkill = "Default";

        GeneralOwnedTroop = new Dictionary<string, Troop>();
        PossessedActiveSkills = new List<string>();
        PossessedPassiveSkills = new List<string>();
        currentSelectTroop = new Troop();

        CurrentSelectedActiveSkills = new List<string>();
        CurrentSelectedPassiveSkills = new List<string>();
    }
    /// <summary>
    /// General的有参构造函数，对将军的预设在这里进行
    /// </summary>
    /// <param name="generalID"></param>
    public General(int generalID)
    {
        GeneralID = generalID;

        // 从 GameDataMgr 中获取相应的 GeneralInfo 字典
        Dictionary<int, General> generalInfoDic = GameDataMgr.GetInstance().generalInfoDic;

        // 根据 GeneralID 从 generalInfoDic 字典中获取对应的数据
        if (generalInfoDic.ContainsKey(generalID))
        {
            General generalData = generalInfoDic[generalID];

            // 将获取到的数据赋值给 General 对象的属性
            GeneralName = generalData.GeneralName;
            GeneralKey = generalData.GeneralKey;
            Level = generalData.Level;
            CurrentEXP = generalData.CurrentEXP;
            MaxEXP = generalData.MaxEXP;
            Strength = generalData.Strength;
            LeaderShip = generalData.LeaderShip;
            Wisdom = generalData.Wisdom;
            Attack = generalData.Attack;
            Defense = generalData.Defense;
            Hp = generalData.Hp;
            CurrentHp = generalData.CurrentHp;
            StratageDamage = generalData.StratageDamage;
            CriticalRate = generalData.CriticalRate;
            CurrentActiveSkills = generalData.CurrentActiveSkills;
            CurrentPassiveSkills = generalData.CurrentPassiveSkills;
            ActiveSkillCount = Wisdom/6+1;
            PassiveSkillCount = Level/5+1;
            UnassignedAttributePoints = generalData.UnassignedAttributePoints;
            MaxAttributePoints = 3;
            TroopSkill = "Default";

            // 进行其他需要的属性赋值操作
            // 初始化玩家拥有的部队和技能列表
            GeneralOwnedTroop = new Dictionary<string, Troop>();
            
            PossessedActiveSkills = new List<string>();
            PossessedPassiveSkills = new List<string>();

            MaxEXP = GameDataMgr.GetInstance().expInfodic[Level + 1].NeedEXP;
            //设定等级生命值成长倍率和统率带来的生命值加成的倍率
            LevelHealthAdjustment = 20 + LeaderShip * 2;
            LeaderShipHealthAdjustment = 20;
            LevelAttackAdjustment = 2;

            //给玩家添加基础兵种，轻弓/戟/弩/骑/刀盾
            //添加弓箭乡勇
            GeneralOwnedTroop.Add("LightArrow", GameDataMgr.GetInstance().GetTroopInfo(0));
            //添加持弩乡勇
            GeneralOwnedTroop.Add("LightCrossbow", GameDataMgr.GetInstance().GetTroopInfo(1));
            //添加持戟乡勇
            GeneralOwnedTroop.Add("LightHalberd", GameDataMgr.GetInstance().GetTroopInfo(2));
            //添加刀牌乡勇
            GeneralOwnedTroop.Add("LightSword", GameDataMgr.GetInstance().GetTroopInfo(3));
            //添加枪骑乡勇
            GeneralOwnedTroop.Add("LightCavalry", GameDataMgr.GetInstance().GetTroopInfo(4));

            //给武将添加Json传入的固有的技能
            PossessedActiveSkills.AddRange(SplitString(CurrentActiveSkills));
            PossessedPassiveSkills.AddRange(SplitString(CurrentPassiveSkills));

            currentSelectTroop = GeneralOwnedTroop["LightCrossbow"];

            CurrentSelectedActiveSkills = new List<string>();
            CurrentSelectedPassiveSkills = new List<string>();

        }
    }
    /// <summary>
    /// 根据逗号分割string的方法，用于将传入的多个技能用逗号分割开，再分别存入将军拥有的技能中
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public string[] SplitString(string target)
    {
        string[] results = target.Split(',');
        if (results.Length > 1)
        {
            return results;
        }
        return new string[] { target };
    }
}

/// <summary>
/// 用于读取Json中所有部队配置信息的数据结构
/// </summary>
public class TroopInfo
{
    public List<Troop> troopInfo;
}

/// <summary>
/// 部队的基本信息
/// </summary>
[System.Serializable]
public class Troop
{
    public int TroopID;
    public string TroopIcon;
    public List<int> TroopNextLevelID;
    public string TroopName;
    public int Movement;
    public int CurrentMovement;
    public float TroopDamageMagnification;
    public float TroopDefenseMagnification;
    public int Morale;
    public int CurrentMorale;
    public string AttackRange;
    public int AttackEffectiveRange;
    public float DamageBonusRate;
    public float DefenseBonusRate;
    public string TroopSkill;
    public string TroopEntry;
    public string ProficientCondition;
    public int ProficientOrNot;
    public string TroopFeature;
}

/// <summary>
/// 用于读取经验值表的数据结构
/// </summary>
public class EXPInfo
{
    public List<EXP> expInfo;
}

/// <summary>
/// 经验值表的详细信息
/// </summary>
[System.Serializable]
public class EXP
{
    //所需要的经验值
    public int NeedEXP;
    //目标等级，对应所需要的经验值，比如目标等级是2的话，所需要经验值就是从1升级到2级需要的经验
    public int TargetLevel;
    //击杀一个地方单位可以得到的经验值
    public int EnemyEXP;
    //地方单位对应的等级，这个等级存储时和TargetLevel相同，也就是和ID相同
    //所以如果想获得击杀X等级的敌军的经验值，可以通过检索和X相同的ID来获得
    public int EnemyLevel;
}

/// <summary>
/// 用于读取兵种升级所需物资表的数据结构
/// </summary>
public class TroopLevelUpInfo
{
    public List<TroopLevelUp> troopLevelUpInfo;
}

/// <summary>
/// 兵种升级所需物资表的详细信息
/// </summary>
[System.Serializable]
public class TroopLevelUp
{
    //物资名称，中文
    public List<string> MaterialName;
    //物资对应ID
    public List<int> MaterialID;
    //物资数量
    public List<int> Number;
    //升级的目标兵种
    public string TroopName;
    //目标兵种ID
    public int TroopID;
}