using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ShinTools;
using UnityEngine.Serialization;

public partial class Damageable : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth { get; set; }

    public float invulnerabiltyTime;
    public bool isInvulnerable { get; set; }

    [Tooltip("Angle from which the damageable is hitable")]
    [Range(0f, 360f)]
    public float hitAngle = 360f;

    [Tooltip("Allow to rotate the world forward vector of the damageable used to define the hitAngle zone")]
    [Range(0f, 360f)]
    public float hitForwardRotation = 360f;

    public UnityEvent OnDeath, OnReceiveDamage, OnHitWhileInvunerable, OnBecomeInvunerable, OnResetDamage;

    [Tooltip("When this gameObject is damaged, these other gameObjects are notified")]
    [EnforceType(typeof(Message.IMessageReceiver))]
    public List<MonoBehaviour> onDamageMessageReceivers;

    protected float m_timeSinceLastHit = 0f;
    protected Collider m_Collider;

    System.Action schedule;

    private void Start()
    {
        //ResetDamage();
        m_Collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (isInvulnerable)
        {
            m_timeSinceLastHit += Time.deltaTime;
            if (m_timeSinceLastHit > invulnerabiltyTime)
            {
                m_timeSinceLastHit = 0;
                isInvulnerable = false;
                OnBecomeInvunerable.Invoke();
            }
        }
    }

    public void ResetDamage()
    {
        currentHealth = maxHealth;
        isInvulnerable = false;
        m_timeSinceLastHit = 0f;
        OnResetDamage.Invoke();
    }

    public void SetColliderState(bool enabled)
    {
        m_Collider.enabled = enabled;
    }

    public void ApplyDamage(DamageMessage data)
    {
        if (currentHealth <= 0)
        {
            // ignore damage if ded.
            // may have to change that if we want to detect hit on death
            return;
        }

        if (isInvulnerable)
        {
            OnHitWhileInvunerable.Invoke();
            return;
        }

        Vector3 forward = transform.forward;
        forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

        Vector3 positionToDamager = data.damageSource - transform.position;
        positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);

        if (Vector3.Angle(forward, positionToDamager) > hitAngle * 0.5f)
            return;

        isInvulnerable = true;
        currentHealth -= data.amount;

        if (currentHealth <= 0)
        {
            schedule += OnDeath.Invoke;
        }
        else
        {
            OnReceiveDamage.Invoke();
        }

        var messageType = currentHealth <= 0 ? Message.MessageType.DEAD : Message.MessageType.DAMAGED;

        for (var i = 0; i < onDamageMessageReceivers.Count; i++)
        {
            var receiver = onDamageMessageReceivers[i] as Message.IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
    }

    private void LateUpdate()
    {
        if (schedule != null)
        {
            schedule();
            schedule = null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 forward = transform.forward;
        forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

        if (Event.current.type == EventType.Repaint)
        {
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(forward), 1.0f,
                EventType.Repaint);
        }


        UnityEditor.Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.1f);
        forward = Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * forward;
        UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, forward, hitAngle, 1.0f);
    }
#endif

}
