using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressRecord_Stroy0 : MonoBehaviour
{
    void Start()
    {

        //���ݵ�ǰ�����Ĺ��³�����¼�½ڼ��׶�
        GameDataMgr.GetInstance().PlayerDataInfo.currentChapter = 0;
        //�͵�ǰ�����ĳ�������һ��
        GameDataMgr.GetInstance().PlayerDataInfo.currentPhase = "Story0";
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
        GameDataMgr.GetInstance().PlayerDataInfo.currentChapter = 0;
        //�͵�ǰ�����ĳ�������һ��
        GameDataMgr.GetInstance().PlayerDataInfo.currentPhase = "Story0";

        //�����¼�
        EventCenter.GetInstance().EventTrigger<int, string>("SendCurrentChapter", 0, GameDataMgr.GetInstance().PlayerDataInfo.date);
    }
}
