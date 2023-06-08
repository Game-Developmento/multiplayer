using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;
using TMPro;


public class PlayerController : NetworkBehaviour
{
    [Header("Player Components")]
    private CharacterController _controller;
    [Header("Move Action")]
    [SerializeField] private InputAction moveAction;
    [SerializeField] float speed = 3f;
    private Vector2 currMovementInput;
    private Vector3 movement = Vector3.zero;
    private bool movePressed;
    [Header("Jump Action")]
    [SerializeField] InputAction jumpAction;
    private bool jumpPressed;
    [Header("Animations")]
    private Animator animator;
    private int isWalkingHash;
    private int jumpTriggerHash;
    void OnValidate()
    {
        // Provide default bindings for the input actions
        if (moveAction == null)
        {
            moveAction = new InputAction(type: InputActionType.Button);
        }
        if (moveAction.bindings.Count == 0)
        {
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/W")
                .With("Down", "<Keyboard>/S")
                .With("Left", "<Keyboard>/A")
                .With("Right", "<Keyboard>/D");
        }
    }
    private void Update()
    {
        // We have to read the button status in Update, because FixedNetworkUpdate might miss it.
        if (HasStateAuthority)
        {
            if (jumpAction.WasPressedThisFrame())
            {
                jumpPressed = true;
                animator.SetTrigger(jumpTriggerHash);
            }
            if (moveAction.WasReleasedThisFrame())
            {
                movePressed = false;
                HandleWalkAnimations();
            }
            else if (moveAction.WasPressedThisFrame())
            {
                movePressed = true;
                HandleWalkAnimations();
            }
        }
    }
    public override void Spawned()
    {
        _controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        jumpTriggerHash = Animator.StringToHash("jump");
    }

    public override void FixedUpdateNetwork()
    {
        // Only move own player and not every other player. Each player controls only its own player object.
        if (!HasStateAuthority)
        {
            return;
            // NetworkTransform only synchronizes changes from the StateAuthority.             
            // If someone without StateAuthority tries to change, the change will be local, and not transmitted to other players.
        }
        if (jumpPressed == false)
        {
            currMovementInput = moveAction.ReadValue<Vector2>();
            movement.x = currMovementInput.x;
            movement.y = 0.0f;
            movement.z = currMovementInput.y;
            _controller.Move(movement * speed * Runner.DeltaTime);
        }
        else
        {
            jumpPressed = false; // Reset for next iteration
        }
    }
    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }
    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }
    private void HandleWalkAnimations()
    {
        bool isWalking = animator.GetBool(isWalkingHash);

        // Determine idle or walking state
        if (movePressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!movePressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }
    }
}
