using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartManager : BasePanel
{
    //玩家的存储路径
    private static string PlayerInfoSaveAdress;

    protected override void Awake()
    {
        //父类的awake，初始化信息
        base.Awake();
        //下面可以处理子类的awake
        PlayerInfoSaveAdress = Application.persistentDataPath + "/PlayerSaveData.json";
    }

    protected override void OnClick(string buttonName)
    {
        //在这里通过名字判断哪个按钮被点击了,然后直接在这里处理逻辑
        switch (buttonName)
        {
            //点击开始新游戏，进入序章剧情
            case "StartGame":

                MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
                //异步加载完成后执行初始化游戏的方法
                SceneMgr.GetInstance().LoadSceneAsyn("Story0", StartNewGame);
                break;
            case "ExitGame":

                QuitGame();
                break;
            case "Setting":
                Debug.Log("Setting");
                break;
            case "LoadGame":
                //读取数据
                GameDataMgr.GetInstance().LoadPlayerInfo();
                //跳转到相应的章节和页面
                SceneMgr.GetInstance().LoadSceneAsyn(GameDataMgr.GetInstance().PlayerDataInfo.currentPhase, LoadScene);
                break;
        }
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

    /// <summary>
    /// 初始化游戏的函数，首次进入游戏会在第0章的故事阶段
    /// </summary>
    public void StartNewGame()
    {

        MusicMgr.GetInstance().PlaySound("maou_se_system48-start", false);
        //隐藏面板
        UIManager.GetInstance().HidePanel("StartPanel");
        //开始新游戏之前先删除旧存档的数据
        if (File.Exists(PlayerInfoSaveAdress))
        {
            //开始新游戏前，清除之前游戏的事件中心
            //如果不清除，可能导致ParseData中的事件监听多次执行，升1级会调用多次升级
            //EventCenter.GetInstance().Clear();
            
            //用目前的方式，则需要注意不能清除ParseData中的事件////////

            //删除原存档文件，如果以后做多个手动存档栏位，可以不需要这步
            File.Delete(PlayerInfoSaveAdress);

            //清空Json数据，避免多次执行ParseData时因为已经有这些值而报错
            /* GameDataMgr.GetInstance().itemInfoDic = new Dictionary<int, Item>();
            GameDataMgr.GetInstance().generalInfoDic = new Dictionary<int, General>();
            GameDataMgr.GetInstance().troopInfoDic = new Dictionary<int, Troop>();
            GameDataMgr.GetInstance().expInfodic = new Dictionary<int, EXP>();
*/
            //清空玩家数据，如果以后做多个手动存档栏位，可以不需要这步
            //或者这步作为读取最近存档的方式，另外实现一个新的手动读取存档的功能
            GameDataMgr.GetInstance().PlayerDataInfo = new PlayerInfo();

            //重新解析数据并且创建新的PlayerDataInfo
            //GameDataMgr.GetInstance().ParseData();

        }
       
    }

    //执行读取存档后所需要的逻辑
    public void LoadScene()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_system48-start", false);
        //隐藏起始界面的panel
        UIManager.GetInstance().HidePanel("StartPanel");
    }
}
