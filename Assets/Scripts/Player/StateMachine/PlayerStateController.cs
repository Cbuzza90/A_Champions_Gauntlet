using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    public PlayerBaseState currentState;
    public PlayerIdleState idleState;
    public PlayerRunningState runningState;
    public PlayerJumpingState jumpingState;
    public PlayerFallingState fallingState;
    public PlayerAttackingState attackingState;
    public PlayerSlidingState slidingState;
    public PlayerCastingState castingState;

    [Header("State Properties")]
    public float moveSpeed = 6f;
    public float jumpForce = 7.5f;
    public float attackCooldownTime = 0.1f;
    public float slideSpeed = 2f;
    public float slideDuration = 0.4f;
    public float slideCooldown = 1.2f;
    public Color immuneColor = new Color(0, 0, 0, 0);
    public Color normalColor = new Color(1, 1, 1, 1);
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckDistance = 0.1f;

    [Header("Runtime Variables")]
    public Vector2 moveDirection = Vector2.zero;
    public bool isGrounded;
    public bool isAttacking;
    public bool isSliding;
    public bool isImmune;
    public float slideCooldownTimer = 0f;

    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        idleState = new PlayerIdleState(this);
        runningState = new PlayerRunningState(this);
        jumpingState = new PlayerJumpingState(this);
        fallingState = new PlayerFallingState(this);
        attackingState = new PlayerAttackingState(this);
        slidingState = new PlayerSlidingState(this);
        castingState = new PlayerCastingState(this);

        currentState = idleState;
        currentState.EnterState();
    }

    void Update()
    {
        currentState.UpdateState();
        CheckGroundStatus();
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState();
        ManageTimers();
    }

    public void TransitionToState(PlayerBaseState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    void CheckGroundStatus()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;
    }

    void ManageTimers()
    {
        if (slideCooldownTimer > 0) slideCooldownTimer -= Time.deltaTime;
    }

    public void HandleMovement()
    {
        if (moveDirection.x != 0)
        {
            Vector3 characterScale = transform.localScale;
            characterScale.x = moveDirection.x < 0 ? -1 : 1;
            transform.localScale = characterScale;
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Stop moving if no input
        }
    }


    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
        Debug.Log("Setting move direction: " + direction);
        if (direction.x != 0)
        {
            TransitionToState(runningState);
        }
        else
        {
            TransitionToState(idleState);
        }
    }


    public void Jump()
    {
        if (isGrounded)  // Ensure the player can only jump if they are on the ground
        {
            TransitionToState(jumpingState);
        }
    }

    public void Attack()
    {
        if (!isAttacking)  // Prevent attacking if already in the middle of an attack (or add cooldown checks)
        {
            TransitionToState(attackingState);
        }
    }


    public void Slide()
    {
        if (isGrounded && !isSliding && slideCooldownTimer <= 0)  // Check conditions for sliding
        {
            TransitionToState(slidingState);
        }
    }


    public void Cast()
    {
        if (!isAttacking)  // Example condition; adjust based on your game's logic
        {
            TransitionToState(castingState);
        }
    }


}
