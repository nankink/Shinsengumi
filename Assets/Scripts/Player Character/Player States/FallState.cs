using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : GroundState
{
    bool grounded;
    bool crounch;

    public FallState(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }

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
        grounded = character.Movement.m_Grounded;

        if (character.PlayerInput.CrouchInput && grounded)
        {
            character.b_Animator.SetBool("isJumping", false);
            stateMachine.ChangeState(character.crouching);
        }

        else if (grounded && character.b_Animator.GetBool("isSheathed"))
        {
            character.b_Animator.SetBool("isJumping", false);
            stateMachine.ChangeState(character.standing);
        }

        else if (grounded && !character.b_Animator.GetBool("isSheathed"))
        {
            character.b_Animator.SetBool("isJumping", false);
            stateMachine.ChangeState(character.imposing);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

}
