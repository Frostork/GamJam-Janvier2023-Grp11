using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Rigidbody SphereRigidbody;
    private Animator _animator;
    private AudioSource _audioSource;
    public int Life;
    public bool CanTakeDamage;
    public bool CanTakeItem;

    [Header("GPE")] 
    [SerializeField] float _speedAccelerate;
    public bool HaveSpeedBoost;

    [SerializeField] private float _speedDecelerate;
    public bool HaveSpeedLack;
    
    [Header("\nItems\n")]
    [Header("Bomb")] [SerializeField] private GameObject _bomb;
    [SerializeField] private GameObject _bombOrigin;
    [SerializeField] private bool _canShootBomb;

    [Header("Shield")]
    public GameObject ShieldPrefab;
    [SerializeField] private bool _canActiveShield;
    public bool HaveShield;

    [Header("Turbo")] 
    public GameObject TurboPrefab;
    [SerializeField] private GameObject FireTurbo;
    [SerializeField] private float _acceleratePower;
    [SerializeField] private bool _canActiveTurbo;
    [SerializeField] private bool _isUsingTurbo;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        CanTakeDamage = true;
        CanTakeItem = true;
    }

    private void Update()
    {
        _bombOrigin.transform.rotation = transform.localRotation;

        if (_canActiveTurbo)
        {
            TurboPrefab.SetActive(true);
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            ActiveItem();
        }

        // if (moveInput > 0)
        // {
        //     _audioSource.pitch += 0.01f;
        //     if (_audioSource.pitch > 2.3f)
        //     {
        //         _audioSource.pitch = 2.3f;
        //     }
        // }
    }

    private void FixedUpdate()
    {
        if (HaveSpeedBoost)
        {
            HaveSpeedBoost = false;
            SphereRigidbody.AddForce(transform.forward * _speedAccelerate, ForceMode.Impulse);
        }

        if (HaveSpeedLack)
        {
            HaveSpeedLack = false;
            SphereRigidbody.AddForce(-transform.forward * _speedDecelerate, ForceMode.Impulse);
        }
    }

    private void ActiveItem()
    {
        if (_canShootBomb)
        {
            _canShootBomb = false;
            var actualBomb = Instantiate(_bomb, _bombOrigin.transform.position, _bombOrigin.transform.localRotation);
            actualBomb.GetComponent<Rigidbody>().AddForce(_bombOrigin.transform.parent.forward * 5000);
        }
        if (_canActiveShield)
        {
            _canActiveShield = false;
            HaveShield = true;
            ShieldPrefab.SetActive(true);
        }

        if (_canActiveTurbo)
        {
            _canActiveTurbo = false;
            FireTurbo.SetActive(true);
            _isUsingTurbo = true;
            SphereRigidbody.AddForce(transform.forward * (_speedAccelerate * _acceleratePower), ForceMode.Impulse);
            StartCoroutine(TurboCoolDown());
        }
    }

    IEnumerator TurboCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        _isUsingTurbo = false;
        TurboPrefab.SetActive(false);
        FireTurbo.SetActive(false);
    }
    #region Take Damage

    public void TakeDamage()
    {
        SphereRigidbody.velocity = Vector3.zero;

        _animator.SetBool("IsHit", true);
        Life -= 1;
        CanTakeDamage = false;
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(2);
        CanTakeDamage = true;
        _animator.SetBool("IsHit", false);
    }

    #endregion
}
