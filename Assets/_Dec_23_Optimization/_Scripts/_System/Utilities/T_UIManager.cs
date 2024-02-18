using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_UIManager : MonoBehaviour
{
    #region =========== Singleton ================
    public static T_UIManager Instance;
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

    #region ========== Variables =================
    [SerializeField] Transform _unitInfoPanel;

    #endregion


    void Start()
    {
        // Events
        T_TurnSystem.Instance.E_TurnChanged += EventOnTurnChanged;
    }


    #region ========== Event Methods =========

    void EventOnTurnChanged(bool value)
    {
        _unitInfoPanel.gameObject.SetActive(value);
    }



    #endregion

}
