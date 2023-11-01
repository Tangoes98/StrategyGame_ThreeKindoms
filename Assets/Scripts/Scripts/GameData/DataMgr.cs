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

        //���������Ϸû��ִ�й�ParseData
        if (GameDataMgr.GetInstance().isParseDataExecuted == false)
        {
            //ִ��һ��
            GameDataMgr.GetInstance().ParseData();
        }
        else
        {
            //���������Ϸ�Ѿ�ִ�й�PaeseData���Ͳ���ִ��
            //ȷ��һ����Ϸִֻ��һ��ParseData
            return;
        }
    }
}
