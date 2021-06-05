using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : GroundState
{
    bool doubleJump;

    public JumpState(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        character.Movement.Jump();
        character.b_Animator.SetBool("isJumping", true);
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (character.b_Rigidbody.velocity.y < -0.2f) stateMachine.ChangeState(character.falling);
    }

}
