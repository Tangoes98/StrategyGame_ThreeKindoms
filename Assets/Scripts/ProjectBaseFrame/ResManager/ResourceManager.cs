using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载模块
/// </summary>
public class ResourceManager : BaseManager<ResourceManager>
{
    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T Load<T> (string name) where T:Object
    {
        T resource = Resources.Load<T>(name);
        //如果资源是GameObject，直接在加载时返回并实例化对象，外部直接使用
        if(resource is GameObject)
            return GameObject.Instantiate(resource);
        else//比如TextAsset，AudioClip等不需要实例化的资源
            return resource;
    }

    /// <summary>
    /// 异步加载资源接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    public void LoadAsync<T> (string name, UnityAction<T> callback) where T: Object
    {
        //开启异步加载的协程
        MonoManager.GetInstance().StartCoroutine(IELoadAsync(name,callback));
    }

    /// <summary>
    /// 异步加载的协程函数
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private IEnumerator IELoadAsync<T>(string name, UnityAction<T> callback) where T:Object
    {
        ResourceRequest r = Resources.LoadAsync<T> (name);
        yield return r;

        if (r.asset is GameObject)
            callback(GameObject.Instantiate(r.asset) as T);
        else
            callback(r.asset as T);
    }


}

