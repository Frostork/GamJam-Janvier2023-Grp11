using UnityEngine;

public class DetectorGround : MonoBehaviour
{
    public LayerMask GroudMask;

    public GameObject player;
    private void OnTriggerStay(Collider other)
    {
        if (Contains(GroudMask, other.gameObject.layer))
        {
            print("ground !");
            player.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
    
    public static bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
