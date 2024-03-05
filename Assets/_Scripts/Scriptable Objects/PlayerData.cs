using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="PlayerData", menuName="Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    public const int kMaxHealth = 1000;
    public const int kMaxShields = 2000;
    public const int kMaxArmor = 99;

    [Header("HP")]
    [SerializeField] [Range(0, kMaxHealth)] private float health;
    [SerializeField] [Range(0, kMaxShields)] private float shields;
    [SerializeField] [Range(0, kMaxArmor)] private int armor;

    [Header("Regen")]
    [SerializeField] private int regenTime;
    [SerializeField] private int healthRegenSpeed;
    [SerializeField] private int shieldsRegenSpeed;


    public float GetHealth() { return health; }
    public void SetHealth(float newHealth) { health = newHealth; }

    public float GetShields() { return shields; }
    public void SetShields(float newShields) { shields = newShields; }

    public int GetArmor() { return armor; }
    public void SetArmor(int newArmor) { armor = newArmor; }

    public int GetRegenTime() { return regenTime; }
    public void SetRegenTime(int newRegenTime) { regenTime = newRegenTime; }

    public int GetHealthRegenSpeed() { return healthRegenSpeed; }
    public void SetHealthRegenSpeed(int newHealthRegenSpeed) { healthRegenSpeed = newHealthRegenSpeed; }

    public int GetShieldsRegenSpeed() { return shieldsRegenSpeed; }
    public void SetShieldsRegenSpeed(int newShieldsRegenSpeed) { shieldsRegenSpeed = newShieldsRegenSpeed; }


}
