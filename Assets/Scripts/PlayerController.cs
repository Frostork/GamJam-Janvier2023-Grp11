using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public enum ControlMode
    {
        Keyboard,
        Buttons
    };

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
        public Axel axel;
    }

    public ControlMode control;

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRb;

    [SerializeField] private GameObject _bombOrigin;
    [SerializeField] private bool canMove;
    public int Life;
    public bool CanTakeDamage;
    [SerializeField] private GameObject bomb;
    [SerializeField] private bool _canShootBomb;

    private Animator _animator;
    public void TakeDamage()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.brakeTorque = 10000 * brakeAcceleration * Time.deltaTime;
            carRb.velocity = Vector3.zero;
        }
        
        _animator.SetBool("IsHit", true);
        Life -= 1;
        CanTakeDamage = false;
        canMove = false;
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(2);
        canMove = true;
        CanTakeDamage = true;
        _animator.SetBool("IsHit", false);
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
        canMove = true;
        CanTakeDamage = true;
    }

    void Update()
    {
        GetInputs();
        AnimateWheels();
        _bombOrigin.transform.rotation = transform.localRotation;
        
        _canShootBomb = true;
        if (_canShootBomb && Input.GetKeyDown(KeyCode.T))
        {
            if (_canShootBomb)
            {
                _canShootBomb = false;
                var actualBomb = Instantiate(bomb, _bombOrigin.transform.position, _bombOrigin.transform.localRotation);
                actualBomb.GetComponent<Rigidbody>().AddForce(_bombOrigin.transform.forward * 5000);
            }
        }
        
        //WheelEffects();
    }

    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    #region Movement Functions
    public void MoveInput(float input)
    {
        moveInput = input;
    }

    public void SteerInput(float input)
    {
        steerInput = input;
    }

    void GetInputs()
    {
        if(canMove && control == ControlMode.Keyboard)
        {
            moveInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }
    }

    void Move()
    {
        if (canMove)
        {
            _animator.SetBool("IsDrive", true);     
            _animator.SetBool("IsIdle", false);
            
            foreach(var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = moveInput * 1000 * maxAcceleration * Time.deltaTime;
            }
        }
        else
        {
            _animator.SetBool("IsDrive", false);
            _animator.SetBool("IsIdle", true);
            
            foreach(var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = 0;
            }
        }
    }

    void Steer()
    {
        foreach(var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space) || moveInput == 0)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }
    #endregion
    void AnimateWheels()
    {
        foreach(var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.rotation = rot;
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    // void WheelEffects()
    // {
    //     foreach (var wheel in wheels)
    //     {
    //         //var dirtParticleMainSettings = wheel.smokeParticle.main;
    //
    //         if (Input.GetKey(KeyCode.Space) && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded == true && carRb.velocity.magnitude >= 10.0f)
    //         {
    //             wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
    //             wheel.smokeParticle.Emit(1);
    //         }
    //         else
    //         {
    //             wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
    //         }
    //     }
    // }
    
}