using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_GridValidationVisual : MonoBehaviour
{
    [Serializable]
    struct gridValidationVisualStruct
    {
        public string name;
        public MeshRenderer visualMeshRenderer;
    }

    [SerializeField] List<gridValidationVisualStruct> _gridValidationVisuals;
    Dictionary<string, MeshRenderer> _gridValidationVisualDictionary;


    //T_GirdPosition _gridPosition;


    #region ========== Public Methods =================

    public Dictionary<string, MeshRenderer> G_GetGridValidationVisualDictionary() => _gridValidationVisualDictionary;

    public void G_SetMoveGridVisual(bool condition) => _gridValidationVisualDictionary["MOVE"].enabled = condition;
    public void G_SetTargetGridVisual(bool condition) => _gridValidationVisualDictionary["TARGET"].enabled = condition;




    #endregion








    void Start()
    {
        Initialization_GridValidationVisualDictionary();

    }



    void Initialization_GridValidationVisualDictionary()
    {
        _gridValidationVisualDictionary = new();

        foreach (var item in _gridValidationVisuals) _gridValidationVisualDictionary.Add(item.name, item.visualMeshRenderer);

        foreach (var item in _gridValidationVisualDictionary) item.Value.enabled = false;
    }



}



