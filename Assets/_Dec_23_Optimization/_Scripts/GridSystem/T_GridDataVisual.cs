using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class T_GridDataVisual : MonoBehaviour
{
    [SerializeField] TextMeshPro _gridInfo;
    [SerializeField] TextMeshPro _PathnodeInfo;
    T_GridData _gridData;
    T_Pathnode _pathNode;



    void Start()
    {
        //T_LevelGridManager.Instance.
    }
    void Update()
    {
        ShowText();
    }


    public void SetIndividualGridData(T_GridData gridData) => _gridData = gridData;
    public void SetPathNode(T_Pathnode pathNode) => _pathNode = pathNode;

    void ShowText()
    {
        _gridInfo.text = _gridData.ToString();


        int gcost = _pathNode.GetGCost();
        int hcost = _pathNode.GetHCost();
        int fcost = _pathNode.GetFCost();
        _PathnodeInfo.text =
            $"G:{gcost}\n" +
            $"H:{hcost}\n" +
            $"F:{fcost}\n";
    }




}
