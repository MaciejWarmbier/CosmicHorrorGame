using System.Collections;
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
    [SerializeField] AudioSource movingAudio;
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

        
        if ((horizontalVelocity.x != 0 || horizontalVelocity.z != 0)) 
        {
            if (!movingAudio.isPlaying)
            {
                movingAudio.Play();
            }
        }
        else
        {
            movingAudio.Stop();
        }
        
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

        CheckForInteraction();
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

        if (Physics.Raycast(ray, out RaycastHit hit, 20f))
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

    public void CheckForInteraction()
    {
        var origin = Camera.main.transform.position;
        var direction = Camera.main.transform.forward;
        Ray ray = new Ray(origin, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, 20f))
        {
            if (hit.transform.CompareTag("Interactable"))
            {
                var interactable = hit.transform.GetComponentInChildren<Interactable>();
                if (interactable != null && interactable.CanBeInteracted) 
                {
                    InteractionPanel.InteractionPanelInstance.SetInteractionTextActive(true);
                }
                else
                {
                    InteractionPanel.InteractionPanelInstance.SetInteractionTextActive(false);
                }
            }
            else
            {
                InteractionPanel.InteractionPanelInstance.SetInteractionTextActive(false);
            }
        }
        else
        {
            InteractionPanel.InteractionPanelInstance.SetInteractionTextActive(false);
        }
    }

    public void OnMouseShootPressed()
    {
        Player.PlayerlInstance.ShootWeapon();
    }

    public void OnChangeWeaponPressed()
    {
        Player.PlayerlInstance.SwitchWeapon();
    }

    public void OnReloadPressed()
    {
        Player.PlayerlInstance.Reload();
    }

    public IEnumerator MovePlayer(Transform pushTransform)
    {
        if (pushTransform != null)
        {
            // Get direction from your postion toward the object you wish to push
            var direction = transform.position - pushTransform.position;
            direction = direction.normalized;
            //controller.SimpleMove(direction * 50);

            Vector3 upVector = new Vector3(0, 0.5f, 0);
            Vector3 throwVector;
            int throwDistance = 1;
            

            for(int i = 0; i < throwDistance * 10; i++)
            {
                if(i<= throwDistance/2)
                {
                    throwVector = direction + upVector;
                }
                else
                {
                    throwVector = direction - upVector;
                }

                controller.Move(throwVector );

                yield return  new WaitForFixedUpdate();
            }
        }
    }
}
