using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Brain : MonoBehaviour
{
    protected static Player_Brain pb_instance;
    public static Player_Brain instance { get {return pb_instance;}}

    #region State machine and states
    public StateMachine stateMachineBrain;

    public GroundState groundState;
    public StandState standing;
    public RollState rolling;
    public CrouchState crouching;
    public JumpState jumping;
    public FallState falling;
    public ImposeState imposing;

    public BasicCombo1_State atk_1;
    public BasicCombo2_State atk_2;
    public BasicCombo3_State atk_3;
    public BasicCombo4_State atk_4;
    public IaiState iaiPrepping;
    public ForwardSlashState iaiSlashing;

    #endregion

    #region Character Classes
    [HideInInspector]public Player_Movement Movement;
    [HideInInspector]public Player_Input PlayerInput;
    [HideInInspector]public Damageable Health;
    [HideInInspector]public Player_Attack Attack;
    [HideInInspector]public CooldownSystem cooldownSystem;
    [HideInInspector]public Player_Helpers Helpers;
    
    #endregion

    // References
    public MeleeWeapon weapon;
    public Animator b_Animator;
    public Collider b_BodyCollider;
    public Collider b_HeadCollider;

    [HideInInspector]public Rigidbody b_Rigidbody;
     [HideInInspector]public CustomGravity customGravity;

    void Start()
    {
        pb_instance = this;

        stateMachineBrain = new StateMachine();
        groundState = new GroundState(this, stateMachineBrain);
        standing = new StandState(this, stateMachineBrain);
        rolling = new RollState(this, stateMachineBrain);
        crouching = new CrouchState(this, stateMachineBrain);
        jumping = new JumpState(this, stateMachineBrain);
        falling = new FallState(this, stateMachineBrain);
        imposing = new ImposeState(this, stateMachineBrain);

        atk_1 = new BasicCombo1_State(this, stateMachineBrain);
        atk_2 = new BasicCombo2_State(this, stateMachineBrain);
        atk_3 = new BasicCombo3_State(this, stateMachineBrain);
        atk_4 = new BasicCombo4_State(this, stateMachineBrain);
        iaiPrepping = new IaiState(this, stateMachineBrain);
        iaiSlashing = new ForwardSlashState(this, stateMachineBrain);


        PlayerInput = GetComponent<Player_Input>();
        Movement = GetComponent<Player_Movement>();
        Attack = GetComponent<Player_Attack>();
        Helpers = GetComponent<Player_Helpers>();
        Health = GetComponent<Damageable>();

        weapon = GetComponentInChildren<MeleeWeapon>();
        b_Rigidbody = GetComponent<Rigidbody>();
        customGravity = GetComponent<CustomGravity>();

        cooldownSystem = GetComponent<CooldownSystem>();


        stateMachineBrain.Initialize(standing);
    }

    private void Update()
    {
        stateMachineBrain.CurrentState.LogicUpdate();
        stateMachineBrain.CurrentState.HandleInput();

        if(Health.isTrueInvulnerable) Helpers.ChangeColor(Color.red, true);
        else Helpers.ChangeColor(Color.black, false);
    }

    private void FixedUpdate()
    {
        stateMachineBrain.CurrentState.PhysicsUpdate();
    }
}
