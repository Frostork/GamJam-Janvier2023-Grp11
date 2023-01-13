using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class BoxItem : MonoBehaviour
{
    public PlayerManager _playerManager;
    public LayerMask _playerMask;
    public int num;
    public bool CanRecover;
    
    [SerializeField] private int chanceItem1;
    [SerializeField] private int chanceItem2;
    [SerializeField] private int chanceItem3;

    private void Start()
    {
        CanRecover = true;
    }

    private void Update()
    {
        if (num > 0 && num <= chanceItem1)
        {
            _playerManager.CanTakeItem = false;
            _playerManager.CanShootBomb = true;
        }
        if (num > chanceItem1 && num <= chanceItem2)
        {
            _playerManager.CanTakeItem = false;
            _playerManager.CanActiveShield = true;
        }
        if (num > chanceItem2 && num <= chanceItem3)
        {
            _playerManager.CanTakeItem = false;
            _playerManager.CanActiveTurbo = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CanRecover && Contains(_playerMask, other.gameObject.layer))
        {
            Random rnd = new Random();
            num = rnd.Next(1, 100);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            CanRecover = false;
            StartCoroutine(Waiting());
        }
    }
    public static bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        CanRecover = true;
    }
}