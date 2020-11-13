using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float orbitXSpeed = 5f;
    [SerializeField] float orbitYSpeed = 10f;
    [SerializeField] float minXAngle = -50f;
    [SerializeField] float maxXAngle = 30f;
    [SerializeField] float rayThickness = 0.5f;
    [SerializeField, Range(0f, 1f)] float positionSmoothing = 0.5f;
    [SerializeField, Range(0f, 1f)] float rotationSmoothing = 0.5f;

    float xAngle;
    float yAngle;
    float currentDistance;
    Quaternion smoothedRotation;
    Quaternion lastRotation;

    void Start()
    {
        transform.position = target.position + offset;
        lastRotation = Quaternion.identity;
    }

    void Update()
    {
        xAngle -= Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * orbitXSpeed * 10 * Time.deltaTime;
        yAngle += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * orbitYSpeed * 10 * Time.deltaTime;
    }

    void FixedUpdate()
    {
        xAngle = Mathf.Clamp(xAngle, minXAngle, maxXAngle);
        Quaternion orbitRotation = Quaternion.Euler(xAngle, yAngle, 0);
        smoothedRotation = Quaternion.Lerp(lastRotation, orbitRotation, 1 - rotationSmoothing);
        lastRotation = smoothedRotation;
    }

    void LateUpdate()
    {
        transform.LookAt(target);
        Vector3 rotatedOffset = smoothedRotation * offset;
        Vector3 desiredPosition = target.position + rotatedOffset;
        Vector3 result = CameraCollision(desiredPosition, rotatedOffset); ;
        float resultDistance = Vector3.Distance(target.position, result);
        currentDistance = Mathf.Lerp(currentDistance, resultDistance, positionSmoothing);
        transform.position = target.position + rotatedOffset.normalized * currentDistance;
        //CameraCollision(desiredPosition, rotatedOffset);
        transform.LookAt(target);
    }

    Vector3 CameraCollision(Vector3 desiredPosition, Vector3 direction)
    {
        float distance = Vector3.Distance(target.position, desiredPosition);
        RaycastHit hit;
        if (Physics.SphereCast(new Ray(target.position, direction), rayThickness, out hit, distance))
        {
            Debug.DrawRay(target.position, direction, Color.blue);
            return hit.point + hit.normal * rayThickness;
        }
        return desiredPosition;
    }
}
