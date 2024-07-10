using UnityEngine;

public class PlayerRunningState : PlayerBaseState
{
    public PlayerRunningState(PlayerStateController controller) : base(controller) { }

    public override void EnterState()
    {
        controller.anim.Play("PlayerRun");
    }

    public override void UpdateState()
    {
        if (controller.moveDirection == Vector2.zero)
        {
            controller.TransitionToState(controller.idleState);
        }
        if (!controller.isGrounded)
        {
            controller.TransitionToState(controller.fallingState);
        }
    }

    public override void FixedUpdateState()
    {
        controller.HandleMovement();
    }

    public override void ExitState() { }
}
