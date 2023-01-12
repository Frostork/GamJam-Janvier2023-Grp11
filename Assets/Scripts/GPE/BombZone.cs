using System;
using Unity.VisualScripting;
using UnityEngine;

public class BombZone : MonoBehaviour
{
    public LayerMask PlayerLayer;
    
    private void OnTriggerEnter(Collider other)
    {
        if (  Contains(PlayerLayer, other.gameObject.layer))
        {
            if (other.gameObject.transform.parent.GetComponent<PlayerController>().CanTakeDamage)
            { 
                other.gameObject.transform.parent.GetComponent<PlayerController>().TakeDamage();
            }
        }
    }

    public static bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}