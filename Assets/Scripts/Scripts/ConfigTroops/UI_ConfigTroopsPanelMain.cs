using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_ConfigTroopsPanelMain : BasePanel
{
    //����Toggle�ĸ������������ToggleGroup
    public Transform SwitchGeneralButtons;
    //ʹ���ֵ�洢�������ɵ�toggle
    private Dictionary<string, Toggle> toggleDict = new Dictionary<string, Toggle>();

    //Toggle֮��ļ��
    public float ToggleSpacing;
    //ʹ��list�洢Toggle�������
    private List<GameObject> generatedToggles = new List<GameObject>();
    //Toggle��ر���
    private int currentStartIndex = 0;
    private int maxTogglesPerPage = 6;
    private int dictionaryCount;

    //ʹ��dictionary�洢���ӵ�е��佫��Ϣ
    private Dictionary<string, General> PlayerOwnedGenerals;
    //����һ��General��洢��ǰѡ�еĽ���������
    public General currentSelectedGeneral;
    //����һ��int�洢��ǰʣ���δ�������Ե�
    private int UnassignedAttributePoint;

    //ʹ��bool��ʾ��ǰ�����������Ŀ���״̬
    private bool AssignAttributePointPanelIsOpen = false;
    //��ʾ�Ƿ�ȷ�ϸ������Ե�
    private bool ChangeConfirmed;

    //����һ��������λ�������ñ���
    public UI_TroopCell SelectTroop;
    //��ǰѡ��ı���
    public Troop currentSelectTroop;

    //��ʾ������������
    private bool showTroopTree;
    private bool showBagPanel2;

    public override void ShowMe()
    {
        base.ShowMe();
        //�����ʱ�͸������ӵ�е��佫����toggle
        GenerateTogglesFromDictionary(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral);

        //����TroopMgr�е��¼������������϶���ص��¼�
        UI_TroopMgr.GetInstance().InitListener();

        //����¼��������������Է�����崫�����¼�
        EventCenter.GetInstance().AddEventListener("ChangeConfirmed", () =>
        {
            //���ȷ�����Է��䣬����boolΪtrue
            ChangeConfirmed = true;
        });
    }

    private void Start()
    {
        //����ȷ�ϸ������Ե�boolΪfalse
        ChangeConfirmed = false;
   
        //�������Button�л�Toggle��ʾ����ʾ���ཫ��
        GetControl<Button>("ButtonShowNextToggle").onClick.AddListener(ShowNextPage);
        GetControl<Button>("ButtonShowPreviousToggle").onClick.AddListener(ShowPreviousPage);

        showTroopTree = false;
        GetControl<Button>("ButtonShowLevelUpPanel").onClick.AddListener(ShowTroopTree);
        GetControl<Button>("ButtonShowBag").onClick.AddListener(ShowBagPanel2);
    }

    private void Update()
    {
        //��Ϸ��������Update�м���ĸ�Toggle��ѡ����
        GetSelectedToggle();
       
    }

    public override void HideMe()
    {
        base.HideMe();
        //�ر�TroopMgr�еļ���
        UI_TroopMgr.GetInstance().RemoveListener();

        //�������ʱ�Ƴ��¼�����
        EventCenter.GetInstance().RemoveEventListener("ChangeConfirmed", () =>
        {

        });
    }

    /// <summary>
    /// ����ĸ�Toggle��ѡ�У�ִ��ѡ�����Toggle��Ҫִ�е��߼�
    /// </summary>
    /// <returns></returns>
    public Toggle GetSelectedToggle()
    {
        foreach (var pair in toggleDict)
        {
            Toggle toggle = pair.Value;
            if (toggle.isOn)
            {
                currentSelectedGeneral = PlayerOwnedGenerals[toggle.name];

                GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral = currentSelectedGeneral;
/*
                Debug.Log(currentSelectedGeneral.GeneralName);
                Debug.Log(GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral.GeneralName);
                Debug.Log(GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral.currentSelectTroop.TroopName);
*/
                //����Ĭ��ѡ��ı���Ϊ�佫��Ĭ�ϱ���
                currentSelectTroop = PlayerOwnedGenerals[toggle.name].currentSelectTroop;

                Debug.Log(currentSelectedGeneral.GeneralName);

                //Debug.Log(toggle.name);
                ChangeImage("ImageGeneral", ResourceManager.GetInstance().Load<Sprite>("Sprites/" + toggle.name));
                //�佫��������
                ChangeText("TextGeneralLevel", "�ȼ���" + PlayerOwnedGenerals[toggle.name].Level.ToString());
                ChangeText("TextGeneralName", PlayerOwnedGenerals[toggle.name].GeneralName);
                //����ֵ����߼�
                ChangeText("TextEXP", "����ֵ��" + PlayerOwnedGenerals[toggle.name].CurrentEXP + "/" + PlayerOwnedGenerals[toggle.name].MaxEXP);
                //�����ʹ���龰Ӧ����ʹ�þ�����ߣ���ʱ����������������ǰ����ֵ������������Ҫ�ľ���ֵ,������
                if (PlayerOwnedGenerals[toggle.name].CurrentEXP >= PlayerOwnedGenerals[toggle.name].MaxEXP)
                {
                    //�����������¼�����,������������е��佫���ݣ������������Ϊ�佫����ʣ�ྭ��
                    EventCenter.GetInstance().EventTrigger("GeneralLevelUp", toggle.name, PlayerOwnedGenerals[toggle.name].CurrentEXP - PlayerOwnedGenerals[toggle.name].MaxEXP);
                    //�õ�����ʱ��õĿɷ������Ե㣬Ȼ����ʾδ�������Ե����ʾ
                    UnassignedAttributePoint = PlayerOwnedGenerals[toggle.name].UnassignedAttributePoints;
                    UpdateToggles(generatedToggles);

                    //��������¾�����
                    EventCenter.GetInstance().EventTrigger("GeneralExpChange", currentSelectedGeneral);

                    //�������������ʾ�����������
                    //�ڷ�����������д������Է��䣬�Լ�������ʾ��
                    if (!AssignAttributePointPanelIsOpen)
                    {
                        UIManager.GetInstance().ShowPanel<UI_AssignAttributePointPanel>("AssignAttributePointPanel", E_UI_Layer.Top, (panel) =>
                        {
                            panel.generalKey = toggle.name;
                        });
                    }
                    //���������������Ѿ��򿪣��һ�û����ɷ��䣬�Ҵ�ʱ��ǰ����ֵ������������Ҫ�ľ���ֵ
                    else if (AssignAttributePointPanelIsOpen)
                    {
                        //ʲôҲ����
                    }
               ////////////////////////////////////////////////////////////////////////////////////

                         //������������bool����Ϊtrue
                         AssignAttributePointPanelIsOpen = true;

                }

                //�����������������ʾ״̬�����Ѿ�ȷ��������Է���
                if (AssignAttributePointPanelIsOpen && ChangeConfirmed && PlayerOwnedGenerals[toggle.name].UnassignedAttributePoints == 0)
                {

                    //������������bool����Ϊfalse
                    AssignAttributePointPanelIsOpen = false;
                    //����Toggle�ϵ���ʾ��������ʾ����ʾ
                    UpdateToggles(generatedToggles);
                    //����ȷ�ϸ������Ե�boolΪfalse
                    ChangeConfirmed = false;
                }
              



                //�����δ�����������0���Ҵ�ʱ�����������û�д�
                if (PlayerOwnedGenerals[toggle.name].UnassignedAttributePoints > 0 && !AssignAttributePointPanelIsOpen)
                {
                    //�򿪷����������
                    UIManager.GetInstance().ShowPanel<UI_AssignAttributePointPanel>("AssignAttributePointPanel", E_UI_Layer.Top, (panel) =>
                    {
                        //��panel��Key��������������
                        panel.generalKey = toggle.name;
                    });
                    //������������bool����Ϊtrue
                    AssignAttributePointPanelIsOpen = true;
                }





                ChangeText("TextHp", "����ֵ(HP)��" + PlayerOwnedGenerals[toggle.name].CurrentHp + "/" + PlayerOwnedGenerals[toggle.name].Hp);

                ChangeText("TextStrength", "���£�" + PlayerOwnedGenerals[toggle.name].Strength);

                ChangeText("TextLeaderShip", "ͳ�ʣ�" + PlayerOwnedGenerals[toggle.name].LeaderShip);

                ChangeText("TextWisdom", "��ı��" + PlayerOwnedGenerals[toggle.name].Wisdom);

                ChangeText("TextAttack", "��������" + PlayerOwnedGenerals[toggle.name].Attack);

                ChangeText("TextDefense", "��������" + PlayerOwnedGenerals[toggle.name].Defense);

                ChangeText("TextCriticalChance", "�����ʣ�" + (PlayerOwnedGenerals[toggle.name].CriticalRate * 100f).ToString("0.00") + "%");

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                //���µ�ǰװ���ı�����λ�ϵı���
                UpdateSelectTroop();

                //��������
                ChangeText("TextSelectTroop", "���֣�" + currentSelectTroop.TroopName);

                ChangeText("TextMorale", "ʿ��ֵ��" + currentSelectTroop.Morale);
                ChangeText("TextMobility", "��������" + currentSelectTroop.Movement);
                ChangeText("TextTroopInfoDetail",  currentSelectTroop.TroopFeature);
                //��һ��ͼƬ����Ӧÿ�����ֵ����飬��ʽ������ͼƬ��ӽ���
                //ChangeImage("ImageTroopInfo", ResourceManager.GetInstance().Load<Sprite>("Sprites/" + currentSelectTroop.TroopID));

                ChangeText("TextTroopSkill", currentSelectTroop.TroopSkill);
                GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral[toggle.name].TroopSkill = currentSelectTroop.TroopSkill;
                ChangeImage("ImageTroopSkillIcon", ResourceManager.GetInstance().Load<Sprite>("Sprites/" + currentSelectTroop.TroopSkill + "Icon"));
                //OpenTroopLevelUpPanelFunc();
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //��������
                //��������




                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //���Ӿ���ֵʱ��ʱ��¼����仯
                EventCenter.GetInstance().EventTrigger("GeneralExpChange", currentSelectedGeneral);
                //��Ҫһ�����ܣ��ڵ������Է�������AssignAttributePointPanelIsOpenΪtrue����ChangeConfirmedΪfalse������£����������ǰ������ʹ���κ����Ӿ���ֵ�Ĺ��ܣ�ʹ�þͳ������ҵ�����ʾ��������Ҫ�������������Է�����ܼ���ʹ�þ���ֵ���ߡ�.
                //��Ҫ�ҩʱ���Ӿ���ֵ�Ĺ���

                return toggle;
            }
        }
        return null;
    }

    /// <summary>
    /// �򿪱��������ĺ���
    /// </summary>
    public void OpenTroopLevelUpPanelFunc()
    {
        //�򿪵�ǰ��ѡ���ֵı�����
        if (showTroopTree)
        {

            GameObject button = GameObject.Find("ButtonShowLevelUpPanel");
            if (button != null)
            {
                Button buttonComponent = button.GetComponent<Button>();
                TMP_Text buttonText = buttonComponent.GetComponentInChildren<TMP_Text>();
                buttonText.text = "�رձ�����";
            }

            UIManager.GetInstance().ShowPanel<UI_TroopLevelUpPanel2>("TroopLevelUpPanel2", E_UI_Layer.Top);
            StartCoroutine(DelayedAccessSetTroopAndGeneral());
       
        }
        else
        {

            GameObject button = GameObject.Find("ButtonShowLevelUpPanel");
            if (button != null)
            {
                Button buttonComponent = button.GetComponent<Button>();
                TMP_Text buttonText = buttonComponent.GetComponentInChildren<TMP_Text>();
                buttonText.text = "�򿪱�����";
            }

            UIManager.GetInstance().HidePanel("TroopLevelUpPanel2");
        }
    }


    /// <summary>
    /// �򿪱������ĺ���
    /// </summary>
    public void OpenBagPanel2Func()
    {
        //�򿪵�ǰ��ѡ���ֵı�����
        if (showBagPanel2)
        {

            GameObject button = GameObject.Find("ButtonShowBag");
            if (button != null)
            {
                Button buttonComponent = button.GetComponent<Button>();
                TMP_Text buttonText = buttonComponent.GetComponentInChildren<TMP_Text>();
                buttonText.text = "�ر����";
            }

            UIManager.GetInstance().ShowPanel<UI_BagPanel2>("BagPanel2", E_UI_Layer.Top);
            StartCoroutine(DelayedAccessToCurrentSelectedGeneral());

        }
        else
        {

            GameObject button = GameObject.Find("ButtonShowBag");
            if (button != null)
            {
                Button buttonComponent = button.GetComponent<Button>();
                TMP_Text buttonText = buttonComponent.GetComponentInChildren<TMP_Text>();
                buttonText.text = "ʹ�õ���";
            }

            UIManager.GetInstance().HidePanel("BagPanel2");
        }
    }


    /// <summary>
    /// �����ť�ı�򿪱�������������,����ѡ�佫�ı��������,���뵱ǰ��ѡ�ı��ֺͽ���������
    /// </summary>
    public void ShowTroopTree()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
        showTroopTree = !showTroopTree;
        OpenTroopLevelUpPanelFunc();
    }

    public void ShowBagPanel2()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound20_Maou-Select", false);
        showBagPanel2 = !showBagPanel2;
        OpenBagPanel2Func();
    }


    /// <summary>
    /// ���µ�ǰѡ��ı���
    /// </summary>
    public void UpdateSelectTroop()
    {
        SelectTroop.InitInfo(currentSelectTroop);
    }

    /// <summary>
    /// ��Toggle�л��ͷ����¼�
    /// </summary>
    /// <param name="value"></param>
    public void OnToggleValueChanged(bool value)
    {
        if (value)
        {
            MusicMgr.GetInstance().PlaySound("maou_se_sound_switch01", false);
            StartCoroutine(DelayedAccessToCurrentSelectedGeneral());
            StartCoroutine(DelayedAccessSetTroopAndGeneral());
            currentSelectedGeneral = GameDataMgr.GetInstance().PlayerDataInfo.currentSelectedGeneral;
        }
    }

    /// <summary>
    /// Э��Ŀ���ǵ�currentSelectedGeneral�л����ٷ���
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayedAccessToCurrentSelectedGeneral()
    {
        yield return null; // �ȴ���һ֡
        EventCenter.GetInstance().EventTrigger("ToggleChanged", currentSelectedGeneral);
    }

    /// <summary>
    /// Э��Ŀ���ǵ�currentSelectedGeneral�л����ٷ��ͽ����ͱ��������Ϣ
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayedAccessSetTroopAndGeneral()
    {
        yield return null; // �ȴ���һ֡
        EventCenter.GetInstance().EventTrigger("UpdateTroopTree", currentSelectedGeneral, currentSelectTroop);
    }

    /// <summary>
    ///����������ƻ����������������ͼƬ 
    /// </summary>
    /// <param name="ComponentName">�����</param>
    /// <param name="NewSprite">��Ҫ���ص�ͼƬ</param>
    public void ChangeImage(string ComponentName, Sprite NewSprite)
    {
        Transform target = transform.Find(ComponentName);
        if (target != null)
        {
            //���Ի�ȡ�Ӷ����ϵ�Image���
            Image imageComponent = target.GetComponent<Image>();
            if (imageComponent != null)
            {
                // �޸�ͼƬ
                imageComponent.sprite = NewSprite;
            }
        }
    }

    /// <summary>
    ///����������ƻ�����������������ı� 
    /// </summary>
    /// <param name="ComponentName">�����</param>
    /// <param name="NewSprite">��Ҫ�ĳɵ��ı�</param>
    public void ChangeText(string ComponentName, string NewText)
    {
        Transform target = transform.Find(ComponentName);
        if (target != null)
        {
            // ���Ի�ȡ�Ӷ����ϵ�Text���
            TMP_Text textComponent = target.GetComponent<TMP_Text>();
            if (textComponent != null)
            {
                // �޸��ı�
                textComponent.text = NewText;
            }
        }
    }

    /// <summary>
    ///������ҵĽ�����������Toggle 
    /// </summary>
    /// <param name="dictionary"></param>
    public void GenerateTogglesFromDictionary(Dictionary<string, General> dictionary)
    {
        //���֮ǰʣ���Toggle
        ClearGeneratedToggles();
        //�趨����Toggle֮��ľ���
        ToggleSpacing = 180f;
        dictionaryCount = dictionary.Count;
        //������Ĵ�������佫���ݵ�dictionary��������
        PlayerOwnedGenerals = dictionary;

        //�趨�ֵ��ŷ�Χ
        int endIndex = currentStartIndex + Mathf.Min(dictionaryCount, maxTogglesPerPage) - 1;
        if (endIndex >= dictionaryCount)
        {
            endIndex -= dictionaryCount;
        }
        //�趨Toggleλ��
        float offset = -((float)(maxTogglesPerPage - 1) * ToggleSpacing) / 2f;

        for (int i = 0; i < Mathf.Min(dictionaryCount, maxTogglesPerPage); i++)
        {
            int indexInDictionary = (currentStartIndex + i) % dictionaryCount;

            KeyValuePair<string, General> pair = dictionary.ElementAt(indexInDictionary);

            //���Toggle��GameObject����������Toggle
            GameObject toggleObject = Instantiate(ResourceManager.GetInstance().Load<GameObject>("UI/ToggleGeneralPage"), SwitchGeneralButtons);
            Toggle toggle = toggleObject.GetComponent<Toggle>();

            //��ø�����Group����Toggle����ȥ
            ToggleGroup toggleGroup = toggleObject.GetComponentInParent<ToggleGroup>();
            toggle.group = toggleGroup;


            //��pair.key�洢�������������Key���ֵ��е�����Ϣ
            toggleObject.name = pair.Key;
            UnassignedAttributePoint = PlayerOwnedGenerals[toggleObject.name].UnassignedAttributePoints;

            //����Key�����ָı�Toggle������ͼƬ�ϵ�ͷ��
            //sprite�е�ͷ����Ҫ��Key�����ּ�Head����������LiuBeiHead
            Transform imageHeadTransform = toggleObject.transform.Find("ImageHead");
            if (imageHeadTransform != null)
            {
                Image imageHead = imageHeadTransform.GetComponent<Image>();
                if (imageHead != null)
                {
                    imageHead.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + pair.Key + "Head");
                }
            }
            //����Key�����ָı�Toggle�ϵ�ͷ��
            Image toggleImage = toggle.GetComponent<Image>();
            toggleImage.sprite = ResourceManager.GetInstance().Load<Sprite>("Sprites/" + pair.Key + "Head");
            //����Key��Ӧ��ֵ���佫���ָı�Toggle�ı�������
            TMP_Text TextToggleGeneralName = toggle.GetComponentInChildren<TMP_Text>();
            TextToggleGeneralName.text = pair.Value.GeneralName;
            //��toggle�����ڸ�����
            toggleObject.transform.SetParent(SwitchGeneralButtons);

            //����Toggle��λ��
            toggleObject.transform.localPosition = new Vector3(offset + i * ToggleSpacing, 450f, 0f);
            //��Toggle����Toggle�ֵ䣬������Ϊ��key������ҵ���
            toggleDict.Add(toggleObject.name, toggle);
            //��Toggle��������list
            generatedToggles.Add(toggleObject);

            //����Ϸʱ��ʾ��ӵ�е��佫�Ƿ���δ��������Ե�
            UpdateToggles(generatedToggles);
        }

        //����ע���¼�����
        RegisterToggleListeners();
    }

    private void RegisterToggleListeners()
    {
        foreach (var toggle in toggleDict.Values)
        {
            toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    /// <summary>
    /// �������δ�������Ե�ʱ��ʾ��ʾ�ĺ���
    /// </summary>
    /// <param name="toggleList"></param>
    public void UpdateToggles(List<GameObject> toggleList)
    {
        foreach (GameObject generatedToggle in toggleList)
        {
            UnassignedAttributePoint = PlayerOwnedGenerals[generatedToggle.name].UnassignedAttributePoints;
            //��Ҫһ�����������������
            Transform childTransform = generatedToggle.transform.Find("ImagePointHint");
            if (childTransform != null)
            {
                Image imageComponent = childTransform.GetComponent<Image>();
                if (imageComponent != null)
                {
                    imageComponent.enabled = false;
                    if (UnassignedAttributePoint > 0)
                    {
                        imageComponent.enabled = true;
                    }
                    else
                    {
                        imageComponent.enabled = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// ���֮ǰ��Toggle�ĺ���
    /// </summary>
    public void ClearGeneratedToggles()
    {
        foreach (GameObject toggleObject in generatedToggles)
        {
            Destroy(toggleObject);
        }
        generatedToggles.Clear();
        toggleDict.Clear();
    }
    /// <summary>
    /// �����һҳ��ʾ��һ����Toggle
    /// </summary>
    public void ShowNextPage()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound_switch01", false);
        currentStartIndex += 1;

        if (currentStartIndex >= dictionaryCount)
        {
            currentStartIndex = 0;
        }

        GenerateTogglesFromDictionary(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral);

        UpdateToggles(generatedToggles);
    }
    /// <summary>
    /// �����һҳ��ʾ��һ����Toggle
    /// </summary>
    public void ShowPreviousPage()
    {
        MusicMgr.GetInstance().PlaySound("maou_se_sound_switch01", false);
        currentStartIndex -= 1;

        if (currentStartIndex < 0)
        {
            currentStartIndex = dictionaryCount - 1;
        }

        GenerateTogglesFromDictionary(GameDataMgr.GetInstance().PlayerDataInfo.PlayerOwnedGeneral);

        UpdateToggles(generatedToggles);
    }
}
