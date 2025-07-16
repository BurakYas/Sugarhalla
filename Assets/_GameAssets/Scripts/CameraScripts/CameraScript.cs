using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public LayerMask environmentLayer;
    public float minDistance = 1f;
    public float maxDistance = 5f;
    public float smoothSpeed = 10f;
    private Vector3 dollyDir;
    private float currentDistance;

    void Start()
    {
        dollyDir = (transform.position - target.position).normalized;
        currentDistance = maxDistance;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + dollyDir * maxDistance;
        RaycastHit hit;

        if (Physics.Raycast(target.position, dollyDir, out hit, maxDistance, environmentLayer))
        {
            currentDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            currentDistance = maxDistance;
        }

        transform.position = Vector3.Lerp(transform.position, target.position + dollyDir * currentDistance, Time.deltaTime * smoothSpeed);
    }
}
