using UnityEngine;

public class PlayerMovementRB : MonoBehaviour
{
    float playerHeight = 5f;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float multiplier = 5f;
    public float jumpForce = 5f;

    // float horizontalMovement;
    float verticalMovement;
    
    [Header("Grounding Check")]
    [SerializeField] LayerMask whatIsGround;
    bool isGrounded;
    public GameObject groundChecker;
    public float groundCheckerRadius = 0.4f;

    Rigidbody rb;
    RaycastHit slopeHit;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight + 0.5f))
        {
            // if the raycast hits a surface that is not straight
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else return false;
        }

        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundChecker.transform.position, groundCheckerRadius, whatIsGround); 

        // Process Inputs
        Inputs();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void FixedUpdate()
    {
        // Update position
        MovePlayer();
    }

    void Inputs()
    {
        verticalMovement = Input.GetAxis("Vertical");

        // Check if character is rotated (possible with boolean variable), then set the correct direction
        // if ...

        // change this to adjust obstacle speed instead
        moveDirection = transform.right * verticalMovement;
        // else ...
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * multiplier, ForceMode.Acceleration);
            Vector3 clampedPos = transform.position;
            clampedPos.x = Mathf.Clamp(clampedPos.x, transform.position.x - 0.1f, transform.position.x + 0.1f);
            transform.position = clampedPos;
        }
        else if (isGrounded && OnSlope())
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * multiplier, ForceMode.Acceleration);

    }
}
