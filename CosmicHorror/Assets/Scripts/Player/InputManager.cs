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

        playerMovement.Escape.performed += _ => OnEscapeClick();
        playerMovement.ChangeLetter.performed += _ => OnEnterClick();
        playerMovement.DialogueInteraction.performed += ctx => OnDialogueClick(ctx.ReadValue<float>());
        playerMovement.Reset.performed += _ => ResetScene();
        playerMovement.DialogueInteraction.performed += ctx => OnDialogueClick(ctx.ReadValue<float>());

        playerMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        playerMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
    }

    private void OnDialogueClick(float number)
    {
        if(DialoguePanel.DialoguePanelInstance.possibleDialogues >= (int)number)
        {
            controls.Disable();
            DialoguePanel.DialoguePanelInstance?.OnDialogueClick((int)number);
        }
    }

    private void ResetScene()
    {
        PlayerStatistics.PlayerStatisticslInstance.ReloadScene();
    }

    public void EnableInputSystem()
    {
        controls.Enable();
    }

    private void OnEscapeClick()
    {
        LetterCanvas.LetterCanvasInstance.Show();
    }

    private void OnEnterClick()
    {
        LetterCanvas.LetterCanvasInstance.Change();
    }

    private void HandleOnInteractionClicked()
    {
        OnInteractionClicked?.Invoke();
        movement.OnInteractionPressed();
    }

    private void FixedUpdate()
    {
        if (Time.timeScale != 0)
        {
            movement.ReceiveInput(horizontalInput);
            mouseLook.ReceiveInput(mouseInput);
        }
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
