using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    public CarController _CarController;
    private Animator _animator;
    public int Life;
    public bool CanTakeDamage;
    public bool CanTakeItem;

    [Header("GPE")]
    [SerializeField] private float _timeToResetSpeed;
    [SerializeField] float _speedAccelerate;
    public bool HaveSpeedBoost;

    [SerializeField] private float _speedDecelerate;
    public bool HaveSpeedLack;
    
    [Header("\nItems\n")]
    [Header("Bomb")] 
    [SerializeField] private GameObject _bomb;
    [SerializeField] private GameObject _bombOrigin;
    public bool CanShootBomb;

    [Header("Shield")]
    public GameObject ShieldPrefab;
    public bool CanActiveShield;
    public bool HaveShield;

    [Header("Turbo")] 
    public GameObject TurboPrefab;
    [SerializeField] private GameObject FireTurbo;
    [SerializeField] private float _acceleratePower;
    public bool CanActiveTurbo;
    [SerializeField] private bool _isUsingTurbo;

    [Header("Audio")] 
    private AudioSource _audioSource;
    public List<AudioClip> AudioClips;
    
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

        if (CanActiveTurbo)
        {
            TurboPrefab.SetActive(true);
        }

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    ActiveItem();
        //}

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
            _CarController.boostBonus = _speedAccelerate;
            StartCoroutine(ResetSpeed());
        }

        if (HaveSpeedLack)
        {
            HaveSpeedLack = false;
            _CarController.boostBonus = _speedDecelerate;
            StartCoroutine(ResetSpeed());
        }
    }

    IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(_timeToResetSpeed);
        _CarController.boostBonus = 1;
    }

    private void ActiveItem()
    {
        if (CanShootBomb)
        {
            CanShootBomb = false;
            var actualBomb = Instantiate(_bomb, _bombOrigin.transform.position, _bombOrigin.transform.localRotation);
            actualBomb.GetComponent<Rigidbody>().AddForce(_bombOrigin.transform.parent.forward * 5000);
        }
        if (CanActiveShield)
        {
            CanActiveShield = false;
            HaveShield = true;
            ShieldPrefab.SetActive(true);
        }

        if (CanActiveTurbo)
        {
            CanActiveTurbo = false;
            FireTurbo.SetActive(true);
            _isUsingTurbo = true;
            _CarController.boostBonus = _speedAccelerate;
            StartCoroutine(TurboCoolDown());
        }
    }

    IEnumerator TurboCoolDown()
    {
        yield return new WaitForSeconds(3);
        _CarController.boostBonus = 1;
        _isUsingTurbo = false;
        TurboPrefab.SetActive(false);
        FireTurbo.SetActive(false);
    }
    #region Take Damage

    public void TakeDamage()
    {
        _audioSource.clip = AudioClips[0];
        _audioSource.Play();
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
