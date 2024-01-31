using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_DrawPathline : MonoBehaviour
{
    public static T_DrawPathline Instance;


    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }



    [SerializeField] LineRenderer _line;



    #region ========== Public Methods =========
    public void G_DrawPathline(List<T_GirdPosition> girdPositions) => DrawPathLine(girdPositions);
    public void G_ClearPathline() => ClearLine();





    #endregion ========================================


    void Start()
    {
        ClearLine();
    }




    void ClearLine()
    {
        _line.positionCount = 0;
    }

    // Position list count, pos1, pos2...
    void DrawPathLine(List<T_GirdPosition> girdPositions)
    {

        if (girdPositions == null)
        {
            ClearLine();
            return;
        }
        _line.positionCount = girdPositions.Count;

        for (int i = 0; i < _line.positionCount; i++)
        {
            _line.SetPosition(i, T_LevelGridManager.Instance.G_GridToWorldPosition(girdPositions[i]));
        }

    }





}
