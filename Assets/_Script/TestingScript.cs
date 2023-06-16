using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    GridSystem _gridSystem;
    [SerializeField] Transform _gridObjectVisual;
    void Start()
    {
        _gridSystem = new GridSystem(10, 10, 2);
        _gridSystem.CreateGridObjectVisual(_gridObjectVisual);

    }

    void Update()
    {
        //Debug.Log(_gridSystem.GetGridPosition(MouseToWorld.Instance.GetMouseWorldPosition()));
    }
}
