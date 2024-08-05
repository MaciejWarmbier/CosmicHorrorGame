using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
#pragma warning disable 649

    public static InputManager InputManagerInstance = null;
    public Action OnInteractionClicked;


    [SerializeField] MovementController movement;
    [SerializeField] LookController mouseLook;

    PlayerController controls;
    PlayerController.PlayerMovementActions playerMovement;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    private void Awake()
    {
        InputManagerInstance = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controls = new PlayerController();
        playerMovement = controls.PlayerMovement;

        playerMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();

        playerMovement.Jump.performed += _ => movement.OnJumpPressed();

        playerMovement.Interaction.performed += _ => HandleOnInteractionClicked();
        playerMovement.Shoot.performed += _ => movement.OnMouseShootPressed();
        playerMovement.Reload.performed += _ => movement.OnReloadPressed();
        playerMovement.ChangeWeapon.performed += _ => movement.OnChangeWeaponPressed();

        playerMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        playerMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
    }

    private void HandleOnInteractionClicked()
    {
        OnInteractionClicked?.Invoke();
        movement.OnInteractionPressed();
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
