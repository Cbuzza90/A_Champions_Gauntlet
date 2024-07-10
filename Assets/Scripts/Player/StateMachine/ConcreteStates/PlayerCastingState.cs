using UnityEngine;

public class PlayerCastingState : PlayerBaseState
{
    public PlayerCastingState(PlayerStateController controller) : base(controller) { }

    public override void EnterState()
    {
        controller.anim.Play("PlayerCast");  // Ensure you have an animation named "PlayerCast"
        controller.isAttacking = true;  // Assuming casting is a type of attack
    }

    public override void UpdateState()
    {
        if (!controller.anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerCast"))
        {
            controller.TransitionToState(controller.idleState);  // Return to idle after casting
        }
    }

    public override void FixedUpdateState()
    {
        // Handle physics related updates if necessary during casting
    }

    public override void ExitState()
    {
        controller.isAttacking = false;
    }
}
