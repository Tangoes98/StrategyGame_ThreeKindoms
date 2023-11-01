using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressRecord_ConfigTroops : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //需要和当前所处的场景名一致
        GameDataMgr.GetInstance().PlayerDataInfo.currentPhase = "ConfigTroops";
        //监听保存事件
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
        //需要和当前所处的场景名一致
        GameDataMgr.GetInstance().PlayerDataInfo.currentPhase = "ConfigTroops";
    }

    public void ChangeChapter(int chapterNumber,string date)
    {
        //更新章节及日期
        GameDataMgr.GetInstance().PlayerDataInfo.currentChapter = chapterNumber;
        GameDataMgr.GetInstance().PlayerDataInfo.date = date;

    }
}
