using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandState : GroundState
{
    bool jumping, crouching, rolling;

    public StandState(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        crouching = false;
        rolling = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (Input.GetButtonDown("Jump")) jumping = true;

        crouching = Input.GetButtonDown("Crouch");

        rolling = Input.GetButtonDown("Roll");
    }

    public override void LogicUpdate()
    {
        Debug.Log("crouching: "+ crouching);

        base.LogicUpdate();
        if (rolling && character.cooldownSystem.IsOnCooldown(character.Movement.Id)) { return; }
        else if (rolling)
        {
            rolling = false;
            stateMachine.ChangeState(character.rolling);
        }

        if(crouching)
        {
           stateMachine.ChangeState(character.crouching);
        }

        if(jumping)
        {
            jumping = false;
            stateMachine.ChangeState(character.jumping);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
