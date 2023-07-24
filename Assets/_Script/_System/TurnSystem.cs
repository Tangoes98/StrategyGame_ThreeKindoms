using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance;
    public event Action OnTurnChanged;

    [SerializeField] int _turnNumber;
    bool _isPlayerTurn = true;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }



    public void NextTurn()
    {
        _turnNumber++;
        _isPlayerTurn = !_isPlayerTurn;

        OnTurnChanged?.Invoke();
    }

    public int GetTurnNumber() => _turnNumber;
    public bool IsPlayerTurn() => _isPlayerTurn;
}

