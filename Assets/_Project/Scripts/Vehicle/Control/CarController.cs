using UnityEngine;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    [SerializeField] private CarStats carStats;
    [SerializeField] private List<WheelConfig> wheels;

    private Rigidbody _rb;
    private float _currentInputY; // Acceleration input
    private float _currentInputX; // Steering input

    [System.Serializable]
    public struct WheelConfig
    {
        public Transform wheelTransform;
        public bool isFrontWheel;
        public bool isDriveWheel;
    }

    private List<WheelLogicWrapper> _wheelWrappers = new List<WheelLogicWrapper>();

    private class WheelLogicWrapper
    {
        public WheelController Controller;
        public WheelConfig Config;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        foreach (var w in wheels)
        {
            var controller = w.wheelTransform.gameObject.AddComponent<WheelController>();
            controller.Initialize(_rb, carStats);

            _wheelWrappers.Add(new WheelLogicWrapper { Controller = controller, Config = w });
        }
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        ApplyInputToWheels();

        foreach (var wrapper in _wheelWrappers)
        {
            wrapper.Controller.HandlePhysics();
        }
    }

    private void GetInput()
    {
        _currentInputY = Input.GetAxis("Vertical"); // W/S or Up/Down
        _currentInputX = Input.GetAxis("Horizontal"); // A/D or Left/Right
    }

    private void ApplyInputToWheels()
    {
        foreach (var wrapper in _wheelWrappers)
        {
            // 1. STEERING
            if (wrapper.Config.isFrontWheel)
            {
                float steerAngle = _currentInputX * carStats.MaxSteerAngle;
                wrapper.Controller.Steer(steerAngle);
            }

            // 2. ACCELERATION
            if (wrapper.Config.isDriveWheel)
            {
                float torque = _currentInputY * carStats.MotorTorque;
                wrapper.Controller.Accelerate(torque);
            }
        }
    }
}