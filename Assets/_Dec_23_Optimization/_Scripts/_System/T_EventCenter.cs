using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_EventCenter : MonoBehaviour
{
    public static T_EventCenter Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }



    void Start()
    {
        T_UnitSelection.Instance.E_UnitSelected += UnitSelectedEvent;
        T_UnitSelection.Instance.E_UnitDeselected += UnitDeselectedEvent;






    }

    #region ========== Public Events =========

    public event Action EventCenter_UnitSelected;
    public event Action EventCenter_UnitDeselected;




    #endregion ==============================
    
    void UnitSelectedEvent()
    {
        EventCenter_UnitSelected?.Invoke();
    }

    void UnitDeselectedEvent()
    {
        EventCenter_UnitDeselected?.Invoke();
    }


}
