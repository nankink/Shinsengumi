using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_Movement : MonoBehaviour, IHasCooldown
{
    // Move
    [Header("Move")]
    public float runSpeed = 50f;
    [Range(0, 0.3f)] public float m_MovementSmoothing = .05f;
    Vector3 m_Velocity = Vector3.zero;
    Vector3 targetVelocity;
    bool isFacingRight;
    [SerializeField] bool canMove;
    [HideInInspector] public bool coroutineRunning;

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
    public bool m_Grounded;
    [SerializeField] public Transform m_GroundChecker;
    public float m_GroundedRadius = .05f;
    [SerializeField] public Transform m_CeilingChecker;
    public float m_CeilingRadius = .05f;
    [SerializeField] public LayerMask m_GroundLayer;

    [Header("Sensoring")]
    [Range(0f, 5f)] public float maxDistance;
    public Transform[] sensorPoints;
    bool[] contact;
    bool cancelMove = false;

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

        contact = new bool[sensorPoints.Length];

        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponentInChildren<Animator>();
        customGravity = GetComponent<CustomGravity>();
    }

    private void Update()
    {
        BodySensoring();
        if (CheckContact() && !m_Grounded) { cancelMove = true; Debug.Log("check"); }
        else cancelMove = false;
    }

    public void MovePlayer(float move)
    {
        if (canMove)
        {
            move *= runSpeed;
            if (move < 0) { isFacingRight = false; gameObject.transform.rotation = Quaternion.Euler(0, -180, 0); }
            if (move > 0) { isFacingRight = true; gameObject.transform.rotation = Quaternion.Euler(0, 0, 0); }

            if (cancelMove) targetVelocity = new Vector3(0, 0, 0);
            else targetVelocity = new Vector3(move * 10f, m_Rigidbody.velocity.y, 0);

            m_Rigidbody.velocity = Vector3.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
            m_Animator.SetFloat("Velocity", Mathf.Abs(move));
        }
    }

    public void Jump()
    {
        if (canMove)
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
        Debug.Log("1");
        if (canMove)
        {
            Debug.Log("2");

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.forward, out hit, 10, m_GroundLayer))
            {
                Debug.Log("3");

                m_Rigidbody.Sleep();
            }
            else
            {
                Debug.Log("4");

                if (targetVelocity == Vector3.zero)
                {
                    Debug.Log("5");

                    Vector3 targetDir;


                    if (isFacingRight) targetDir = new Vector3(1, 0, 0);
                    else targetDir = new Vector3(-1, 0, 0);
                    Debug.Log("6" + targetDir);

                    m_Rigidbody.velocity += targetDir.normalized * m_RollForce;
                }
                else
                {
                    m_Rigidbody.velocity += targetVelocity.normalized * m_RollForce;
                    Debug.Log("7" + targetVelocity);

                }
            }
        }
    }

    public void BodySensoring()
    {
        for (int i = 0; i < sensorPoints.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(sensorPoints[i].position, transform.right, out hit, maxDistance, m_GroundLayer))
            {
                contact[i] = true;
            }
            else contact[i] = false;
        }
    }

    public bool CheckContact()
    {
        for (int i = 0; i < contact.Length; i++)
        {
            if (!contact[i]) return false;
        }
        return true;
    }


    public void MoveForward(float delay, float dist)
    {
        StartCoroutine(DelayAction(delay, dist));
    }

    IEnumerator DelayAction(float delay, float dist)
    {
        yield return new WaitForSeconds(delay);
        Vector3 targetDir;

        if (isFacingRight) targetDir = new Vector3(1, 0, 0);
        else targetDir = new Vector3(-1, 0, 0);

        m_Rigidbody.velocity += targetDir * dist;

    }

    public void DelayMove(float time)
    {
        StartCoroutine(DelayMovement(time));
    }

    IEnumerator DelayMovement(float time)
    {
        coroutineRunning = true;
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
        coroutineRunning = false;
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
