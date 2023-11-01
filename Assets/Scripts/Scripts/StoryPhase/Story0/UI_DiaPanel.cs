using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class UI_DiaPanel : BasePanel
{
    public string currentPhase;
    //判断鼠标点击次数

    public bool MenuIsOpen;

    protected override void Awake()
    {
        //父类的awake，初始化信息
        base.Awake();
        MenuIsOpen = false;

        GetControl<Button>("NextPhase").onClick.AddListener(GoToNextPhase);
        GetControl<Button>("OpenMenu").onClick.AddListener(OpenMenu);
    }

    public void GoToNextPhase()
    {

        currentPhase = GameDataMgr.GetInstance().PlayerDataInfo.currentPhase;
        Debug.Log(currentPhase);

        UIManager.GetInstance().HidePanel("MenuPanel");
        switch (currentPhase)
        {
            case "Story0":
                SceneMgr.GetInstance().LoadSceneAsyn("Intro_Chapter1", AfterLoadFunctions);
                break;

            case "Intro_Chapter1":
                SceneMgr.GetInstance().LoadSceneAsynByIndex(3, AfterLoadFunctions); 
                break;

            case "Intro_Chapter2":
                SceneMgr.GetInstance().LoadSceneAsynByIndex(4, AfterLoadFunctions);
                break;

            case "Story1":
                SceneMgr.GetInstance().LoadSceneAsynByIndex(7, AfterLoadFunctionsToConfig);
                break;

            case "Story2":
                SceneMgr.GetInstance().LoadSceneAsynByIndex(7, AfterLoadFunctionsToConfig);
                break;
        }
    }
    public void OpenMenu()
    {
        //当次数为奇数，第一次点击，打开面板
        if (!MenuIsOpen)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            UIManager.GetInstance().ShowPanel<UI_GameMenu>("MenuPanel", E_UI_Layer.Top);
            MenuIsOpen = true;
        }

        //当次数为偶数，第二次点击，关闭面板
        if (MenuIsOpen)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            UIManager.GetInstance().HidePanel("MenuPanel");
            MenuIsOpen = false;
        }
    }

  


    //执行切换场景后需要执行的函数
    public void AfterLoadFunctions()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);

        MenuIsOpen = false;
    }

   public void AfterLoadFunctionsToConfig()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
        UIManager.GetInstance().HidePanel("DiaPanel");
        MenuIsOpen = false;
    }
}

/* /// <summary>
   /// 协程目的是等currentSelectedGeneral切换好再发送
   /// </summary>
   /// <returns></returns>
   private IEnumerator DelayedToNextScene()
   {
       yield return null; // 等待下一帧

       UIManager.GetInstance().HidePanel("MenuPanel");
       switch (currentPhase)
         {
             case "Story0":
                 SceneMgr.GetInstance().LoadSceneAsyn("Intro_Chapter1", AfterLoadFunctions);
                 break;

             case "Intro_Chapter1":
                 SceneMgr.GetInstance().LoadSceneAsyn("Stroy1", AfterLoadFunctions);
                 break;

             case "Stroy1":
                 SceneMgr.GetInstance().LoadSceneAsyn("ConfigurationUnit", AfterLoadFunctionsToConfig);
                 break;
         }
   }*/

