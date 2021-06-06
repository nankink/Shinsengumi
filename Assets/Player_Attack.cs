using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShinTools;
using System;

public class Player_Attack : MonoBehaviour
{
    public bool canAttack;

    Player_Brain player_Brain;
    public MeleeWeapon meleeWeapon;

    protected bool m_InAttack;
    protected bool m_InCombo;
    protected Damageable m_Damageable;

    protected Collider[] m_OverlapResult = new Collider[0];

    // ANIMATOR STUFF
    protected AnimatorStateInfo m_CurrentStateInfo;
    protected AnimatorStateInfo m_NextStateInfo;
    protected AnimatorStateInfo m_PreviousCurrentStateInfo;
    protected AnimatorStateInfo m_PreviousNextStateInfo;
    protected bool m_PreviousIsAnimatorTransitioning;
    protected bool m_IsAnimatorTransitioning;
    
    readonly int m_HashMeleeAttack = Animator.StringToHash("");
    
    readonly int m_HashBasicCombo1 = Animator.StringToHash("");
    readonly int m_HashBasicCombo2 = Animator.StringToHash("");
    readonly int m_HashBasicCombo3 = Animator.StringToHash("");
    
    readonly int m_HashBlockInput = Animator.StringToHash("");


    public void SetCanAttack(bool canAttack)
    {
        this.canAttack = canAttack;
    }

    void Reset()
    {
        meleeWeapon = GetComponentInChildren<MeleeWeapon>();
    }

    private void Awake()
    {
        meleeWeapon.SetOwner(gameObject);   
    
        player_Brain = GetComponent<Player_Brain>();
    }

    private void OnEnable()
    {
        m_Damageable = GetComponent<Damageable>();
        m_Damageable.onDamageMessageReceivers.Add(this);
        m_Damageable.isInvulnerable = true;
    }

    private void OnDisable()
    {
        m_Damageable.onDamageMessageReceivers.Remove(this);
    }

    // i dont think this is needed for my implementation
    bool IsWeaponEquiped()
    {
        return false;
    }

    public void EquipMeleeWeapon(bool equip)
    {
        meleeWeapon.enabled = equip;
        m_InAttack = false;
        m_InCombo = equip;
   
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
    }

    void CacheAnimatorState()
    {
        m_PreviousCurrentStateInfo = m_CurrentStateInfo;
        m_PreviousNextStateInfo = m_NextStateInfo;
        m_PreviousIsAnimatorTransitioning = m_IsAnimatorTransitioning;

        m_CurrentStateInfo = player_Brain.b_Animator.GetCurrentAnimatorStateInfo(0);
        m_NextStateInfo = player_Brain.b_Animator.GetNextAnimatorStateInfo(0);
        m_IsAnimatorTransitioning = player_Brain.b_Animator.IsInTransition(0);
    }

    void UpdateInputBlocking()
    {
        bool inputBlocked = m_CurrentStateInfo.tagHash == m_HashBlockInput && !m_IsAnimatorTransitioning;
        inputBlocked |= m_NextStateInfo.tagHash == m_HashBlockInput;
        player_Brain.PlayerInput.playerInputBlocked = inputBlocked;
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
        m_Damageable.isInvulnerable = true;
    }
}
