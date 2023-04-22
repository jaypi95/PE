using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetObject;
    public Transform springObject;
    public float zoomDistanceThreshold;
    public float zoomSpeed;
    public float zoomFactor;

    private Vector3 _initialOffset;
    private Vector3 _cameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        zoomDistanceThreshold = 2.0f;
        zoomSpeed = 2.0f;
        zoomFactor = 0.5f;
        _initialOffset = transform.position - targetObject.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _cameraPosition = targetObject.position + _initialOffset;
        var transform1 = transform;
        transform1.position = _cameraPosition;

        // Check the distance between the camera and the Spring object
        var distanceToSpring = Vector3.Distance(transform1.position, springObject.position);

        if (!(distanceToSpring < zoomDistanceThreshold)) return;
        // Calculate a new camera position based on the distance to the Spring object
        var position = transform.position;
        Vector3 springDirection = (springObject.position - position).normalized;
        float zoomAmount = (zoomDistanceThreshold - distanceToSpring) * zoomFactor;
        Vector3 zoomPosition = position + springDirection * zoomAmount;

        // Smoothly move the camera to the new zoom position
        position = Vector3.Lerp(position, zoomPosition, Time.deltaTime * zoomSpeed);
        transform.position = position;
    }
}