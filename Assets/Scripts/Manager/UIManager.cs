using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject spellSelectionPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleSpellSelectionPanel();
        }
    }

    void ToggleSpellSelectionPanel()
    {
        if (spellSelectionPanel != null)
        {
            spellSelectionPanel.SetActive(!spellSelectionPanel.activeSelf);
        }
    }
}
