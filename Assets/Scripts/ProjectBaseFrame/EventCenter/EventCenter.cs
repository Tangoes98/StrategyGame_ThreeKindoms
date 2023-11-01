using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo
{

}
//�޲�
public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}
//������
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

//˫����
public class EventInfo<T, U> : IEventInfo
{
    public UnityAction<T, U> actions;

    public EventInfo(UnityAction<T, U> action)
    {
        actions += action;
    }
}
//������
public class EventInfo<T, U, I> : IEventInfo
{
    public UnityAction<T, U, I> actions;

    public EventInfo(UnityAction<T, U, I> action)
    {
        actions += action;
    }
}
//�Ĳ���
public class EventInfo<T, U, I, O> : IEventInfo
{
    public UnityAction<T, U, I, O> actions;

    public EventInfo(UnityAction<T, U, I, O> action)
    {
        actions += action;
    }
}


public class EventCenter : BaseManager<EventCenter>
{
    //Key��Ӧ�¼���
    //Value��Ӧ�������¼��Ķ�Ӧί�к���
    //��������¼Ϊ�޷���ֵ����һ��������ί��
    private Dictionary<string, IEventInfo> eventDictionary = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// ����¼������ĵ������ӿ�
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    //name�¼�����action׼�����������¼���ί�к���
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        //�Ƿ���ڶ�Ӧ���¼��ļ���
        //��
        if (eventDictionary.ContainsKey(name))
        {
            (eventDictionary[name] as EventInfo<T>).actions += action;
        }
        //û��
        else
        {
            eventDictionary.Add(name, new EventInfo<T>(action));
        }
    }
    /// <summary>
    /// ����¼�������˫�������ؽӿ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener<T, U>(string name, UnityAction<T, U> action)
    {
        if (eventDictionary.ContainsKey(name))
        {
            (eventDictionary[name] as EventInfo<T, U>).actions += action;
        }
        else
        {
            eventDictionary.Add(name, new EventInfo<T, U>(action));
        }
    }

    /// <summary>
    /// ����¼����������������ؽӿ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener<T, U, I>(string name, UnityAction<T, U, I> action)
    {
        if (eventDictionary.ContainsKey(name))
        {
            (eventDictionary[name] as EventInfo<T, U, I>).actions += action;
        }
        else
        {
            eventDictionary.Add(name, new EventInfo<T, U, I>(action));
        }
    }

    /// <summary>
    /// ����¼��������Ĳ������ؽӿ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener<T, U, I, O>(string name, UnityAction<T, U, I, O> action)
    {
        if (eventDictionary.ContainsKey(name))
        {
            (eventDictionary[name] as EventInfo<T, U, I, O>).actions += action;
        }
        else
        {
            eventDictionary.Add(name, new EventInfo<T, U, I, O>(action));
        }
    }

    /// <summary>
    /// ����¼��������޲����ؽӿ�
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener(string name, UnityAction action)
    {
        //�Ƿ���ڶ�Ӧ���¼��ļ���
        //��
        if (eventDictionary.ContainsKey(name))
        {
            (eventDictionary[name] as EventInfo).actions += action;
        }
        //û��
        else
        {
            eventDictionary.Add(name, new EventInfo(action));
        }
    }


    /// <summary>
    /// �Ƴ��¼�����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    //name��Ӧ�¼���
    //action��Ӧ֮ǰ��ӵ�ί�к���
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if(eventDictionary.ContainsKey(name))
            (eventDictionary[name] as EventInfo<T>).actions -= action;
    }

    public void RemoveEventListener<T,U>(string name, UnityAction<T,U> action)
    {
        if (eventDictionary.ContainsKey(name))
            (eventDictionary[name] as EventInfo<T,U>).actions -= action;
    }

    public void RemoveEventListener<T, U ,I>(string name, UnityAction<T, U, I> action)
    {
        if (eventDictionary.ContainsKey(name))
            (eventDictionary[name] as EventInfo<T, U, I>).actions -= action;
    }


    /// <summary>
    /// �Ƴ��¼��������޲����ؽӿ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDictionary.ContainsKey(name))
            (eventDictionary[name] as EventInfo).actions -= action;
    }

    /// <summary>
    /// �������¼�����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="info"></param>
    //name�¼���������
    public void EventTrigger<T>(string name,T info)
    {
        //��û�ж�Ӧ���¼�����
        //�е����
        if (eventDictionary.ContainsKey(name))
        {
            if((eventDictionary[name] as EventInfo<T>).actions != null)
               //ִ������¼�
               (eventDictionary[name] as EventInfo<T>).actions.Invoke(info);
        }
    }
    /// <summary>
    /// ˫�����¼�����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="name"></param>
    /// <param name="info"></param>
    /// <param name="info2"></param>
    public void EventTrigger<T, U>(string name, T info, U info2)
    {
        //��û�ж�Ӧ���¼�����
        //�е����
        if (eventDictionary.ContainsKey(name))
        {
            if ((eventDictionary[name] as EventInfo<T, U>).actions != null)
                //ִ������¼�
                (eventDictionary[name] as EventInfo<T, U>).actions.Invoke(info,info2);
        }
    }

    /// <summary>
    /// �������¼�����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="name"></param>
    /// <param name="info"></param>
    /// <param name="info2"></param>
    public void EventTrigger<T, U, I>(string name, T info, U info2, I info3)
    {
        //��û�ж�Ӧ���¼�����
        //�е����
        if (eventDictionary.ContainsKey(name))
        {
            if ((eventDictionary[name] as EventInfo<T, U, I>).actions != null)
                //ִ������¼�
                (eventDictionary[name] as EventInfo<T, U, I>).actions.Invoke(info, info2, info3);
        }
    }

    public void EventTrigger<T, U, I, O>(string name, T info, U info2, I info3, O info4)
    {
        //��û�ж�Ӧ���¼�����
        //�е����
        if (eventDictionary.ContainsKey(name))
        {
            if ((eventDictionary[name] as EventInfo<T, U, I, O>).actions != null)
                //ִ������¼�
                (eventDictionary[name] as EventInfo<T, U, I, O>).actions.Invoke(info, info2, info3, info4);
        }
    }
    /// <summary>
    /// �¼��������޲����ؽӿ�
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger(string name)
    {
        //��û�ж�Ӧ���¼�����
        //�е����
        if (eventDictionary.ContainsKey(name))
        {
            if ((eventDictionary[name] as EventInfo).actions != null)
                //ִ������¼�
                (eventDictionary[name] as EventInfo).actions.Invoke();
        }
    }

    /// <summary>
    /// �������л�ʱ������¼����Ĵ�����¼�
    /// </summary>
    public void Clear()
    {
        eventDictionary.Clear();
    }
}
