using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPickup : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float shields;
    private void OnTriggerEnter(Collider collision)
    {
        IPlayerDamageable playerDamageable = collision.transform.GetComponent<IPlayerDamageable>();
        
        if (playerDamageable != null)
        {            
            playerDamageable.AddHealth(health);
            playerDamageable.AddShields(shields);
            Destroy(this.gameObject);
        }
    }
}
