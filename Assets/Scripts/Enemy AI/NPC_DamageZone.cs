using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class NPC_DamageZone : MonoBehaviour
{
    private BoxCollider _damageCollider;
    public int damage;
    public string targetTag;
    private List<Collider> _damageTargetList;

    private void Awake()
    {
        _damageCollider = GetComponent<BoxCollider>();
        _damageCollider.isTrigger = true;
        _damageCollider.enabled = false;
        _damageTargetList = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag) && !_damageTargetList.Contains(other))
        {
            var targetCC = other.GetComponent<CharacterTakeDamage>();

            if (targetCC != null)
            {
                targetCC.TakeDamage(damage, transform.position);
            }
            _damageTargetList.Add(other);
        }
    }

    public void EnableDamageCaster()
    {
        _damageTargetList.Clear();
        _damageCollider.enabled = true;
    }

    public void DisableDamageCaster()
    {
        _damageTargetList.Clear();
        _damageCollider.enabled = false;
    }
}
