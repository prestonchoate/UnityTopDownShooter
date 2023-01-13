using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    private GameObject target;
    [SerializeField]
    private Vector3 offset;

    private float damping = 0.5f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        if (target is null)
        {
            Debug.Log("Failed to find player object. Camera follow will not work!");
        }
    }
    void Update()
    {
        Vector3 dest = target.transform.position + offset;
        dest.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, dest, ref velocity, damping);


    }
}
