
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float verticalSpeed = 3f;
    public float rotationSpeed = 100f;
    public float smoothness = 5f;

    private Rigidbody rb;

    private float horizontalInput;
    private float forwardInput;
    private float verticalInput;
    private float rotationInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        forwardInput = Input.GetAxisRaw("Vertical");

        verticalInput = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            verticalInput = -1f;
        }

        rotationInput = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            rotationInput = -1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotationInput = 1f;
        }
    }

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(
            horizontalInput,
            0f,
            forwardInput
        );

        direction = Vector3.ClampMagnitude(direction, 1f);

        Vector3 horizontalMovement =
            transform.TransformDirection(direction) * moveSpeed;

        Vector3 verticalMovement =
            Vector3.up * verticalInput * verticalSpeed;

        Vector3 targetVelocity =
            horizontalMovement + verticalMovement;

        rb.linearVelocity = Vector3.Lerp(
            rb.linearVelocity,
            targetVelocity,
            Mathf.Clamp01(smoothness * Time.fixedDeltaTime)
        );

        float rotationAmount =
            rotationInput * rotationSpeed * Time.fixedDeltaTime;

        Quaternion newRotation =
            rb.rotation * Quaternion.Euler(0f, rotationAmount, 0f);

        rb.MoveRotation(newRotation);
    }
    void OnCollisionEnter(Collision collision)
{
    // Ignore safe landing pads
    if (collision.gameObject.CompareTag("SafePad"))
    {
        return;
    }

    // Trigger game over when hitting an obstacle
    if (GameManager.Instance != null)
    {
        GameManager.Instance.TriggerGameOver();
    }
}
}
