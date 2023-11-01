using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///缓存池模块，避免性能浪费 
/// </summary>
public class PoolData
{
    //缓存池中某一类对象容器挂载的父级
    public GameObject fatherObj;
    //对象的容器
    public List<GameObject> poolList;

    public PoolData (GameObject obj, GameObject bufferPool)
    {
        //给每一类容器创造一个父对象，并将这个父对象作为整个缓存池对象的子物体
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = bufferPool.transform;
        //第一次放入对象创建List
        poolList = new List<GameObject>(){};
        PushObj(obj);
    }

    /// <summary>
    /// 往一类容器中存对象
    /// </summary>
    /// <param name="obj"></param>
    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);
        poolList.Add(obj);
        obj.transform.parent = fatherObj.transform;
    }
    /// <summary>
    /// 从一类容器中取对象
    /// </summary>
    /// <returns></returns>
    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }
}

public class PoolManager : BaseManager<PoolManager>
{
    public Dictionary<string, PoolData> poolDictionary = new Dictionary<string,PoolData>();
    private GameObject bufferPool;

    /// <summary>
    /// 从缓存池取出object
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public void GetObject(string name, UnityAction<GameObject> callBack)
    {
        //有暂时存放object的“容器”，且容器内有东西可以拿
        if (poolDictionary.ContainsKey(name) && poolDictionary[name].poolList.Count > 0)
        {
            callBack(poolDictionary[name].GetObj());
        }
        //没有东西可以拿
        //从Resources文件夹加载并且实例化一个object
        else
        {
            //通过异步加载资源创建对象
            ResourceManager.GetInstance().LoadAsync<GameObject>(name, (gameObj) =>
            {
                gameObj.name = name;
                callBack(gameObj);
            });

            //obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            //把对象名字改成容器名字
            //obj.name = name;
        }
    }

    /// <summary>
    /// 把暂时不用的object放入缓存池
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public void PushObject(string name, GameObject obj)
    {
        //在放入缓存池时，如果缓存池为空，创建一个缓存池对象
        if(bufferPool == null)
            bufferPool = new GameObject("BufferPool");
       
        //如果有容器
        if (poolDictionary.ContainsKey(name))
        {
            poolDictionary[name].PushObj(obj);
        }
        //第一次放入，没有容器
        else
        {
            poolDictionary.Add(name, new PoolData(obj,bufferPool));
        }
    }

    /// <summary>
    /// 清空缓存池，用于场景切换时
    /// </summary>
    public void Clear()
    {
        poolDictionary.Clear();
        bufferPool = null;
    }

}
