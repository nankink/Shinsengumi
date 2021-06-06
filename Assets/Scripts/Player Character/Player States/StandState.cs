using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandState : GroundState
{
    //bool jumping, crouching, rolling;

    protected bool p_Attack;

    public StandState(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }


    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {

        base.LogicUpdate();
        if (character.PlayerInput.RollInput && character.cooldownSystem.IsOnCooldown(character.Movement.Id)) { return; }
        else if (character.PlayerInput.RollInput)
        {
            stateMachine.ChangeState(character.rolling);
        }

        if (character.PlayerInput.CrouchInput)
        {
            stateMachine.ChangeState(character.crouching);
        }

        if (character.PlayerInput.JumpInput)
        {
            stateMachine.ChangeState(character.jumping);
        }

        if (character.PlayerInput.AttackInput)
        {
            stateMachine.ChangeState(character.atk_1);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
