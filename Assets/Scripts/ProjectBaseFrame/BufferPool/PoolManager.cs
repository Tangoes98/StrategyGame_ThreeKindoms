using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///�����ģ�飬���������˷� 
/// </summary>
public class PoolData
{
    //�������ĳһ������������صĸ���
    public GameObject fatherObj;
    //���������
    public List<GameObject> poolList;

    public PoolData (GameObject obj, GameObject bufferPool)
    {
        //��ÿһ����������һ�������󣬲��������������Ϊ��������ض����������
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = bufferPool.transform;
        //��һ�η�����󴴽�List
        poolList = new List<GameObject>(){};
        PushObj(obj);
    }

    /// <summary>
    /// ��һ�������д����
    /// </summary>
    /// <param name="obj"></param>
    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);
        poolList.Add(obj);
        obj.transform.parent = fatherObj.transform;
    }
    /// <summary>
    /// ��һ��������ȡ����
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
    /// �ӻ����ȡ��object
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public void GetObject(string name, UnityAction<GameObject> callBack)
    {
        //����ʱ���object�ġ������������������ж���������
        if (poolDictionary.ContainsKey(name) && poolDictionary[name].poolList.Count > 0)
        {
            callBack(poolDictionary[name].GetObj());
        }
        //û�ж���������
        //��Resources�ļ��м��ز���ʵ����һ��object
        else
        {
            //ͨ���첽������Դ��������
            ResourceManager.GetInstance().LoadAsync<GameObject>(name, (gameObj) =>
            {
                gameObj.name = name;
                callBack(gameObj);
            });

            //obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            //�Ѷ������ָĳ���������
            //obj.name = name;
        }
    }

    /// <summary>
    /// ����ʱ���õ�object���뻺���
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public void PushObject(string name, GameObject obj)
    {
        //�ڷ��뻺���ʱ����������Ϊ�գ�����һ������ض���
        if(bufferPool == null)
            bufferPool = new GameObject("BufferPool");
       
        //���������
        if (poolDictionary.ContainsKey(name))
        {
            poolDictionary[name].PushObj(obj);
        }
        //��һ�η��룬û������
        else
        {
            poolDictionary.Add(name, new PoolData(obj,bufferPool));
        }
    }

    /// <summary>
    /// ��ջ���أ����ڳ����л�ʱ
    /// </summary>
    public void Clear()
    {
        poolDictionary.Clear();
        bufferPool = null;
    }

}
