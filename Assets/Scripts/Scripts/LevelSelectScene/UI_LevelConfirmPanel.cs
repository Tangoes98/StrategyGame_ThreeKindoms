using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelConfirmPanel : BasePanel
{
    public Image content;
    private string currentLevel;
    private bool isC1_MainLevel1Finished;
    private bool isC1_SubLevel1Finished;

    private bool isC2_MainLevel1Finished;
    private bool isC2_SubLevel1Finished;

    protected override void Awake()
    {
        base.Awake();
        //�����°�ť����ؿ���ر�ҳ��
        GetControl<Button>("ButtonStartLevel").onClick.AddListener(startLevel);
        GetControl<Button>("ButtonReturn").onClick.AddListener(Return);


        isC1_MainLevel1Finished = GameDataMgr.GetInstance().PlayerDataInfo.C1_MainLevel1IsFinished;
        isC1_SubLevel1Finished = GameDataMgr.GetInstance().PlayerDataInfo.C1_SubLevel1IsFinished;

        EventCenter.GetInstance().AddEventListener<string>("LevelContentInfoUpdate", (CurrentLevel) =>
        {
            currentLevel = CurrentLevel;
            switch (currentLevel)
            {
                case "C1_MainLevel1":
                    if (isC1_MainLevel1Finished)
                        GetControl<TMP_Text>("TextLimit").text = "���½ڵȼ�����Ϊ3. �����ȼ����޽������ø��ྭ��ֵ. ������Ȼ���Ի�õ���.";
                    else
                        GetControl<TMP_Text>("TextLimit").text = "";
                    //content.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/LevelInfo/XX");
                    break;

                case "C1_SubLevel1":
                    if (isC1_SubLevel1Finished)
                        GetControl<TMP_Text>("TextLimit").text = "���½ڵȼ�����Ϊ3. �����ȼ����޽������ø��ྭ��ֵ. ������Ȼ���Ի�õ���.";
                    else
                        GetControl<TMP_Text>("TextLimit").text = "";
                    //content.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/LevelInfo/XX");
                    break;

                case "C2_MainLevel1":
                    if (isC2_MainLevel1Finished)
                        GetControl<TMP_Text>("TextLimit").text = "���½ڵȼ�����Ϊ3. �����ȼ����޽������ø��ྭ��ֵ. ������Ȼ���Ի�õ���.";
                    else
                        GetControl<TMP_Text>("TextLimit").text = "";
                    //content.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/LevelInfo/XX");
                    break;

                case "C2_SubLevel1":
                    if (isC2_SubLevel1Finished)
                        GetControl<TMP_Text>("TextLimit").text = "���½ڵȼ�����Ϊ3. �����ȼ����޽������ø��ྭ��ֵ. ������Ȼ���Ի�õ���.";
                    else
                        GetControl<TMP_Text>("TextLimit").text = "";
                    //content.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/LevelInfo/XX");
                    break;

            }
        });
    }

    private void Update()
    {
        Debug.Log(currentLevel);
    }

    public override void HideMe()
    {
        base.HideMe();
        EventCenter.GetInstance().RemoveEventListener<string>("LevelContentInfoUpdate", (Currentlevel) =>
        {
        });
    }

    public void startLevel()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
        //����ؿ�
    }

    public void Return()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
        UIManager.GetInstance().HidePanel("LevelConfirmPanel");
    }
}
