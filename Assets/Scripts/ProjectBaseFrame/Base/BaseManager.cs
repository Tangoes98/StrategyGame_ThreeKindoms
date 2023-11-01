using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ģʽ����ģ��
/// </summary>
/// <typeparam name="T"></typeparam>
//�÷��ͺ��Ǹ���ʹ�����
public class BaseManager<T> where T: new() //Լ����T�������޲ι��캯��������Ϊ���Ͷ�����
{

    private static T instance;
    //ȷ��GameManager��Ψһ��
    public static T GetInstance()
    {
        if (instance == null)
            instance = new T();
        return instance;
    }
}

/*//ʹ����
public class GameManager: BaseManager<GameManager>
{
}
*/
//֮��ĵ���ģʽ�඼���Լ̳�����࣬��ʡ����

