using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    GridPosition _unitGridPosition;

    UnitMovementAction _moveAction;
    UnitSelfTriggerAction _selfTriggerAction;
    UnitAttackAction _attackAction;
    List<UnitBaseAction> _UnitBaseActionList;
    List<UnitBaseConstructAction> _UnitBaseConstructsActionList;
    ConstructCampAction _campAction;
    ConstructFlagAction _flagAction;
    GainConstructPointsAction _gainConstructPointsAction;
    ConstructRopeLadderAction _ropeLadder;
    ConstructFloatingBridge _floatingBridge;


    [SerializeField] bool _isEnemy;
    [SerializeField] int _unitActionPoints;
    int _maxActionPoints;
    HealthSystem _healthSystem;


    [Header("CONSTRUCTION FIELD")]
    [SerializeField] int _unitConstructionPoints;


    void Awake()
    {
        _moveAction = GetComponent<UnitMovementAction>();
        _selfTriggerAction = GetComponent<UnitSelfTriggerAction>();
        _attackAction = GetComponent<UnitAttackAction>();
        _UnitBaseActionList = new List<UnitBaseAction>();

        _campAction = GetComponent<ConstructCampAction>();
        _flagAction = GetComponent<ConstructFlagAction>();
        _gainConstructPointsAction = GetComponent<GainConstructPointsAction>();
        _ropeLadder = GetComponent<ConstructRopeLadderAction>();
        _floatingBridge = GetComponent<ConstructFloatingBridge>();

        _UnitBaseConstructsActionList = new List<UnitBaseConstructAction>();

        _healthSystem = GetComponent<HealthSystem>();
    }

    void Start()
    {
        _unitGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        LevelGrid.Instance.AddUnitToGridObject(_unitGridPosition, this);


        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        _maxActionPoints = _unitActionPoints;

        _healthSystem.OnDead += HealthSystem_OnDead;



        UnitActionValidation(_moveAction, _UnitBaseActionList);
        UnitActionValidation(_selfTriggerAction, _UnitBaseActionList);
        UnitActionValidation(_attackAction, _UnitBaseActionList);

        UnitConstructActionValidation(_campAction, _UnitBaseConstructsActionList);
        UnitConstructActionValidation(_flagAction, _UnitBaseConstructsActionList);
        UnitConstructActionValidation(_gainConstructPointsAction, _UnitBaseConstructsActionList);
        UnitConstructActionValidation(_ropeLadder, _UnitBaseConstructsActionList);
        UnitConstructActionValidation(_floatingBridge, _UnitBaseConstructsActionList);
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

    #region Unit Action Validation

    void UnitActionValidation(UnitBaseAction action, List<UnitBaseAction> actionList)
    {
        if (action.IsEnabled()) actionList.Add(action);
    }
    void UnitConstructActionValidation(UnitBaseConstructAction action, List<UnitBaseConstructAction> actionList)
    {
        if (action.IsEnabled()) actionList.Add(action);
    }

    #endregion

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

        _moveAction.SetMoveDistance(_moveAction.GetMaxMoveDistance());
    }

    #endregion

    #region OnUnitDeath
    void HealthSystem_OnDead()
    {
        LevelGrid.Instance.RemoveUnitFromGridObject(_unitGridPosition, this);
        Destroy(gameObject);
    }
    #endregion

    #region Unit Construction Points Functions
    public int GetUnitConstructionPoints() => _unitConstructionPoints;
    public void AddUnitConstructionPoints(int points) => _unitConstructionPoints += points;



    #endregion

    public GridPosition GetUnitGridPosition() => _unitGridPosition;
    public List<UnitBaseAction> GetUnitBaseActionList() => _UnitBaseActionList;
    public bool IsEnemyUnit() => _isEnemy;
    //public List<UnitBaseAction> GetUnitConstructActionList() => _UnitConstructsActionList;
    public List<UnitBaseConstructAction> GetUnitConstructActionList() => _UnitBaseConstructsActionList;
}
