using System;
using System.Collections.Generic;
using System.Linq;
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

        #endregion


        #region ============= Monobehaviour =============================

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
        #endregion


        #region ================== Inputfield Utilities =================
        //* press "`" to enable the console panel
        void CallConsole()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                _isActive = !_isActive;
                _consolePanel.gameObject.SetActive(_isActive);
            }
        }

        //* event after "Enter" is pressed
        void OnEndEditing()
        {
            _inputField.onEndEdit.AddListener(OnEndEditEvent);
        }

        void OnEndEditEvent(string s)
        {
            _inputField.text = null;
            AddConsoleLog("Type in " + s);
            ConsoleCommand(s);
        }

        //* add tmpro text object to the console
        void AddConsoleLog(string s)
        {
            var logObject = Instantiate(_consoleTextObject, _consoleContentObject);

            if (logObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI tmproText))
            {
                tmproText.text = s;
            }
        }

        #region ======== Console Commands =========
        enum CommandList
        {
            AddMorale,
            SubMorale,
            SetFire,


        }
        void ConsoleCommand(string s)
        {
            string command = ReadInputCommand(s);

            switch (command)
            {
                case "AddMorale":
                    Debug.Log("AddMorale");
                    break;
                case "SubMorale":
                    Debug.Log("SubMorale");
                    break;
                case "SetFire":
                    Command_SetFire(s);
                    break;
            }

        }

        string ReadInputCommand(string s)
        {
            string[] _strings = Enum.GetNames(typeof(CommandList));
            string command = "EMPTY";

            foreach (var item in _strings)
            {
                if (!s.Contains(item)) continue;
                command = item;
            }
            return command;
        }
        void Command_SetFire(string s)
        {
            T_GridPosition gp = CommandInputToGridPosition(s);
            T_Terrain terrain = T_LevelGridManager.Instance.G_GetGridPosData(gp).GetSurfaceTerrain();
            terrain.G_GetChildTerrainType<T_Forest>().G_SetIsFlaming(true);
        }


        int CommandInputToCombinedInt(string s)
        {
            List<char> intChars = CharToNumberCharList(s);
            string outString = new(intChars.ToArray());
            return StringToInt(outString);
        }

        int StringToInt(string s)
        {
            int outInt;
            if (int.TryParse(s, out outInt)) return outInt;
            else return -1;
        }
        List<char> CharToNumberCharList(string s)
        {
            List<char> intChars = new();
            foreach (var item in s.ToArray())
            {
                if (!char.IsNumber(item)) continue;
                intChars.Add(item);
            }
            return intChars;
        }
        T_GridPosition CommandInputToGridPosition(string s)
        {
            List<char> charList = CharToNumberCharList(s);
            int x = StringToInt(charList[0].ToString());
            int z = StringToInt(charList[1].ToString());
            return new T_GridPosition(x, z);
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
