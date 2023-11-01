using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using LitJson;
using System.IO;
using Unity.VisualScripting;
//װ��Json�����list�����ֱ�����Json�ļ���ֵ��Ӧ
public class Dialogues
{
    public List<Dialogue>dialogue;
}
//װ��ÿ�����ݵ��࣬���ֱ�����Json�ļ���ֵ��Ӧ
[System.Serializable]
public class Dialogue
{
    public string IsOption;
    public int DialogueID;
    public string Character;
    public string PicturePosition;
    public string Content;
    public int JumpToID;
}

public class DialogueManager : MonoBehaviour
{
    //���ڼ������Ƿ�λ�ڰ�ť��������
    private bool hitButton;
    //��ť����
    public Rect ButtonArea;

    //���MenuPanel�Ƿ���
    private bool MenuPanelIsOpen;

    //����ɫ����
    public SpriteRenderer SpriteLeft;
    //�Ҳ��ɫ����
    public SpriteRenderer SpriteRight;
    //����
    public SpriteRenderer backGround;

    //��ɫ�����ı�
    public TMP_Text nameText;
    //��ɫ�Ի��ı�
    public TMP_Text contentText;

    //���浱ǰ�ĶԻ�ID����ֵ
    public int dialogIndex = 0;

    //Ϊ�˷�����ã������ֵ�װ�����ݼ����е�����
    private Dictionary<int, Dialogue> dialogdic = new Dictionary<int, Dialogue>();

    //�ж��Ƿ�����л�����/ѭ����Ч������
    public bool canSend;
    //��ǰ�Ի�ID����ֵ�������ж��Ƿ�����л�����
    public int currentdialogIndex;

    AudioSource audioSourceBird;
    AudioSource audioSourceHorse;
    public float currentBGMValue = 0.3f;

    private void Awake()
    {
        UIManager.GetInstance().ShowPanel<UI_DiaPanel>("DiaPanel", E_UI_Layer.Bot);

        //�����˵��Ƿ�򿪣�һ���򿪾ͽ�������Ϊtrue
        MenuPanelIsOpen = false;
        EventCenter.GetInstance().AddEventListener("MenuIsOpen", () =>
        {
            MenuPanelIsOpen = true;
        });
        EventCenter.GetInstance().AddEventListener("MenuIsClose", () =>
        {
            MenuPanelIsOpen = false;
        });

    }
    // Start is called before the first frame update
    void Start()
    {
        //��ʼʱ���ű�������
        MusicMgr.GetInstance().PlayBkMusic("Ancestral Spirits");
        MusicMgr.GetInstance().ChangeBKValue(0.3f);
        //����������
        InputMgr.GetInstance().StartOrEndCheck(true);
        //�ö�������ʱ�Ƴ���������ֹͣ��ع���
        EventCenter.GetInstance().AddEventListener<KeyCode>("KeyButtonDown", CheckInputDown);
        //EventCenter.GetInstance().AddEventListener<KeyCode>("KeyButtonUp", CheckInputUp);

        //��ʼʱ�Զ�����Json�ļ�
        ParseData();

        //��ʼ���趨
        canSend = true;
        currentdialogIndex = dialogIndex;
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("KeyButtonDown", CheckInputDown);
        EventCenter.GetInstance().RemoveEventListener("MenuIsOpen", () => { });
        EventCenter.GetInstance().RemoveEventListener("MenuIsClose", () => { });
        InputMgr.GetInstance().StartOrEndCheck(false);
    }


    // Update is called once per frame
    void Update()
    {
        checkButtonHit();
        //��飬�����е��ض�λ��ʱ���л�������BGM
        CheckEnviroment("Home", 0);
        CheckEnviroment("Camp", 6);
        CheckEnviroment("Home", 12);
        //������ʾ�͸��¶Ի�
        ShowDialogue();
    }
    /// <summary>
    /// ����Json�ļ�
    /// </summary>
    public void ParseData()
    {
        //��ȡ������Json�ļ�
        Dialogues dialogues = new Dialogues();
        /*string jsonStr = File.ReadAllText(Application.streamingAssetsPath + "/Dialogue0.json");
        dialogues = JsonMapper.ToObject<Dialogues>(jsonStr);*/
        dialogues = JsonMgr.Instance.LoadData<Dialogues>("StroyDIalogue/Dialogue0");
        //�����ݼ��ϰ���ID�ŷֱ����
        for (int i = 0; i < dialogues.dialogue.Count; i++)
        {
            dialogdic.Add(dialogues.dialogue[i].DialogueID, dialogues.dialogue[i]);
        }

    }

    /// <summary>
    /// ���ݶԻ�ID���õ���ϸ��Ϣ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dialogue GetInFo(int id)
    {
        if (dialogdic.ContainsKey(id))
            return dialogdic[id];
        return null;
    }

    /// <summary>
    /// �����ı�
    /// </summary>
    /// <param name="ID">Json�ļ��е�ID����</param>
    /// <param name="name">Character��Name</param>
    /// <param name="content">�Ի�����</param>
    public void UpdateText(int ID, string name, string content)
    {
        nameText.text = name;
        contentText.text = content;
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="name"></param>
    /// <param name="position"></param>
    public void UpdateImage(int ID, string name, string position)
    {
        if (position == "left")
        {
            SpriteLeft.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + name);
        }
        else if (position == "right")
        {
            SpriteRight.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + name);
        }
        else if (position == "no")
        {
            SpriteLeft.sprite = null;
            SpriteRight.sprite = null;
        }
    }

    /// <summary>
    /// ��ʾ�ı�������
    /// </summary>
    public void ShowDialogue()
    {
        //�������������ʱ�������ı�������
        if (dialogdic.ContainsKey(dialogIndex))
        {
            UpdateText(GetInFo(dialogIndex).DialogueID, GetInFo(dialogIndex).Character, GetInFo(dialogIndex).Content);
            UpdateImage(GetInFo(dialogIndex).DialogueID, GetInFo(dialogIndex).Character, GetInFo(dialogIndex).PicturePosition);
        }

    }

    /// <summary>
    /// �ı䱳��ͼƬ
    /// </summary>
    /// <param name="name"></param>
    public void ChangeBackGround(string name)
    {
        backGround.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + name);
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="key"></param>
    private void CheckInputDown(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Mouse0:
                //�������Ч
                MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
                PressMouse();
                break;
        }
    }


    /// <summary>
    /// ������棬Ŀǰδ����
    /// </summary>
    public void Clear()
    {
        PoolManager.GetInstance().Clear();
        EventCenter.GetInstance().Clear();
    }

    /// <summary>
    /// �����������ִ�е��߼�
    /// </summary>
    private void PressMouse()
    {
        //�����겻λ�ڰ�ť���������Ҳ˵�û�д�
        if (!hitButton && !MenuPanelIsOpen)
        {
            //�������ʹ�Ի�������ȥ
            dialogIndex = GetInFo(dialogIndex).JumpToID;
            currentdialogIndex = dialogIndex;

            if (dialogIndex == dialogdic.Count)
            {
                SceneMgr.GetInstance().LoadSceneAsyn("Intro_Chapter1", () =>
                {

                    //UIManager.GetInstance().HidePanel("DiaPanel");
                    //Clear();

                });
            }
        }
        // ����������жϣ���������������𲽼�С����ֵ
        if (dialogIndex >= dialogdic.Count - 6)
        {
            if(audioSourceBird != null)
            {
                MusicMgr.GetInstance().StopSound(audioSourceBird);
            }
            // �𲽼�С����ֵ��������0.1
            float targetValue = Mathf.Max(0.1f, currentBGMValue - 0.05f);
            MusicMgr.GetInstance().ChangeBKValue(targetValue);
            currentBGMValue = targetValue;
        }

    }

    /// <summary>
    /// �ı价��
    /// </summary>
    /// <param name="Environment"></param>
    public void ChangeEnvironment(string Environment)
    {

        if (Environment == "Home")
        {
            MusicMgr.GetInstance().StopSound(audioSourceHorse);
            canSend = false;
            ChangeBackGround("Home");
            MusicMgr.GetInstance().PlaySound("Bird", true, (source) =>
            {
                audioSourceBird = source;
            });
            MusicMgr.GetInstance().ChangeSoundValue(0.3f);
        }

        if (Environment == "Camp")
        {
            MusicMgr.GetInstance().StopSound(audioSourceBird);
            canSend = false;
            ChangeBackGround("Camp");
            MusicMgr.GetInstance().PlaySound("CampHorse", true, (source) =>
            {
                audioSourceHorse = source;
            });
            MusicMgr.GetInstance().ChangeSoundValue(0.3f);
        }
    }



    /// <summary>
    /// ���ʲôʱ���л�����
    /// </summary>
    /// <param name="Environment">//��������</param>
    /// <param name="changeID">�����ڸ�IDʱ�л�</param>
    public void CheckEnviroment(string Environment,int changeID)
    {
        //�ж��Ƿ��ܹ��л�����������
        if (canSend && currentdialogIndex == changeID)
        {
            ChangeEnvironment(Environment);
            canSend = false;
            currentdialogIndex++;
            if (currentdialogIndex > dialogIndex)
            {
                canSend = true;
            }
        }
    }

    /// <summary>
    /// �������Ƿ�λ�ڰ�ť��������
    /// </summary>
    private void checkButtonHit()
    {
            Vector3 MousePosition = Input.mousePosition;
            if (ButtonArea.Contains(MousePosition))
            {
                hitButton = true;
            }
            else
            {
                hitButton = false;
            }
    }
}
