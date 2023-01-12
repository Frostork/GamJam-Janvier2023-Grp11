using UnityEngine;

public class SpeedTile : MonoBehaviour
{
    public LayerMask PlayerLayer;
    
    private void OnTriggerEnter(Collider other)
    {
        if (  Contains(PlayerLayer, other.gameObject.layer))
        {
            if (!other.gameObject.GetComponentInParent<PlayerController>().HaveSpeedBoost)
            {
                print("boost !");
                other.gameObject.GetComponentInParent<PlayerController>().HaveSpeedBoost = true;
            }
        }
    }

    public static bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
