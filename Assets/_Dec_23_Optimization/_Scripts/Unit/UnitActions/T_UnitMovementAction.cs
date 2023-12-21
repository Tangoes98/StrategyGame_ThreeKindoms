using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_UnitMovementAction : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _stopDistance;
    [SerializeField] T_Unit _unit;

    [Header("DEBUG AREA")]
    [SerializeField] Vector3 _targetPosition;

    #region Public Properties

    public void SetStartTargetPosition(Vector3 position) => _targetPosition = position;

    #endregion



    void Update()
    {
        if (T_UnitSelection.Instance.GetSelectedUnit() != _unit) return;

        MouseClickToMove();
        UnitMovement();

    }

    void MouseClickToMove()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _targetPosition = T_MouseController.Instance.GetMouseWorldPosition();
        }
    }


    void UnitMovement()
    {
        if (Vector3.Distance(_targetPosition, _unit.transform.position) > _stopDistance)
        {
            //set the direction where unit move to
            Vector3 targetDirection = (_targetPosition - _unit.transform.position).normalized;

            // unit movement to target direction
            _unit.transform.position += targetDirection * _moveSpeed * Time.deltaTime;

            // unit rotation animation
            _unit.transform.forward = Vector3.Slerp(transform.forward, targetDirection, Time.deltaTime * _rotateSpeed);
        }

    }








}
