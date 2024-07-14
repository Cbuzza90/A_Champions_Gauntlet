using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSpellManager : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private PlayerStateController playerController;
    public SpellScriptableObject[] spellSlots = new SpellScriptableObject[4];
    private int selectedSlot = -1;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.SelectSpell1.performed += _ => SelectSpellSlot(0);
        inputActions.Player.SelectSpell2.performed += _ => SelectSpellSlot(1);
        inputActions.Player.SelectSpell3.performed += _ => SelectSpellSlot(2);
        inputActions.Player.SelectSpell4.performed += _ => SelectSpellSlot(3);

        inputActions.Player.Cast.performed += _ => CastSelectedSpell();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void SelectSpellSlot(int slotIndex)
    {
        selectedSlot = slotIndex;
        UpdateSelectedSlotUI();
        Debug.Log("Selected slot: " + selectedSlot);
    }

    private void CastSelectedSpell()
    {
        if (selectedSlot >= 0 && spellSlots[selectedSlot] != null)
        {
            CastSpell(spellSlots[selectedSlot]);
        }
    }

    private void CastSpell(SpellScriptableObject spell)
    {
        Debug.Log("Casting spell: " + spell.spellName);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;
        Vector3 direction = (mousePosition - transform.position).normalized;

        if (spell.spellName == "FrostBoomerang")
        {
            if (playerController.FrostBoomerangeCharges > 0)
            {
                FrostBoomerang.CastBoomerang(spell, transform.position, transform);
                playerController.FrostBoomerangeCharges--; // Decrement charges here
            }
        }
        else
        {
            GameObject spellPrefab = Instantiate(spell.spellPrefab, transform.position, Quaternion.identity);
            spellPrefab.transform.right = direction; // Directly apply direction to all spells

            Fireball fireball = spellPrefab.GetComponent<Fireball>();
            if (fireball != null)
            {
                fireball.spellData = spell;
            }

            GlacialSpike glacialSpike = spellPrefab.GetComponent<GlacialSpike>();
            if (glacialSpike != null)
            {
                glacialSpike.spellData = spell;
                glacialSpike.shootDirection = direction;
            }

            PoisonSpear poisonSpear = spellPrefab.GetComponent<PoisonSpear>();
            if (poisonSpear != null)
            {
                poisonSpear.spellData = spell;
            }

            FlipCharacter(direction.x);
        }
    }

    private void FlipCharacter(float directionX)
    {
        Vector3 characterScale = transform.localScale;
        if (directionX > 0 && characterScale.x < 0 || directionX < 0 && characterScale.x > 0)
        {
            characterScale.x *= -1;
        }
        transform.localScale = characterScale;
    }

    public void BindSpellToSlot(SpellScriptableObject spell, int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < spellSlots.Length)
        {
            spellSlots[slotIndex] = spell;
            UpdateSpellSlotUI(slotIndex, spell);
            Debug.Log("Bound " + spell.spellName + " to slot " + slotIndex);
        }
    }

    private void UpdateSpellSlotUI(int slotIndex, SpellScriptableObject spell)
    {
        Transform spellSlotPanel = GameObject.Find("SpellSlotsPanel").transform;
        Transform spellSlot = spellSlotPanel.GetChild(slotIndex);

        Image slotImage = spellSlot.GetComponent<Image>();
        if (slotImage != null)
        {
            slotImage.sprite = spell.icon;
        }
    }

    private void UpdateSelectedSlotUI()
    {
        Transform spellSlotPanel = GameObject.Find("SpellSlotsPanel").transform;

        for (int i = 0; i < spellSlotPanel.childCount; i++)
        {
            Image slotImage = spellSlotPanel.GetChild(i).GetComponent<Image>();
            if (slotImage != null)
            {
                slotImage.color = (i == selectedSlot) ? Color.green : Color.white;
            }
        }
    }
}
