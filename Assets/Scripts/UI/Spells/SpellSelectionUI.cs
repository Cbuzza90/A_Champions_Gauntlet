using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellSelectionUI : MonoBehaviour
{
    public GameObject spellIconTemplate;
    public SpellScriptableObject[] availableSpells;
    public GameObject spellSelectionPanel; // Reference to the spell selection panel

    void Start()
    {
        PopulateSpells();
        spellSelectionPanel.SetActive(false); // Ensure the panel starts hidden
    }

    void PopulateSpells()
    {
        foreach (SpellScriptableObject spell in availableSpells)
        {
            GameObject spellIcon = Instantiate(spellIconTemplate, spellSelectionPanel.transform);
            if (spellIcon == null)
            {
                Debug.LogError("Failed to instantiate spellIconTemplate.");
                return;
            }

            Image iconImage = spellIcon.GetComponent<Image>();
            if (iconImage == null)
            {
                Debug.LogError("SpellIconTemplate is missing an Image component.");
                return;
            }

            SpellIcon spellIconScript = spellIcon.GetComponent<SpellIcon>();
            if (spellIconScript == null)
            {
                Debug.LogError("SpellIconTemplate is missing the SpellIcon script.");
                return;
            }

            iconImage.sprite = spell.icon;
            spellIconScript.spell = spell;

            TextMeshProUGUI spellText = spellIcon.GetComponentInChildren<TextMeshProUGUI>();
            if (spellText != null)
            {
                spellText.text = spell.spellName;
            }
        }

        if (spellIconTemplate != null)
        {
            spellIconTemplate.SetActive(false);
        }
    }
}
