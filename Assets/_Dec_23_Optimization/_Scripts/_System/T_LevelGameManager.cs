using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_LevelGameManager : MonoBehaviour
{
    public static T_LevelGameManager Instance;
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



    #region ===================== Public Properties =======================








    #endregion ===============================================================




    void Start()
    {

    }




}
