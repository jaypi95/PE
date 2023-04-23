using DefaultNamespace;
using UnityEngine;

public class Wuerfel2Controller : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Statistics _statisticsVelocityX;
    private Statistics _statisticsImpulseX;
    private Statistics _statisticsPositionX;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        InitializeStatistics();
    }

    private void InitializeStatistics()
    {
        _statisticsVelocityX = new Statistics(_rigidbody.name, "velocity", "v", "x");
        _statisticsImpulseX = new Statistics(_rigidbody.name, "impulse", "F", "x");
        _statisticsPositionX = new Statistics(_rigidbody.name, "position", "r", "x");
    }

    private void FixedUpdate()
    {
        UpdateStatistics();
    }

    private void UpdateStatistics()
    {
        _statisticsVelocityX.AddTimeStep(Time.time, _rigidbody.velocity.x);
        _statisticsImpulseX.AddTimeStep(Time.time, _rigidbody.mass * _rigidbody.velocity.x);
        _statisticsPositionX.AddTimeStep(Time.time, _rigidbody.position.x);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Wuerfel1")) return;
        AttachToCollidingObject(collision);
    }

    private void AttachToCollidingObject(Collision collision)
    {
        var fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
    }

    private void OnApplicationQuit()
    {
        WriteStatisticsToCsv();
    }

    private void WriteStatisticsToCsv()
    {
        _statisticsVelocityX.WriteTimeSeriesToCsv();
        _statisticsImpulseX.WriteTimeSeriesToCsv();
        _statisticsPositionX.WriteTimeSeriesToCsv();
    }
}