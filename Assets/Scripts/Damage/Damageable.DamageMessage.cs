using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Damageable : MonoBehaviour
{
    public struct DamageMessage
    {
        public MonoBehaviour damager;
        public float amount;
        public Vector3 direction;
        public Vector3 damageSource;
        public bool throwing;

        public bool stopCamera;
    }
}