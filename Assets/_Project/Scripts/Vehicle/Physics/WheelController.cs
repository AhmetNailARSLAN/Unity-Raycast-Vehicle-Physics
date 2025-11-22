using UnityEngine;

/// <summary>
/// Handles the physics calculations for a single wheel using Raycasting.
/// Applies Hooke's Law and Damping forces to the main vehicle body.
/// </summary>
public class WheelController : MonoBehaviour
{
    private Rigidbody _carRb;
    private CarStats _stats;

    public bool IsGrounded { get; private set; }

    /// <summary>
    /// Initializes the wheel with necessary dependencies.
    /// </summary>
    public void Initialize(Rigidbody carRb, CarStats stats)
    {
        _carRb = carRb;
        _stats = stats;
    }

    /// <summary>
    /// Performs the raycast and applies suspension forces.
    /// Should be called in FixedUpdate.
    /// </summary>
    public void HandlePhysics()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = -transform.up;

        // Calculate the total length needed for the ray (Suspension Rest Length + Wheel Radius)
        float maxRayLength = _stats.RestLength + _stats.WheelRadius;

        RaycastHit hit;

        // Visualize the ray in the Scene view for debugging purposes
        Debug.DrawRay(rayOrigin, rayDirection * maxRayLength, Color.red);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxRayLength))
        {
            IsGrounded = true;

            // --- 1. CALCULATE SPRING FORCE (Hooke's Law: F = -k * x) ---

            // Determine the current length of the spring based on raycast hit distance
            float currentSpringLength = hit.distance - _stats.WheelRadius;

            // Calculate how much the spring is compressed (0 to 1 normalized value can be useful, but here we need distance)
            // Compression = RestLength - CurrentLength
            float springCompression = (_stats.RestLength - currentSpringLength) / _stats.RestLength;

            // Apply stiffness
            float springForce = springCompression * _stats.SpringStiffness;

            // --- 2. CALCULATE DAMPER FORCE (Shock Absorption) ---

            // Get the velocity of the vehicle at the wheel's position
            // We project this velocity onto the local up vector of the wheel to get vertical speed
            float wheelVelocity = Vector3.Dot(_carRb.GetPointVelocity(transform.position), transform.up);

            // Damping force opposes the velocity
            float damperForce = wheelVelocity * _stats.DamperStiffness;

            // --- 3. APPLY TOTAL FORCE ---

            float totalUpwardForce = springForce - damperForce;

            // Apply the calculated force at the specific position of the wheel
            // Using transform.up ensures the force is applied relative to the car's rotation (normal to the chassis)
            _carRb.AddForceAtPosition(transform.up * totalUpwardForce, transform.position);
        }
        else
        {
            IsGrounded = false;
        }
    }
}