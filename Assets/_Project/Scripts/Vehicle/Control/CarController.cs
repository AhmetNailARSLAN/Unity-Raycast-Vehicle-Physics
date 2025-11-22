using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Main controller class that orchestrates the vehicle's components.
/// Manages input distribution and physics updates for all wheels.
/// </summary>
public class CarController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private CarStats carStats;
    [SerializeField] private List<Transform> wheelTransforms;

    private Rigidbody _rb;
    private List<WheelController> _wheels = new List<WheelController>();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        ValidateDependencies();
        InitializeWheels();
    }

    private void ValidateDependencies()
    {
        if (carStats == null)
        {
            Debug.LogError("CarStats is missing! Please assign a scriptable object.");
        }

        if (wheelTransforms == null || wheelTransforms.Count == 0)
        {
            Debug.LogError("No wheel transforms assigned!");
        }
    }

    private void InitializeWheels()
    {
        // Iterate through provided transforms and attach physics logic
        foreach (var t in wheelTransforms)
        {
            // Adding component at runtime (Composition)
            var wheel = t.gameObject.AddComponent<WheelController>();
            wheel.Initialize(_rb, carStats);
            _wheels.Add(wheel);
        }
    }

    private void FixedUpdate()
    {
        foreach (var wheel in _wheels)
        {
            wheel.HandlePhysics();
        }
    }
}