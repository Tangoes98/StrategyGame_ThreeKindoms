using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_GameManager : MonoBehaviour
{
    public static T_GameManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }
    
    public enum Game_State
    {
        UnitSelection,
        ActionPreview,
        EnemyTurn,
    }

    // T_Morale t_Morale;

    // public static T GetInstance<T>()
    // {

    // }



    #region ===================== Public Properties =======================








    #endregion ===============================================================




    void Start()
    {

    }




}
