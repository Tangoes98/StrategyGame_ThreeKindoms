using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    GridPosition _unitGridPosition;

    UnitMovementAction _moveAction;
    UnitSelfTriggerAction _selfTriggerAction;
    UnitAttackAction _attackAction;
    UnitBaseAction[] _baseActionArray;

    [SerializeField] bool _isEnemy;
    [SerializeField] int _unitActionPoints;
    int _maxActionPoints;


    void Awake()
    {
        _moveAction = GetComponent<UnitMovementAction>();
        _selfTriggerAction = GetComponent<UnitSelfTriggerAction>();
        _attackAction = GetComponent<UnitAttackAction>();

        _baseActionArray = GetComponents<UnitBaseAction>();

    }

    void Start()
    {
        _unitGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        LevelGrid.Instance.AddUnitToGridObject(_unitGridPosition, this);


        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        _maxActionPoints = _unitActionPoints;
    }

    void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        // Update GridObject contains current unit
        if (newGridPosition != _unitGridPosition)
        {
            LevelGrid.Instance.RemoveUnitFromGridObject(_unitGridPosition, this);
            _unitGridPosition = newGridPosition;
            LevelGrid.Instance.AddUnitToGridObject(_unitGridPosition, this);
        }
    }

    #region Action points functions
    public bool TrySpendActionPointsToTakeAction(UnitBaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(UnitBaseAction baseAction)
    {
        return _unitActionPoints >= baseAction.GetActionCost();
    }

    void SpendActionPoints(int amount)
    {
        _unitActionPoints -= amount;
    }

    public int GetActionPoints() => _unitActionPoints;

    void TurnSystem_OnTurnChanged()
    {
        bool isPlayerTurnCheck = TurnSystem.Instance.IsPlayerTurn();
        
        if (_isEnemy && isPlayerTurnCheck) return;
        if (!_isEnemy && !isPlayerTurnCheck) return;

        _unitActionPoints = _maxActionPoints;
    }

    #endregion




    public GridPosition GetUnitGridPosition() => _unitGridPosition;
    public UnitBaseAction[] GetUnitBaseActionArray() => _baseActionArray;
    public bool IsEnemyUnit() => _isEnemy;
}
