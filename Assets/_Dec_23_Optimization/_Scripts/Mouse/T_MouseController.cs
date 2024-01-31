using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_MouseController : MonoBehaviour
{
    public static T_MouseController Instance;

    [SerializeField]
    LayerMask _mouseLayerMask;

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
        transform.position = G_GetMouseWorldPosition();

        // Check if mouse button is active
        Is_LMB_Down();
        Is_RMB_Down();
    }

    public Vector3 G_GetMouseWorldPosition()
    {
        // Generate ray from main camera to the world
        // return the world position
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, _mouseLayerMask);
        return raycastHit.point;
    }

    public T_GirdPosition G_GetMouseGridPosition()
    {
        return T_LevelGridManager.Instance.G_WorldToGridPosition(G_GetMouseWorldPosition());
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
