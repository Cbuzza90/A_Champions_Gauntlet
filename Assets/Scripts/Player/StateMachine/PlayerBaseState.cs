using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateController controller;
    protected PlayerBaseState(PlayerStateController stateController)
    {
        controller = stateController;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
}
