using UnityEngine;
using static PlayerStateController;

public class PlayerAttackingState : PlayerBaseState
{
    public PlayerAttackingState(PlayerStateController controller) : base(controller) { }

    public override void EnterState()
    {
        Debug.Log("Entering attacking state");        
        controller.isAttacking = true;
        controller.attackTimer = GetAttackDurationForCurrentStage(); // Set attack duration based on animation
        controller.attackTransitionTimer = controller.attackTimer + 1f; // Allow a transition window after the attack
        if (controller.currentAttackStage == BasicAttackStage.None) // If no attack stage is set, start from the beginning
        {
            SetupNextAttack();
        }
    }

    public override void UpdateState()
    {
        if (controller.attackTimer > 0)
        {
            controller.attackTimer -= Time.deltaTime;
        }

        if (controller.attackTransitionTimer > 0)
        {
            controller.attackTransitionTimer -= Time.deltaTime;
        }

        if (controller.attackCooldownTimer > 0)
        {
            controller.attackCooldownTimer -= Time.deltaTime;
        }

        if (controller.attackTransitionTimer <= 0)
        {
            ResetAttack(); // Reset if transition window has closed
        }


    }

    public override void FixedUpdateState()
    {
        controller.HandleMovement(); // Continue to allow horizontal movement well attacking
    }

    public override void ExitState()
    {
        Debug.Log("Exiting attacking state");
        controller.isAttacking = false;
        controller.currentAttackStage = BasicAttackStage.None;
    }

    private void SetupNextAttack()
    {
        if (controller.currentAttackStage < BasicAttackStage.Stage3)
        {
            controller.currentAttackStage++;            
            Debug.Log("Setting up next attack: " + controller.currentAttackStage);
            controller.attackTimer = GetAttackDurationForCurrentStage();
            controller.attackTransitionTimer = controller.attackTimer + 0.5f;
            controller.anim.Play(GetAnimationForCurrentStage());
            
            
        }
        else
        {
            ResetAttack();
        }
    }

    private void ResetAttack()
    {
        controller.currentAttackStage = BasicAttackStage.None;
        controller.TransitionToState(controller.idleState);
    }

    private string GetAnimationForCurrentStage()
    {
        switch (controller.currentAttackStage)
        {
            case BasicAttackStage.Stage1:
                return "PlayerAttack";
            case BasicAttackStage.Stage2:
                return "PlayerAttack2";
            case BasicAttackStage.Stage3:
                return "PlayerAttack3";
            default:
                return "PlayerIdle";
        }
    }

    private float GetAttackDurationForCurrentStage()
    {
        switch (controller.currentAttackStage)
        {
            case BasicAttackStage.Stage1:

                return 0.3f; // Duration of PlayerAttack animation
            case BasicAttackStage.Stage2:
                return 0.3f; // Duration of PlayerAttack2 animation
            case BasicAttackStage.Stage3:
                return 0.32f; // Duration of PlayerAttack3 animation
            default:
                return 0f;
        }
    }


    public void TrySetupNextAttack()
    {
        if (controller.attackTransitionTimer > 0 && controller.attackTimer <= 0 && controller.attackCooldownTimer <= 0)
        {
            Debug.Log("Trying to setup next attack");
            controller.attackCooldownTimer = 0.17f;
            SetupNextAttack();
        }
    }
}
