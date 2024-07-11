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
    public float slideSpeed = 2f;
    public float slideDuration = 0.4f;
    public float slideCooldown = 1.2f;
    public Color immuneColor = new Color(0, 0, 0, 0);
    public Color normalColor = new Color(1, 1, 1, 1);
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckDistance = 0.1f;
    

    [Header("Attack Properties")]
    public float attackTransitionTimer;
    public float attackTimer;
    public float attackCooldownTimer;
    public BasicAttackStage currentAttackStage = BasicAttackStage.None;

    public enum BasicAttackStage
    {
        None,
        Stage1,
        Stage2,
        Stage3
    }

    [Header("Runtime Variables")]
    public Vector2 moveDirection = Vector2.zero;
    public bool isGrounded;
    public bool isAttacking;
    public bool isAirAttacking;
    public bool isSliding;
    public bool isImmune;
    public bool isFalling;
    public bool isCasting;
    public bool isJumping;
    public bool isMoving;
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
        Vector3 leftFootPosition = groundCheck.position + Vector3.left * 0.3f;
        Vector3 rightFootPosition = groundCheck.position + Vector3.right * 0.25f;

        Vector3 leftEndPosition = leftFootPosition + Vector3.down * groundCheckDistance;
        Vector3 rightEndPosition = rightFootPosition + Vector3.down * groundCheckDistance;

        RaycastHit2D leftHit = Physics2D.Raycast(leftFootPosition, Vector2.down, groundCheckDistance, groundLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rightFootPosition, Vector2.down, groundCheckDistance, groundLayer);

        isGrounded = leftHit.collider != null || rightHit.collider != null;

        if (leftHit.collider != null)
            Debug.DrawLine(leftFootPosition, leftEndPosition, Color.red, 0.1f);
        else
            Debug.DrawLine(leftFootPosition, leftEndPosition, Color.green, 0.1f);

        if (rightHit.collider != null)
            Debug.DrawLine(rightFootPosition, rightEndPosition, Color.red, 0.1f);
        else
            Debug.DrawLine(rightFootPosition, rightEndPosition, Color.green, 0.1f);
    }

    void ManageTimers()
    {
        if (slideCooldownTimer > 0) slideCooldownTimer -= Time.deltaTime;
    }

    public void HandleMovement()
    {
        if (moveDirection.x != 0)
        {
            Vector2 characterScale = transform.localScale;
            characterScale.x = moveDirection.x < 0 ? -4 : 4;
            transform.localScale = characterScale;
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
        if (direction.x != 0)
        {
            TransitionToState(runningState);
        }
        else if (!isGrounded)
        {
            TransitionToState(fallingState);
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            TransitionToState(jumpingState);
        }
    }

    public void Attack()
    {
        if (isAttacking)
        {
            ((PlayerAttackingState)attackingState).TrySetupNextAttack(); // Call SetupNextAttack if already attacking
        }
        else
        {
            TransitionToState(attackingState);
        }
    }

    public void Slide()
    {
        if (isGrounded && !isSliding && slideCooldownTimer <= 0)
        {
            TransitionToState(slidingState);
        }
    }

    public void Cast()
    {
        if (!isAttacking)
        {
            TransitionToState(castingState);
        }
    }
}
