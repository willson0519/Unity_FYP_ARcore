using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicThirdPerson : MonoBehaviour
{
    // Start is called before the first frame update
    public float CameraMoveSpeed = 120.0f;
    public GameObject CameraFollowObj;
    Vector3 followPOS;
    public float clampAngle = 80.0f;
    public float inputSensitivity = 150.0f;
    public GameObject CameraObj;
    public GameObject PlayerObj;
    public float camDistanceXToPlayer;
    public float camDistanceYToPlayer;
    public float camDistanceZToPlayer;
    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    public float smoothX;
    private float smoothY;
    private float rotX = 0.0f;
    private float rotY = 0.0f;
    public GameObject Player;
    public float movementSpeed = 0.5f;


    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = mouseX;
        finalInputZ = mouseY;

        rotY += finalInputX * inputSensitivity * Time.deltaTime;
        rotX += finalInputZ * inputSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        if (inputX != 0)
        {
            Player.transform.Translate(inputX * movementSpeed, 0f, 0f);
        }
        if (inputZ != 0)
        {
            Player.transform.Translate(0f, 0f, inputZ * movementSpeed);
        }
        if (mouseX != 0)
        {
            Player.transform.Rotate(0f, finalInputX * inputSensitivity * Time.deltaTime, 0f);
        }

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

    }

    void LateUpdate()
    {
        CameUpdater();
    }

    void CameUpdater()
    {
        Transform target = CameraFollowObj.transform;

        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
