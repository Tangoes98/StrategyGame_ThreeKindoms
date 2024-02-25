using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Forest : T_Terrain
{
    #region ========== Variables ==========

    [Header("Reference")]
    [SerializeField] GameObject m_flameVisual;

    [Header("Settings")]
    [SerializeField] int m_moveCostWithFlaming;
    int m_moveCostwithoutFlaming;

    [Header("Debugging")]
    [SerializeField] bool m_isFlaming;





    #endregion

    #region ============== Public ===============
    public void G_SetIsFlaming(bool value) => IsFlamingCheck(value);

    #endregion

    #region =========== Monobehaviour =========
    protected override void Start()
    {
        base.Start();
        m_isFlaming = false;
        m_moveCostwithoutFlaming = _terrainMoveCost;
        IsFlamingCheck(m_isFlaming);

    }

    protected override void Update()
    {
        base.Update();

    }


    #endregion

    #region  =========== Flame related ==============

    void IsFlamingCheck(bool value)
    {
        m_isFlaming = value;
        m_flameVisual.SetActive(value);

        if (value) _terrainMoveCost = m_moveCostWithFlaming;
        else _terrainMoveCost = m_moveCostwithoutFlaming;

        _gridPathNode.G_SetTerrainMoveCost(_terrainMoveCost);
    }

    #endregion
}
