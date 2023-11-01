using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//MonoBehaviour
//�ò��̳�Mono�������ʹ��Э�̵�MonoBahaviour�Ĺ���

public class MonoController : MonoBehaviour
{
    public event UnityAction updateEvent;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateEvent != null)
            updateEvent();
    }

    /// <summary>
    /// ���ⲿ�ṩ�����֡�����¼��ĺ���
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    /// <summary>
    /// ���ⲿ�ṩ���Ƴ�֡�����¼��ĺ���
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }

}
