using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBoss : MonoBehaviour
{
    public HealthBoss healthBoss;

    public void OnRayCastHit(RaycastWeapon weapon, Vector3 direction)
    {
        healthBoss.TakeDamage(weapon.damage, direction);

    }
}

