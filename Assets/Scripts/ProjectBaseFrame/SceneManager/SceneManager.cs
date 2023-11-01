using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneMgr : BaseManager<SceneMgr>
{   /// <summary>
    /// ͬ�����س�����ִ�к���,������ɿ���
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSceneSyn(string name,UnityAction fun)
    {
        //ͬ�����س���
        SceneManager.LoadScene(name);
        //ϣ�����سɹ���ִ�еĺ���
        fun();
    }


    /// <summary>
    /// �첽���س�����ִ�к����Ľӿ�
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSceneAsyn(string name, UnityAction fun)
    {
        MonoManager.GetInstance().StartCoroutine(IELoadSceneAsyn(name, fun));
    }

    /// <summary>
    /// Э���첽���س���
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    /// <returns></returns>
    private IEnumerator IELoadSceneAsyn(string name, UnityAction fun)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //�õ��������صĽ���,ֻҪ�첽����û�����
        while (!ao.isDone)
        {   //ͨ���¼��������ⷢ���������
            EventCenter.GetInstance().EventTrigger("UpdateLoadingProgress",ao.progress);
            //���ص�ǰ�첽���صĽ���
            yield return ao.progress;
        }
       
        //������ɹ���ִ��fun����
        fun();

    }

    //////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>
    /// �첽���س�����ִ�к����Ľӿ�
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSceneAsynByIndex(int index, UnityAction fun)
    {
        MonoManager.GetInstance().StartCoroutine(IELoadSceneAsynByIndex(index, fun));
    }

    /// <summary>
    /// Э���첽���س���
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    /// <returns></returns>
    private IEnumerator IELoadSceneAsynByIndex(int index, UnityAction fun)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(index);
        //�õ��������صĽ���,ֻҪ�첽����û�����
        while (!ao.isDone)
        {   //ͨ���¼��������ⷢ���������
            EventCenter.GetInstance().EventTrigger("UpdateLoadingProgress", ao.progress);
            //���ص�ǰ�첽���صĽ���
            yield return ao.progress;
        }

        //������ɹ���ִ��fun����
        fun();

    }
}

