using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateController controller) : base(controller) { }

    public override void EnterState()
    {
        controller.anim.Play("PlayerIdle");
    }

    public override void UpdateState()
    {
        if (controller.moveDirection != Vector2.zero)
        {
            controller.TransitionToState(controller.runningState);
        }
        if (!controller.isGrounded)
        {
            controller.TransitionToState(controller.fallingState);
        }
    }

    public override void FixedUpdateState() { }

    public override void ExitState() { }
}
