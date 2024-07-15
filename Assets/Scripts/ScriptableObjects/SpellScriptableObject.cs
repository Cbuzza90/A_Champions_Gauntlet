using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]
public class SpellScriptableObject : ScriptableObject
{
    // Variables
    public float DamageAmount = 10f;
    public float LifeSpan = 5f;
    public float Speed = 10f;
    public float Cooldown = 5f;
    public float SpellRadius = 0.5f;
    public float glacialSpikeChargeTime = 2f; // Add this variable for charge time
    public GameObject spellPrefab;
    public Sprite icon;
    public string spellName;

    // Poison-specific properties
    public float PoisonDamage;
    public int PoisonTicks;
    public float PoisonInterval;

    // Fields specific to ChainLightning
    public int numberOfChainHits = 4;
    public float chainRadius = 5f;

    // Fields specific to ChargedBolt
    public int numberOfBolts = 6;
}
