using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Brain : MonoBehaviour
{
    #region State machine and states
    public StateMachine stateMachineBrain;

    public GroundState groundState;
    public StandState standing;
    public RollState rolling;
    public CrouchState crouching;
    public JumpState jumping;
    public FallState falling;
    #endregion

    #region Character Classes
    [HideInInspector]public Player_Movement Movement;
    [HideInInspector]public CooldownSystem cooldownSystem;
    #endregion

    public Text currentstate;

    public Animator b_Animator;
    public Collider b_BodyCollider;
    public Collider b_HeadCollider;
    public SkinnedMeshRenderer mesh;

    [HideInInspector]public Rigidbody b_Rigidbody;
    [HideInInspector]public Material meshMaterial;
    [HideInInspector]public CustomGravity customGravity;



    void Start()
    {
        stateMachineBrain = new StateMachine();
        groundState = new GroundState(this, stateMachineBrain);
        standing = new StandState(this, stateMachineBrain);
        rolling = new RollState(this, stateMachineBrain);
        crouching = new CrouchState(this, stateMachineBrain);
        jumping = new JumpState(this, stateMachineBrain);
        falling = new FallState(this, stateMachineBrain);

        Movement = GetComponent<Player_Movement>();
        b_Rigidbody = GetComponent<Rigidbody>();
        customGravity = GetComponent<CustomGravity>();

        cooldownSystem = GetComponent<CooldownSystem>();
        meshMaterial = mesh.material;

        stateMachineBrain.Initialize(standing);
    }

    private void Update()
    {
        stateMachineBrain.CurrentState.LogicUpdate();
        stateMachineBrain.CurrentState.HandleInput();

        if(Input.GetKey(KeyCode.Y)) b_Animator.SetTrigger("Attack");
    }

    private void FixedUpdate()
    {
        stateMachineBrain.CurrentState.PhysicsUpdate();
    }


}
