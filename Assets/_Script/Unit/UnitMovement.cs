using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [Header("Movement")]

    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _stopDistance;
    Vector3 _targetPosition;
    Unit _unit;
    [SerializeField] int _UnitMaxMoveDistance;


    void Start()
    {
        _unit = transform.GetComponent<Unit>();
        _targetPosition = transform.position;
    }

    void Update()
    {
        Move(_targetPosition);
    }

    void Move(Vector3 targetPosition)
    {
        if (Vector3.Distance(targetPosition, transform.position) > _stopDistance)
        {
            //set the direction where unit move to
            Vector3 targetDirection = (targetPosition - this.transform.position).normalized;

            // unit movement to target direction
            transform.position += targetDirection * _moveSpeed * Time.deltaTime;

            // unit rotation animation
            transform.forward = Vector3.Lerp(transform.forward, targetDirection, Time.deltaTime * _rotateSpeed);
        }
    }

    public void SetTargetPosition(Vector3 targetPosition) => _targetPosition = targetPosition;

    public List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetUnitGridPosition();

        for (int x = -_UnitMaxMoveDistance; x <= _UnitMaxMoveDistance; x++)
        {
            for (int z = -_UnitMaxMoveDistance; z <= _UnitMaxMoveDistance; z++)
            {

                // Unit will not move over than max move distance
                int moveDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (moveDistance > _UnitMaxMoveDistance) continue;


                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition reachableGridposition = offsetGridPosition + unitGridPosition;

                // Check if the girdPosition is inside gridSystem
                if (!LevelGrid.Instance.IsValidGridPosition(reachableGridposition)) continue;

                //Debug.Log(reachableGridposition);

                validGridPositionList.Add(reachableGridposition);

            }
        }
        return validGridPositionList;
    }
}
