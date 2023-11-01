using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ȱ�㣺����ýű������˶�Σ���ô���޷���֤������
//���һ���ýű�ֻ�ܹ�һ��

public class MonoBehaviourBase<T> : MonoBehaviour where T : MonoBehaviour   
{
    private static T instance;

    public static T GetInstance()
    {
        //�̳���MonoBehaviour�Ľű�����ֱ��new
        //ֻ��ͨ���϶��������ϣ�����ͨ���ӽű���APIʵ�֣�AddComponent
        return instance;  
    }

    //���������麯��������������д
    protected virtual void Awake()
    {
        instance = this as T;
    }
}
