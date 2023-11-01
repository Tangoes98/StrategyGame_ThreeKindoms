using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �������ģ��
/// </summary>
public class InputMgr : BaseManager<InputMgr>
{
    private bool isStart =false;

    /// <summary>
    /// ���캯�������Update����/MonoManager�еĹ���
    /// </summary>
    public InputMgr()
    {
        MonoManager.GetInstance().AddUpdateListener(MyUpdate);
    }

    /// <summary>
    /// ����ģʽ����״̬
    /// </summary>
    /// <param name="isOpen"></param>
    public void StartOrEndCheck(bool isOpen)
    {
        isStart = isOpen;
    }



    /// <summary>
    /// ���ʲô���������»���̧����
    /// </summary>
    /// <param name="key"></param>
    private void CheckKeyCode(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            //�¼�����ģ��ַ������¼�
            EventCenter.GetInstance().EventTrigger("KeyButtonDown", key);
        }

        if (Input.GetKeyUp(key))
        {
            //�¼�����ģ��ַ�̧���¼�
            EventCenter.GetInstance().EventTrigger("KeyButtonUp", key);
        }
    }


    //ͨ��MonoManagerʵ�ֵ�Update
    private void MyUpdate()
    {
        //���û�п��������⣬�Ͳ���⣬ֱ��return
        if (!isStart)
            return;
        //Update�����¼��������öԶ�Ӧ�����ļ��   
        CheckKeyCode(KeyCode.Mouse0);
        
    }
}
