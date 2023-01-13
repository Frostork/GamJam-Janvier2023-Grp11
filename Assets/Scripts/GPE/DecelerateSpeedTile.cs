using UnityEngine;

public class DecelerateSpeedTile : MonoBehaviour
{
    public LayerMask PlayerLayer;
    
    private void OnTriggerEnter(Collider other)
    {
        if (Contains(PlayerLayer, other.gameObject.layer))
        {
            if (!other.gameObject.GetComponent<PlayerManager>().HaveSpeedLack)
            {
                print("lack of speed !");
                other.gameObject.GetComponent<PlayerManager>().HaveSpeedLack = true;
            }
        }
    }

    public static bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
