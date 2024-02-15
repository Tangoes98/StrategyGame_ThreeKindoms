using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace DebugConsole
{
    public class T_GameConsole : MonoBehaviour
    {

        #region ================= Variables ====================
        // IN-GAME DEBUGGER
        public static T_GameConsole Instance;

        TextMeshProUGUI tmPro;

        //=== Settings === 
        [SerializeField] bool _isActive;
        event Action<string> E_OnEndEdit;

        // === Reference ===
        [SerializeField] Transform _consolePanel;
        [SerializeField] TMP_InputField _inputField;
        [SerializeField] Transform _consoleContentObject;
        [SerializeField] Transform _consoleTextObject;







        #endregion ============================================================

        #region =================== Public Getters ===================

        public void G_ConsoleLog(string s) => AddConsoleLog(s);

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
            E_OnEndEdit += OnEndEditEvent;
        }


        private void Update()
        {
            CallConsole();


            if (!_isActive) return;




        }



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
                E_OnEndEdit?.Invoke(text);
            });
        }

        void OnEndEditEvent(string s)
        {
            Debug.Log(s);
            _inputField.text = null;
            AddConsoleLog(s);

        }

        void AddConsoleLog(string s)
        {
            var logObject = Instantiate(_consoleTextObject, _consoleContentObject);

            if (logObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI tmproText))
            {
                tmproText.text = s;
            }
        }

    }
}
