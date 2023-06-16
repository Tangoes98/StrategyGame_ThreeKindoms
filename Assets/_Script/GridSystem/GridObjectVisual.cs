using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridObjectVisual : MonoBehaviour
{
    [SerializeField] TextMeshPro _gridPositionText;

    GridObject _gridObject;

    void Start()
    {

    }
    void Update()
    {
        SetGridText();
    }

    public void SetGridObeject(GridObject gridObject)
    {
        _gridObject = gridObject;
    }

    void SetGridText()
    {
        _gridPositionText.text = _gridObject.ToString();
    }
}
