using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SpellScriptableObject spell;
    private bool isHovered;
    private PlayerSpellManager player;
    private Image iconImage;

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

    void Update()
    {
        if (isHovered)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (player != null) player.BindSpellToSlot(spell, 0);
                else Debug.LogError("Player component not assigned.");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (player != null) player.BindSpellToSlot(spell, 1);
                else Debug.LogError("Player component not assigned.");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (player != null) player.BindSpellToSlot(spell, 2);
                else Debug.LogError("Player component not assigned.");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (player != null) player.BindSpellToSlot(spell, 3);
                else Debug.LogError("Player component not assigned.");
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
