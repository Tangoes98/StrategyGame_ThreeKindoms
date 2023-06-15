using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [Header("Movement")]

    [SerializeField]
    float _moveSpeed;
    [SerializeField]
    float _rotateSpeed;
    [SerializeField]
    float _stopDistance;
    public Vector3 _targetPosition;


    void Start()
    {
        _targetPosition = transform.position;
    }

    void Update()
    {
        // get mouse to world position
        if (Input.GetMouseButtonDown(0))
        {
            _targetPosition = MouseToWorld.instance.GetMouseWorldPosition();
        }

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
}
