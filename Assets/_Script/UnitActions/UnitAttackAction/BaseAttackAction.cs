using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseAttackAction : UnitBaseAction
{
    public enum UnitTypes
    {
        Archer, Crossbow, Halberd, Shield, Cavalry
    }

    [SerializeField] protected UnitTypes _unitType;
    [SerializeField] protected string _actionName;
    [SerializeField] protected int _actionCost;
    [SerializeField] protected int _attackRange;
    [SerializeField] protected int _excludedAttackRange;
    [SerializeField] protected int _damageAmount;

    protected List<GridPosition> _excludedGridPosition = new();



    protected void ShowActionGridPositionRange(GridPosition unitGridPosition, int range, int excludedRange, UnitTypes type)
    {
        List<GridPosition> rangedGridPositionList = new List<GridPosition>();
        List<GridPosition> excludedGridPositionList = new List<GridPosition>();

        for (int x = -excludedRange; x <= excludedRange; x++)
        {
            for (int z = -excludedRange; z <= excludedRange; z++)
            {
                int distance = Mathf.Abs(x) + Mathf.Abs(z);
                if (distance > excludedRange) continue;

                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition validGridPosition = unitGridPosition + offsetGridPosition;

                // gridposition in the unit system
                if (!LevelGrid.Instance.IsValidGridPosition(validGridPosition)) continue;

                excludedGridPositionList.Add(validGridPosition);
            }
        }

        _excludedGridPosition = excludedGridPositionList;

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                int distance = Mathf.Abs(x) + Mathf.Abs(z);

                if (type != UnitTypes.Halberd) if (distance > range) continue;

                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition showGridPosition = unitGridPosition + offsetGridPosition;

                // gridposition in the unit system
                if (!LevelGrid.Instance.IsValidGridPosition(showGridPosition)) continue;

                // gridposition is not unit self gridposition
                if (showGridPosition == unitGridPosition) continue;

                // skip if gridposition is in excludedGridPositionList
                if (excludedGridPositionList.Contains(showGridPosition)) continue;

                rangedGridPositionList.Add(showGridPosition);

            }
        }

        GridSystemVisual.Instance.ShowGridpositionRangeVisual(rangedGridPositionList);
    }














}
