using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraJoystick : MonoBehaviour
{
    public Transform playerCamera;
    public Joystick camStick;
    protected float CameraAngle;
    protected float CameraAngleSpeed = 2f;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        CameraAngle += camStick.Horizontal * CameraAngleSpeed;

        playerCamera.position = transform.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * new Vector3(0, 3.54f, -9.4f);
        playerCamera.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - playerCamera.position, Vector3.up);
    }
}
