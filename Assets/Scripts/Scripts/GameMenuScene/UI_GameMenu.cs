using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_GameMenu : BasePanel
{
    public AudioSource loopSound;
    protected override void Awake()
    {
        //�����awake����ʼ����Ϣ
        base.Awake();
        //������Դ��������awake
        //����ʱ�����¼���Menu�Ѿ���
        EventCenter.GetInstance().EventTrigger("MenuIsOpen");
    }

    private void OnDestroy()
    {
        //������ʱ�����¼���Menu�Ѿ����ر�
        EventCenter.GetInstance().EventTrigger("MenuIsClose");
    }

    protected override void OnClick(string buttonName)
    {
        //������ͨ�������ж��ĸ���ť�������,Ȼ��ֱ�������ﴦ���߼�
        switch (buttonName)
        {
            //�����ʼ����Ϸ���������¾���
            case "ReturnToMenu":
                //�����ز˵�
                UIManager.GetInstance().HidePanel("MenuPanel");
                //���س�ʼ������ִ���߼��ر����п��ܲ�����Panel
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
                //�����ز˵�
                UIManager.GetInstance().HidePanel("MenuPanel");
                //�õ���Ҵ浵����
                GameDataMgr.GetInstance().LoadPlayerInfo();
                //���ݴ浵���ݵõ���ҵ�Ŀ��ҳ��
                string targetPhase =  GameDataMgr.GetInstance().PlayerDataInfo.currentPhase;
                //Ŀ��ҳ�����֮ǰ��Ҵ浵������¼�ĵ�ǰҳ�棬����Ҳ��ʵʱ������
                switch (targetPhase)
                {
                    //��ת����Ӧ���½ں�ҳ��
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

                        //����и���ĳ��������������......
                }

                break ;

            case "SaveGame":

                MusicMgr.GetInstance().PlaySound("maou_se_system47-save", false);

                UIManager.GetInstance().HidePanel("MenuPanel");
                //�����¼���������������
                EventCenter.GetInstance().EventTrigger("SavePlayerInfo");
                break;

        }
    }

    /// <summary>
    /// ִ�з������˵�����Ҫִ�еĺ���
    /// </summary>
    public void AfterReturnToMenu()
    {

        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
        //�ر�ȫ�����ܴ��ڵ�panel
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
        //ÿ���һ���������������һ��������ر�

    }

    //��ҪӦ�Ը��ֲ�ͬ���������panel�ķ�ʽ
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

    //��ҪӦ�Ը��ֲ�ͬ���������panel�ķ�ʽ
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


    // ����Ҫ�˳���Ϸ�ĵط����ø÷���
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
