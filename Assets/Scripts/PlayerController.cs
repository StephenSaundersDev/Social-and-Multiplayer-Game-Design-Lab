using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;//**

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;

    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Joystick moveStick;
    public Joystick camStick;

    Vector3 direction;

    private float directionY;

    bool pressedJump;
    public bool groundedPlayer;

    float jHorizontal = 0f;
    float jVertical = 0f;

    Vector3 playerVelocity;

    public PhotonView view;//**

    public Button jumpButton;

    float CameraAngle;
    float CameraAngleSpeed = 2f;


    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (view.IsMine) //**
        {
        PlayerMovement();
        CameraControls();
        }
    }

    public void PlayerMovement()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        jHorizontal = moveStick.Horizontal * speed;
        jVertical = moveStick.Vertical * speed;
        direction = new Vector3(jHorizontal, 0f, jVertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void CameraControls()
    {
        CameraAngle += camStick.Horizontal * CameraAngleSpeed;

        cam.position = transform.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * new Vector3(0, 3.54f, -9.4f);
        cam.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - cam.position, Vector3.up);
    }

    public void JumpButtonPressed()
    {
            if (groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            }
    }

    public void SetJoysticks(GameObject camera) //*
    {
        Joystick[] tempJoystickList = camera.GetComponentsInChildren<Joystick>();
        foreach(Joystick temp in tempJoystickList)
        {
            if (temp.tag == "Joystick Movement")
                moveStick = temp;
            else if (temp.tag == "Joystick Camera")
                camStick = temp;
        }

        Button[] tempButtonList = camera.GetComponentsInChildren<Button>();
        foreach (Button temp in tempButtonList)
        {
            if (temp.tag == "Jump Button")
                jumpButton = temp;
        }

        jumpButton.onClick.AddListener(JumpButtonPressed);

        cam = camera.transform;
    }

}
