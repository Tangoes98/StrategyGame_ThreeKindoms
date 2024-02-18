using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Morale : MonoBehaviour
{
    #region =========== Singleton ================
    public static T_Morale Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }
    #endregion
    #region ======== Variables ========== 

    [SerializeField] int _currentMorale;


    #endregion
    #region ======== Public Properties =========

    public int G_GetCurrentMorale() => _currentMorale;

    public void G_SetCurrentMorale(int value) => _currentMorale = value;

    #endregion ==================================

    void Start()
    {
    
    }





}
