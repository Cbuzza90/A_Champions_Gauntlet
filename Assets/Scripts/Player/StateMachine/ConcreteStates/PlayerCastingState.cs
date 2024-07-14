using UnityEngine;

public class PlayerCastingState : PlayerBaseState
{
    public PlayerCastingState(PlayerStateController controller) : base(controller) { }

    public override void EnterState()
    {
        controller.anim.Play("PlayerCast");  // Ensure you have an animation named "PlayerCast"
        controller.isCasting = true;  // Set isCasting to true
    }

    public override void UpdateState()
    {
        // Check if the casting animation is done
        AnimatorStateInfo stateInfo = controller.anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("PlayerCast") && stateInfo.normalizedTime >= 1f)
        {
            controller.TransitionToState(controller.idleState);
        }
    }

    public override void FixedUpdateState()
    {
        // Handle physics-related updates if necessary during casting
    }

    public override void ExitState()
    {
        controller.isCasting = false;
    }
}
