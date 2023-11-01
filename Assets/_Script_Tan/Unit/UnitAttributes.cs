using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAttributes : MonoBehaviour
{
    public enum GENERAL_NAMES
    {
        LiuBei,
        GuanYu,
        ZhangFei,
        JianYong,
        ZhangBao,
        ZhangLiang,
        ChengYuanZhi,
        GaoSheng,
        DengMao,
        SunZhong,
        HuangJinQuShuai,
    }

    [SerializeField] GENERAL_NAMES generalNameEnumList;


    [SerializeField] int generalID;
    [SerializeField] string generalName;
    [SerializeField] string generalKey;
    [SerializeField] int level;
    [SerializeField] float currentExp;
    [SerializeField] float maxExp;
    [SerializeField] int strength;
    [SerializeField] int leaderShip;
    [SerializeField] int wisdom;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] float Hp;
    [SerializeField] float currentHp;
    [SerializeField] float stratageDamage;
    [SerializeField] float criticalRate;
    [SerializeField] string currentActiveSkill;
    [SerializeField] string currentPassiveSkill;
    [SerializeField] int activeSkillCount;
    [SerializeField] int passiveSkillCount;



    void Start()
    {
        Debug.Log((int)generalNameEnumList);

        General general = GameDataMgr.GetInstance().GetGeneralInfo((int)generalNameEnumList);

        Debug.Log(general.Level);

        generalID = general.GeneralID;
        generalName = general.GeneralName;
        generalKey = general.GeneralKey;
        level = general.Level;
        currentExp = general.CurrentEXP;
        maxExp = general.MaxEXP;
        strength = general.Strength;
        leaderShip = general.LeaderShip;
        wisdom = general.Wisdom;
        attack = general.Attack;
        defense = general.Defense;
        Hp = general.Hp;
        currentHp = general.CurrentHp;
        stratageDamage = general.StratageDamage;
        criticalRate = (float)general.CriticalRate;
        currentActiveSkill = general.CurrentActiveSkills;
        currentPassiveSkill = general.CurrentPassiveSkills;
        activeSkillCount = general.ActiveSkillCount;
        passiveSkillCount = general.PassiveSkillCount;


    }









}
