using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 给不继承MonoBehaviour的类添加帧更新事件和协程的接口
/// </summary>
//可以给外部添加帧更新事件的方法
//可以给外部添加协程的方法
public class MonoManager : BaseManager<MonoManager>
{
    public MonoController controller;

    public MonoManager()
    {
        //新建controller对象，并为其添加脚本 
        //因为继承了单例模式基类，所以如果该对象是null，就会new一个对象，并且只会进行一次
        //保证了MonoController的唯一性
        GameObject obj = new GameObject("MonoController");
        controller = obj.AddComponent<MonoController>();
    }
    /// <summary>
    /// 让不继承MonoBehaviour的类可以使用update
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);
    }
    /// <summary>
    /// 让不继承MonoBehaviour的类取消使用update
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        controller.RemoveUpdateListener(fun);
    }
    /// <summary>
    /// 让不继承MonoBehaviour的类可以开启协程
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
