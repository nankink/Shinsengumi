using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class Player_Movement : MonoBehaviour, IHasCooldown
{
    // Move
    [Header("Move")]
    public float runSpeed = 50f;
    [Range(0, 0.3f)] public float m_MovementSmoothing = .05f;
    Vector3 m_Velocity = Vector3.zero;
    Vector3 targetVelocity;
    bool isFacingRight;

    // Roll
    [Space]
    [Header("Rolling")]
    public float m_RollForce;
    public float m_RollingTimer;
    public float m_RollCooldown;

    // Roll cooldown
    int rollId = 1;
    public int Id => rollId;
    public float CooldownDuration => m_RollCooldown;

    [Space]
    [Header("Jumping")]
    [SerializeField] bool m_AirControl = true;
    [SerializeField] float m_JumpForce = 700f;

    [Space]
    [Header("Crouch")]
    [Range(0, 1f)] public float m_CrouchSpeed = .36f;

    // Detectors
    [Header("Miscellaneous")]
    [ReadOnly] public bool m_Grounded;
    [SerializeField]public  Transform m_GroundChecker;
    public float m_GroundedRadius = .05f;
    [SerializeField]public  Transform m_CeilingChecker;
    public float m_CeilingRadius = .05f;
    [SerializeField]public LayerMask m_GroundLayer;



    // Components
    Rigidbody m_Rigidbody;
    Animator m_Animator;
    CustomGravity customGravity;

    #region Events
    [Header("Events")]
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;
    #endregion

    void Awake()
    {
        if (OnLandEvent == null) OnLandEvent = new UnityEvent();
        if (OnCrouchEvent == null) OnCrouchEvent = new BoolEvent();

        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponentInChildren<Animator>();
        customGravity = GetComponent<CustomGravity>();
    }

    public void MovePlayer(float move)
    {
        move *= runSpeed;
        if (move < 0) { isFacingRight = false; m_Animator.gameObject.transform.rotation = Quaternion.Euler(0, -90, 0); }
        if (move > 0) { isFacingRight = true; m_Animator.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0); }

        targetVelocity = new Vector3(move * 10f, m_Rigidbody.velocity.y, 0);
        m_Rigidbody.velocity = Vector3.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        m_Animator.SetFloat("Velocity", Mathf.Abs(move));
    }

    public void Jump()
    {
        m_Rigidbody.velocity += new Vector3(0f, m_JumpForce, 0f);
    }

    public bool CheckForCeiling()
    {
        bool ceiling = Physics.CheckSphere(m_CeilingChecker.position, m_CeilingRadius, m_GroundLayer);
        return ceiling;
    }

    public void CheckForGround()
    {
        if (Physics.Raycast(m_GroundChecker.position, Vector3.down, m_GroundedRadius, m_GroundLayer))
        {
            // On Ground
            m_Grounded = true;
            customGravity.GravityScale(3);
        }
        else
        {
            // On Air
            m_Grounded = false;
            if (m_Rigidbody.velocity.y < 0.5) customGravity.GravityScale(6);
        }
    }

    public void Roll()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, targetVelocity, out hit, 10, m_GroundLayer))
        {
            m_Rigidbody.Sleep();
        }
        else
        {
            if (targetVelocity == Vector3.zero)
            {
                Vector3 targetDir;

                if (isFacingRight) targetDir = new Vector3(1, 0, 0);
                else targetDir = new Vector3(-1, 0, 0);

                m_Rigidbody.MovePosition(transform.position + targetDir.normalized * m_RollForce * Time.deltaTime);
            }
            else
                m_Rigidbody.MovePosition(transform.position + targetVelocity.normalized * m_RollForce * Time.deltaTime);
        }
    }
}
