using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//缺点：如果该脚本被挂了多次，那么就无法保证单例化
//因此一个该脚本只能挂一次

public class MonoBehaviourBase<T> : MonoBehaviour where T : MonoBehaviour   
{
    private static T instance;

    public static T GetInstance()
    {
        //继承了MonoBehaviour的脚本不能直接new
        //只能通过拖动到对象上，或者通过加脚本的API实现：AddComponent
        return instance;  
    }

    //被保护的虚函数，方便子类重写
    protected virtual void Awake()
    {
        instance = this as T;
    }
}
