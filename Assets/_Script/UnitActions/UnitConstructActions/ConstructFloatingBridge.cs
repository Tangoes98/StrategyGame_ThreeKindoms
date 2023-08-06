using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructFloatingBridge : UnitBaseConstructAction
{
    [Header("Action_FloatingBridge")]
    [SerializeField] Transform _floatingBridgePrefab;


    GridPosition _firstBridgePosition;
    bool _isSecondAction;
    int _buildActionCountdown = 2;

    #region Getters and Setters


    public void SetBuildActionCountDown(int countNumber) => _buildActionCountdown = countNumber;
    public void SetIsSecondAction(bool isSecondAction) => _isSecondAction = isSecondAction;

    #endregion



    public override int GetActionCost() => _actionCost;
    public override string GetActionName() => _actionName;
    public override bool IsEnabled() => _isActionEnabled;
    public override int GetConstructionActionCost() => _constructCost;
    public override bool IsSpendingConstructionCost() => _isSpendingConstructionCost;



    protected override void Update()
    {
        base.Update();

        if (!_isActive) return;

        BuildSecondBridge();

        BuildFloatingBridge(_targetPosition);

        if (_buildActionCountdown < 1) ActionCompleted();


    }

    public override void TakeAction(GridPosition mouseGridPosition, Action onActionCompleted)
    {
        _isActive = true;
        _onActionCompleted = onActionCompleted;
        _buildActionCountdown = 2;
        _firstBridgePosition = mouseGridPosition;

        Pathfinding.Instance.UpdatingGridMoveCost(mouseGridPosition);

        BuildFirstBridge(mouseGridPosition);

    }

    void BuildFirstBridge(GridPosition gridPosition)
    {
        Debug.Log("FloatingBridge_Built_1");
        SetActionWorldPosition(gridPosition);

        _isSecondAction = true;
        _buildActionCountdown -= 1;
    }



    void BuildSecondBridge()
    {
        if (!_isSecondAction) return;

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("FloatingBridge_Built_2");
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseToWorld.Instance.GetMouseWorldPosition());

            // return if not selecting valid grid position
            if (!IsValidActionGridPosition(mouseGridPosition)) return;

            Pathfinding.Instance.UpdatingGridMoveCost(mouseGridPosition);

            SetActionWorldPosition(mouseGridPosition);
            _buildActionCountdown -= 1;

        }
    }

    Vector3 SetActionWorldPosition(GridPosition gridPosition)
        => _targetPosition = LevelGrid.Instance.GetWorldPositionWithHeight(gridPosition);
    void BuildFloatingBridge(Vector3 worldPosition)
    {
        Transform campTransform = Instantiate(_floatingBridgePrefab, worldPosition, Quaternion.identity);
        Construction floatingBridge = campTransform.GetComponent<Construction>();

        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(worldPosition);
        LevelGrid.Instance.AddConstructionToGrdObject(gridPosition, floatingBridge);
    }


    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> actionGridPositionList = new List<GridPosition>();

        GridPosition startGridPosition = _unitGridPosition;

        if (_isSecondAction) startGridPosition = _firstBridgePosition;

        for (int x = -_actionRange; x <= _actionRange; x++)
        {
            for (int z = -_actionRange; z <= _actionRange; z++)
            {
                int distance = Mathf.Abs(x) + Mathf.Abs(z);
                if (distance > _actionRange) continue;

                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition tempValidGridposition = offsetGridPosition + startGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(tempValidGridposition)) continue;

                // Skip if gridposition is not Water
                PathNode pathNode = Pathfinding.Instance.GetNode(tempValidGridposition);
                var terrain = pathNode.GetTerrain().GetTerrainType();
                if (terrain != TerrainType.TerrainTypeEnum.StreamShallow) continue;

                // If floating bridge can be built above DeepStream 
                // ||terrain != TerrainType.TerrainTypeEnum.StreamDeep

                // Skip if gridposition is too high
                if (!Pathfinding.Instance.CanClimbNeighbourGrid(startGridPosition, tempValidGridposition)) continue;

                // Skip if gridposition already contains a Construction
                if (LevelGrid.Instance.HasConstructionOnGridPosition(tempValidGridposition)) continue;

                actionGridPositionList.Add(tempValidGridposition);
            }
        }
        if (_buildActionCountdown < 1)
        {
            List<GridPosition> enmptyGridPositionlist = new List<GridPosition>();
            return enmptyGridPositionlist;
        }

        return actionGridPositionList;
    }


}
