using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//自动创建的单例模式基类，不需要手动拖脚本
//使用时自动生成一个空物体然后GetInstance就行了
//唐老狮推荐
public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        if(instance == null)
        {
            //创建一个空对象
            GameObject obj = new GameObject();
            //设置对象的名字为脚本名
            obj.name = typeof(T).ToString();

            //过场景不移除生成的空对象
            //因为单例模式对象往往存在于整个程序的生命周期中
            GameObject.DontDestroyOnLoad(obj);

            //将该对象上的脚本单例化
            instance = obj.AddComponent<T>();
        }
        return instance;
    }
}

