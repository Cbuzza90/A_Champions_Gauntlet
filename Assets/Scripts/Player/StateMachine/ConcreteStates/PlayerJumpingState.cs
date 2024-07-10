using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    public PlayerJumpingState(PlayerStateController controller) : base(controller) { }

    public override void EnterState()
    {
        controller.anim.Play("PlayerJump");
        controller.rb.velocity = new Vector2(controller.rb.velocity.x, controller.jumpForce);
        controller.isGrounded = false;
    }

    public override void UpdateState()
    {
        if (controller.rb.velocity.y <= 0) // Start falling when ascent is complete
        {
            controller.TransitionToState(controller.fallingState);
        }
    }

    public override void FixedUpdateState()
    {
        controller.HandleMovement(); // Continue to allow horizontal movement in the air
    }

    public override void ExitState()
    {
        // Any cleanup if needed
    }
}
