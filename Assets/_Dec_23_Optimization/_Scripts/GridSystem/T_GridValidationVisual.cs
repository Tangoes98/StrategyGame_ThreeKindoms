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


    
    // Gird Validation Visual Names and Intro: //
    // MOVE_GRID --> LIGHT GREEN
    // ATTACK_RANGE --> LIGHT RED
    // VALID_ATTACK --> BRIGHT RED


    #region ========== Public Property =================

    public Dictionary<string, MeshRenderer> G_GetGridValidationVisualDictionary() => _gridValidationVisualDictionary;

    public void G_SetMoveGridVisual(bool condition) => _gridValidationVisualDictionary["MOVE_GRID"].enabled = condition;
    public void G_SetAttackRangeGridVisual(bool condition) => _gridValidationVisualDictionary["ATTACK_RANGE"].enabled = condition;

    public void G_SetGridVisual(string visualName, bool condition) => _gridValidationVisualDictionary[visualName].enabled = condition;





    #endregion ==================================================




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



