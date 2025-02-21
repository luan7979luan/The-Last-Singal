using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxNPC2 : MonoBehaviour
{
    public HealthNPC2 healthNPC2;

    public void OnRayCastHit(RaycastWeapon weapon, Vector3 direction)
    {
        healthNPC2.TakeDamage(weapon.damage, direction);

    }
}
