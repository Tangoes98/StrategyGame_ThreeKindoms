using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DebugConsole
{
    public class T_GameConsole : MonoBehaviour
    {

        #region ================= Variables ====================
        // IN-GAME DEBUGGER
        public static T_GameConsole Instance;

        TextMeshProUGUI tmPro;
        [SerializeField] bool _isActive;
        event Action<string> E_onEndEdit;
        event Action E_onNextTurnButtonClicked;

        // === Reference ===
        [Header("Reference")]
        [SerializeField] Transform _consolePanel;
        [SerializeField] TMP_InputField _inputField;
        [SerializeField] Transform _consoleContentObject;
        [SerializeField] Transform _consoleTextObject;

        [Header("Stats Panel")]
        [SerializeField] Button _debugButton_nextTurn;
        [SerializeField] TextMeshProUGUI _currentTurn;
        [SerializeField] TextMeshProUGUI _gameStatsText;







        #endregion

        #region =================== Public Properties ===================

        public void Log(string s) => AddConsoleLog(s);
        public void G_ConsoleLogAttack(T_Unit a, T_Unit b, int damage, float heightMulti, float moraleMulti)
            => AddConsoleLog_OnDamage(a, b, damage, heightMulti, moraleMulti);

        #endregion =========================================================
        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple instances occured");
                Destroy(Instance);
            }
            Instance = this;
        }


        private void Start()
        {
            _isActive = false;
            _consolePanel.gameObject.SetActive(_isActive);

            OnEndEditing();
            NextTurnButtonOnClick();

        }


        private void Update()
        {
            CallConsole();


            if (!_isActive) return;

            //ShowCurrentTurnText();
            ShowGameStatsText();


        }


        #region ================== Inputfield Utilities =================
        void CallConsole()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                _isActive = !_isActive;
                _consolePanel.gameObject.SetActive(_isActive);
            }
        }

        void OnEndEditing()
        {
            _inputField.onEndEdit.AddListener((string text) =>
            {
                E_onEndEdit?.Invoke(text);
            });
            E_onEndEdit += OnEndEditEvent;
        }

        void OnEndEditEvent(string s)
        {
            _inputField.text = null;
            AddConsoleLog("Type in " + s);

            ConsoleCommand(s);
        }

        void AddConsoleLog(string s)
        {
            var logObject = Instantiate(_consoleTextObject, _consoleContentObject);

            if (logObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI tmproText))
            {
                tmproText.text = s;
            }
        }

        #region ======== Console Commands =========
        enum ConsoleCommandList
        {
            ADD_MORALE,

        }
        void ConsoleCommand(string s)
        {
            // float outFloat = T_CombatManager.Instance.G_GetHeightDamageMultiplier(StringToInt(s));
            // AddConsoleLog("HeightDamageMultiplier is " + outFloat.ToString());
            // if (!s.Contains("COMMAND")) return;
            // Debug.Log("Contains Command");

            if (s.Contains("ADDMORALE"))
            {
                T_Morale.Instance.G_SetCurrentMorale(T_Morale.Instance.G_GetCurrentMorale() + 1);
            }
            if (s.Contains("SUBMORALE"))
            {
                T_Morale.Instance.G_SetCurrentMorale(T_Morale.Instance.G_GetCurrentMorale() - 1);
            }
        }

        int StringToInt(string s)
        {
            int outInt;
            if (int.TryParse(s, out outInt)) return outInt;
            else return -1;
        }

        #endregion

        #region ============ Console Log - DealDamage ============

        void AddConsoleLog_OnDamage(T_Unit a, T_Unit b, int damage, float heightMulti, float moraleMulti)
        {
            string s1 =
            $"{a.ToString()} dealing damage {damage} to {b.ToString()}";
            string s2 =
            $"HeightDamageMultiplier is {heightMulti}, MoraleDamageMultiplier is {moraleMulti}";
            AddConsoleLog(s1);
            AddConsoleLog(s2);
        }






        #endregion
        #endregion

        #region ==================== Console Button =======================

        void NextTurnButtonOnClick()
        {
            _debugButton_nextTurn.onClick.AddListener(() => { E_onNextTurnButtonClicked?.Invoke(); });
            E_onNextTurnButtonClicked += NextTurnButtonOnClickEvent;
        }

        void NextTurnButtonOnClickEvent()
        {
            T_TurnSystem.Instance.G_NextTurnButtonOnClick();
        }







        #endregion

        #region ================ Console Stats =============
        void ShowGameStatsText()
        {

            _gameStatsText.text = GameStats();


        }
        string GameStats()
        {



            string currentTurn = "CURRENT_TURN: " + T_TurnSystem.Instance.G_CurrentTurn().ToString() + "\n";
            string currentMorale = "CURRENT_MORALE: " + T_Morale.Instance.G_GetCurrentMorale() + "\n";

            string gameStats =
            currentTurn + currentMorale;
            return gameStats;
        }



        #endregion

    }
}
