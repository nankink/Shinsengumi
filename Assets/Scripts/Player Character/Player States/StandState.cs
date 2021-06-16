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
        character.b_Animator.SetBool("isSheathed", true);
        if(!character.Attack.isSheathed) {character.Movement.DelayMove(1f);}
        // wait for the animation


        character.Attack.isSheathed = true;
        character.weapon.gameObject.SetActive(false);
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

        if (character.PlayerInput.SheathInput)
        {
            character.b_Animator.ResetTrigger("Sheath");
            character.b_Animator.SetTrigger("Sheath");

            stateMachine.ChangeState(character.imposing);
        }

        if(character.PlayerInput.IaiInput && !character.cooldownSystem.IsOnCooldown(character.iaiPrepping.Id))
        {
            stateMachine.ChangeState(character.iaiPrepping);
        }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();


        if (character.b_Rigidbody.velocity.y < -0.2f) stateMachine.ChangeState(character.falling);
    }

}
