using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float moveInput;
    private float turnInput;
    public float fwdSpeed;
    public float revSpeed;
    public float turnSpeed;

    public Rigidbody SphereRigidbody;

    private void Start()
    {
        SphereRigidbody.transform.parent = null;
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        moveInput *= moveInput > 0 ? fwdSpeed : revSpeed;

        transform.position = SphereRigidbody.transform.position;

        float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
        transform.Rotate(0, newRotation, 0, Space.World);
    }

    private void FixedUpdate()
    {
        SphereRigidbody.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
    }
}