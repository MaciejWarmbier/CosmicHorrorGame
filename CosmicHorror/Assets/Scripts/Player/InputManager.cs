using UnityEngine;

public class InputManager : MonoBehaviour
{
#pragma warning disable 649

    [SerializeField] MovementController movement;
    [SerializeField] LookController mouseLook;

    PlayerController controls;
    PlayerController.PlayerMovementActions playerMovement;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controls = new PlayerController();
        playerMovement = controls.PlayerMovement;

        playerMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();

        playerMovement.Jump.performed += _ => movement.OnJumpPressed();

        playerMovement.Interaction.performed += _ => movement.OnInteractionPressed();

        playerMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        playerMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
    }

    private void Update()
    {
        movement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDestroy()
    {
        controls.Disable();
    }
}
