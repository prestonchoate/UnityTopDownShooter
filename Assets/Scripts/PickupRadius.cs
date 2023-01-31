using UnityEngine;

public class PickupRadius : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Pickups"))
        {
            other.transform.position = Vector3.Lerp(other.transform.position, transform.position, 0.25f);
        }
    }
}
