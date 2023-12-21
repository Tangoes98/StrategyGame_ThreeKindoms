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


    [SerializeField] bool _isUnitMoveAction;



    #region Public Access Properties

    public bool GetIsUnitMoveAction() => _isUnitMoveAction;
    public void SetIsUnitMoveAction(bool isMoveAction) => _isUnitMoveAction = isMoveAction;







    #endregion




    void Start()
    {
        _isUnitMoveAction = false;
    }




}
