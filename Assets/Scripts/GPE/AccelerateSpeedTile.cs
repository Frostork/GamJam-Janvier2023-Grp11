using UnityEngine;

public class AccelerateSpeedTile : MonoBehaviour
{
    public LayerMask PlayerLayer;
    
    private void OnTriggerEnter(Collider other)
    {
        if (Contains(PlayerLayer, other.gameObject.layer))
        {
            if (!other.gameObject.GetComponent<PlayerManager>().HaveSpeedBoost)
            {
                print("boost !");
                other.gameObject.GetComponent<PlayerManager>().HaveSpeedBoost = true;
            }
        }
    }

    public static bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
