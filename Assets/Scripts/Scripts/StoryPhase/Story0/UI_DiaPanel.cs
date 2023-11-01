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
    //�ж����������

    public bool MenuIsOpen;

    protected override void Awake()
    {
        //�����awake����ʼ����Ϣ
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
        //������Ϊ��������һ�ε���������
        if (!MenuIsOpen)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            UIManager.GetInstance().ShowPanel<UI_GameMenu>("MenuPanel", E_UI_Layer.Top);
            MenuIsOpen = true;
        }

        //������Ϊż�����ڶ��ε�����ر����
        if (MenuIsOpen)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
            UIManager.GetInstance().HidePanel("MenuPanel");
            MenuIsOpen = false;
        }
    }

  


    //ִ���л���������Ҫִ�еĺ���
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
   /// Э��Ŀ���ǵ�currentSelectedGeneral�л����ٷ���
   /// </summary>
   /// <returns></returns>
   private IEnumerator DelayedToNextScene()
   {
       yield return null; // �ȴ���һ֡

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

