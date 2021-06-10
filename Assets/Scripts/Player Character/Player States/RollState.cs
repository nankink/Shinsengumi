using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : State
{
    bool roll;
    float currentRollTimer;
    bool crouch;

    float startRollTimer => character.Movement.m_RollingTimer;

    public RollState(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        crouch = false;
        character.b_Animator.SetTrigger("DoABarrelRoll");

        character.Helpers.ChangeColor(Color.blue, true);

        character.b_Animator.SetBool("isCrouching", false);
        currentRollTimer = startRollTimer;
        roll = true;

        
        character.b_BodyCollider.isTrigger = true;
        character.b_HeadCollider.isTrigger = true;        
        character.Health.SetTrueInvunerability(true);

        character.b_Rigidbody.velocity = new Vector3(character.b_Rigidbody.velocity.x, 0f, character.b_Rigidbody.velocity.z);
    }

    public override void Exit()
    {
        base.Exit();
        roll = false;
        character.cooldownSystem.PutOnCooldown(character.Movement);
        
        character.b_BodyCollider.isTrigger = false;
        character.b_HeadCollider.isTrigger = false;
        character.Health.SetTrueInvunerability(false);
        

        currentRollTimer = 0;

        character.Helpers.ChangeColor(Color.blue, false);
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if(currentRollTimer >= startRollTimer || currentRollTimer > 0) currentRollTimer -= Time.deltaTime;
        else if(currentRollTimer <= 0) roll = false;

        if(character.PlayerInput.CrouchInput && !roll) stateMachine.ChangeState(character.crouching);

        else if(!roll && character.b_Animator.GetBool("isSheathed")) stateMachine.ChangeState(character.standing);
        else if(!roll && !character.b_Animator.GetBool("isSheathed")) stateMachine.ChangeState(character.imposing);

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(roll) character.Movement.Roll();

    }
}
