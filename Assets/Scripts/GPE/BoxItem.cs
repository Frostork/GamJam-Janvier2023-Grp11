using UnityEngine;

public class BoxItem : MonoBehaviour
{
    public LayerMask _playerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (Contains(_playerMask, other.gameObject.layer))
        {

        }
    }

    public static bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
