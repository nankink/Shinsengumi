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

    public Transform somewhere;

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
   //     ExtraRotation();

        if(navMeshAgent.pathPending) animator.SetFloat("Velocity", 1f);
        else animator.SetFloat("Velocity", 0f);

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
        Vector3 lookRotation = navMeshAgent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookRotation), extraRotationSpeed * Time.deltaTime);
    }

}
