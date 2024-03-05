using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDamageable
{
    public void TakeDamage(float damage);
    public void RegenHealth();
    public void RegenShields();
}
