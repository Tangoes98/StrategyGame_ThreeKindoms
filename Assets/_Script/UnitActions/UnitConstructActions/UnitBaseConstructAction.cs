using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBaseConstructAction : UnitBaseAction
{
    [SerializeField] protected string _actionName;
    [SerializeField] protected int _actionRange;
    [SerializeField] protected int _actionCost;
    [SerializeField] protected int _useCount;
    [SerializeField] protected bool _isSpendingConstructionCost;
    [SerializeField] protected int _actionConstructionCost;
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

    public bool IsOverUseCount() => _useCount < 1 && _useCount > -1;
    //public void SetBuildActionCountdown(int buildActionCountdown) => _buildActionCountdown = buildActionCountdown;


    protected void UnitIdentificationCheck<ConstructionType>(Transform constructionPrfab) where ConstructionType : Construction
    {
        bool isEenmyUnit = _unit.IsEnemyUnit();
        ConstructionType camp = constructionPrfab.GetComponent<ConstructionType>();
        if (isEenmyUnit) camp.SetConstructionOccupationCondition(Construction.ConstructionOccupationConditionType.Enemy);
        if (!isEenmyUnit) camp.SetConstructionOccupationCondition(Construction.ConstructionOccupationConditionType.Friend);
    }
}
