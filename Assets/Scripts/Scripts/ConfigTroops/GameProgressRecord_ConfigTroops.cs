using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressRecord_ConfigTroops : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //��Ҫ�͵�ǰ�����ĳ�����һ��
        GameDataMgr.GetInstance().PlayerDataInfo.currentPhase = "ConfigTroops";
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
        GameDataMgr.GetInstance().PlayerDataInfo.currentPhase = "ConfigTroops";
    }

    public void ChangeChapter(int chapterNumber,string date)
    {
        //�����½ڼ�����
        GameDataMgr.GetInstance().PlayerDataInfo.currentChapter = chapterNumber;
        GameDataMgr.GetInstance().PlayerDataInfo.date = date;

    }
}
