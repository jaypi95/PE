using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private Transform springObject;
    [SerializeField] private float zoomDistanceThreshold = 2.0f;
    [SerializeField] private float zoomSpeed = 2.0f;
    [SerializeField] private float zoomFactor = 0.5f;

    private Vector3 _initialOffset;

    private void Start()
    {
        _initialOffset = transform.position - targetObject.position;
    }

    private void FixedUpdate()
    {
        UpdateCameraPosition();
        ZoomIfNeeded();
    }

    private void UpdateCameraPosition()
    {
        var newPosition = targetObject.position + _initialOffset;
        transform.position = newPosition;
    }

    private void ZoomIfNeeded()
    {
        var distanceToSpring = Vector3.Distance(transform.position, springObject.position);

        if (distanceToSpring >= zoomDistanceThreshold) return;
        UpdateZoomPosition(distanceToSpring);
    }

    private void UpdateZoomPosition(float distanceToSpring)
    {
        var position = transform.position;
        Vector3 springDirection = (springObject.position - position).normalized;
        float zoomAmount = (zoomDistanceThreshold - distanceToSpring) * zoomFactor;
        Vector3 zoomPosition = position + springDirection * zoomAmount;

        position = Vector3.Lerp(position, zoomPosition, Time.deltaTime * zoomSpeed);
        transform.position = position;
    }
}