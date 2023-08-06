using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBaseConstructAction : UnitBaseAction
{
    [SerializeField] protected string _actionName;
    [SerializeField] protected int _actionRange;
    [SerializeField] protected int _actionCost;
    [SerializeField] protected int _useLimit;
    [SerializeField] protected bool _isSpendingConstructionCost;
    [SerializeField] protected int _constructCost;
    protected Vector3 _targetPosition;
    //protected int _buildActionCountdown = 2;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
    }

    public bool IsOverUseCount() => _useLimit < 1 && _useLimit > -1;
    //public void SetBuildActionCountdown(int buildActionCountdown) => _buildActionCountdown = buildActionCountdown;


    protected void UnitIdentificationCheck<ConstructionType>(Transform constructionPrfab) where ConstructionType : Construction
    {
        bool isEenmyUnit = _unit.IsEnemyUnit();
        ConstructionType camp = constructionPrfab.GetComponent<ConstructionType>();
        if (isEenmyUnit) camp.SetConstructionOccupationCondition(Construction.ConstructionOccupationConditionType.Enemy);
        if (!isEenmyUnit) camp.SetConstructionOccupationCondition(Construction.ConstructionOccupationConditionType.Friend);
    }


    public abstract int GetConstructionActionCost();
    public abstract bool IsSpendingConstructionCost();
}
