using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Unit : MonoBehaviour
{


    // unit data has to update to grid data(position, ...)

    T_LevelGridManager _levelGridManagerInstance;
    void Start()
    {
        _levelGridManagerInstance = T_LevelGridManager.Instance;
    }

    void Update()
    {
        UpdateUnitGridPosition();
    }

    void UpdateUnitGridPosition()
    {
        T_GirdPosition girdPosition = _levelGridManagerInstance.WorldToGridPosition(this.transform.position);
        this.transform.position = _levelGridManagerInstance.GridToWorldPosition(girdPosition);
    }
}
