using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SpellIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SpellScriptableObject spell;
    private bool isHovered;
    private PlayerSpellManager player;
    private Image iconImage;

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.SelectSpell1.performed += _ => TryBindSpellToSlot(0);
        inputActions.Player.SelectSpell2.performed += _ => TryBindSpellToSlot(1);
        inputActions.Player.SelectSpell3.performed += _ => TryBindSpellToSlot(2);
        inputActions.Player.SelectSpell4.performed += _ => TryBindSpellToSlot(3);
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Start()
    {
        player = FindObjectOfType<PlayerSpellManager>();
        if (player == null)
        {
            Debug.LogError("Player component not found in the scene.");
        }

        iconImage = GetComponent<Image>();
        if (iconImage == null)
        {
            Debug.LogError("Image component not found on SpellIcon.");
        }
    }

    void TryBindSpellToSlot(int slotIndex)
    {
        if (isHovered)
        {
            if (player != null)
            {
                player.BindSpellToSlot(spell, slotIndex);
            }
            else
            {
                Debug.LogError("Player component not assigned.");
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        // Add hover effect
        if (iconImage != null)
        {
            iconImage.color = Color.red; // Change color to red or any other effect
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        // Remove hover effect
        if (iconImage != null)
        {
            iconImage.color = Color.white; // Change color back to white or original color
        }
    }
}
