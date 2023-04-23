using DefaultNamespace;
using TMPro;
using UnityEngine;

public class Wuerfel1Controller : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbodyCube2;
    [SerializeField] private Rigidbody flame;
    [SerializeField] private TextMeshProUGUI currentSpeed;

    private Rigidbody _rigidbody;
    private float _targetSpeed = 2.0f;
    private float _accelerationTime = 3f;
    private float _acceleration;
    private float _constantForce;
    private bool _reachedTargetSpeed;
    private float _springConstant = 0.5f;
    private Statistics _statisticsVelocityX;
    private Statistics _statisticsImpulseX;
    private Statistics _statisticsImpulseBothCubesX;
    private Statistics _statisticsPositionX;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        InitializeStatistics();
        InitializePhysics();
    }

    private void InitializeStatistics()
    {
        _statisticsVelocityX = new Statistics(_rigidbody.name, "velocity", "v", "x");
        _statisticsImpulseX = new Statistics(_rigidbody.name, "impulse", "F", "x");
        _statisticsPositionX = new Statistics(_rigidbody.name, "position", "r", "x");
        _statisticsImpulseBothCubesX = new Statistics("Impulse both cubes", "impulse", "F", "x");
    }

    private void InitializePhysics()
    {
        _acceleration = _targetSpeed / _accelerationTime;
        _constantForce = _rigidbody.mass * _acceleration;
        _reachedTargetSpeed = false;
    }

    private void FixedUpdate()
    {
        UpdateStatistics();
        HandleVelocityAndFlame();
        QuitIfOutOfBounds();
    }

    private void UpdateStatistics()
    {
        _statisticsVelocityX.AddTimeStep(Time.time, _rigidbody.velocity.x);
        _statisticsImpulseX.AddTimeStep(Time.time, _rigidbody.mass * _rigidbody.velocity.x);
        _statisticsPositionX.AddTimeStep(Time.time, _rigidbody.position.x);
        _statisticsImpulseBothCubesX.AddTimeStep(Time.time,
            _rigidbody.mass * _rigidbody.velocity.x + _rigidbodyCube2.mass * _rigidbodyCube2.velocity.x);
        currentSpeed.SetText($"Velocity on X-axis of Wuerfel 1:\n{_rigidbody.velocity.x}");
    }

    private void HandleVelocityAndFlame()
    {
        if (_rigidbody.velocity.x < _targetSpeed && !_reachedTargetSpeed)
        {
            _rigidbody.AddForce(_constantForce, 0, 0);
            SimulateFlame();
            return;
        }

        _reachedTargetSpeed = true;
        SimulateFlame(false);
    }

    private void SimulateFlame(bool isGrowing = true)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spring"))
        {
            BounceOffSpring();
        }
    }

    private void BounceOffSpring()
    {
        _rigidbody.velocity = -_rigidbody.velocity * _springConstant;
    }

    private void QuitIfOutOfBounds()
    {
        if (_rigidbody.position.y < 0)
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    private void OnApplicationQuit()
    {
        _statisticsVelocityX.WriteTimeSeriesToCsv();
        _statisticsImpulseX.WriteTimeSeriesToCsv();
        _statisticsPositionX.WriteTimeSeriesToCsv();
        _statisticsImpulseBothCubesX.WriteTimeSeriesToCsv();
    }
}