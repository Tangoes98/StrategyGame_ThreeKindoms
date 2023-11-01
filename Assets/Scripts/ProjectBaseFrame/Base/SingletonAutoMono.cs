using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Զ������ĵ���ģʽ���࣬����Ҫ�ֶ��Ͻű�
//ʹ��ʱ�Զ�����һ��������Ȼ��GetInstance������
//����ʨ�Ƽ�
public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        if(instance == null)
        {
            //����һ���ն���
            GameObject obj = new GameObject();
            //���ö��������Ϊ�ű���
            obj.name = typeof(T).ToString();

            //���������Ƴ����ɵĿն���
            //��Ϊ����ģʽ���������������������������������
            GameObject.DontDestroyOnLoad(obj);

            //���ö����ϵĽű�������
            instance = obj.AddComponent<T>();
        }
        return instance;
    }
}

