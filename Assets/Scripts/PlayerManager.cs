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

    [SerializeField] private bool _canMove;

    [Header("GPE")] 
    [SerializeField] float _speedAccelerate;
    public bool HaveSpeedBoost;
    
    [Header("\nItems\n")]
    [Header("Bomb")] [SerializeField] private GameObject _bomb;
    [SerializeField] private GameObject _bombOrigin;
    [SerializeField] private bool _canShootBomb;

    [Header("Shield")]
    public GameObject ShieldPrefab;
    [SerializeField] private bool _canActiveShield;
    public bool HaveShield;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _canMove = true;
        CanTakeDamage = true;
        CanTakeItem = true;
    }

    private void Update()
    {
        _bombOrigin.transform.rotation = transform.localRotation;
        
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
    }
    
    #region Take Damage

    public void TakeDamage()
    {
        SphereRigidbody.velocity = Vector3.zero;

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
}
