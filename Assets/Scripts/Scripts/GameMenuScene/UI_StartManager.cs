using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartManager : BasePanel
{
    //��ҵĴ洢·��
    private static string PlayerInfoSaveAdress;

    protected override void Awake()
    {
        //�����awake����ʼ����Ϣ
        base.Awake();
        //������Դ��������awake
        PlayerInfoSaveAdress = Application.persistentDataPath + "/PlayerSaveData.json";
    }

    protected override void OnClick(string buttonName)
    {
        //������ͨ�������ж��ĸ���ť�������,Ȼ��ֱ�������ﴦ���߼�
        switch (buttonName)
        {
            //�����ʼ����Ϸ���������¾���
            case "StartGame":

                MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
                //�첽������ɺ�ִ�г�ʼ����Ϸ�ķ���
                SceneMgr.GetInstance().LoadSceneAsyn("Story0", StartNewGame);
                break;
            case "ExitGame":

                QuitGame();
                break;
            case "Setting":
                Debug.Log("Setting");
                break;
            case "LoadGame":
                //��ȡ����
                GameDataMgr.GetInstance().LoadPlayerInfo();
                //��ת����Ӧ���½ں�ҳ��
                SceneMgr.GetInstance().LoadSceneAsyn(GameDataMgr.GetInstance().PlayerDataInfo.currentPhase, LoadScene);
                break;
        }
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

    /// <summary>
    /// ��ʼ����Ϸ�ĺ������״ν�����Ϸ���ڵ�0�µĹ��½׶�
    /// </summary>
    public void StartNewGame()
    {

        MusicMgr.GetInstance().PlaySound("maou_se_system48-start", false);
        //�������
        UIManager.GetInstance().HidePanel("StartPanel");
        //��ʼ����Ϸ֮ǰ��ɾ���ɴ浵������
        if (File.Exists(PlayerInfoSaveAdress))
        {
            //��ʼ����Ϸǰ�����֮ǰ��Ϸ���¼�����
            //�������������ܵ���ParseData�е��¼��������ִ�У���1������ö������
            //EventCenter.GetInstance().Clear();
            
            //��Ŀǰ�ķ�ʽ������Ҫע�ⲻ�����ParseData�е��¼�////////

            //ɾ��ԭ�浵�ļ�������Ժ�������ֶ��浵��λ�����Բ���Ҫ�ⲽ
            File.Delete(PlayerInfoSaveAdress);

            //���Json���ݣ�������ִ��ParseDataʱ��Ϊ�Ѿ�����Щֵ������
            /* GameDataMgr.GetInstance().itemInfoDic = new Dictionary<int, Item>();
            GameDataMgr.GetInstance().generalInfoDic = new Dictionary<int, General>();
            GameDataMgr.GetInstance().troopInfoDic = new Dictionary<int, Troop>();
            GameDataMgr.GetInstance().expInfodic = new Dictionary<int, EXP>();
*/
            //���������ݣ�����Ժ�������ֶ��浵��λ�����Բ���Ҫ�ⲽ
            //�����ⲽ��Ϊ��ȡ����浵�ķ�ʽ������ʵ��һ���µ��ֶ���ȡ�浵�Ĺ���
            GameDataMgr.GetInstance().PlayerDataInfo = new PlayerInfo();

            //���½������ݲ��Ҵ����µ�PlayerDataInfo
            //GameDataMgr.GetInstance().ParseData();

        }
       
    }

    //ִ�ж�ȡ�浵������Ҫ���߼�
    public void LoadScene()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_system48-start", false);
        //������ʼ�����panel
        UIManager.GetInstance().HidePanel("StartPanel");
    }
}
