using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private PlayerStateController stateController;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        stateController = GetComponent<PlayerStateController>();

        // Movement input
        playerInputActions.Player.Move.performed += ctx => {
            Vector2 input = ctx.ReadValue<Vector2>();
            //Debug.Log("Move input: " + input);
            stateController.SetMoveDirection(input);
        };
        playerInputActions.Player.Move.canceled += ctx => {
            //Debug.Log("Move input canceled");
            stateController.SetMoveDirection(Vector2.zero);
        };

        // Jump input
        playerInputActions.Player.Jump.performed += ctx => {
            Debug.Log("Jump performed");
            stateController.Jump();
        };

        // Attack input
        playerInputActions.Player.Attack.performed += ctx => stateController.Attack();

        // Slide input
        playerInputActions.Player.Slide.performed += ctx => stateController.Slide();

        // Cast input
        playerInputActions.Player.Cast.performed += ctx => stateController.Cast();

        playerInputActions.Player.Enable();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

}
