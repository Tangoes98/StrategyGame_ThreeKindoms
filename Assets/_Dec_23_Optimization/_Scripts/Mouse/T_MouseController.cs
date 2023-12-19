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
        transform.position = GetMouseWorldPosition();
    }

    public Vector3 GetMouseWorldPosition()
    {
        // Generate ray from main camera to the world
        // return the world position
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, _mouseLayerMask);
        return raycastHit.point;
    }
}
