using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string fire = "Fire";
    [SerializeField] private string toggleWeapon = "ToggleWeapon";
    [SerializeField] private string rotateObject = "RotateObject";
    [SerializeField] private string reload = "Reload";

    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction fireAction;
    private InputAction toggleWeaponAction;
    private InputAction rotateObjectAction;
    private InputAction reloadAction;

    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }

    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }
    public bool FireTriggered { get; private set; }
    public bool ToggleWeaponTriggered { get; private set; }
    public bool RotateObjectTriggered { get; private set; }
    public bool ReloadTriggered { get; private set; }

    private bool usingMobileMovement;
    private bool usingMobileRotation;

    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);
        sprintAction = mapReference.FindAction(sprint);
        fireAction = mapReference.FindAction(fire);
        toggleWeaponAction = mapReference.FindAction(toggleWeapon);
        rotateObjectAction = mapReference.FindAction(rotateObject);
        reloadAction = mapReference.FindAction(reload);

        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
        movementAction.performed += inputInfo =>
        {
            if (!usingMobileMovement)
                MovementInput = inputInfo.ReadValue<Vector2>();
        };

        movementAction.canceled += inputInfo =>
        {
            if (!usingMobileMovement)
                MovementInput = Vector2.zero;
        };

        rotationAction.performed += inputInfo =>
        {
            if (!usingMobileRotation)
                RotationInput = inputInfo.ReadValue<Vector2>();
        };

        rotationAction.canceled += inputInfo =>
        {
            if (!usingMobileRotation)
                RotationInput = Vector2.zero;
        };

        jumpAction.performed += _ => JumpTriggered = true;
        jumpAction.canceled += _ => JumpTriggered = false;

        sprintAction.performed += _ => SprintTriggered = true;
        sprintAction.canceled += _ => SprintTriggered = false;

        fireAction.performed += _ => FireTriggered = true;
        fireAction.canceled += _ => FireTriggered = false;

        toggleWeaponAction.performed += _ => ToggleWeaponTriggered = true;
        toggleWeaponAction.canceled += _ => ToggleWeaponTriggered = false;

        rotateObjectAction.performed += _ => RotateObjectTriggered = true;
        rotateObjectAction.canceled += _ => RotateObjectTriggered = false;

        reloadAction.performed += _ => ReloadTriggered = true;
        reloadAction.canceled += _ => ReloadTriggered = false;
    }

    public void SetMovementInput(Vector2 input)
    {
        usingMobileMovement = input.magnitude > 0.1f;
        MovementInput = input;
    }

    public void SetRotationInput(Vector2 input)
    {
        usingMobileRotation = input.magnitude > 0.1f;
        RotationInput = input;
    }

    public bool ConsumeToggleWeaponTriggered()
    {
        if (!ToggleWeaponTriggered) return false;

        ToggleWeaponTriggered = false;
        return true;
    }

    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
    }
}