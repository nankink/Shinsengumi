using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : State
{
    bool roll;
    float currentRollTimer;
    bool crouch;

    float startRollTimer => character.Movement.m_RollingTimer;

    Color oldColor;

    public RollState(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        crouch = false;

        character.b_Animator.SetTrigger("DoABarrelRoll");
        
        // Change color
        oldColor = character.meshMaterial.color;
        character.meshMaterial.color = Color.blue;

        currentRollTimer = startRollTimer;
        roll = true;
        character.b_BodyCollider.isTrigger = true;
        character.b_HeadCollider.isTrigger = true;        
        character.b_Rigidbody.velocity = new Vector3(character.b_Rigidbody.velocity.x, 0f, character.b_Rigidbody.velocity.z);
    }

    public override void Exit()
    {
        base.Exit();
        roll = false;
        character.cooldownSystem.PutOnCooldown(character.Movement);
        character.b_BodyCollider.isTrigger = false;
        character.b_HeadCollider.isTrigger = false;
        currentRollTimer = 0;

        character.meshMaterial.color = oldColor;


    }

    public override void HandleInput()
    {
        base.HandleInput();
        crouch = Input.GetButton("Crouch");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(currentRollTimer >= startRollTimer || currentRollTimer > 0) currentRollTimer -= Time.deltaTime;
        else if(currentRollTimer <= 0) roll = false;

        if(crouch && !roll) stateMachine.ChangeState(character.crouching);

        else if(!roll) stateMachine.ChangeState(character.standing);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(roll) character.Movement.Roll();

    }
}
