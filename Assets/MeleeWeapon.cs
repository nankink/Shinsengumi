using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeleeWeapon : MonoBehaviour
{
    public float damage = 1f;
    public LayerMask targetLayers;

    [System.Serializable]
    public class AttackPoint
    {
        public float radius;
        public Vector3 offset;
        public Transform attackRoot;

#if UNITY_EDITOR
        [NonSerialized] public List<Vector3> previousPositions = new List<Vector3>();
#endif
    }

    public AttackPoint[] attackPoints = new AttackPoint[0];

    protected bool mw_IsThrowingHit = false;
    protected bool mw_InAttack = false;
    public bool throwingHit
    {
        get { return mw_IsThrowingHit; }
        set { mw_IsThrowingHit = value; }
    }

    protected GameObject mw_Owner;

    protected Vector3[] mw_PreviousPos = null;
    protected Vector3 mw_Direction;

    protected static RaycastHit[] s_RaycastHitCache = new RaycastHit[32];
    protected static Collider[] s_ColliderCache = new Collider[32];

    public void SetOwner(GameObject owner)
    {
        mw_Owner = owner;
    }

    public void BeginAttack(bool throwingAttack)
    {
        throwingHit = throwingAttack;
        mw_InAttack = true;

        mw_PreviousPos = new Vector3[attackPoints.Length];
        for (int i = 0; i < attackPoints.Length; i++)
        {
            Vector3 worldPos = attackPoints[i].attackRoot.position +
                               attackPoints[i].attackRoot.TransformVector(attackPoints[i].offset);
            mw_PreviousPos[i] = worldPos;

#if UNITY_EDITOR
            attackPoints[i].previousPositions.Clear();
            attackPoints[i].previousPositions.Add(mw_PreviousPos[i]);
#endif
        }
    }

    public void EndAttack()
    {
        mw_InAttack = false;

#if UNITY_EDITOR
        for (int i = 0; i < attackPoints.Length; i++)
        {
            attackPoints[i].previousPositions.Clear();
        }
#endif
    }

    private void FixedUpdate()
    {
        if (mw_InAttack)
        {
            for (int i = 0; i < attackPoints.Length; i++)
            {
                AttackPoint pts = attackPoints[i];

                Vector3 worldPos = pts.attackRoot.position + pts.attackRoot.TransformVector(pts.offset);
                Vector3 attackVector = worldPos - mw_PreviousPos[i];

                if (attackVector.magnitude < 0.001f)
                {
                    attackVector = Vector3.forward * 0.0001f;
                }

                Ray r = new Ray(worldPos, attackVector.normalized);
                int contacts = Physics.SphereCastNonAlloc(r, pts.radius, s_RaycastHitCache, attackVector.magnitude, ~0, QueryTriggerInteraction.Ignore);
                for (int k = 0; k < contacts; k++)
                {
                    Collider col = s_RaycastHitCache[k].collider;
                    if (col != null)
                    {
                        CheckDamage(col, pts);
                    }

                    mw_PreviousPos[i] = worldPos;

#if UNITY_EDITOR
                    pts.previousPositions.Add(mw_PreviousPos[i]);
#endif
                }
            }
        }
    }

    bool CheckDamage(Collider other, AttackPoint pts)
    {
        Damageable d = other.GetComponent<Damageable>();
        if (d == null) return false;

        // Ignore self harm, but do not end the attack
        if (d.gameObject == mw_Owner)
            return true;

        // Hit object that not on OUR layer. this ends the attack.
        if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
            return false;


        Damageable.DamageMessage data;
        data.amount = damage;
        data.damager = this;
        data.direction = mw_Direction.normalized;
        data.damageSource = mw_Owner.transform.position;
        data.throwing = mw_IsThrowingHit;
        data.stopCamera = false;

        d.ApplyDamage(data);

        return true;
    }



#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < attackPoints.Length; ++i)
        {
            AttackPoint pts = attackPoints[i];

            if (pts.attackRoot != null)
            {
                Vector3 worldPos = pts.attackRoot.TransformVector(pts.offset);
                Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
                Gizmos.DrawSphere(pts.attackRoot.position + worldPos, pts.radius);
            }

            if (pts.previousPositions.Count > 1)
            {
                UnityEditor.Handles.DrawAAPolyLine(10, pts.previousPositions.ToArray());
            }
        }
    }

#endif

}
