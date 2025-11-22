using UnityEngine;

[CreateAssetMenu(fileName = "NewCarStats", menuName = "Vehicle/Car Stats")]
public class CarStats : ScriptableObject
{
    [Header("Suspension Physics")]
    [Tooltip("The length of the suspension spring when it is not compressed (meters).")]
    public float RestLength = 0.8f;

    [Tooltip("The maximum distance the suspension can compress or extend.")]
    public float SpringTravel = 0.3f;

    [Tooltip("Spring constant (k) in Hooke's Law. Higher values mean stiffer suspension (Newton/Meter).")]
    public float SpringStiffness = 30000f;

    [Tooltip("Damping force to reduce oscillation and bouncing (Newton Second/Meter).")]
    public float DamperStiffness = 4000f;

    [Header("Wheel Settings")]
    [Tooltip("Radius of the wheel visual mesh.")]
    public float WheelRadius = 0.4f;
}