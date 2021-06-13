using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sensors : MonoBehaviour
{
    public LayerMask groundLayer;
    [Range(0f, 5f)] public float maxDistance;
    public Transform[] sensorPoints;
    bool[] contact;

    void Start()
    {
        contact = new bool[sensorPoints.Length];
  }

    private void Update()
    {
        BodySensoring();
    }

    public bool CheckContact()
    {
        for (int i = 0; i < contact.Length; i++)
        {
            if (!contact[i]) return false;
        }
        return true;
    }

    public void BodySensoring()
    {
        for (int i = 0; i < sensorPoints.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(sensorPoints[i].position, transform.right, out hit, maxDistance, groundLayer))
            {
                contact[i] = true;
            }
            else contact[i] = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < sensorPoints.Length; i++)
        {
            Gizmos.DrawLine(sensorPoints[i].position, sensorPoints[i].position + transform.right * maxDistance);
        }
    }

}
