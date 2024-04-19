using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 11f;
    Vector2 horizontalInput;

    [SerializeField] float jumpHeight = 3.5f;
    bool jump;

    [SerializeField] float gravity = -30f;
    Vector3 verticalVelocity = Vector3.zero;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);
        if (isGrounded)
        {
            verticalVelocity.y = 0;
        }

        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed;
        controller.Move(horizontalVelocity * Time.deltaTime);

        if (jump)
        {
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
            jump = false;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }

    public void OnJumpPressed()
    {
        jump = true;
    }

    public void OnInteractionPressed()
    {
        var origin = Camera.main.transform.position;
        var direction = Camera.main.transform.forward;
        Ray ray = new Ray(origin, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            if (hit.transform.CompareTag("Interactable"))
            {
                hit.transform.GetComponent<Interactable>().OnRaycastHit();
                Debug.Log("Did Hit");
            }
            Debug.Log("Did not Hit");
        }
        else
        {
            Debug.Log("Did not Hit");
        }
    }

    public void OnMouseShootPressed()
    {
        Player.PlayerlInstance.ShootWeapon();
    }
}
