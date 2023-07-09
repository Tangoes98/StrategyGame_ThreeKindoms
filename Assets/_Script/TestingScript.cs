using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Debug.Log(_gridSystem.GetGridPosition(MouseToWorld.Instance.GetMouseWorldPosition()));
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseToWorld.Instance.GetMouseWorldPosition());
            GridPosition startGridPosition = new GridPosition(0,0);

            GridPosition testGridPosition = new GridPosition(1,1);
            Pathfinding.Instance.GetNode(testGridPosition).SetMoveCost(0.5f);

            List<GridPosition> gridPositionList = Pathfinding.Instance.GetValidMoveGridPoisitionList(startGridPosition, 2);

            for (int i = 0; i < gridPositionList.Count; i++)
            {
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i]) + Vector3.up * 1000f,
                    Color.red,
                    1000f

                );
            }

        }
    }
}


//LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1])