using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    private Vector3 _lastVelocity;

    private float _speed;
    private Vector3 _direction;

    [SerializeField] private float _timeAlived;
    [SerializeField] private GameObject _damageZone;
    [SerializeField] private LayerMask _playerMask;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(Explosed());
    }

    private void Update()
    {
        _lastVelocity = _rigidbody.velocity;

        _rigidbody.velocity -= _rigidbody.velocity / 1500;
    }

    private void OnCollisionEnter(Collision col)
    {
        _speed = _lastVelocity.magnitude;
        _direction = Vector3.Reflect(_lastVelocity.normalized, col.contacts[0].normal);

        _rigidbody.velocity = _direction * Mathf.Max(_speed, 0f);

        if (Contains(_playerMask, col.gameObject.layer))
        {
            print("aïe");
                col.gameObject.transform.parent.GetComponent<PlayerController>().TakeDamage();
            if (col.gameObject.transform.parent.GetComponent<PlayerController>().CanTakeDamage)
            { 
            }
        }
    }
    public static bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    IEnumerator Explosed()
    {
        //yield return new WaitForSeconds(_timeAlived);
        _damageZone.SetActive(true);
        yield return new WaitForSeconds(2);
        //Destroy(gameObject);
    }
}
