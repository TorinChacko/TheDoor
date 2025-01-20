using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float wallCheckDistance = 0.1f;
    [SerializeField] private float wallSlideSpeed = 0.3f;
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float apexTimeThreshold = 0.1f;
    [SerializeField] private float apexJumpBoost = 2f;

    private Rigidbody2D body;
    private int remainingJumps;
    private const int maxJumps = 2;
    private bool isGrounded;
    private bool wasGrounded;
    private bool isTouchingWall;
    private int facingDirection = 1;
    private bool isDashing;
    private float lastDashTime;
    private float lastJumpTime;
    private bool isInApexTime;

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
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Check for dash input
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryDash();
        }

        // Check if grounded and touching wall
        wasGrounded = isGrounded;
        isGrounded = IsGroundedOnAnySide();
        isTouchingWall = CheckWallCollision();

        // Update facing direction
        if (horizontalInput > 0) facingDirection = 1;
        else if (horizontalInput < 0) facingDirection = -1;

        // Reset jumps when landing
        if (isGrounded && !wasGrounded)
        {
            remainingJumps = maxJumps;
        }

        // Check for apex time
        isInApexTime = !isGrounded && Mathf.Abs(body.linearVelocity.y) < apexTimeThreshold;

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || isInApexTime || remainingJumps > 0))
        {
            Jump();
        }

        // Apply movement
        if (!isDashing)
        {
            ApplyMovement(horizontalInput);
        }

        // Better jump physics
        ApplyJumpPhysics();

        // Level reset functionality
        if (Input.GetKeyDown(KeyCode.P))
        {
            ResetLevel();
        }

        // Debug output
        Debug.Log($"Is Grounded: {isGrounded}, Is Touching Wall: {isTouchingWall}, Remaining Jumps: {remainingJumps}, Is Dashing: {isDashing}, Is In Apex Time: {isInApexTime}");
    }

    private void TryDash()
    {
        if (Time.time - lastDashTime >= dashCooldown && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        float originalGravity = body.gravityScale;
        body.gravityScale = 0;
        body.linearVelocity = new Vector2(facingDirection * dashForce, 0);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        body.gravityScale = originalGravity;
    }

    private void ApplyMovement(float horizontalInput)
    {
        if (isTouchingWall && !isGrounded && horizontalInput != 0)
        {
            // Wall slide
            body.linearVelocity = new Vector2(body.linearVelocity.x, Mathf.Clamp(body.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            // Normal movement
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
        }
    }

    private void ApplyJumpPhysics()
    {
        if (body.linearVelocity.y < 0)
        {
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (body.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
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

    private bool CheckWallCollision()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, groundLayer);
        return hit.collider != null;
    }

    private void Jump()
    {
        float jumpVelocity = jumpForce;

        if (isInApexTime)
        {
            jumpVelocity += apexJumpBoost;
        }

        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpVelocity);
        remainingJumps--;
        lastJumpTime = Time.time;
    }

    private void ResetLevel()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
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

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * facingDirection * wallCheckDistance);
    }
}
