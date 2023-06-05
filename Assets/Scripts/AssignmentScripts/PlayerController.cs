using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

public class PlayerController : NetworkBehaviour
{
    [Header("Player Components")]
    private CharacterController _controller;
    [Header("Move Actions")]
    [SerializeField] private InputAction moveAction;
    [SerializeField] float speed = 3f;
    private Vector2 currMovementInput;
    private Vector3 velocity = Vector3.zero;

    void OnValidate()
    {
        // Provide default bindings for the input actions.
        // Based on answer by DMGregory: https://gamedev.stackexchange.com/a/205345/18261
        if (moveAction == null)
            moveAction = new InputAction(type: InputActionType.Button);
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
    public override void Spawned()
    {
        _controller = GetComponent<CharacterController>();
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

        currMovementInput = moveAction.ReadValue<Vector2>();
        velocity.x = currMovementInput.x * speed;
        velocity.z = currMovementInput.y * speed;
        _controller.Move(velocity * Runner.DeltaTime);
    }
    private void OnEnable() { moveAction.Enable(); }
    private void OnDisable() { moveAction.Disable(); }
}
