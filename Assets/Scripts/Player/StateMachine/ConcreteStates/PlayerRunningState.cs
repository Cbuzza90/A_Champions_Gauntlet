using UnityEngine;

public class PlayerRunningState : PlayerBaseState
{
    public PlayerRunningState(PlayerStateController controller) : base(controller) { }

    public override void EnterState()
    {
        if (controller.isGrounded)
        {
            controller.anim.Play("PlayerRun");
        }
        
    }

    public override void UpdateState()
    {
        if (controller.moveDirection == Vector2.zero && controller.isGrounded)
        {
            Debug.Log("Transitioning to idle state from running state");
            controller.TransitionToState(controller.idleState);
        }
        if (!controller.isGrounded)
        {
            Debug.Log("Transitioning to falling state from running state");
            controller.TransitionToState(controller.fallingState);
        }
    }

    public override void FixedUpdateState()
    {
        controller.HandleMovement();
    }

    public override void ExitState() { }
}
