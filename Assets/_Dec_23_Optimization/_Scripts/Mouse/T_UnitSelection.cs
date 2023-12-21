using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_UnitSelection : MonoBehaviour
{
    public static T_UnitSelection Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;

    }

    [SerializeField] LayerMask _unitLayerMask;

    [SerializeField] T_Unit _selectedUnit;

    #region Public Properties

    public T_Unit GetSelectedUnit() => _selectedUnit;


    #endregion

    void Update()
    {
        SelectUnit();
        DeselectUnit();
    }


    // Casting ray to check unit
    Transform RaycastingUnit()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask);

        return raycastHit.transform;
    }

    // Mouse click to select/assign unit
    void SelectUnit()
    {
        if (!RaycastingUnit()) return;

        if (Input.GetMouseButtonDown(0)) _selectedUnit = RaycastingUnit().GetComponent<T_Unit>();
    }

    void DeselectUnit()
    {
        if (Input.GetMouseButtonDown(0) && !RaycastingUnit()) _selectedUnit = null;
    }





}
