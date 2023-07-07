using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathNodeVisual : MonoBehaviour
{
    [SerializeField] TextMeshPro _gridPositionText;
    [SerializeField] TextMeshPro _gCostText;
    [SerializeField] TextMeshPro _hCostText;
    [SerializeField] TextMeshPro _fCostText;
    [SerializeField] TextMeshPro _accumulatedMoveDistanceText;
    [SerializeField] TextMeshPro _moveCostText;

    PathNode _pathNode;

    void Start()
    {

    }
    void Update()
    {
        SetGridText();
    }

    public void SetPathNode(PathNode pathNode)
    {
        _pathNode = pathNode;
    }

    void SetGridText()
    {
        _gridPositionText.text = _pathNode.ToString();
        _gCostText.text = _pathNode.GetGCost().ToString();
        _hCostText.text = _pathNode.GetHCost().ToString();
        _fCostText.text = _pathNode.GetFCost().ToString();
        _accumulatedMoveDistanceText.text = _pathNode.GetAccucmulatedMoveDistance().ToString();
        _moveCostText.text = _pathNode.GetMoveCost().ToString();
    }

}
