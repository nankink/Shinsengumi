using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImposeState : GroundState
{

    public ImposeState(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        character.b_Animator.SetBool("isSheathed", false);     
        if(character.Attack.isSheathed) {character.Movement.DelayMove(1f);}
        // wait for the animation

        character.Attack.isSheathed = false;
        character.weapon.gameObject.SetActive(true);
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

            stateMachine.ChangeState(character.standing);
        }

        if (character.PlayerInput.AttackInput && character.cooldownSystem.IsOnCooldown(character.Attack.Id) && character.Movement.coroutineRunning) { return; }
        else if (character.PlayerInput.AttackInput && !character.Movement.coroutineRunning)
        {
            stateMachine.ChangeState(character.atk_1);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
