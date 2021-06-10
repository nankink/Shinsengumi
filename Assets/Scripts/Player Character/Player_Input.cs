using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    protected float i_motion;
    protected bool i_jump;
    protected bool i_attack;
    protected bool i_crouch;
    protected bool i_roll;
    protected bool i_sheath;
    protected bool i_iai;
    protected bool i_externalInputBlocked;

    [HideInInspector]
    public bool playerInputBlocked;

    WaitForSeconds i_AttackInputWait;
    Coroutine i_AttackWaitCoroutine;
    const float i_AttackInputDuration = 0.03f;

    #region GET INPUTS
    public float MoveInput
    {
        get
        {
            if(playerInputBlocked || i_externalInputBlocked)
                return 0f;
            return i_motion;
        }
    }

    public bool JumpInput
    {
        get
        {
            return i_jump && !playerInputBlocked && !i_externalInputBlocked;
        }
    }

    public bool AttackInput
    {
        get{
            return i_attack && !playerInputBlocked && !i_externalInputBlocked;
        }
    }

    public bool CrouchInput
    {
        get 
        {
            return i_crouch && !playerInputBlocked && !i_externalInputBlocked;
        }
    }

    public bool RollInput
    {
        get
        {
            return i_roll && !playerInputBlocked && !i_externalInputBlocked;
        }
    }

    public bool SheathInput
    {
        get
        {
            return i_sheath && !playerInputBlocked && !i_externalInputBlocked;
        }
    }

    public bool IaiInput
    {
        get 
        {
            return i_iai && !playerInputBlocked && !i_externalInputBlocked;
        }
    }

    #endregion

    private void Awake()
    {
        i_AttackInputWait = new WaitForSeconds(i_AttackInputDuration);
    }

    private void Update()
    {
        i_motion = Input.GetAxisRaw("Horizontal");
        i_jump = Input.GetButton("Jump");
        i_crouch = Input.GetButton("Crouch");
        i_roll = Input.GetButton("Roll");

        i_sheath = Input.GetButtonDown("Sheath");

        if(Input.GetButtonDown("Fire1"))
        {
            if(i_AttackWaitCoroutine != null) StopCoroutine(i_AttackWaitCoroutine);
            i_AttackWaitCoroutine = StartCoroutine(AttackWait());
        }

        i_iai = Input.GetButton("Fire2");

    }

    IEnumerator AttackWait()
    {
        i_attack = true;
        yield return i_AttackInputWait;
        i_attack = false;
    }

    public bool HaveControl()
    {
        return !i_externalInputBlocked;
    }

    public void ReleaseControl()
    {
        i_externalInputBlocked = true;
    }

    public void GainControl()
    {
        i_externalInputBlocked = false;
    }

}
