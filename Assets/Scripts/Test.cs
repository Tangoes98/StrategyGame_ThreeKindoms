using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // 在按下A键时执行A逻辑
            GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral["LiuBei"].CurrentEXP += 200;
            //增加该武将拥有的所有兵种
            GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral["LiuBei"].GeneralOwnedTroop.Add("WuHuanCavalry", GameDataMgr.GetInstance().GetTroopInfo(5));
           
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // 在按下A键时执行A逻辑
            GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral["GuanYu"].CurrentEXP += 8000;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            // 在按下A键时执行A逻辑
            GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral["ZhangFei"].CurrentEXP += 200;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            EventCenter.GetInstance().EventTrigger("MainLevel1IsFinished");
            //EventCenter.GetInstance().EventTrigger<string, bool>("LevelContentInfoUpdate", "C1_MainLevel1", true);

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            EventCenter.GetInstance().EventTrigger("SubLevel1IsFinished");
            //EventCenter.GetInstance().EventTrigger<string, bool>("LevelContentInfoUpdate", "C1_SubLevel1", true);

        }

    }
}
