using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private float hangTime = 0.2f;
    [SerializeField] private float hangGravityMultiplier = 0.5f;

    private Rigidbody2D body;
    private int remainingJumps;
    private const int maxJumps = 2;
    private bool isGrounded;
    private float hangTimeCounter;
    private float initialGravityScale;

    private Transform groundCheck;
    private LayerMask groundLayer;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        remainingJumps = maxJumps;
        initialGravityScale = body.gravityScale;

        // Get the GroundCheck transform
        groundCheck = transform.Find("GroundCheck");
        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck not found! Please create a child object named 'GroundCheck' at the character's feet.");
        }

        // Set the ground layer
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        // Horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Reset jumps when grounded
        if (isGrounded && body.linearVelocity.y <= 0)
        {
            remainingJumps = maxJumps;
            hangTimeCounter = 0f;
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0)
        {
            Jump();
        }

        // Hang time
        if (!isGrounded && Mathf.Abs(body.linearVelocity.y) < 0.1f)
        {
            hangTimeCounter += Time.deltaTime;
            if (hangTimeCounter <= hangTime)
            {
                body.gravityScale = initialGravityScale * hangGravityMultiplier;
            }
            else
            {
                body.gravityScale = initialGravityScale;
            }
        }
        else
        {
            body.gravityScale = initialGravityScale;
            hangTimeCounter = 0f;
        }
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        remainingJumps--;
        hangTimeCounter = 0f;
    }
}
