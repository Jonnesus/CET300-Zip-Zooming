using UnityEngine;

public class OldWheelController : MonoBehaviour
{
    [Header("Suspension")]
    [SerializeField] private float restLength;
    [SerializeField] private float springTravel;
    [SerializeField] private float springStiffness;
    [SerializeField] private float damperStiffness;

    private float minLength;
    private float maxLength;
    private float lastLength;
    private float springLength;
    private float springForce;
    private float springVelocity;
    private float damperForce;

    private Vector3 suspensionForce;

    [Header("Wheel")]
    public bool frontLeft;
    public bool frontRight;
    public bool rearLeft;
    public bool rearRight;

    public float steerAngle;

    [SerializeField] private float wheelRadius;
    [SerializeField] private float wheelAngle;
    [SerializeField] private float steerTime;

    [Header("Movement")]
    [SerializeField] private float speedMultiplier;

    private float forceX;
    private float forceY;

    private Vector3 wheelVelocityLS;

    [Header("Misc")]
    private Rigidbody rb;
    private InputManager inputManager;

    private void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
        inputManager = transform.root.GetComponent<InputManager>();
        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, -transform.up * (springLength + wheelRadius), Color.green);

        // Calculates steering angle based on Ackermann Steering
        Steering();
    }

    private void FixedUpdate()
    {
        // Caculates suspension at wheel location and applies the force to the car rb
        Suspension();

        // Calculates velocity
        Velocity();
    }

    private void Suspension()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            // Tracks length of suspension from last frame
            lastLength = springLength;

            // Calculates current springLength & clamps between minLength and maxLength
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);

            // Calculates desired springVelocity, springForce and damperForce
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLength - springLength);
            damperForce = damperStiffness * springVelocity;

            // Calculates overall suspensionForce with a damper applied
            suspensionForce = (springForce + damperForce) * transform.up;

            // Calculates wheel velocity from local space
            wheelVelocityLS = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));

            // Applies suspensionForce and velocity forces to car rb
            rb.AddForceAtPosition(suspensionForce + (forceX * transform.forward) + (forceY * -transform.right), hit.point);
        }
    }

    private void Steering()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Vector3.up * wheelAngle);
    }

    private void Velocity()
    {
        forceX = inputManager.move * speedMultiplier * springForce;
        forceY = wheelVelocityLS.x * springForce;
    }
}