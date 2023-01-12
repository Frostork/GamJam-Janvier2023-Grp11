using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Init

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

    #endregion

    #region Init Movement Variable

    public ControlMode control;

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    float moveInput;
    float steerInput;

    #endregion

    #region Init Variable

    private Rigidbody _carRb;
    private Animator _animator;

    public int Life;
    public bool CanTakeDamage;
    public bool CanTakeItem;

    [SerializeField] private bool _canMove;

    [Header("Bomb")] [SerializeField] private GameObject _bomb;
    [SerializeField] private GameObject _bombOrigin;
    [SerializeField] private bool _canShootBomb;

    [Header("Shield")]
    public GameObject ShieldPrefab;
    [SerializeField] private bool _canActiveShield;
    public bool HaveShield;

    [Header("Wheels")] public List<Wheel> wheels;

    #endregion

    #region Take Damage

    public void TakeDamage()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.brakeTorque = 10000 * brakeAcceleration * Time.deltaTime;
            _carRb.velocity = Vector3.zero;
        }

        _animator.SetBool("IsHit", true);
        Life -= 1;
        CanTakeDamage = false;
        _canMove = false;
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(2);
        _canMove = true;
        CanTakeDamage = true;
        _animator.SetBool("IsHit", false);
    }

    #endregion

    void Start()
    {
        _animator = GetComponent<Animator>();
        _carRb = GetComponent<Rigidbody>();
        _carRb.centerOfMass = _centerOfMass;
        _canMove = true;
        CanTakeDamage = true;
        CanTakeItem = true;
    }

    private void ActiveItem()
    {
        if (_canShootBomb)
        {
            _canShootBomb = false;
            var actualBomb = Instantiate(_bomb, _bombOrigin.transform.position, _bombOrigin.transform.localRotation);
            actualBomb.GetComponent<Rigidbody>().AddForce(_bombOrigin.transform.forward * 5000);

            if (_canActiveShield)
            {
                _canActiveShield = false;
                ShieldPrefab.SetActive(true);
            }
        }
    }

    void Update()
    {
        GetInputs();
        AnimateWheels();
        _bombOrigin.transform.rotation = transform.localRotation;

        if (Input.GetKeyDown(KeyCode.T))
        {
            ActiveItem();
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
        if (_canMove && control == ControlMode.Keyboard)
        {
            moveInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }
    }

    void Move()
    {
        if (_canMove)
        {
            _animator.SetBool("IsDrive", true);
            _animator.SetBool("IsIdle", false);

            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = moveInput * 1000 * maxAcceleration * Time.deltaTime;
            }
        }
        else
        {
            _animator.SetBool("IsDrive", false);
            _animator.SetBool("IsIdle", true);

            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = 0;
            }
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
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
        foreach (var wheel in wheels)
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