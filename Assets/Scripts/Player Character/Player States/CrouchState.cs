using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : GroundState
{
    bool crouchHeld;
    float walkspeed;
    bool belowCeiling;
    bool roll;

    Color oldColor;

    public CrouchState(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        character.b_Animator.SetBool("isCrouching", true);
        walkspeed = character.Movement.m_CrouchSpeed;
        belowCeiling = false;

        character.b_HeadCollider.enabled = false;

        oldColor = character.meshMaterial.color;
        character.meshMaterial.color = Color.green;
    }

    public override void Exit()
    {
        base.Exit();
        character.b_HeadCollider.enabled = true;

        character.meshMaterial.color = oldColor;

    }

    public override void HandleInput()
    {
        base.HandleInput();
        crouchHeld = Input.GetButton("Crouch");
        roll = Input.GetButtonDown("Roll");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!crouchHeld || belowCeiling)
        {
            character.b_Animator.SetBool("isCrouching", false);
            stateMachine.ChangeState(character.standing);
        }

        if (roll && character.cooldownSystem.IsOnCooldown(character.Movement.Id)) { return; }
        else if (roll)
        {
            character.b_Animator.SetBool("isCrouching", false);
            stateMachine.ChangeState(character.rolling);
        }
    }

    public override void PhysicsUpdate()
    {
        character.Movement.CheckForGround();

        belowCeiling = character.Movement.CheckForCeiling();

        character.Movement.MovePlayer(xInput * walkspeed * Time.fixedDeltaTime);
    }



}
