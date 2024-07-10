using UnityEngine;

public class PlayerSlidingState : PlayerBaseState
{
    private float slideTimer;

    public PlayerSlidingState(PlayerStateController controller) : base(controller) { }

    public override void EnterState()
    {
        controller.anim.Play("PlayerSlide");
        controller.isSliding = true;
        controller.isImmune = true;
        controller.spriteRenderer.color = controller.immuneColor;
        controller.rb.velocity = new Vector2(controller.transform.localScale.x * controller.slideSpeed, controller.rb.velocity.y);
        slideTimer = controller.slideDuration;
    }

    public override void UpdateState()
    {
        if (slideTimer <= 0 || !controller.isSliding)
        {
            controller.TransitionToState(controller.isGrounded ? (controller.moveDirection != Vector2.zero ? controller.runningState : controller.idleState) : controller.fallingState);
        }
    }

    public override void FixedUpdateState()
    {
        slideTimer -= Time.fixedDeltaTime;
    }

    public override void ExitState()
    {
        controller.isSliding = false;
        controller.isImmune = false;
        controller.spriteRenderer.color = controller.normalColor;
        controller.slideCooldownTimer = controller.slideCooldown; // Reset the slide cooldown
    }
}
