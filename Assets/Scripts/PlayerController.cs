using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;             // Velocidad de movimiento del jugador
    public float mouseSensitivity = 2f;  // Sensibilidad del mouse
    public Rigidbody rb;                 // Referencia al Rigidbody
    public Camera playerCamera;          // Referencia a la cámara del jugador

    private float verticalRotation = 0f;

   

    void Start()
    {
        // Ocultar y bloquear el cursor del mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        
        

    }

    void HandleMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * speed;
        float verticalMovement = Input.GetAxis("Vertical") * speed;

        Vector3 movement = new Vector3(horizontalMovement, 0f, verticalMovement) * Time.deltaTime;
        movement = transform.TransformDirection(movement);
        rb.MovePosition(rb.position + movement);
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90f);

        transform.Rotate(Vector3.up * mouseX);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
