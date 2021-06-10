using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public float damage;
    public LayerMask damageLayers;

    private void OnTriggerStay(Collider other)
    {
        if((damageLayers.value & 1 << other.gameObject.layer) == 0)
        return;

        Damageable d = other.GetComponentInChildren<Damageable>();
        if(d!= null && !d.isInvulnerableFromDamage)
        {
            Damageable.DamageMessage message = new Damageable.DamageMessage
            {
                damageSource = transform.position,
                damager = this,
                amount = damage,
                direction = (other.transform.position - transform.position).normalized,
                throwing = false
            };
            d.ApplyDamage(message);
        }
    }


}
