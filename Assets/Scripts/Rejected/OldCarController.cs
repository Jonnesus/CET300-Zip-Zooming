using UnityEngine;

[RequireComponent (typeof(InputManager))]
public class OldCarController : MonoBehaviour
{
    public OldWheelController[] wheels;

    [Header("Car Specs")]
    [SerializeField] private float wheelBase;
    [SerializeField] private float rearTrack;
    [SerializeField] private float turnRadius;

    [Header("Inputs")]
    [SerializeField] private float ackermannAngleLeft;
    [SerializeField] private float ackermannAngleRight;

    private InputManager inputManager;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        // Turning right
        if (inputManager.steer > 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * inputManager.steer;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * inputManager.steer;
        }
        // Turning left
        else if (inputManager.steer < 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * inputManager.steer;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * inputManager.steer;
        }
        // Straight
        else
        {
            ackermannAngleLeft = 0;
            ackermannAngleRight = 0;
        }

        foreach (OldWheelController w in wheels)
        {
            if (w.frontLeft)
                w.steerAngle = ackermannAngleLeft;
            if (w.frontRight)
                w.steerAngle = ackermannAngleRight;
        }
    }
}