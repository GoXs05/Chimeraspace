using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IPlayerDamageable
{
    private const float kArmorMultiplier = 0.01f;
    private const float kMaxRegenHealth = PlayerData.kMaxHealth * 0.75f;
    private const float kMaxRegenShields = PlayerData.kMaxShields * 0.75f;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private UIBarScript playerHealthBar;
    [SerializeField] private UIBarScript playerShieldsBar;

    private float timeSinceHealthDamageTaken;
    private float timeSinceShieldsDamageTaken;


    public void TakeDamage(float damage) 
    {
        float armoredDamage = damage - (damage * playerData.GetArmor() * kArmorMultiplier);
        

        if (playerData.GetShields() <= 0f) 
        {
            playerData.SetHealth(playerData.GetHealth() - armoredDamage);
            playerHealthBar.UpdateUIBar(PlayerData.kMaxHealth, playerData.GetHealth());
            timeSinceHealthDamageTaken = 0f;
            timeSinceShieldsDamageTaken = 0f;
        } 
        else if (playerData.GetShields() >= damage)
        {
            playerData.SetShields(playerData.GetShields() - damage);
            playerShieldsBar.UpdateUIBar(PlayerData.kMaxShields, playerData.GetShields());
            timeSinceShieldsDamageTaken = 0f;
        }
        else
        {
            float healthDamage = (damage - playerData.GetShields()) - ((damage - playerData.GetShields()) * playerData.GetArmor() * kArmorMultiplier);
            playerData.SetHealth(playerData.GetHealth() - healthDamage);
            playerData.SetShields(0);

            timeSinceHealthDamageTaken = 0f;
            timeSinceShieldsDamageTaken = 0f;

            playerHealthBar.UpdateUIBar(PlayerData.kMaxHealth, playerData.GetHealth());
            playerShieldsBar.UpdateUIBar(PlayerData.kMaxShields, playerData.GetShields());
        }
    }
    public void RegenHealth()
    {
        Debug.Log("regenerating health: " + playerData.GetHealthRegenSpeed());

        float regenSpeedFrame = playerData.GetHealthRegenSpeed() * Time.deltaTime;

        if ((regenSpeedFrame + playerData.GetHealth()) < kMaxRegenHealth) 
        {
            playerData.SetHealth(regenSpeedFrame + playerData.GetHealth());
        }
        else
        {
            playerData.SetHealth(kMaxRegenHealth);
        }

        playerHealthBar.UpdateUIBar(PlayerData.kMaxHealth, playerData.GetHealth());
    }

    public void RegenShields()
    {
        Debug.Log("regenerating shields: " + playerData.GetShieldsRegenSpeed());
        float regenSpeedFrame = playerData.GetShieldsRegenSpeed() * Time.deltaTime;

        if ((regenSpeedFrame + playerData.GetShields()) < kMaxRegenShields) 
        {
            playerData.SetShields(regenSpeedFrame + playerData.GetShields());
        }
        else
        {
            playerData.SetShields(kMaxRegenShields);
        }

        playerShieldsBar.UpdateUIBar(PlayerData.kMaxShields, playerData.GetShields());
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            TakeDamage(200f);
        }
        timeSinceHealthDamageTaken += Time.deltaTime;
        timeSinceShieldsDamageTaken += Time.deltaTime;

        if (timeSinceHealthDamageTaken > playerData.GetRegenTime() && NeedToRegenHealth()) 
        {
            RegenHealth();
        }
        if (timeSinceShieldsDamageTaken > playerData.GetRegenTime() && NeedToRegenShields())
        {
            RegenShields();
        }
    }

    private bool NeedToRegenHealth() 
    {
        if (playerData.GetHealth() < kMaxRegenHealth) return true;
        return false;
    }

    private bool NeedToRegenShields()
    {
        if (playerData.GetShields() < kMaxRegenShields) return true;
        return false;
    }

    private void Start()
    {
        playerData.SetHealth(PlayerData.kMaxHealth);
        playerData.SetShields(PlayerData.kMaxShields);

        playerHealthBar.UpdateUIBar(PlayerData.kMaxHealth, playerData.GetHealth());
        playerShieldsBar.UpdateUIBar(PlayerData.kMaxShields, playerData.GetShields());

        timeSinceHealthDamageTaken = 0f;
        timeSinceShieldsDamageTaken = 0f;
    }
}
