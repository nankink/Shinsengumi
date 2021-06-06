using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCombo4_State : State
{
    float currentTimeInState;
    float maxTimeInState;

    float minInputWindow = 0;
    float maxInputWindow;

    float delay = 0.23f;
    float moveDist = 15f;

    //Debug
    Color oldColor;

    public BasicCombo4_State(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        character.b_Animator.SetTrigger("Attack");
        character.Movement.MoveForward(delay, moveDist);

        currentTimeInState = 0;
        maxTimeInState = character.b_Animator.GetCurrentAnimatorStateInfo(0).length;
        maxInputWindow = maxTimeInState;

        character.Attack.EquipMeleeWeapon(true);

        // Debug
        oldColor = character.meshMaterial.color;
        character.meshMaterial.color = Color.cyan;

    }

    public override void Exit()
    {
        base.Exit();
        character.Attack.EquipMeleeWeapon(false);
        character.b_Animator.ResetTrigger("Attack");

        character.meshMaterial.color = oldColor;
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (character.b_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95 && !character.b_Animator.IsInTransition(0))
        {
            character.b_Animator.SetTrigger("ExitCombo");
            stateMachine.ChangeState(character.standing);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
