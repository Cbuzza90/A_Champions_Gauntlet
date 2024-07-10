using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSpellManager : MonoBehaviour
{
    public SpellScriptableObject[] spellSlots = new SpellScriptableObject[4];
    private int selectedSlot = -1;


    private void Start()
    {
    }

    void Update()
    {
        HandleSpellSelection();
        HandleSpellCasting();
    }

    private void FixedUpdate()
    {
        
    }
    

    void HandleSpellSelection()
    {
         /* 
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSpellSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSpellSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSpellSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSpellSlot(3);
         */
    }

    void SelectSpellSlot(int slotIndex)
    {
        selectedSlot = slotIndex;
        UpdateSelectedSlotUI();
    }

    void HandleSpellCasting()
    {
        /*
         * if (Input.GetMouseButtonDown(1) && selectedSlot >= 0 && spellSlots[selectedSlot] != null)
        {
            CastSpell(spellSlots[selectedSlot]);
        }
        */
    }

    void CastSpell(SpellScriptableObject spell)
    {
        Debug.Log("Casting a spell");
        //if (spell.spellPrefab != null)
        {
        //    Instantiate(spell.spellPrefab, transform.position, transform.rotation);
        }
    }

    public void BindSpellToSlot(SpellScriptableObject spell, int slotIndex)
    {
        spellSlots[slotIndex] = spell;
        UpdateSpellSlotUI(slotIndex, spell);
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
        // Implement logic to visually indicate which slot is selected
        Transform spellSlotPanel = GameObject.Find("SpellSlotsPanel").transform;

        for (int i = 0; i < spellSlotPanel.childCount; i++)
        {
            Image slotImage = spellSlotPanel.GetChild(i).GetComponent<Image>();
            if (slotImage != null)
            {
                slotImage.color = (i == selectedSlot) ? Color.green : Color.white; // Change color to indicate selected slot
            }
        }
    }
}
