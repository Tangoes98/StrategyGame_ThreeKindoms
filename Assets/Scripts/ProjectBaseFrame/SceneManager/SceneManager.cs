using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneMgr : BaseManager<SceneMgr>
{   /// <summary>
    /// 同步加载场景并执行函数,可能造成卡顿
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSceneSyn(string name,UnityAction fun)
    {
        //同步加载场景
        SceneManager.LoadScene(name);
        //希望加载成功后执行的函数
        fun();
    }


    /// <summary>
    /// 异步加载场景后执行函数的接口
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSceneAsyn(string name, UnityAction fun)
    {
        MonoManager.GetInstance().StartCoroutine(IELoadSceneAsyn(name, fun));
    }

    /// <summary>
    /// 协程异步加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    /// <returns></returns>
    private IEnumerator IELoadSceneAsyn(string name, UnityAction fun)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //得到场景加载的进度,只要异步加载没有完成
        while (!ao.isDone)
        {   //通过事件中心向外发布进度清况
            EventCenter.GetInstance().EventTrigger("UpdateLoadingProgress",ao.progress);
            //返回当前异步加载的进度
            yield return ao.progress;
        }
       
        //加载完成过后，执行fun函数
        fun();

    }

    //////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>
    /// 异步加载场景后执行函数的接口
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSceneAsynByIndex(int index, UnityAction fun)
    {
        MonoManager.GetInstance().StartCoroutine(IELoadSceneAsynByIndex(index, fun));
    }

    /// <summary>
    /// 协程异步加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    /// <returns></returns>
    private IEnumerator IELoadSceneAsynByIndex(int index, UnityAction fun)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(index);
        //得到场景加载的进度,只要异步加载没有完成
        while (!ao.isDone)
        {   //通过事件中心向外发布进度清况
            EventCenter.GetInstance().EventTrigger("UpdateLoadingProgress", ao.progress);
            //返回当前异步加载的进度
            yield return ao.progress;
        }

        //加载完成过后，执行fun函数
        fun();

    }
}

