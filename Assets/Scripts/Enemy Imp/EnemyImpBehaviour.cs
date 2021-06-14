using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MonsterLove.StateMachine;

public partial class EnemyImpBehaviour : MonoBehaviour
{
    public enum Enemy_ImpState
    {
        Idle,
        Wandering,
        Attacking,
        Kicking
    }

    StateMachine<Enemy_ImpState, StateDriverUnity> fsm;
    NavMeshAgent navMeshAgent;
    float extraRotationSpeed = 10f;
    public Animator animator;

    public LayerMask groundLayer;
    Vector3 lookRotation;

    public Transform somewhere;
    float slerp;

    bool targetSeeing = false;
    FieldOfView fieldOfView;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        fieldOfView = GetComponentInChildren<FieldOfView>();

        fsm = new StateMachine<Enemy_ImpState, StateDriverUnity>(this);
        fsm.ChangeState(Enemy_ImpState.Idle);
    }

    private void Update()
    {
        if(navMeshAgent.hasPath) 
        {
            ExtraRotation();
            
        animator.SetFloat("Velocity", 1);
        }
        else 
        {

        animator.SetFloat("Velocity", 0);
        }

        fsm.Driver.Update.Invoke();
    }

    void Idle_Enter()
    {
        fsm.ChangeState(Enemy_ImpState.Wandering);
    }

    void Idle_Update()
    {

    }

    void Idle_FixedUpdate()
    {
    }

    void Idle_Exit()
    {
    }

    void ExtraRotation()
    {
        lookRotation = navMeshAgent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookRotation), extraRotationSpeed * Time.deltaTime);
    }

}
