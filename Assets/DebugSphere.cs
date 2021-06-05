using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSphere : MonoBehaviour
{
    public Color colorSphere;
    public float radius = 0.2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = colorSphere;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
