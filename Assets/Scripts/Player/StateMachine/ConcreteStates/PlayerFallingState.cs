using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    public PlayerFallingState(PlayerStateController controller) : base(controller) { }

    public override void EnterState()
    {
        controller.anim.Play("PlayerFalling");
    }

    public override void UpdateState()
    {
        if (controller.isGrounded)
        {
            if (controller.moveDirection != Vector2.zero)
            {
                controller.TransitionToState(controller.runningState);
            }
            else
            {
                controller.TransitionToState(controller.idleState);
            }
        }
    }

    public override void FixedUpdateState()
    {
        controller.HandleMovement(); // Maintain horizontal movement control
    }

    public override void ExitState()
    {
        // Any cleanup if needed
    }
}
