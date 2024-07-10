using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float attackTimer;

    public PlayerAttackingState(PlayerStateController controller) : base(controller) { }

    public override void EnterState()
    {
        controller.anim.Play("PlayerAttack");
        controller.isAttacking = true;
        attackTimer = controller.attackCooldownTime;
    }

    public override void UpdateState()
    {
        if (attackTimer <= 0)
        {
            controller.isAttacking = false;
            controller.TransitionToState(controller.idleState); // Or another appropriate state
        }
    }

    public override void FixedUpdateState()
    {
        attackTimer -= Time.fixedDeltaTime;
    }

    public override void ExitState()
    {
        controller.isAttacking = false;
    }
}
