using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    private Vector3 _lastVelocity;

    private float _speed;
    private Vector3 _direction;

    [SerializeField] private float _timeAlived;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(Explosed());
    }

    private void Update()
    {
        _lastVelocity = _rigidbody.velocity;

    }

    private void OnCollisionEnter(Collision col)
    {
        _speed = _lastVelocity.magnitude;
        _direction = Vector3.Reflect(_lastVelocity.normalized, col.contacts[0].normal);

        _rigidbody.velocity = _direction * Mathf.Max(_speed, 0f);
    }

    IEnumerator Explosed()
    {
        yield return new WaitForSeconds(_timeAlived);
        Destroy(gameObject);
    }
}
