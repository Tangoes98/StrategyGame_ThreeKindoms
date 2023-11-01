using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

        //如果本次游戏没有执行过ParseData
        if (GameDataMgr.GetInstance().isParseDataExecuted == false)
        {
            //执行一次
            GameDataMgr.GetInstance().ParseData();
        }
        else
        {
            //如果本次游戏已经执行过PaeseData，就不再执行
            //确保一次游戏只执行一次ParseData
            return;
        }
    }
}
