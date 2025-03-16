using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(InputManager))]

public class CarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] rayPoints;
    [SerializeField] private Transform accelerationPoint;
    [SerializeField] private LayerMask drivableLayer;
    [SerializeField] private GameObject[] tyres = new GameObject[4];
    [SerializeField] private TrailRenderer[] skidMarks = new TrailRenderer[2];
    [SerializeField] private ParticleSystem[] skidSmoke = new ParticleSystem[2];

    private Rigidbody carRB;
    private InputManager inputManager;

    [Header("Suspension")]
    [SerializeField] private float springStiffess;
    [SerializeField] private float damperStiffness;
    [SerializeField] private float restLength;
    [SerializeField] private float springTravel;
    [SerializeField] private float wheelRadius;

    private int[] wheelIsGrounded = new int[4];
    private bool isGrounded = false;

    [Header("Car Settings")]
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float decceleration = 100f;
    [SerializeField] private float steerStrength = 15f;
    [SerializeField] private float dragCoeficient = 1f;
    [SerializeField] private float passiveAirRoll = 1f;
    [SerializeField] private AnimationCurve turningCurve;

    private Vector3 currentCarLocalVelocity = Vector3.zero;
    private float carVelocityRatio = 0;

    [Header("Visuals")]
    [SerializeField] private float tyreRotationSpeed = 3000f;
    [SerializeField] private float maxSteeringAngle = 30f;
    [SerializeField] private float minSideSkidVelocity = 10f;

    [Header("Audio")]
    [SerializeField] private AudioSource engineSound;
    [SerializeField] private AudioSource skidSound;
    [SerializeField]
    [Range(0, 1)] private float minPitch = 1f;
    [SerializeField]
    [Range(1,5)] private float maxPitch = 5f;

    private void Start()
    {
        carRB = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
    }

    private void FixedUpdate()
    {
        Suspension();
        GroundCheck();
        CarVeloctiy();
        Movement();
        TyreVisuals();
        EngineSound();
    }

    #region Suspension

    private void Suspension()
    {
        for (int i = 0; i < rayPoints.Length; i++)
        {
            RaycastHit hit;
            float maxLength = (restLength + springTravel) / 1.5f;

            if (Physics.Raycast(rayPoints[i].position, -rayPoints[i].up, out hit, maxLength + wheelRadius, drivableLayer))
            {
                wheelIsGrounded[i] = 1;

                float currentSpringLength = hit.distance - wheelRadius;
                float springCompression = (restLength - currentSpringLength) / springTravel;

                float springVelocity = Vector3.Dot(carRB.GetPointVelocity(rayPoints[i].position), rayPoints[i].up);
                float damperForce = damperStiffness * springVelocity;

                float springForce = springStiffess * springCompression;

                float netForce = springForce - damperForce;

                carRB.AddForceAtPosition(netForce * rayPoints[i].up, rayPoints[i].position);

                SetTyrePosition(tyres[i], hit.point + rayPoints[i].up * wheelRadius);

                Debug.DrawLine(rayPoints[i].position, hit.point, Color.red);
            }
            else
            {
                wheelIsGrounded[i] = 0;

                SetTyrePosition(tyres[i], rayPoints[i].position - rayPoints[i].up * maxLength);

                Debug.DrawLine(rayPoints[i].position, rayPoints[i].position + (wheelRadius + maxLength) * -rayPoints[i].up, Color.green);
            }
        }
    }

    #endregion

    #region Ground Check

    private void GroundCheck()
    {
        int groundedWheels = 0;

        for (int i = 0; i < wheelIsGrounded.Length; i++)
            groundedWheels += wheelIsGrounded[i];

        if (groundedWheels > 1)
            isGrounded = true;
        else
            isGrounded = false;
    }

    #endregion

    #region Movement

    private void CarVeloctiy()
    {
        currentCarLocalVelocity = transform.InverseTransformDirection(carRB.linearVelocity);
        carVelocityRatio = currentCarLocalVelocity.z / maxSpeed;
    }

    private void Movement()
    {
        if (isGrounded)
        {
            Acceleration();
            Deceleration();
            Turn();
            SidewaysDrag();

            if (inputManager.move == 0)
            {
                carRB.linearDamping = 1f;

                if (Mathf.Abs(carVelocityRatio) < 0.0005f)
                    carRB.linearVelocity = Vector3.zero;
            }
            else
                carRB.linearDamping = 0.2f;
        }
        else
        {
            transform.Rotate(passiveAirRoll, 0, 0);
        }
    }

    private void Acceleration()
    {
        if (currentCarLocalVelocity.z < maxSpeed)
            carRB.AddForceAtPosition(acceleration * inputManager.move * transform.forward, accelerationPoint.position, ForceMode.Acceleration);
    }

    private void Deceleration()
    {
        carRB.AddForceAtPosition(decceleration * inputManager.move * -transform.forward, accelerationPoint.position, ForceMode.Acceleration);
    }
    private void Turn()
    {
        carRB.AddTorque(steerStrength * inputManager.steer * turningCurve.Evaluate(Mathf.Abs(carVelocityRatio)) * Mathf.Sign(carVelocityRatio) * transform.up, ForceMode.Acceleration);
    }

    private void SidewaysDrag()
    {
        float currentSidewaysSpeed = currentCarLocalVelocity.x;
        float dragForceMagnitude = -currentSidewaysSpeed * dragCoeficient;
        Vector3 dragForce = transform.right * dragForceMagnitude;

        carRB.AddForceAtPosition(dragForce, carRB.worldCenterOfMass, ForceMode.Acceleration);
    }

    #endregion

    #region Visuals

    private void SetTyrePosition(GameObject tyre, Vector3 targetPosition)
    {
        tyre.transform.position = targetPosition;
        VFX();
    }

    private void TyreVisuals()
    {
        float steeringAngle = inputManager.steer * maxSteeringAngle;

        for (int i = 0; i < tyres.Length; i++)
        {
            if (i < 2)
            {
                tyres[i].transform.Rotate(Vector3.right, tyreRotationSpeed * carVelocityRatio * Time.deltaTime, Space.Self);
                tyres[i].transform.localEulerAngles = new Vector3(tyres[i].transform.localEulerAngles.x, steeringAngle, tyres[i].transform.localEulerAngles.z);
            }
            else
            {
                tyres[i].transform.Rotate(Vector3.right, tyreRotationSpeed * inputManager.move * Time.deltaTime, Space.Self);
            }
        }
    }

    private void VFX()
    {
        if (isGrounded && Mathf.Abs(currentCarLocalVelocity.x) > minSideSkidVelocity && carVelocityRatio > 0)
        {
            ToggleSkidMarks(true);
            ToggleSkidSmoke(true);
            ToggleSkidSound(true);
        }
        else
        {
            ToggleSkidMarks(false);
            ToggleSkidSmoke(false);
            ToggleSkidSound(false);
        }
    }

    private void ToggleSkidMarks(bool toggle)
    {
        foreach (var skidMark in skidMarks)
        {
            skidMark.emitting = toggle;
        }
    }

    private void ToggleSkidSmoke(bool toggle)
    {
        foreach (var skidSmoke in skidSmoke)
        {
            if (toggle)
                skidSmoke.Play();
            else
                skidSmoke.Stop();
        }
    }

    #endregion

    #region Audio

    private void EngineSound()
    {
        engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.Abs(carVelocityRatio));
    }

    private void ToggleSkidSound(bool toggle)
    {
        skidSound.mute = !toggle;
    }

    #endregion
}