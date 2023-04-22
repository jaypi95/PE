using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wuerfel2Controller : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Statistics _statisticsVelocityX { get; set; }
    private Statistics _statisticsImpulseX { get; set; }
    private Statistics _statisticsPositionX { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _statisticsVelocityX = new(_rigidbody.name, "velocity", "v", "x");
        _statisticsImpulseX = new(_rigidbody.name, "impulse", "F", "x");
        _statisticsPositionX = new(_rigidbody.name, "position", "r", "x");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        _statisticsVelocityX.AddTimeStep(Time.time, _rigidbody.velocity.x);
        _statisticsImpulseX.AddTimeStep(Time.time, _rigidbody.mass * _rigidbody.velocity.x);
        _statisticsPositionX.AddTimeStep(Time.time, _rigidbody.position.x);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Wuerfel1")) return;
        var fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
    }

    void OnApplicationQuit()
    {
        _statisticsVelocityX.WriteTimeSeriesToCsv();
        _statisticsImpulseX.WriteTimeSeriesToCsv();
        _statisticsPositionX.WriteTimeSeriesToCsv();
    }
}