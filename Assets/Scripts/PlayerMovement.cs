using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    private Rigidbody2D body;
    private int remainingJumps;
    private const int maxJumps = 2;
    private bool isGrounded;
    private bool wasGrounded;

    [SerializeField] private Transform groundChecksParent;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        remainingJumps = maxJumps;

        if (groundChecksParent == null)
        {
            Debug.LogError("GroundChecks parent not assigned! Please assign the parent object containing the ground check points.");
        }
    }

    private void Update()
    {
        // Horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        // Check if grounded on any side
        wasGrounded = isGrounded;
        isGrounded = IsGroundedOnAnySide();

        // Reset jumps when landing
        if (isGrounded && !wasGrounded)
        {
            remainingJumps = maxJumps;
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || remainingJumps > 0))
        {
            Jump();
        }

        // Better jump physics
        if (body.linearVelocity.y < 0)
        {
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (body.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Debug output
        Debug.Log($"Is Grounded: {isGrounded}, Remaining Jumps: {remainingJumps}");
    }

    private bool IsGroundedOnAnySide()
    {
        foreach (Transform checkPoint in groundChecksParent)
        {
            if (Physics2D.OverlapCircle(checkPoint.position, groundCheckRadius, groundLayer))
            {
                return true;
            }
        }
        return false;
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        remainingJumps--;
    }

    private void OnDrawGizmos()
    {
        if (groundChecksParent != null)
        {
            Gizmos.color = Color.red;
            foreach (Transform checkPoint in groundChecksParent)
            {
                Gizmos.DrawWireSphere(checkPoint.position, groundCheckRadius);
            }
        }
    }
}
