using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Variables
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 7.5f;
    [SerializeField] private float attackCooldownTime = 0.1f;
    [SerializeField] private float slideSpeed = 2f;
    [SerializeField] private float slideDuration = 0.4f;
    [SerializeField] private float slideCooldown = 1.2f;
    [SerializeField] private Color immuneColor = new Color(0, 0, 0, 0);
    [SerializeField] private Color normalColor = new Color(1, 1, 1, 1);
    [SerializeField] private LayerMask groundLayer; // Layer for ground detection
    [SerializeField] private Transform groundCheck; // Position for ground check
    [SerializeField] private float groundCheckDistance = 0.1f; // Distance for ground check
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackTimer = 0f;
    [SerializeField] private float nextAttackWindow = 0f;
    [SerializeField] private float slideCooldownTimer = 0f;
    [SerializeField] private float airAttackDuration = 0f;
    [SerializeField] private Vector2 moveDirection = Vector2.zero;

    //Flags
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isAirAttacking;
    [SerializeField] private bool isCasting;
    [SerializeField] private bool isSliding;
    [SerializeField] private bool isImmune;
    
    //Components
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private PlayerInputActions playerInputActions;
    private Coroutine attackCoroutine;

    //Animation States
    private static readonly string PLAYER_IDLE = "PlayerIdle";
    private static readonly string PLAYER_RUN = "PlayerRun";
    private static readonly string PLAYER_JUMP = "PlayerJump";
    private static readonly string PLAYER_FALL = "PlayerFalling";
    private static readonly string PLAYER_ATTACK = "PlayerAttack";
    private static readonly string PLAYER_ATTACK2 = "PlayerAttack2";
    private static readonly string PLAYER_ATTACK3 = "PlayerAttack3";
    private static readonly string PLAYER_SLIDE = "PlayerRoll";
    private static readonly string PLAYER_AIRATTACK = "PlayerAirAttack1";
    private static readonly string PLAYER_AIRROLL = "PlayerAirRoll";

    [SerializeField] private enum BasicAttackStage { None, Stage1, Stage2, Stage3 }
    [SerializeField]private BasicAttackStage currentBasicAttackStage = BasicAttackStage.None;
    private string currentAnimationState;

    private void Awake()
    {
        InitializeComponents();
        InitializeInputActions();
    }

    private void InitializeComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (spriteRenderer == null) Debug.LogError("SpriteRenderer component not found on " + gameObject.name);
        if (rb == null) Debug.LogError("Rigidbody2D component not found on " + gameObject.name);
        if (anim == null) Debug.LogError("Animator component not found on " + gameObject.name);
    }

    private void InitializeInputActions()
    {
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Move.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Move.canceled += ctx => moveDirection = Vector2.zero;
        playerInputActions.Player.Jump.performed += ctx => Jump();
        playerInputActions.Player.Attack.performed += ctx => StartAttack();
        playerInputActions.Player.Attack.canceled += ctx => StopAttack();
        playerInputActions.Player.Slide.performed += ctx => StartSlide();
    }

    private void OnEnable() => playerInputActions.Enable();
    private void OnDisable() => playerInputActions.Disable();

    private void Update()
    {
        HandleMovement();
        AttackCooldownTimer();
        SlideCooldownTimer();
        CheckGroundStatus();
    }

    private void FixedUpdate() => AnimationManager();

    private void HandleMovement()
    {
        if (isSliding) return;

        float xAxis = moveDirection.x;
        isMoving = xAxis != 0;

        if (isMoving)
        {
            Vector3 characterScale = transform.localScale;
            characterScale.x = xAxis < 0 ? -Mathf.Abs(characterScale.x) : Mathf.Abs(characterScale.x);
            transform.localScale = characterScale;
            rb.velocity = new Vector2(xAxis * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void Jump()
    {
        isJumping = true;
        Debug.Log("Jump() called");
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            StartCoroutine(JumpCoroutine());
        }
    }

    private void StartSlide()
    {
        if (!isSliding && slideCooldownTimer <= 0f)
        {
            StartCoroutine(SlideOrAirRollCoroutine(isGrounded));
            slideCooldownTimer = slideCooldown;
        }
    }

    private IEnumerator SlideOrAirRollCoroutine(bool grounded)
    {
        spriteRenderer.color = immuneColor;

        float direction = transform.localScale.x;
        rb.velocity = new Vector2(direction * slideSpeed, rb.velocity.y);

        yield return new WaitForSeconds(slideDuration);

        spriteRenderer.color = normalColor;

        if (!isGrounded && !isAttacking)
        {
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void StartAttack()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackRepeatedly());
        }
    }

    private void StopAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    private IEnumerator AttackRepeatedly()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(attackCooldownTime);
        }
    }

    private void Attack()
    {
        if (!isFalling && !isJumping && !isSliding)
        {
            if (attackTimer <= 0f)
            {
                if (!isAttacking)
                {
                    currentBasicAttackStage = BasicAttackStage.Stage1;
                }
                else
                {
                    currentBasicAttackStage = nextAttackWindow > 0f && currentBasicAttackStage < BasicAttackStage.Stage3
                        ? currentBasicAttackStage + 1
                        : BasicAttackStage.Stage1;
                }
                PerformBasicAttack();
            }
        }
        else if (!isGrounded && !isSliding && !isAttacking)
        {
            StartCoroutine(AirAttackCoroutine());
        }
    }

    private IEnumerator AirAttackCoroutine()
    {

        airAttackDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(airAttackDuration);

        if (!isGrounded && !isSliding)
        {
        }
    }

    private IEnumerator JumpCoroutine()
    {
        float jumpAnimationDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(jumpAnimationDuration);
        isJumping = false;


    }

    private void PerformBasicAttack()
    {
        switch (currentBasicAttackStage)
        {
            case BasicAttackStage.Stage1:
                break;
            case BasicAttackStage.Stage2:
                break;
            case BasicAttackStage.Stage3:
                break;
        }
        attackDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        attackTimer = attackDuration;
        nextAttackWindow = attackCooldownTime - attackDuration + 1.1f;
    }

    private void ResetAttack()
    {
        currentBasicAttackStage = BasicAttackStage.None;
    }

    private void AttackCooldownTimer()
    {
        if (attackTimer > 0) attackTimer -= Time.deltaTime;
        if (nextAttackWindow > 0) nextAttackWindow -= Time.deltaTime;
        if (nextAttackWindow <= 0 && isAttacking && airAttackDuration <= 0)
        {
            ResetAttack();
        }
    }

    private void SlideCooldownTimer()
    {
        if (slideCooldownTimer > 0) slideCooldownTimer -= Time.deltaTime;
    }

    private void AnimationManager()
    {
        if (!isMoving)
        {
            Debug.Log("Moving in AnimationManager Called");
            ChangeAnimationState(PLAYER_IDLE);
        }

        if (isMoving && isGrounded && !isSliding)
        {
            Debug.Log("Running in AnimationManager Called");
            ChangeAnimationState(PLAYER_RUN);
        }

        if (isJumping)
        {
            Debug.Log("Jumping in AnimationManager Called");
            ChangeAnimationState(PLAYER_JUMP);
        }
    }

    private void ChangeAnimationState(string newAnimationState)
    {
        if (currentAnimationState == newAnimationState) return;

        anim.Play(newAnimationState);
        currentAnimationState = newAnimationState;
    }

    private void CheckGroundStatus()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
    }
}
