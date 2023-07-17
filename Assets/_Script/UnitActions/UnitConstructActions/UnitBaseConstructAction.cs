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

}
