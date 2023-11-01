using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressRecord_Stroy1 : MonoBehaviour
{
    void Start()
    {

        //根据当前所处的故事场景记录章节及阶段
        GameDataMgr.GetInstance().PlayerDataInfo.currentChapter = 1;
        //和当前所处的场景名称一致
        GameDataMgr.GetInstance().PlayerDataInfo.currentPhase = "Story1";
        //更改日期
        GameDataMgr.GetInstance().PlayerDataInfo.date = "184年秋,汉光和七年";
        //监听保存事件
        EventCenter.GetInstance().AddEventListener("SavePlayerInfo", SavePlayerInfo);
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener("SavePlayerInfo", SavePlayerInfo);
    }
    public void SavePlayerInfo()
    {
        //根据当前所处的故事场景记录章节及阶段
        GameDataMgr.GetInstance().PlayerDataInfo.currentChapter = 1;
        //和当前所处的场景名称一致
        GameDataMgr.GetInstance().PlayerDataInfo.currentPhase = "Story1";

        //发送事件
        EventCenter.GetInstance().EventTrigger<int,string>("SendCurrentChapter", 1, GameDataMgr.GetInstance().PlayerDataInfo.date);
    }
}
