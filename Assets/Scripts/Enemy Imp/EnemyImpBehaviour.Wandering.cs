using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MonsterLove.StateMachine;

public partial class EnemyImpBehaviour : MonoBehaviour
{
    [Header("Wander")]
    public float minDelay = 1f;
    public float maxDelay = 2f;
    public float wanderRadius = 10f;
    public float maxDeviation = 10f;
    Vector3 originPos;

    float timer;
    float wanderTime = 8f;

    void Wandering_Enter()
    {
        Debug.Log("wandering entered");
        originPos = transform.position;
        timer = wanderTime;

    }

    void Wandering_Update()
    {
        timer += Time.deltaTime;

        if(timer >= wanderTime)
        {
            Vector3 newPos = RandomNavCircle(transform.position, wanderRadius);
            navMeshAgent.SetDestination(newPos);
            timer = 0;
        }
    
    }

    void Wandering_Exit()
    {

    }

    public Vector3 RandomNavCircle(Vector3 origin, float dist) 
    {
        Vector3 randTarget = Random.insideUnitCircle * dist;
    
        randTarget += originPos;
 
        NavMeshHit navHit;
        NavMesh.SamplePosition (randTarget, out navHit, dist, -1);
 
        return navHit.position;
    }
}
