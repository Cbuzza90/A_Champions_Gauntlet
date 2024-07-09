using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Variables
    [SerializeField] public float moveSpeed = 6f;
    [SerializeField] public float jumpForce = 7.5f;
    [SerializeField] private float attackCooldownTime = 0.1f;
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackTimer = 0f;
    [SerializeField] private float nextAttackWindow = 0f;
    [SerializeField] private float slideSpeed = 2f; // Slide speed
    [SerializeField] private float slideDuration = 0.4f; // Duration of the slide
    [SerializeField] private float slideCooldown = 1.2f; // Cooldown between slides
    [SerializeField] private float slideCooldownTimer = 0f; // Timer for the slide cooldown
    private Vector2 moveDirection = Vector2.zero;

    // Attack Stages
    public BasicAttackStage currentBasicAttackStage = BasicAttackStage.None;
    public enum BasicAttackStage
    {
        None,
        Stage1,
        Stage2,
        Stage3,
    }

    // Components
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerInputActions playerInputActions;

    // Flags
    public bool isJumping;
    public bool isMoving;
    public bool isGrounded;
    public bool isFalling;
    public bool isAttacking;
    public bool isCasting;
    public bool isSliding;

    // Animation States
    private string currentAnimationState;
    const string PLAYER_IDLE = "PlayerIdle";
    const string PLAYER_RUN = "PlayerRun";
    const string PLAYER_JUMP = "PlayerJump";
    const string PLAYER_FALL = "PlayerFalling";
    const string PLAYER_ATTACK = "PlayerAttack";
    const string PLAYER_ATTACK2 = "PlayerAttack2";
    const string PLAYER_ATTACK3 = "PlayerAttack3";
    const string PLAYER_SLIDE = "PlayerRoll";
    private Coroutine attackCoroutine;

    private void Awake()
    {
        // Initialize components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Ensure components are assigned
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on " + gameObject.name);
        }

        if (anim == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }

        // Initialize input actions
        playerInputActions = new PlayerInputActions();

        // Set up event handlers
        playerInputActions.Player.Move.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Move.canceled += ctx => moveDirection = Vector2.zero;
        playerInputActions.Player.Jump.performed += ctx => Jump();
        playerInputActions.Player.Attack.performed += ctx => StartAttack();
        playerInputActions.Player.Attack.canceled += ctx => StopAttack();
        playerInputActions.Player.Slide.performed += ctx => StartSlide();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        
        AttackCooldownTimer();
        SlideCooldownTimer();
    }

    private void FixedUpdate()
    {
        AnimationManager();
    }

    private void HandleMovement()
    {
        if (isSliding) return; // Prevent movement while sliding

        // Movement Left/Right
        float xAxis = moveDirection.x;
        isMoving = xAxis != 0;
        if (isMoving)
        {
            // Flip the player based on direction
            Vector3 characterScale = transform.localScale;
            characterScale.x = xAxis < 0 ? -Mathf.Abs(characterScale.x) : Mathf.Abs(characterScale.x);
            transform.localScale = characterScale;
            rb.velocity = new Vector2(xAxis * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Immediately stop horizontal movement
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            isGrounded = false;
            isAttacking = false;
            StartCoroutine(JumpCoroutine());
        }
    }

    private void StartSlide()
    {
        if (isGrounded && !isSliding && slideCooldownTimer <= 0f)
        {
            StartCoroutine(SlideCoroutine());
            slideCooldownTimer = slideCooldown;
        }
    }

    private IEnumerator SlideCoroutine()
    {
        isSliding = true;

        ChangeAnimationState(PLAYER_SLIDE);

        // Get the direction of the slide based on the player's facing direction
        float slideDirection = transform.localScale.x;
        rb.velocity = new Vector2(slideDirection * slideSpeed, rb.velocity.y);

        yield return new WaitForSeconds(slideDuration);

        isSliding = false;
        rb.velocity = new Vector2(0, rb.velocity.y); // Stop sliding
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
            yield return new WaitForSeconds(attackCooldownTime); // Adjust the interval as needed
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
                    isAttacking = true;
                }
                else
                {
                    if (nextAttackWindow > 0f)
                    {
                        currentBasicAttackStage++;
                        if (currentBasicAttackStage > BasicAttackStage.Stage3)
                        {
                            currentBasicAttackStage = BasicAttackStage.Stage1;
                        }
                    }
                    else
                    {
                        currentBasicAttackStage = BasicAttackStage.Stage1;
                    }
                }
                PerformBasicAttack();
            }
        }
    }

    IEnumerator JumpCoroutine()
    {
        // Play jump animation
        ChangeAnimationState(PLAYER_JUMP);

        // Get the length of the jump animation
        float jumpAnimationDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(jumpAnimationDuration);

        // Transition to fall animation if still in the air
        if (!isGrounded)
        {
            isFalling = true;
            ChangeAnimationState(PLAYER_FALL);
        }
    }

    void PerformBasicAttack()
    {
        switch (currentBasicAttackStage)
        {
            case BasicAttackStage.Stage1:
                ChangeAnimationState(PLAYER_ATTACK);
                break;
            case BasicAttackStage.Stage2:
                ChangeAnimationState(PLAYER_ATTACK2);
                break;
            case BasicAttackStage.Stage3:
                ChangeAnimationState(PLAYER_ATTACK3);
                break;
        }
        attackDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        attackTimer = attackDuration;
        nextAttackWindow = attackCooldownTime - attackDuration + 1.1f;
    }

    void ResetAttack()
    {
        Debug.Log("Attack Reset");
        isAttacking = false;
        currentBasicAttackStage = BasicAttackStage.None;
    }

    void AttackCooldownTimer()
    {
        // Update the timers
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (nextAttackWindow > 0)
        {
            nextAttackWindow -= Time.deltaTime;
        }

        // Change to idle animation if the attack timer has expired and the player is attacking
        if (attackTimer <= 0 && isAttacking && !isMoving && !isSliding)
        {
            ChangeAnimationState(PLAYER_IDLE);
        }

        // Reset the attack if the next attack window has expired and the player is attacking
        if (nextAttackWindow <= 0 && isAttacking)
        {
            ResetAttack();
        }
    }

    void SlideCooldownTimer()
    {
        if (slideCooldownTimer > 0) // Corrected condition
        {
            slideCooldownTimer -= Time.deltaTime;
        }
    }

    private void AnimationManager()
    {
        // Idle
        if (!isMoving && isGrounded && !isJumping && !isFalling && !isAttacking && !isSliding)
        {
            ChangeAnimationState(PLAYER_IDLE);
        }

        // Running
        if (isMoving && isGrounded && !isJumping && !isFalling && attackTimer <= 0 && !isSliding)
        {
            ChangeAnimationState(PLAYER_RUN);
        }
    }

    private void ChangeAnimationState(string newAnimationState)
    {
        if (currentAnimationState == newAnimationState) return;

        anim.Play(newAnimationState);
        currentAnimationState = newAnimationState;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !isAttacking)
        {
            Debug.Log("Grounded layermask triggered");
            isGrounded = true;
            isJumping = false;
            isFalling = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !isAttacking)
        {
            Debug.Log("Grounded secondary layermask triggered");
            isGrounded = false;
        }
    }
}
