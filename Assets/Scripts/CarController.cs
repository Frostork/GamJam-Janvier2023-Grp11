using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private float _maxSpeed = 15f;
    [SerializeField] private float _drag = 0.98f;
    [SerializeField] private float _steerAngle = 20f;

    private Vector3 MoveForce;

    private void Update()
    {
        MoveForce += transform.forward * (_moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
        transform.position += MoveForce * Time.deltaTime;

        float steerInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * (steerInput * MoveForce.magnitude * _steerAngle * Time.deltaTime));

        MoveForce *= _drag;
        MoveForce = Vector3.ClampMagnitude(MoveForce, _maxSpeed);
    }
}
