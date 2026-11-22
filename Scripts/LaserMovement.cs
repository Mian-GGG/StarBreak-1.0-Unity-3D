using UnityEngine;

public class LaserMovement : MonoBehaviour
{
    private Vector3 startPos;
    private float maxDistance;
    void Start()
    {
        startPos = transform.position;
        maxDistance = 2f;
    }

    void Update()
    {
        transform.position += transform.forward * 2f;
        if (Vector3.Distance(transform.position, startPos) > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
