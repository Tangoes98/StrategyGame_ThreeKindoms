using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] CinemachineVirtualCamera cinemachineCam;
    CinemachineTransposer cinemachineTransposer;
    Vector3 targetOffset;

    float MIN_FOLLOW_Y_OFFSET = 2f;
    float MAX_FOLLOW_Y_OFFSET = 12f;

    void Start()
    {
        cinemachineTransposer = cinemachineCam.GetCinemachineComponent<CinemachineTransposer>();
        targetOffset = cinemachineTransposer.m_FollowOffset;

    }

    void Update()
    {
        Vector3 moveVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveVector.z = +1f;

        if (Input.GetKey(KeyCode.S)) moveVector.z = -1f;

        if (Input.GetKey(KeyCode.D)) moveVector.x = +1f;

        if (Input.GetKey(KeyCode.A)) moveVector.x = -1f;


        Vector3 transformMovement = transform.forward * moveVector.z + transform.right * moveVector.x;
        transform.position += transformMovement * moveSpeed * Time.deltaTime;

        Vector3 rotationVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.E)) rotationVector.y = -1f;

        if (Input.GetKey(KeyCode.Q)) rotationVector.y = +1f;

        transform.eulerAngles += rotationVector * Time.deltaTime * rotationSpeed;




        Vector3 followOffset = cinemachineTransposer.m_FollowOffset;

        float zoomAmount = 1f;

        if (Input.mouseScrollDelta.y > 0) targetOffset.y -= zoomAmount;

        if (Input.mouseScrollDelta.y < 0) targetOffset.y += zoomAmount;

        targetOffset.y = Mathf.Clamp(targetOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        float zoomSpeed = 5f;
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetOffset, Time.deltaTime * zoomSpeed);
    }
}
