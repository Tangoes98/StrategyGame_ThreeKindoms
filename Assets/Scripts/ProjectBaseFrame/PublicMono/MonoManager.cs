using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �����̳�MonoBehaviour�������֡�����¼���Э�̵Ľӿ�
/// </summary>
//���Ը��ⲿ���֡�����¼��ķ���
//���Ը��ⲿ���Э�̵ķ���
public class MonoManager : BaseManager<MonoManager>
{
    public MonoController controller;

    public MonoManager()
    {
        //�½�controller���󣬲�Ϊ����ӽű� 
        //��Ϊ�̳��˵���ģʽ���࣬��������ö�����null���ͻ�newһ�����󣬲���ֻ�����һ��
        //��֤��MonoController��Ψһ��
        GameObject obj = new GameObject("MonoController");
        controller = obj.AddComponent<MonoController>();
    }
    /// <summary>
    /// �ò��̳�MonoBehaviour�������ʹ��update
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);
    }
    /// <summary>
    /// �ò��̳�MonoBehaviour����ȡ��ʹ��update
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        controller.RemoveUpdateListener(fun);
    }
    /// <summary>
    /// �ò��̳�MonoBehaviour������Կ���Э��
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }

    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return controller.StartCoroutine(methodName, value);
    }

    public Coroutine StartCoroutine(string methodName)
    {
        return controller.StartCoroutine(methodName);
    }
}
