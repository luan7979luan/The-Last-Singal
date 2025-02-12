using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxMelee : MonoBehaviour
{
   public Healthmelee healthmelee;

    public void OnRayCastHit(RaycastWeapon weapon, Vector3 direction)
    {
        healthmelee.TakeDamage(weapon.damage, direction);
        
    }
}
