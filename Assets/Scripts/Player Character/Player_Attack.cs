using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShinTools;
using System;

public class Player_Attack : MonoBehaviour, IHasCooldown, Message.IMessageReceiver
{
    public bool canAttack;
    public bool isSheathed;

    // Cooldown
    int atkId = 2;
    public int Id => atkId;
    public float CooldownDuration {get; set;}


    public MeleeWeapon meleeWeapon;
    public float damage = 3;
    public float baseDamage;
    protected bool m_InAttack;
    protected bool m_InCombo;
    Damageable m_Damageable;

    protected Collider[] m_OverlapResult = new Collider[0];

    public void SetCanAttack(bool canAttack)
    {
        this.canAttack = canAttack;
    }

    private void Start()
    {
        meleeWeapon.SetOwner(gameObject);   

        m_Damageable = GetComponent<Damageable>();
        m_Damageable.onDamageMessageReceivers.Add(this);
    }

    private void OnEnable()
    {
        m_Damageable = GetComponent<Damageable>();
        m_Damageable.onDamageMessageReceivers.Add(this);
    }

    private void OnDisable()
    {
        m_Damageable.onDamageMessageReceivers.Remove(this);
    }

    public void EquipMeleeWeapon(bool equip)
    {
        meleeWeapon.enabled = equip;
        m_InAttack = false;
        m_InCombo = equip;

        if(damage >= baseDamage + 5f)
        {
        meleeWeapon.Damage = damage;
        }
        else meleeWeapon.Damage = baseDamage;
    }

    public void MeleeAttackStart(int throwing = 0)
    {
        meleeWeapon.BeginAttack(throwing != 0);
        m_InAttack = true;
    }

    public void MeleeAttackEnd()
    {
        meleeWeapon.EndAttack();
        m_InAttack = false;
        meleeWeapon.Damage = baseDamage;
    }

    public void OnReceiveMessage(Message.MessageType type, object sender, object data)
    {
        switch(type)
        {
            case Message.MessageType.DAMAGED:
                {
                Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
                Damaged(damageData);
                }
                break;
            case Message.MessageType.DEAD:
                {
                Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
                Die(damageData);
                }
                break;
        }
    }

    void Damaged(Damageable.DamageMessage damageMessage)
    {
        // localHurt direction of the damage;
        Vector3 forward = damageMessage.damageSource - transform.position;
        forward.y = 0f;
        Vector3 localHurt = transform.InverseTransformDirection(forward);
    
    }

    void Die(Damageable.DamageMessage damageMessage)
    {
        m_Damageable.isInvulnerableFromDamage = true;
    }
}
