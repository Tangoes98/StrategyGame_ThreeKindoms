using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class T_GridDataVisual : MonoBehaviour
{
    [SerializeField] TextMeshPro _gridInfo;
    T_GridData _gridData;

    void Start()
    {
        //T_LevelGridManager.Instance.
    }
    void Update()
    {
        ShowText();
    }


    public void SetIndividualGridData(T_GridData gridData)
    {
        _gridData = gridData;
    }

    void ShowText()
    {
        _gridInfo.text = _gridData.ToString();
    }




}
