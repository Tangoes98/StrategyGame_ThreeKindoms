using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressRecord_Intro2 : MonoBehaviour
{

    void Start()
    {

        //���ݵ�ǰ�����Ĺ��³�����¼�½ڼ��׶�
        GameDataMgr.GetInstance().PlayerDataInfo.currentChapter = 2;
        //�͵�ǰ�����ĳ�������һ��
        GameDataMgr.GetInstance().PlayerDataInfo.currentPhase = "Intro_Chapter2";
        //��������
        GameDataMgr.GetInstance().PlayerDataInfo.date = "188����,����ƽ����";
        //���������¼�
        EventCenter.GetInstance().AddEventListener("SavePlayerInfo", SavePlayerInfo);
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener("SavePlayerInfo", SavePlayerInfo);
    }
    public void SavePlayerInfo()
    {
        //���ݵ�ǰ�����Ĺ��³�����¼�½ڼ��׶�
        GameDataMgr.GetInstance().PlayerDataInfo.currentChapter = 2;
        //�͵�ǰ�����ĳ�������һ��
        GameDataMgr.GetInstance().PlayerDataInfo.currentPhase = "Intro_Chapter2";

        //�����¼�
        EventCenter.GetInstance().EventTrigger<int, string>("SendCurrentChapter", 1, GameDataMgr.GetInstance().PlayerDataInfo.date);
    }
}

