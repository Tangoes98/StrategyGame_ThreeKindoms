using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式基类模块
/// </summary>
/// <typeparam name="T"></typeparam>
//用泛型涵盖各种使用情况
public class BaseManager<T> where T: new() //约束：T必须有无参构造函数才能作为泛型对象传入
{

    private static T instance;
    //确保GameManager的唯一性
    public static T GetInstance()
    {
        if (instance == null)
            instance = new T();
        return instance;
    }
}

/*//使用例
public class GameManager: BaseManager<GameManager>
{
}
*/
//之后的单例模式类都可以继承这个类，节省代码

