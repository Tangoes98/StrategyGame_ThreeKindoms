using System;
using System.Collections;
using System.Collections.Generic;
using DebugConsole;
using UnityEngine;
using UnityEngine.UI;

public class T_TurnSystem : MonoBehaviour
{
    #region =========== Singleton ================
    public static T_TurnSystem Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }
    #endregion


    #region ============= Variables =============

    [SerializeField] int _currentTurn;
    [SerializeField] bool _isPlayerTurn;

    public event Action<bool> E_TurnChanged;

    // === Reference ===
    [Header("Reference")]
    [SerializeField] Button _nextTurnButton;





    #endregion

    #region ========== Public Variables ==========

    public int G_CurrentTurn() => _currentTurn;
    public bool G_IsPlayerTurn() => _isPlayerTurn;
    public void G_NextTurnButtonOnClick() => NextTurnEvent();




    #endregion

    #region ============= Start/Update ===========

    void Start()
    {
        _currentTurn = 0;
        _isPlayerTurn = true;

        NextTurnButtonOnClick();
    }




    #endregion

    #region ============ Methods =============


    void NextTurnButtonOnClick()
    {
        _nextTurnButton.onClick.AddListener(() =>
        {
            NextTurnEvent();
        });
    }

    void NextTurnEvent()
    {
        _isPlayerTurn = !_isPlayerTurn;
        _currentTurn++;
        E_TurnChanged?.Invoke(_isPlayerTurn);
    }













    #endregion

}
