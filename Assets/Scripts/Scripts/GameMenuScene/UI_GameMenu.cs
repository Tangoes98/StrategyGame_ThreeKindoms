using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_GameMenu : BasePanel
{
    public AudioSource loopSound;
    protected override void Awake()
    {
        //父类的awake，初始化信息
        base.Awake();
        //下面可以处理子类的awake
        //激活时发送事件，Menu已经打开
        EventCenter.GetInstance().EventTrigger("MenuIsOpen");
    }

    private void OnDestroy()
    {
        //被销毁时发送事件，Menu已经被关闭
        EventCenter.GetInstance().EventTrigger("MenuIsClose");
    }

    protected override void OnClick(string buttonName)
    {
        //在这里通过名字判断哪个按钮被点击了,然后直接在这里处理逻辑
        switch (buttonName)
        {
            //点击开始新游戏，进入序章剧情
            case "ReturnToMenu":
                //先隐藏菜单
                UIManager.GetInstance().HidePanel("MenuPanel");
                //返回初始场景，执行逻辑关闭所有可能残留的Panel
                SceneMgr.GetInstance().LoadSceneAsyn("StartScene", AfterReturnToMenu);
                break;

            case "ExitGame":
                MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
                Debug.Log("ExitGame");
                QuitGame();
                break;

            case "Setting":
                Debug.Log("Setting");
                break;

            case "LoadGame":
                //先隐藏菜单
                UIManager.GetInstance().HidePanel("MenuPanel");
                //得到玩家存档数据
                GameDataMgr.GetInstance().LoadPlayerInfo();
                //根据存档数据得到玩家的目标页面
                string targetPhase =  GameDataMgr.GetInstance().PlayerDataInfo.currentPhase;
                //目标页面就是之前玩家存档后所记录的当前页面，数据也被实时保存了
                switch (targetPhase)
                {
                    //跳转到相应的章节和页面
                    case ("Story0"):

                        PoolManager.GetInstance().Clear();
                        SceneMgr.GetInstance().LoadSceneAsyn(targetPhase, AfterLoadFunctions_Story); 
                        break;

                    case ("ConfigurationUnit"):

                        PoolManager.GetInstance().Clear();
                        SceneMgr.GetInstance().LoadSceneAsyn(targetPhase, AfterLoadFunctions_Config);
                        break;

                    case ("ManageSupplies"):

                        PoolManager.GetInstance().Clear();
                        SceneMgr.GetInstance().LoadSceneAsyn(targetPhase, AfterLoadFunctions_ManageSupplies);
                        break;

                    case ("ConfigTroops"):

                        PoolManager.GetInstance().Clear();
                        SceneMgr.GetInstance().LoadSceneAsyn(targetPhase, AfterLoadFunctions_ConfigTroops);
                        break;

                    case ("LevelSelect"):
                        PoolManager.GetInstance().Clear();
                        SceneMgr.GetInstance().LoadSceneAsyn(targetPhase, AfterLoadFunctions_LevelSelect);
                        break;

                        //今后有更多的场景就在这里添加......
                }

                break ;

            case "SaveGame":

                MusicMgr.GetInstance().PlaySound("maou_se_system47-save", false);

                UIManager.GetInstance().HidePanel("MenuPanel");
                //触发事件监听，保存数据
                EventCenter.GetInstance().EventTrigger("SavePlayerInfo");
                break;

        }
    }

    /// <summary>
    /// 执行返回主菜单后需要执行的函数
    /// </summary>
    public void AfterReturnToMenu()
    {

        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
        //关闭全部可能存在的panel
        PoolManager.GetInstance().Clear();

        UIManager.GetInstance().HidePanel("LevelConfirmPanel");
        UIManager.GetInstance().HidePanel("HintPanel");
        UIManager.GetInstance().HidePanel("Chapter1Options");
        UIManager.GetInstance().HidePanel("Chapter1Options");
        UIManager.GetInstance().HidePanel("LevelSelectPanel");
        UIManager.GetInstance().HidePanel("SellConfirmPanel");
        UIManager.GetInstance().HidePanel("BagPanel2");
        UIManager.GetInstance().HidePanel("ActiveSkillPool");
        UIManager.GetInstance().HidePanel("ActiveSkillScrollView");
        UIManager.GetInstance().HidePanel("PassiveSkillsPool");
        UIManager.GetInstance().HidePanel("PassiveSkillsScrollView");
        UIManager.GetInstance().HidePanel("TroopLevelUpPanel2");
        UIManager.GetInstance().HidePanel("TroopSelectPanel");
        UIManager.GetInstance().HidePanel("TipsPanel");
        UIManager.GetInstance().HidePanel("ShopTipsPanel");
        UIManager.GetInstance().HidePanel("ConfigPanel");
        UIManager.GetInstance().HidePanel("ManageSuppliesPanel");
        UIManager.GetInstance().HidePanel("BagPanel");
        UIManager.GetInstance().HidePanel("ShopPanel");
        UIManager.GetInstance().HidePanel("DiaPanel");
        UIManager.GetInstance().HidePanel("ConfigTroopsPanel");
        UIManager.GetInstance().HidePanel("ConfigTroopsPanelMain");
        UIManager.GetInstance().HidePanel("AssignAttributePointPanel");
        //每多加一个面板就在这里加入一个，方便关闭

    }

    //需要应对各种不同情况的隐藏panel的方式
    public void AfterLoadFunctions_Story()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_system48-start", false);
        PoolManager.GetInstance().Clear();

        UIManager.GetInstance().HidePanel("LevelConfirmPanel");
        UIManager.GetInstance().HidePanel("HintPanel");
        UIManager.GetInstance().HidePanel("Chapter1Options");
        UIManager.GetInstance().HidePanel("LevelSelectPanel");
        UIManager.GetInstance().HidePanel("SellConfirmPanel");
        UIManager.GetInstance().HidePanel("BagPanel2");
        UIManager.GetInstance().HidePanel("ActiveSkillPool");
        UIManager.GetInstance().HidePanel("ActiveSkillScrollView");
        UIManager.GetInstance().HidePanel("PassiveSkillsPool");
        UIManager.GetInstance().HidePanel("PassiveSkillsScrollView");
        UIManager.GetInstance().HidePanel("TroopLevelUpPanel2");
        UIManager.GetInstance().HidePanel("TroopSelectPanel");
        UIManager.GetInstance().HidePanel("TipsPanel");
        UIManager.GetInstance().HidePanel("ShopTipsPanel");
        UIManager.GetInstance().HidePanel("ConfigPanel");
        UIManager.GetInstance().HidePanel("ManageSuppliesPanel");
        UIManager.GetInstance().HidePanel("BagPanel");
        UIManager.GetInstance().HidePanel("ShopPanel");
        //UIManager.GetInstance().HidePanel("DiaPanel");
        UIManager.GetInstance().HidePanel("ConfigTroopsPanel");
        UIManager.GetInstance().HidePanel("ConfigTroopsPanelMain");
        UIManager.GetInstance().HidePanel("AssignAttributePointPanel");
    }

    //需要应对各种不同情况的隐藏panel的方式
    public void AfterLoadFunctions_LevelSelect()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_system48-start", false);
        PoolManager.GetInstance().Clear();

        UIManager.GetInstance().HidePanel("LevelConfirmPanel");
        UIManager.GetInstance().HidePanel("HintPanel");
        UIManager.GetInstance().HidePanel("Chapter1Options");
        UIManager.GetInstance().HidePanel("SellConfirmPanel");
        UIManager.GetInstance().HidePanel("BagPanel2");
        UIManager.GetInstance().HidePanel("ActiveSkillPool");
        UIManager.GetInstance().HidePanel("ActiveSkillScrollView");
        UIManager.GetInstance().HidePanel("PassiveSkillsPool");
        UIManager.GetInstance().HidePanel("PassiveSkillsScrollView");
        UIManager.GetInstance().HidePanel("TroopLevelUpPanel2");
        UIManager.GetInstance().HidePanel("TroopSelectPanel");
        UIManager.GetInstance().HidePanel("TipsPanel");
        UIManager.GetInstance().HidePanel("ShopTipsPanel");
        UIManager.GetInstance().HidePanel("ConfigPanel");
        UIManager.GetInstance().HidePanel("ManageSuppliesPanel");
        UIManager.GetInstance().HidePanel("BagPanel");
        UIManager.GetInstance().HidePanel("ShopPanel");
        UIManager.GetInstance().HidePanel("DiaPanel");
        UIManager.GetInstance().HidePanel("ConfigTroopsPanel");
        UIManager.GetInstance().HidePanel("ConfigTroopsPanelMain");
        UIManager.GetInstance().HidePanel("AssignAttributePointPanel");
    }

    public void AfterLoadFunctions_Config()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_system48-start", false);
        PoolManager.GetInstance().Clear();

        UIManager.GetInstance().HidePanel("LevelConfirmPanel");
        UIManager.GetInstance().HidePanel("HintPanel");
        UIManager.GetInstance().HidePanel("Chapter1Options");
        UIManager.GetInstance().HidePanel("LevelSelectPanel");
        UIManager.GetInstance().HidePanel("SellConfirmPanel");
        UIManager.GetInstance().HidePanel("BagPanel2");
        UIManager.GetInstance().HidePanel("ActiveSkillPool");
        UIManager.GetInstance().HidePanel("ActiveSkillScrollView");
        UIManager.GetInstance().HidePanel("PassiveSkillsPool");
        UIManager.GetInstance().HidePanel("PassiveSkillsScrollView");
        UIManager.GetInstance().HidePanel("TroopLevelUpPanel2");
        UIManager.GetInstance().HidePanel("TroopSelectPanel");
        UIManager.GetInstance().HidePanel("TipsPanel");
        UIManager.GetInstance().HidePanel("ShopTipsPanel");
        //UIManager.GetInstance().HidePanel("ConfigPanel");
        UIManager.GetInstance().HidePanel("ManageSuppliesPanel");
        UIManager.GetInstance().HidePanel("BagPanel");
        UIManager.GetInstance().HidePanel("ShopPanel");
        UIManager.GetInstance().HidePanel("DiaPanel");
        UIManager.GetInstance().HidePanel("ConfigTroopsPanel");
        UIManager.GetInstance().HidePanel("ConfigTroopsPanelMain");
        UIManager.GetInstance().HidePanel("AssignAttributePointPanel");
    }

    public void AfterLoadFunctions_ManageSupplies()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_system48-start", false);
        PoolManager.GetInstance().Clear();

        UIManager.GetInstance().HidePanel("LevelConfirmPanel");
        UIManager.GetInstance().HidePanel("HintPanel");
        UIManager.GetInstance().HidePanel("Chapter1Options");
        UIManager.GetInstance().HidePanel("LevelSelectPanel");
        UIManager.GetInstance().HidePanel("SellConfirmPanel");
        UIManager.GetInstance().HidePanel("BagPanel2");
        UIManager.GetInstance().HidePanel("ActiveSkillPool");
        UIManager.GetInstance().HidePanel("ActiveSkillScrollView");
        UIManager.GetInstance().HidePanel("PassiveSkillsPool");
        UIManager.GetInstance().HidePanel("PassiveSkillsScrollView");
        UIManager.GetInstance().HidePanel("TroopLevelUpPanel2");
        UIManager.GetInstance().HidePanel("TroopSelectPanel");
        //UIManager.GetInstance().HidePanel("TipsPanel");
        UIManager.GetInstance().HidePanel("ShopTipsPanel");
        UIManager.GetInstance().HidePanel("ConfigPanel");
        //UIManager.GetInstance().HidePanel("ManageSuppliesPanel");
        //UIManager.GetInstance().HidePanel("BagPanel");
        //UIManager.GetInstance().HidePanel("ShopPanel");
        UIManager.GetInstance().HidePanel("DiaPanel");
        UIManager.GetInstance().HidePanel("ConfigTroopsPanel");
        UIManager.GetInstance().HidePanel("ConfigTroopsPanelMain");
        UIManager.GetInstance().HidePanel("AssignAttributePointPanel");
    }

    public void AfterLoadFunctions_ConfigTroops()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_system48-start", false);

        UIManager.GetInstance().HidePanel("LevelConfirmPanel");
        UIManager.GetInstance().HidePanel("HintPanel");
        UIManager.GetInstance().HidePanel("Chapter1Options");
        UIManager.GetInstance().HidePanel("LevelSelectPanel");
        UIManager.GetInstance().HidePanel("SellConfirmPanel");
        PoolManager.GetInstance().Clear();
        UIManager.GetInstance().HidePanel("TroopLevelUpPanel2");
        UIManager.GetInstance().HidePanel("TipsPanel");
        UIManager.GetInstance().HidePanel("ShopTipsPanel");
        UIManager.GetInstance().HidePanel("ConfigPanel");
        UIManager.GetInstance().HidePanel("ManageSuppliesPanel");
        UIManager.GetInstance().HidePanel("BagPanel");
        UIManager.GetInstance().HidePanel("ShopPanel");
        UIManager.GetInstance().HidePanel("DiaPanel");
        //UIManager.GetInstance().HidePanel("ConfigTroopsPanel");
        //UIManager.GetInstance().HidePanel("ConfigTroopsPanelMain");
        //UIManager.GetInstance().HidePanel("AssignAttributePointPanel");

   
    }


    // 在需要退出游戏的地方调用该方法
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
