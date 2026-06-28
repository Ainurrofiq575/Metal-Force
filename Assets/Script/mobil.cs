using UnityEngine;

public class mobil : MonoBehaviour
{
    [Header("Joystick")]
    public FixedJoystick steerJoystick;
    public FixedJoystick moveJoystick;

    [Header("Mobile Buttons")]
    public MobileButton gasButton;
    public MobileButton remButton;
    public MobileButton leftButton;
    public MobileButton rightButton;

    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBreakForce;
    private bool isBreaking;
    private Rigidbody rb;

    [Header("Car Settings")]
    [SerializeField] private float motorForce = 1000f;
    [SerializeField] private float breakForce = 3000f;
    [SerializeField] private float maxSteerAngle = 12f;

    [Header("Stability Setting")]
    [SerializeField] private float centerOfMassY = -0.8f;

    [Header("Drift Setting")]
    [SerializeField] private float normalGrip = 1.2f;
    [SerializeField] private float driftGrip = 0.8f;

    [Header("Engine Audio")]
    public AudioSource engineSource;
    public float engineGasVolume = 0.45f;
    public float engineIdleVolume = 0.08f;
    public float engineVolumeSpeed = 4f;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [Header("Wheel Meshes")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.centerOfMass = new Vector3(0f, centerOfMassY, 0f);
        }

        if (engineSource == null)
        {
            engineSource = GetComponent<AudioSource>();
        }

        if (engineSource != null)
        {
            engineSource.loop = true;

            if (AudioManager.instance != null && !AudioManager.instance.soundOn)
            {
                engineSource.volume = 0f;

                if (engineSource.isPlaying)
                {
                    engineSource.Stop();
                }
            }
            else
            {
                engineSource.volume = engineIdleVolume;

                if (!engineSource.isPlaying)
                {
                    engineSource.Play();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        HandleDrift();
        HandleEngineSound();
        UpdateWheels();
    }

    private void GetInput()
    {
        // =========================
        // KEYBOARD INPUT LAPTOP
        // =========================
        float keyboardHorizontal = Input.GetAxis("Horizontal");
        float keyboardVertical = Input.GetAxis("Vertical");
        bool keyboardBrake = Input.GetKey(KeyCode.Space);

        // =========================
        // JOYSTICK INPUT
        // =========================
        float joystickHorizontal = steerJoystick != null
            ? steerJoystick.Horizontal
            : 0f;

        float joystickVertical = moveJoystick != null
            ? moveJoystick.Vertical
            : 0f;

        // =========================
        // MOBILE BUTTON INPUT
        // =========================
        float mobileHorizontal = 0f;
        float mobileVertical = 0f;

        if (leftButton != null && leftButton.isPressed)
        {
            mobileHorizontal = -1f;
        }

        if (rightButton != null && rightButton.isPressed)
        {
            mobileHorizontal = 1f;
        }

        if (gasButton != null && gasButton.isPressed)
        {
            mobileVertical = 1f;
        }

        if (remButton != null && remButton.isPressed)
        {
            mobileVertical = -1f;
        }

        // =========================
        // PRIORITAS BELOK
        // Mobile Button > Joystick > Keyboard
        // =========================
        if (Mathf.Abs(mobileHorizontal) > 0.1f)
        {
            horizontalInput = mobileHorizontal;
        }
        else if (Mathf.Abs(joystickHorizontal) > 0.1f)
        {
            horizontalInput = joystickHorizontal;
        }
        else
        {
            horizontalInput = keyboardHorizontal;
        }

        // =========================
        // PRIORITAS GAS
        // Mobile Button > Joystick > Keyboard
        // =========================
        if (Mathf.Abs(mobileVertical) > 0.1f)
        {
            verticalInput = mobileVertical;
        }
        else if (Mathf.Abs(joystickVertical) > 0.1f)
        {
            verticalInput = joystickVertical;
        }
        else
        {
            verticalInput = keyboardVertical;
        }

        // =========================
        // REM
        // Space laptop untuk brake / drift
        // =========================
        isBreaking = keyboardBrake;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque =
            verticalInput * motorForce;

        frontRightWheelCollider.motorTorque =
            verticalInput * motorForce;

        currentBreakForce =
            isBreaking ? breakForce * 0.3f : 0f;

        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontLeftWheelCollider.brakeTorque =
            currentBreakForce;

        frontRightWheelCollider.brakeTorque =
            currentBreakForce;

        rearLeftWheelCollider.brakeTorque =
            isBreaking ? breakForce : 0f;

        rearRightWheelCollider.brakeTorque =
            isBreaking ? breakForce : 0f;
    }

    private void HandleDrift()
    {
        if (isBreaking)
        {
            SetRearGrip(driftGrip);
        }
        else
        {
            SetRearGrip(normalGrip);
        }
    }

    private void HandleEngineSound()
    {
        if (engineSource == null) return;

        // SOUND OFF = ENGINE BENAR-BENAR MATI
        if (AudioManager.instance != null && !AudioManager.instance.soundOn)
        {
            engineSource.volume = 0f;

            if (engineSource.isPlaying)
            {
                engineSource.Stop();
            }

            return;
        }

        // SOUND ON = ENGINE HIDUP LAGI
        if (!engineSource.isPlaying)
        {
            engineSource.Play();
        }

        float targetVolume = engineIdleVolume;

        // MAJU ATAU MUNDUR SAMA-SAMA BUNYI GAS
        if (Mathf.Abs(verticalInput) > 0.1f)
        {
            targetVolume = engineGasVolume;
        }

        engineSource.volume = Mathf.Lerp(
            engineSource.volume,
            targetVolume,
            engineVolumeSpeed * Time.deltaTime
        );
    }

    private void SetRearGrip(float grip)
    {
        WheelFrictionCurve leftFriction =
            rearLeftWheelCollider.sidewaysFriction;

        WheelFrictionCurve rightFriction =
            rearRightWheelCollider.sidewaysFriction;

        leftFriction.stiffness = grip;
        rightFriction.stiffness = grip;

        rearLeftWheelCollider.sidewaysFriction =
            leftFriction;

        rearRightWheelCollider.sidewaysFriction =
            rightFriction;
    }

    private void HandleSteering()
    {
        currentSteerAngle =
            maxSteerAngle * horizontalInput;

        frontLeftWheelCollider.steerAngle =
            currentSteerAngle;

        frontRightWheelCollider.steerAngle =
            currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateWheelPos(
            frontLeftWheelCollider,
            frontLeftWheelTransform
        );

        UpdateWheelPos(
            frontRightWheelCollider,
            frontRightWheelTransform
        );

        UpdateWheelPos(
            rearLeftWheelCollider,
            rearLeftWheelTransform
        );

        UpdateWheelPos(
            rearRightWheelCollider,
            rearRightWheelTransform
        );
    }

    private void UpdateWheelPos(
        WheelCollider wheelCollider,
        Transform wheelTransform
    )
    {
        Vector3 pos;
        Quaternion rot;

        wheelCollider.GetWorldPose(
            out pos,
            out rot
        );

        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}