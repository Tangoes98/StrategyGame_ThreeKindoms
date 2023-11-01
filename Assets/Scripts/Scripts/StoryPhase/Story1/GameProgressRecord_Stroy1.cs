using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressRecord_Stroy1 : MonoBehaviour
{
    void Start()
    {

        //���ݵ�ǰ�����Ĺ��³�����¼�½ڼ��׶�
        GameDataMgr.GetInstance().PlayerDataInfo.currentChapter = 1;
        //�͵�ǰ�����ĳ�������һ��
        GameDataMgr.GetInstance().PlayerDataInfo.currentPhase = "Story1";
        //��������
        GameDataMgr.GetInstance().PlayerDataInfo.date = "184����,���������";
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
        GameDataMgr.GetInstance().PlayerDataInfo.currentChapter = 1;
        //�͵�ǰ�����ĳ�������һ��
        GameDataMgr.GetInstance().PlayerDataInfo.currentPhase = "Story1";

        //�����¼�
        EventCenter.GetInstance().EventTrigger<int,string>("SendCurrentChapter", 1, GameDataMgr.GetInstance().PlayerDataInfo.date);
    }
}
