using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public GameObject skillPanel;
    private PlayerInputActions playerInputActions;
    private InputAction toggleSkillPanelAction;

    void Awake()
    {
        // Initialize the input system actions
        playerInputActions = new PlayerInputActions();

        // Subscribe to the toggle action event
        toggleSkillPanelAction = playerInputActions.UI.ToggleSkillPanel;
        toggleSkillPanelAction.Enable();
        toggleSkillPanelAction.performed += ToggleSkillPanel;
    }

    void OnDestroy()
    {
        // Cleanup - unsubscribe from the event
        toggleSkillPanelAction.Disable();
        toggleSkillPanelAction.performed -= ToggleSkillPanel;
    }

    private void ToggleSkillPanel(InputAction.CallbackContext context)
    {
        if (skillPanel != null)
        {
            skillPanel.SetActive(!skillPanel.activeSelf);
            Debug.Log("Skill Panel Toggled: " + skillPanel.activeSelf);
        }
    }
}
