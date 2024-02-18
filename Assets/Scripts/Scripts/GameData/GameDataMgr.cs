using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GameDataMgr : BaseManager<GameDataMgr>
{
    //�ֵ�洢��Ҵ�Json��ȡ������Ʒ����
    public Dictionary<int, Item> itemInfoDic = new Dictionary<int, Item>();
    //�ֵ�洢��Ҵ�Json��ȡ�����佫����
    public Dictionary<int, General> generalInfoDic = new Dictionary<int, General>();
    //�ֵ�洢��Ҵ�Json��ȡ���ı�������
    public Dictionary<int, Troop> troopInfoDic = new Dictionary<int, Troop>();
    //���ֵ�洢��Ҵ�Json��ȡ���ľ���ֵ��
    public Dictionary<int, EXP> expInfodic = new Dictionary<int, EXP>();
    //���ֵ�洢��Ҵ�Json��ȡ���ı��������������ʱ�
    public Dictionary<int, TroopLevelUp> troopLevelUpInfodic = new Dictionary<int, TroopLevelUp>();

    //��ҵĴ洢·��
    private static string PlayerInfoSaveAdress = Application.persistentDataPath + "/PlayerSaveData.json";

    //��ҵ�������Ϣ
    public PlayerInfo PlayerDataInfo;

    public bool isParseDataExecuted = false;

    /// <summary>
    /// �����洢���ݵ�Json��Ϣ
    /// </summary>
    public void ParseData()
    {

        //��ȡ������Json�ļ�//��Ʒ��Ϣ
        ItemInfo itemInfos = JsonMgr.Instance.LoadData<ItemInfo>("ItemInfo");
        //�����ݼ��ϰ���ID�ŷֱ����
        for (int i = 0; i < itemInfos.itemInfo.Count; ++i)
        {
            itemInfoDic.Add(itemInfos.itemInfo[i].ID, itemInfos.itemInfo[i]);
        }

        //��ȡ������Json�ļ�//������Ϣ
        GeneralInfo generalInfos = JsonMgr.Instance.LoadData<GeneralInfo>("GeneralInfo");
        //�����ݼ��ϰ���ID�ŷֱ����
        for (int i = 0; i < generalInfos.generalInfo.Count; ++i)
        {
            generalInfoDic.Add(generalInfos.generalInfo[i].GeneralID, generalInfos.generalInfo[i]);
        }

        //��ȡ������Json�ļ�//������Ϣ
        TroopInfo troopInfos = JsonMgr.Instance.LoadData<TroopInfo>("TroopInfo");
        //�����ݼ��ϰ���ID�ŷֱ����
        for (int i = 0; i < troopInfos.troopInfo.Count; ++i)
        {
            troopInfoDic.Add(troopInfos.troopInfo[i].TroopID, troopInfos.troopInfo[i]);
        }

        //��ȡ������Json�ļ�//����ֵ��Ϣ
        EXPInfo expInfos = JsonMgr.Instance.LoadData<EXPInfo>("EXPInfo");
        //�����ݼ��ϰ���ID�ŷֱ����
        for (int i = 0; i < expInfos.expInfo.Count; ++i)
        {
            expInfodic.Add(expInfos.expInfo[i].TargetLevel, expInfos.expInfo[i]);
        }

        //��ȡ������Json�ļ�//����ֵ��Ϣ
        TroopLevelUpInfo troopLevelUpInfos = JsonMgr.Instance.LoadData<TroopLevelUpInfo>("TroopLevelUpInfo");
        //�����ݼ��ϰ���ID�ŷֱ����
        for (int i = 0; i < troopLevelUpInfos.troopLevelUpInfo.Count; ++i)
        {
            troopLevelUpInfodic.Add(troopLevelUpInfos.troopLevelUpInfo[i].TroopID, troopLevelUpInfos.troopLevelUpInfo[i]);
        }

        //û���������ʱ����ʼ��һ��Ĭ������
        if (PlayerDataInfo == null)
        {
            PlayerDataInfo = new PlayerInfo();
            //�洢��
            JsonMgr.Instance.SaveData(PlayerDataInfo, "PlayerSaveData");
        }
        //File.WriteAllBytes(PlayerInfoSaveAdress, Encoding.UTF8.GetBytes(xxx));

        ////////////////////////�¼������������ᴩ������Ϸ�����ݱ仯///////////////////////////////////////////////////////////
        ///��Щ��������ɾ����ÿ��ִ��һ��ParseData�ͻᵼ����ҵĴ浵�仯�����⣬һ��Ҫ�ž�
        //ͨ���¼���������Ǯ�Ƿ��б仯
        EventCenter.GetInstance().AddEventListener<int>("MoneyChange", ChangeMoney);
        //ͨ���¼������ȼ��Ƿ��б仯
        EventCenter.GetInstance().AddEventListener<string, int>("GeneralLevelUp", GeneralLevelUp);
        //ͨ���¼��������Ե��Ƿ��б仯
        EventCenter.GetInstance().AddEventListener<string, string, int>("AssignAttributePoint", AssignAttributePoint);
        //ͨ���¼������Ƿ�浵
        EventCenter.GetInstance().AddEventListener("SavePlayerInfo", SavePlayerInfo);
        //ͨ���¼������������Ը������Ե�仯       
        EventCenter.GetInstance().AddEventListener<string,int,int,int>("AttributeChangeConfirmed", AttributeChangeConfirm);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //ִ����󣬽���ǰParseDataִ��״̬����Ϊ�Ѿ�ִ�й�
        isParseDataExecuted = true;
    }
    /// <summary>
    /// �������ʹ��GameDataMgr���ý�Ǯ�ı�
    /// </summary>
    /// <param name="money"></param>
    private void ChangeMoney(int money)
    {
        PlayerDataInfo.ChangeMoney(money);
        //����Ǯ�ʹ洢����
        EventCenter.GetInstance().EventTrigger("SavePlayerInfo");
    }

    /// <summary>
    /// �������ʹ��GameDataMgr���ö������Ըı�
    /// </summary>
    /// <param name="generalKey"></param>
    private void AttributeChangeConfirm(string generalKey,int AlreadyAssignedPointCountS,int AlreadyAssignedPointCountL,int AlreadyAssignedPointCountW)
    {
        PlayerDataInfo.AttributeChangeConfirm(generalKey, AlreadyAssignedPointCountS, AlreadyAssignedPointCountL, AlreadyAssignedPointCountW);
        //���Ըı䣬�洢����
        EventCenter.GetInstance().EventTrigger("SavePlayerInfo");
    }

    /// <summary>
    /// �������ʹ��GameDataMgr���õȼ��ı�
    /// </summary>
    /// <param name="generalKey"></param>
    /// <param name="excessEXP"></param>
    private void GeneralLevelUp(string generalKey, int excessEXP)
    {

        PlayerDataInfo.GeneralLevelUp(generalKey, excessEXP);
        //�����ʹ洢����
        EventCenter.GetInstance().EventTrigger("SavePlayerInfo");
    }
    /// <summary>
    /// �������ʹ��GameDataMgr���ý������Ե�ı�
    /// </summary>
    /// <param name="generalKey"></param>
    /// <param name="attributeType"></param>
    /// <param name="number"></param>
    private void AssignAttributePoint(string generalKey, string attributeType, int number)
    {
        PlayerDataInfo.AssignAttributePoint(generalKey, attributeType, number);
        //�������Ե�ʹ洢����
        EventCenter.GetInstance().EventTrigger("SavePlayerInfo");
    }

    /// <summary>
    /// �������ʹ��GameDataMgr���ô浵
    /// ���������Ϣ----�Ժ�����Ϊ�Զ��浵/���ٴ浵
    /// </summary>
    public void SavePlayerInfo()
    {
        JsonMgr.Instance.SaveData(PlayerDataInfo, "PlayerSaveData");
    }


    /// <summary>
    /// ��ȡ�����Ϣ----�Ժ�����Ϊ���ٶ�ȡ����浵
    /// </summary>
    public void LoadPlayerInfo()
    {
        if (File.Exists(PlayerInfoSaveAdress))
        {
            //��ȡ��Ϊ�浵��Json�ļ�
            PlayerDataInfo = JsonMgr.Instance.LoadData<PlayerInfo>("PlayerSaveData");
        }
    }


    //δ����Ҫһ���ֶ��浵�ķ�ʽ
    //δ����Ҫһ���ֶ������ķ�ʽ

    /// <summary>
    /// ���ݵ���ID��õ�����Ϣ
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
    /// �����佫ID����佫��Ϣ
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
    /// ���ݲ�����Ϣ��ò�����Ϣ
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
/// ��ҵ����е��ߺ���Դ
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
        //��ʼ��ݣ��Ժ��ڹ��½׶θ������
        date = "184����,���������";
        //�����ʼ��Ǯ
        money = 1000;
        //�����ʼ�½ڣ�һ����Stroy��������
        currentChapter = 0;
        //�����ʼ�׶Σ�һ��ÿ�¿�ͷ���ھ���׶Σ�ÿ�ν����µĳ������͸���Ϊ��Ӧ�Ľ׶�
        currentPhase = "Story0";

        AllItems = new List<PlayerItemInfo>();
        activeSkillBooks = new List<PlayerItemInfo>();
        passiveSkillBooks = new List<PlayerItemInfo>();
        Materials = new List<PlayerItemInfo>();

        //��¼�ؿ�ͨ�����
        C1_MainLevel1IsFinished = false;
        C1_SubLevel1IsFinished = false;

        C2_MainLevel1IsFinished = false;
        C2_SubLevel1IsFinished = false;


        //////////////////////////////////////////////////////////////////////////////////////////////
        //��������ӵ��߾���������������¶��ǲ����õ�/////////////////////////
        //activeSkillBooks.Add(AddPlayerItemInfo(1, 2));
        //��һ����������ƷID���ڶ����������������
        AddItemForPlayer(AddPlayerItemInfo(1, 2));
        AddItemForPlayer(AddPlayerItemInfo(50, 3));
        AddItemForPlayer(AddPlayerItemInfo(66, 5));
        AddItemForPlayer(AddPlayerItemInfo(68, 3));
        /////////////////////////////////////////////////////////////////////

        PlayerOwnedGeneral = new Dictionary<string, General>();

        //�������ӳ�ʼ������������
        //ͨ��GetGeneralInfo���뽫����Id�õ��洢��ȫ�������ֵ��еĽ�����Ȼ��洢Ϊ��ҵĽ���
        //�������
        //�����ý���IDʹ�ñ����ڹ��캯����(Json�ж�ȡ������)��ֵ��ʼ��һ������
        //��Ϊ������ID��0�����Դ���0����ΪJson����0Ϊ��ʼֵ
        General LiuBei = new General(0);
        //����ʼ����Ľ�����ӵ������ӵ�еĽ����ֵ䣬����Ӣ������Ϊ�����ڵ���ͷ��������ز�ʱ����ֱ��ʹ�ü�������
        PlayerOwnedGeneral.Add("LiuBei",LiuBei);

        currentSelectedGeneral = PlayerOwnedGeneral["LiuBei"];
        //////////////////////ֱ����Ӽ��ܵķ������������ܻ���ʹ�ü�����ʱʹ��////////////////////////////////////////////////////
        PlayerOwnedGeneral["LiuBei"].PossessedActiveSkills.Add("Arson Fire");

        //��ӹ���ͬ����
        General GuanYu = new General(1);
        PlayerOwnedGeneral.Add("GuanYu", GuanYu);


        PlayerOwnedGeneral["GuanYu"].PossessedPassiveSkills.Add("Good Surrounder");
        PlayerOwnedGeneral["GuanYu"].PossessedPassiveSkills.Add("Good Collaborator");

        //����ŷɣ�ͬ����
        General ZhangFei = new General(2);
        PlayerOwnedGeneral.Add("ZhangFei", ZhangFei);
    }

    /// <summary>
    /// �������Ե�ĺ���
    /// </summary>
    /// <param name="generalKey"></param>
    /// <param name="attributeType"></param>
    /// <param name="number"></param>
    public void AssignAttributePoint(string generalKey, string attributeType, int number)
    {
        //ÿ�η����Ӧ���Լ�number
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
        //ÿ�η����佫��δ�������Ե�ҲҪ��ȥnumber
        PlayerOwnedGeneral[generalKey].UnassignedAttributePoints -= number;
    }
    /// <summary>
    /// ȷ�����Է������ҵĶ������Ըı�
    /// </summary>
    /// <param name="generalKey"></param>
    public void AttributeChangeConfirm(string generalKey,int AlreadyAssignedPointCountS, int AlreadyAssignedPointCountL, int AlreadyAssignedPointCountW)
    {
        //ÿ�η������Ժ���¸��佫�������Ϣ
        
        //���´��������Ա仯
        //����������
        PlayerOwnedGeneral[generalKey].Attack += AlreadyAssignedPointCountS * 10;
        PlayerOwnedGeneral[generalKey].CriticalRate += 0.01 * AlreadyAssignedPointCountS;

        //ͳ�ʴ��������Ա仯
        //ͳ�ʴ����ķ���ֵ����
        PlayerOwnedGeneral[generalKey].Defense += AlreadyAssignedPointCountL * 10;
        //�佫�ȼ���������ֵ����
        PlayerOwnedGeneral[generalKey].LevelHealthAdjustment = 20 + PlayerOwnedGeneral[generalKey].LeaderShip * 2;
        //�佫ͳ����ɵı���ֵ����
        //����ͳ�ʻ�õ�����ֵ������Ϊ������ֵͳ�ʵ���ֵ*���ӵ�ͳ��
        PlayerOwnedGeneral[generalKey].Hp += AlreadyAssignedPointCountL * PlayerOwnedGeneral[generalKey].LeaderShipHealthAdjustment;
        PlayerOwnedGeneral[generalKey].CurrentHp = PlayerOwnedGeneral[generalKey].Hp;

        //��ı���������Ա仯
        //��ı�˺�����
        PlayerOwnedGeneral[generalKey].StratageDamage += AlreadyAssignedPointCountW * 10;
        //��ı�����Ŀ�Я��������������������
        PlayerOwnedGeneral[generalKey].ActiveSkillCount = (PlayerOwnedGeneral[generalKey].Wisdom / 6) + 1;


        EventCenter.GetInstance().EventTrigger<General>("ToggleChanged", PlayerOwnedGeneral[generalKey]);
    }

    /// <summary>
    /// ���������ĺ���
    /// </summary>
    /// <param name="GeneralKey">�����keyֵ</param>
    public void GeneralLevelUp(string generalKey, int excessEXP)
    {
        //��ǰ�ȼ���1
        PlayerOwnedGeneral[generalKey].Level += 1;
        //�ȼ��������3��ɷ������Ե�
        PlayerOwnedGeneral[generalKey].UnassignedAttributePoints += 3;
        //�ȼ�����ֵ������ߣ�������ֵ���ӣ�
        PlayerOwnedGeneral[generalKey].MaxEXP = GameDataMgr.GetInstance().expInfodic[PlayerOwnedGeneral[generalKey].Level + 1].NeedEXP;
        //��յ�ǰ����ֵ����������ʱ��ʣ�ྭ��
        PlayerOwnedGeneral[generalKey].CurrentEXP = excessEXP;
        
        //�ȼ�����������HP��������
        PlayerOwnedGeneral[generalKey].Hp += GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[generalKey].LevelHealthAdjustment;
        //ͳ�ʴ���������ֵ�ɳ�ֵΪ20������ֵͳ�ʵ���ֵ��
        PlayerOwnedGeneral[generalKey].LeaderShipHealthAdjustment = 20;
        PlayerOwnedGeneral[generalKey].CurrentHp = PlayerOwnedGeneral[generalKey].Hp;
        //�ȼ������Ĺ���������
        PlayerOwnedGeneral[generalKey].Attack += PlayerOwnedGeneral[generalKey].LevelAttackAdjustment;
        //�ȼ������ı���������������
        PlayerOwnedGeneral[generalKey].PassiveSkillCount = (PlayerOwnedGeneral[generalKey].Level / 5) + 1;

        EventCenter.GetInstance().EventTrigger<General>("ToggleChanged", PlayerOwnedGeneral[generalKey]);
    }


    /// <summary>
    /// �ı���ҵ�ǰǮ���ĺ���
    /// </summary>
    /// <param name="money">��Ǯ���߼�Ǯ������</param>
    public void ChangeMoney(int money)
    {
        //�ж���ҵ�Ǯ��������ȥ��������ָ���
        //��������monyС��0����ζ�������ִ�л�Ǯ�Ķ���
        if (money < 0 && this.money < money)
            return;
        //�ı���ҵ�Ǯ
        this.money += money;
    }

    /// <summary>
    /// �����ҵ����б������Ϣ���Ѿ����ڵ��������͵ĵ�����
    /// </summary>
    /// <param name="info"></param>
    public void AddItemForPlayer(PlayerItemInfo info)
    {
        //������ҵ����б��id�õ���Ʒ
        Item item = GameDataMgr.GetInstance().GetItemInfo(info.id);

        //Ĭ���Զ���ӵ�AllItems��
        //����AllItems���Ƿ��Ѿ�������ͬ�ĵ���
        PlayerItemInfo existingItem = AllItems.Find(itemInfo => itemInfo.id == info.id);
        if (existingItem != null)
        {
            //����Ѿ����ڸö�������������Ʒ�ﲻ�ı�������������������������ڸ��ļ���
        }
        else
        {
            // ���򣬽��µ�PlayerItemInfo������ӵ�AllItems��
            AllItems.Add(info);
        }

        //Ȼ����������ӵ�����������
        switch (item.Type)
        {
            //����������
            case "ActiveSkillBook":
                PlayerItemInfo existingASB = activeSkillBooks.Find(itemInfo => itemInfo.id == info.id);
                if (existingASB != null)
                {
                    // ����Ѵ�����ͬ��PlayerItemInfo������ֻ�޸�����
                    existingASB.number += 1;
                }
                else
                {
                    // ���򣬽��µ�PlayerItemInfo������ӵ�ActiveSkillBook��
                    activeSkillBooks.Add(info);
                }
                break;
            //����������
            case "PassiveSkillBook":
                PlayerItemInfo existingPSB = passiveSkillBooks.Find(itemInfo => itemInfo.id == info.id);
                if (existingPSB != null)
                {
                    // ����Ѵ�����ͬ��PlayerItemInfo������ֻ�޸�����
                    existingPSB.number += 1;
                }
                else
                {
                    // ���򣬽��µ�PlayerItemInfo������ӵ�PassiveSkillBook��
                    passiveSkillBooks.Add(info);
                }
                break;
            //����
            case "Material":
                PlayerItemInfo existingMaterial = Materials.Find(itemInfo => itemInfo.id == info.id);
                if (existingMaterial != null)
                {
                    // ����Ѵ�����ͬ��PlayerItemInfo������ֻ�޸�����
                    existingMaterial.number += 1;
                }
                else
                {
                    // ���򣬽��µ�PlayerItemInfo������ӵ�Material��
                    Materials.Add(info);
                }
                break;
        }
    }

    /// <summary>
    /// Ϊ��ҵ����б����ָ��id���ߵĺ���
    /// </summary>
    /// <param name="id">����ID</param>
    /// <param name="number">��������</param>
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
/// ���ӵ�еĵ�����Ϣ
/// </summary>
public class PlayerItemInfo
{
    public int id;
    public int number;
}

//���ڶ�ȡJson�����е���������Ϣ�����ݽṹ
public class ItemInfo
{
    public List<Item> itemInfo;
}

//װ�ص���ÿ�����ݵ��࣬���ֱ�����Json�ļ���ֵ��Ӧ
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

//���ڶ�ȡJson�����н���������Ϣ�����ݽṹ
public class GeneralInfo
{
    public List<General> generalInfo;
}

/// <summary>
///����Ļ�����Ϣ 
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
    /// General���޲ι��캯����ʹ���вι��캯��ǰ���뽨���޲ι��캯��
    /// </summary>
    public General()
    {
        GeneralID = 0;  // Ĭ�ϵ� GeneralID
        GeneralName = "Default Name";  // Ĭ�ϵ� GeneralName
        GeneralKey = "DefaultKey";
        Level = 1;  // Ĭ�ϵ� Level
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
    /// General���вι��캯�����Խ�����Ԥ�����������
    /// </summary>
    /// <param name="generalID"></param>
    public General(int generalID)
    {
        GeneralID = generalID;

        // �� GameDataMgr �л�ȡ��Ӧ�� GeneralInfo �ֵ�
        Dictionary<int, General> generalInfoDic = GameDataMgr.GetInstance().generalInfoDic;

        // ���� GeneralID �� generalInfoDic �ֵ��л�ȡ��Ӧ������
        if (generalInfoDic.ContainsKey(generalID))
        {
            General generalData = generalInfoDic[generalID];

            // ����ȡ�������ݸ�ֵ�� General ���������
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

            // ����������Ҫ�����Ը�ֵ����
            // ��ʼ�����ӵ�еĲ��Ӻͼ����б�
            GeneralOwnedTroop = new Dictionary<string, Troop>();
            
            PossessedActiveSkills = new List<string>();
            PossessedPassiveSkills = new List<string>();

            MaxEXP = GameDataMgr.GetInstance().expInfodic[Level + 1].NeedEXP;
            //�趨�ȼ�����ֵ�ɳ����ʺ�ͳ�ʴ���������ֵ�ӳɵı���
            LevelHealthAdjustment = 20 + LeaderShip * 2;
            LeaderShipHealthAdjustment = 20;
            LevelAttackAdjustment = 2;

            //�������ӻ������֣��ṭ/�/��/��/����
            //��ӹ�������
            GeneralOwnedTroop.Add("LightArrow", GameDataMgr.GetInstance().GetTroopInfo(0));
            //��ӳ�������
            GeneralOwnedTroop.Add("LightCrossbow", GameDataMgr.GetInstance().GetTroopInfo(1));
            //��ӳ������
            GeneralOwnedTroop.Add("LightHalberd", GameDataMgr.GetInstance().GetTroopInfo(2));
            //��ӵ�������
            GeneralOwnedTroop.Add("LightSword", GameDataMgr.GetInstance().GetTroopInfo(3));
            //���ǹ������
            GeneralOwnedTroop.Add("LightCavalry", GameDataMgr.GetInstance().GetTroopInfo(4));

            //���佫���Json����Ĺ��еļ���
            PossessedActiveSkills.AddRange(SplitString(CurrentActiveSkills));
            PossessedPassiveSkills.AddRange(SplitString(CurrentPassiveSkills));

            currentSelectTroop = GeneralOwnedTroop["LightCrossbow"];

            CurrentSelectedActiveSkills = new List<string>();
            CurrentSelectedPassiveSkills = new List<string>();

        }
    }
    /// <summary>
    /// ���ݶ��ŷָ�string�ķ��������ڽ�����Ķ�������ö��ŷָ���ٷֱ���뽫��ӵ�еļ�����
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
/// ���ڶ�ȡJson�����в���������Ϣ�����ݽṹ
/// </summary>
public class TroopInfo
{
    public List<Troop> troopInfo;
}

/// <summary>
/// ���ӵĻ�����Ϣ
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
/// ���ڶ�ȡ����ֵ������ݽṹ
/// </summary>
public class EXPInfo
{
    public List<EXP> expInfo;
}

/// <summary>
/// ����ֵ�����ϸ��Ϣ
/// </summary>
[System.Serializable]
public class EXP
{
    //����Ҫ�ľ���ֵ
    public int NeedEXP;
    //Ŀ��ȼ�����Ӧ����Ҫ�ľ���ֵ������Ŀ��ȼ���2�Ļ�������Ҫ����ֵ���Ǵ�1������2����Ҫ�ľ���
    public int TargetLevel;
    //��ɱһ���ط���λ���Եõ��ľ���ֵ
    public int EnemyEXP;
    //�ط���λ��Ӧ�ĵȼ�������ȼ��洢ʱ��TargetLevel��ͬ��Ҳ���Ǻ�ID��ͬ
    //����������û�ɱX�ȼ��ĵо��ľ���ֵ������ͨ��������X��ͬ��ID�����
    public int EnemyLevel;
}

/// <summary>
/// ���ڶ�ȡ���������������ʱ�����ݽṹ
/// </summary>
public class TroopLevelUpInfo
{
    public List<TroopLevelUp> troopLevelUpInfo;
}

/// <summary>
/// ���������������ʱ����ϸ��Ϣ
/// </summary>
[System.Serializable]
public class TroopLevelUp
{
    //�������ƣ�����
    public List<string> MaterialName;
    //���ʶ�ӦID
    public List<int> MaterialID;
    //��������
    public List<int> Number;
    //������Ŀ�����
    public string TroopName;
    //Ŀ�����ID
    public int TroopID;
}