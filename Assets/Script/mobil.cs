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

    private float horizontalInput;
    private float verticalInput;

    private float currentSteerAngle;
    private float currentBreakForce;

    private bool isBreaking;

    private Rigidbody rb;

    // =====================================================
    // CAR SETTINGS
    // =====================================================

    [Header("Car Settings")]
    [SerializeField] private float motorForce = 1000f;
    [SerializeField] private float breakForce = 3000f;


    // =====================================================
    // STEERING SETTINGS
    // =====================================================

    [Header("Steering Settings")]

    // Belokan maksimum ketika kecepatan rendah
    [SerializeField] private float maxSteerAngle = 28f;

    // Belokan maksimum ketika sedang ngebut
    [SerializeField] private float highSpeedSteerAngle = 10f;

    // Pada kecepatan ini steering sudah memakai
    // highSpeedSteerAngle
    [SerializeField] private float highSpeedThreshold = 25f;

    // Semakin besar = steering semakin cepat merespon
    [SerializeField] private float steeringSmoothSpeed = 100f;


    // =====================================================
    // STABILITY SETTINGS
    // =====================================================

    [Header("Stability Settings")]

    // Semakin minus = mobil semakin stabil
    [SerializeField] private float centerOfMassY = -1.1f;

    // Menahan mobil supaya tidak berputar liar
    [SerializeField] private float angularDamping = 2.5f;

    // Menahan body roll saat menikung
    [SerializeField] private float antiRollForce = 5000f;

    // Membantu mobil tetap menempel ke jalan
    [SerializeField] private float downforce = 60f;

    // Sudut maksimum sebelum bantuan anti-terguling aktif
    [SerializeField] private float maxTiltAngle = 35f;

    // Kekuatan bantuan mengembalikan mobil tetap tegak
    [SerializeField] private float uprightAssist = 8f;


    // =====================================================
    // DRIFT SETTINGS
    // =====================================================

    [Header("Drift Settings")]
    [SerializeField] private float normalGrip = 1.2f;
    [SerializeField] private float driftGrip = 0.8f;


    // =====================================================
    // ENGINE AUDIO
    // =====================================================

    [Header("Engine Audio")]

    public AudioSource engineSource;

    public float engineGasVolume = 0.45f;
    public float engineIdleVolume = 0.08f;
    public float engineVolumeSpeed = 4f;


    // =====================================================
    // WHEEL COLLIDERS
    // =====================================================

    [Header("Wheel Colliders")]

    [SerializeField]
    private WheelCollider frontLeftWheelCollider;

    [SerializeField]
    private WheelCollider frontRightWheelCollider;

    [SerializeField]
    private WheelCollider rearLeftWheelCollider;

    [SerializeField]
    private WheelCollider rearRightWheelCollider;


    // =====================================================
    // WHEEL MESHES
    // =====================================================

    [Header("Wheel Meshes")]

    [SerializeField]
    private Transform frontLeftWheelTransform;

    [SerializeField]
    private Transform frontRightWheelTransform;

    [SerializeField]
    private Transform rearLeftWheelTransform;

    [SerializeField]
    private Transform rearRightWheelTransform;


    // =====================================================
    // START
    // =====================================================

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Menurunkan titik berat mobil
            rb.centerOfMass = new Vector3(
                0f,
                centerOfMassY,
                0f
            );

            // Mengurangi mobil muter / oleng berlebihan
            rb.angularDamping = angularDamping;
        }


        // =========================
        // ENGINE AUDIO
        // =========================

        if (engineSource == null)
        {
            engineSource = GetComponent<AudioSource>();
        }

        if (engineSource != null)
        {
            engineSource.loop = true;

            if (
                AudioManager.instance != null &&
                !AudioManager.instance.soundOn
            )
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


    // =====================================================
    // FIXED UPDATE
    // =====================================================

    private void FixedUpdate()
    {
        GetInput();

        HandleMotor();

        HandleSteering();

        HandleDrift();

        HandleStability();

        HandleEngineSound();

        UpdateWheels();
    }


    // =====================================================
    // INPUT
    // =====================================================

    private void GetInput()
    {
        // =========================
        // KEYBOARD
        // =========================

        float keyboardHorizontal =
            Input.GetAxis("Horizontal");

        float keyboardVertical =
            Input.GetAxis("Vertical");

        bool keyboardBrake =
            Input.GetKey(KeyCode.Space);


        // =========================
        // JOYSTICK
        // =========================

        float joystickHorizontal =
            steerJoystick != null
            ? steerJoystick.Horizontal
            : 0f;

        float joystickVertical =
            moveJoystick != null
            ? moveJoystick.Vertical
            : 0f;


        // =========================
        // MOBILE BUTTON
        // =========================

        float mobileHorizontal = 0f;
        float mobileVertical = 0f;


        if (
            leftButton != null &&
            leftButton.isPressed
        )
        {
            mobileHorizontal = -1f;
        }


        if (
            rightButton != null &&
            rightButton.isPressed
        )
        {
            mobileHorizontal = 1f;
        }


        if (
            gasButton != null &&
            gasButton.isPressed
        )
        {
            mobileVertical = 1f;
        }


        if (
            remButton != null &&
            remButton.isPressed
        )
        {
            mobileVertical = -1f;
        }


        // =========================
        // PRIORITAS BELOK
        // Mobile > Joystick > Keyboard
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
        // BRAKE / DRIFT
        // =========================

        isBreaking = keyboardBrake;
    }


    // =====================================================
    // MOTOR
    // =====================================================

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque =
            verticalInput * motorForce;

        frontRightWheelCollider.motorTorque =
            verticalInput * motorForce;


        currentBreakForce =
            isBreaking
            ? breakForce * 0.3f
            : 0f;


        ApplyBreaking();
    }


    private void ApplyBreaking()
    {
        frontLeftWheelCollider.brakeTorque =
            currentBreakForce;

        frontRightWheelCollider.brakeTorque =
            currentBreakForce;


        rearLeftWheelCollider.brakeTorque =
            isBreaking
            ? breakForce
            : 0f;

        rearRightWheelCollider.brakeTorque =
            isBreaking
            ? breakForce
            : 0f;
    }


    // =====================================================
    // STEERING
    // =====================================================

    private void HandleSteering()
    {
        if (rb == null)
            return;


        // Kecepatan mobil dalam meter per detik
        float speed = rb.linearVelocity.magnitude;


        // 0 = lambat
        // 1 = sudah mencapai highSpeedThreshold
        float speedPercent = Mathf.Clamp01(
            speed / highSpeedThreshold
        );


        // Saat lambat:
        // steering = 28 derajat
        //
        // Saat cepat:
        // steering turun sampai 10 derajat
        float allowedSteerAngle = Mathf.Lerp(
            maxSteerAngle,
            highSpeedSteerAngle,
            speedPercent
        );


        float targetSteerAngle =
            allowedSteerAngle * horizontalInput;


        // Steering berubah secara perlahan dan mulus
        currentSteerAngle = Mathf.MoveTowards(
            currentSteerAngle,
            targetSteerAngle,
            steeringSmoothSpeed * Time.fixedDeltaTime
        );


        frontLeftWheelCollider.steerAngle =
            currentSteerAngle;

        frontRightWheelCollider.steerAngle =
            currentSteerAngle;
    }


    // =====================================================
    // DRIFT
    // =====================================================

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


    // =====================================================
    // STABILITY
    // =====================================================

    private void HandleStability()
    {
        if (rb == null)
            return;


        // =========================
        // ANTI ROLL DEPAN
        // =========================

        ApplyAntiRoll(
            frontLeftWheelCollider,
            frontRightWheelCollider
        );


        // =========================
        // ANTI ROLL BELAKANG
        // =========================

        ApplyAntiRoll(
            rearLeftWheelCollider,
            rearRightWheelCollider
        );


        // =========================
        // DOWNFORCE
        // =========================

        float speed = rb.linearVelocity.magnitude;

        rb.AddForce(
            -transform.up * downforce * speed,
            ForceMode.Force
        );


        // =========================
        // ANTI TERGULING ASSIST
        // =========================

        float tiltAngle = Vector3.Angle(
            transform.up,
            Vector3.up
        );


        if (tiltAngle > maxTiltAngle)
        {
            Vector3 correctionAxis = Vector3.Cross(
                transform.up,
                Vector3.up
            );

            rb.AddTorque(
                correctionAxis * uprightAssist,
                ForceMode.Acceleration
            );
        }
    }


    // =====================================================
    // ANTI ROLL BAR
    // =====================================================

    private void ApplyAntiRoll(
        WheelCollider leftWheel,
        WheelCollider rightWheel
    )
    {
        if (
            leftWheel == null ||
            rightWheel == null ||
            rb == null
        )
        {
            return;
        }


        float leftTravel = 1f;
        float rightTravel = 1f;


        bool leftGrounded =
            leftWheel.GetGroundHit(out WheelHit leftHit);

        bool rightGrounded =
            rightWheel.GetGroundHit(out WheelHit rightHit);


        if (leftGrounded)
        {
            leftTravel =
                (
                    -leftWheel.transform
                        .InverseTransformPoint(leftHit.point).y
                    - leftWheel.radius
                )
                / leftWheel.suspensionDistance;
        }


        if (rightGrounded)
        {
            rightTravel =
                (
                    -rightWheel.transform
                        .InverseTransformPoint(rightHit.point).y
                    - rightWheel.radius
                )
                / rightWheel.suspensionDistance;
        }


        float force =
            (leftTravel - rightTravel)
            * antiRollForce;


        if (leftGrounded)
        {
            rb.AddForceAtPosition(
                leftWheel.transform.up * -force,
                leftWheel.transform.position
            );
        }


        if (rightGrounded)
        {
            rb.AddForceAtPosition(
                rightWheel.transform.up * force,
                rightWheel.transform.position
            );
        }
    }


    // =====================================================
    // ENGINE AUDIO
    // =====================================================

    private void HandleEngineSound()
    {
        if (engineSource == null)
            return;


        // Sound global mati
        if (
            AudioManager.instance != null &&
            !AudioManager.instance.soundOn
        )
        {
            engineSource.volume = 0f;

            if (engineSource.isPlaying)
            {
                engineSource.Stop();
            }

            return;
        }


        // Sound global hidup lagi
        if (!engineSource.isPlaying)
        {
            engineSource.Play();
        }


        float targetVolume = engineIdleVolume;


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


    // =====================================================
    // WHEEL VISUAL
    // =====================================================

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
        if (
            wheelCollider == null ||
            wheelTransform == null
        )
        {
            return;
        }


        wheelCollider.GetWorldPose(
            out Vector3 pos,
            out Quaternion rot
        );


        wheelTransform.position = pos;

        wheelTransform.rotation = rot;
    }
}