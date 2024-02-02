using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_DrawPathline : MonoBehaviour
{
    public static T_DrawPathline Instance;
    Vector3 _lineOffset = new Vector3(0, .5f, 0);



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

        List<Vector3> linePositions = new();

        for (int i = 0; i < girdPositions.Count; i++)
        {
            // Vector3 girdWorldPosition = T_LevelGridManager.Instance.G_GridToWorldPositionWithFloor(girdPositions[i]);
            // Vector3 lineOffset = new Vector3(0, .5f, 0);
            // Vector3 linePosition = girdWorldPosition + lineOffset;
            // _line.SetPosition(i, linePosition);




            var levelGird = T_LevelGridManager.Instance;

            Vector3 girdWorldPosition = levelGird.G_GridToWorldPositionWithFloor(girdPositions[i]);
            Vector3 linePosition = girdWorldPosition + _lineOffset;
            linePositions.Add(linePosition);

            if (i + 1 == girdPositions.Count) break;

            //todo: compare the gird floor infor
            if (levelGird.G_IsGridPositionsOnSameFloorHeight(girdPositions[i], girdPositions[i + 1], out Vector3 gridOffsetWorldpos)) continue;
            else linePositions.Add(gridOffsetWorldpos + _lineOffset);

        }

        _line.positionCount = linePositions.Count;
        for (int i = 0; i < _line.positionCount; i++)
        {
            _line.SetPosition(i, linePositions[i]);
        }

    }





}
