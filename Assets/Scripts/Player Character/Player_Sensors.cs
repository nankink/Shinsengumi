using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sensors : MonoBehaviour
{
    LayerMask groundLayer;
    [Range(0.5f, 5f)] public float maxDistance = 1f;
    public Transform[] sensorPoints;

    void Start()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    public void BodySensoring()
    {
        for (int i = 0; i < sensorPoints.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(sensorPoints[i].position, Vector3.forward, out hit, maxDistance, groundLayer))
            {

            }
        }
    }
}
