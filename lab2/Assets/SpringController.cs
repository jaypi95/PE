using UnityEngine;

public class SpringController : MonoBehaviour
{
    public float springConstant;
    public float initialLength;
    public float compressionSpeed;
    private Rigidbody collidingRigidbody;
    private Rigidbody _springRigidbody;

    private bool _isCompressed;
    private bool _isCompressing;

    private void Start()
    {
        initialLength = 0.1f;
        springConstant = 1000f;
        compressionSpeed = 0.1f;
        _springRigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.attachedRigidbody) return;
        collidingRigidbody = collider.attachedRigidbody;
        _isCompressed = true;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.attachedRigidbody)
        {
            _isCompressed = false;
        }
    }

    private void FixedUpdate()
    {
        if (_isCompressed)
        {
            Vector3 velocity = collidingRigidbody.velocity;
            collidingRigidbody.velocity = new Vector3(velocity.x - compressionSpeed, velocity.y, velocity.z);

            float compressionLength = initialLength - Mathf.Abs(transform.position.x - collidingRigidbody.position.x);

            if (compressionLength >= 0)
            {
                bool isCompressing = compressionLength > 0.01f;

                if (isCompressing)
                {
                    float force = springConstant * compressionLength;
                    _springRigidbody.AddForce(force, 0, 0);
                }
            }
        }
    }

}