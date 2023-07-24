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
    protected int _buildActionCountdown = 2;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
    }

    public bool IsOverUseCount() => _useCount < 1 && _useCount > 0;
    public void SetBuildActionCountdown(int buildActionCountdown) => _buildActionCountdown = buildActionCountdown;
}
