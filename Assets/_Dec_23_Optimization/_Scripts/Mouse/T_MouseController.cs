using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_MouseController : MonoBehaviour
{
    public static T_MouseController Instance;

    [SerializeField] LayerMask _mouseGroundLayerMask;
    [SerializeField] LayerMask _mouseGridSelectionLayerMask;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }

    void Update()
    {
        // For mouse cursor if possible
        transform.position = G_GetMouseWorldPosition(_mouseGroundLayerMask);

        // Check if mouse button is active
        Is_LMB_Down();
        Is_RMB_Down();
    }

    public Vector3 G_GetMouseWorldPosition(LayerMask layerMask)
    {
        // Generate ray from main camera to the world
        // return the world position
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, layerMask);
        return raycastHit.point;
    }

    // Raycast layermask for gird_visuals, return grid_position based on grid_visual_object transform 
    public T_GridPosition G_GetMouseGridPosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, _mouseGridSelectionLayerMask);
        if (!raycastHit.transform) return new T_GridPosition(0, 0);
        Vector3 gridWorldPosition = raycastHit.transform.position;
        return T_LevelGridManager.Instance.G_WorldToGridPosition(gridWorldPosition);
    }

    public static bool Is_LMB_Down()
    {
        if (Input.GetMouseButtonDown(0)) return true;
        else return false;
    }
    public static bool Is_RMB_Down()
    {
        if (Input.GetMouseButtonDown(1)) return true;
        else return false;
    }







}
