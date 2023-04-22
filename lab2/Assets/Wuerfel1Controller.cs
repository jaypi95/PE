using System;
using System.Collections.Generic;
using System.Globalization;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Wuerfel1Controller : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Rigidbody _rigidbodyCube2;

    public Rigidbody flame;

    // make new variable for TextMeshPro component
    [FormerlySerializedAs("text")] public TextMeshProUGUI currentSpeed;
    private float _targetSpeed;
    private float _constantForce;
    private float _acceleration;
    private float _accelerationTime;
    private bool _reachedTargetSpeed;
    private float _springConstant;
    private Statistics _statisticsVelocityX { get; set; }
    private Statistics _statisticsImpulseX { get; set; }
    private Statistics _statisticsImpulseBothCubesX { get; set; }
    private Statistics _statisticsPositionX { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _targetSpeed = 2.0f;
        _accelerationTime = 3f; // 0 < _accelerationTime < 5 seconds
        _acceleration = _targetSpeed / _accelerationTime;
        _constantForce = _rigidbody.mass * _acceleration;
        _reachedTargetSpeed = false;
        _springConstant = 0.5f;
        _statisticsVelocityX = new(_rigidbody.name, "velocity", "v", "x");
        _statisticsImpulseX = new(_rigidbody.name, "impulse", "F", "x");
        _statisticsPositionX = new(_rigidbody.name, "position", "r", "x");
        _statisticsImpulseBothCubesX = new("Impulse both cubes", "impulse", "F", "x");
    }

    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        if (_rigidbody.position.y < 0)
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
        _statisticsVelocityX.AddTimeStep(Time.time, _rigidbody.velocity.x);
        _statisticsImpulseX.AddTimeStep(Time.time, _rigidbody.mass * _rigidbody.velocity.x);
        _statisticsPositionX.AddTimeStep(Time.time, _rigidbody.position.x);
        _statisticsImpulseBothCubesX.AddTimeStep(Time.time, _rigidbody.mass * _rigidbody.velocity.x + _rigidbodyCube2.mass * _rigidbodyCube2.velocity.x);

        currentSpeed.SetText($"Velocity on X-axis of Wuerfel 1:\n{_rigidbody.velocity.x.ToString()}");
        if (_rigidbody.velocity.x < _targetSpeed && !_reachedTargetSpeed)
        {
            _rigidbody.AddForce(_constantForce, 0, 0);
            simulateFlame();
            return;
        }

        _reachedTargetSpeed = true;

        simulateFlame(false);
    }

    void simulateFlame(bool isGrowing = true)
    {
        if (_rigidbody.velocity.x < 0) return;

        if (isGrowing)
        {
            if (flame.transform.localScale.x < 0.25f)
                flame.transform.localScale += new Vector3(0.005f, 0.005f, 0.005f);
        }
        else
        {
            if (_rigidbody.velocity.x > 0 && flame.transform.localScale.x > 0f)
            {
                flame.transform.localScale -= new Vector3(0.005f, 0.005f, 0.005f);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spring"))
        {
            _rigidbody.velocity = -_rigidbody.velocity * _springConstant;
        }
    }
    
    void OnApplicationQuit()
    {
        _statisticsVelocityX.WriteTimeSeriesToCsv();
        _statisticsImpulseX.WriteTimeSeriesToCsv();
        _statisticsPositionX.WriteTimeSeriesToCsv();
        _statisticsImpulseBothCubesX.WriteTimeSeriesToCsv();
    }
}