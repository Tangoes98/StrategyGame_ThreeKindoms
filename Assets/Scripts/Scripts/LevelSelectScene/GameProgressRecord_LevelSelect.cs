using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressRecord_LevelSelect : MonoBehaviour
{
    void Start()
    {
       
        //��Ҫ�͵�ǰ�����ĳ�����һ��
        GameDataMgr.GetInstance().PlayerDataInfo.currentPhase = "LevelSelectScene";
        //���������¼�
        EventCenter.GetInstance().AddEventListener("SavePlayerInfo", SavePlayerInfo);
        EventCenter.GetInstance().AddEventListener<int,string>("SendCurrentChapter", ChangeChapter);
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener("SavePlayerInfo", SavePlayerInfo);
        EventCenter.GetInstance().RemoveEventListener<int, string>("SendCurrentChapter", ChangeChapter);
    }

    public void SavePlayerInfo()
    {
        
        //��Ҫ�͵�ǰ�����ĳ�����һ��
        GameDataMgr.GetInstance().PlayerDataInfo.currentPhase = "LevelSelectScene";
    }

    public void ChangeChapter(int chapterNumber,string date)
    {
        //�����½ڼ�����
        GameDataMgr.GetInstance().PlayerDataInfo.currentChapter = chapterNumber;
        GameDataMgr.GetInstance().PlayerDataInfo.date = date;
    }
}
