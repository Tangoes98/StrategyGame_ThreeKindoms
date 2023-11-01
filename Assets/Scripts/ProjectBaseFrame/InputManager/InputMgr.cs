using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 输入控制模块
/// </summary>
public class InputMgr : BaseManager<InputMgr>
{
    private bool isStart =false;

    /// <summary>
    /// 构造函数中添加Update监听/MonoManager中的功能
    /// </summary>
    public InputMgr()
    {
        MonoManager.GetInstance().AddUpdateListener(MyUpdate);
    }

    /// <summary>
    /// 输入模式开关状态
    /// </summary>
    /// <param name="isOpen"></param>
    public void StartOrEndCheck(bool isOpen)
    {
        isStart = isOpen;
    }



    /// <summary>
    /// 检测什么按键被按下或者抬起了
    /// </summary>
    /// <param name="key"></param>
    private void CheckKeyCode(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            //事件中心模块分发按下事件
            EventCenter.GetInstance().EventTrigger("KeyButtonDown", key);
        }

        if (Input.GetKeyUp(key))
        {
            //事件中心模块分发抬起事件
            EventCenter.GetInstance().EventTrigger("KeyButtonUp", key);
        }
    }


    //通过MonoManager实现的Update
    private void MyUpdate()
    {
        //如果没有开启输入检测，就不检测，直接return
        if (!isStart)
            return;
        //Update中向事件中心设置对对应按键的检测   
        CheckKeyCode(KeyCode.Mouse0);
        
    }
}
