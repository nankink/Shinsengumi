using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCombo3_State : State
{
    float currentTimeInState;
    float maxTimeInState;

    float minInputWindow = 0.25f;
    float maxInputWindow;

    float delay = 0.15f;
    float moveDist = 12f;

    bool triggerAtk;
    bool exitAtk;

    //Debug
    Color oldColor;

    public BasicCombo3_State(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        character.b_Animator.SetTrigger("Attack");
        character.Movement.MoveForward(delay, moveDist);

        triggerAtk = exitAtk = false;

        currentTimeInState = 0;
        maxTimeInState = character.b_Animator.GetCurrentAnimatorStateInfo(0).length;
        maxInputWindow = maxTimeInState;

        character.Attack.EquipMeleeWeapon(true);

        // Debug
        oldColor = character.meshMaterial.color;
        character.meshMaterial.color = Color.yellow;

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
        currentTimeInState += Time.deltaTime;


        if (character.PlayerInput.AttackInput)
        {
            if (currentTimeInState >= minInputWindow && currentTimeInState < maxInputWindow)
            {
                triggerAtk = true;
            }
        }
        else
        {
            if (currentTimeInState > maxTimeInState)
            {
                exitAtk = true;
            }
        }

        if (triggerAtk)
        {
            if (character.b_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95 && !character.b_Animator.IsInTransition(0))
            {
                exitAtk = false;
                triggerAtk = false;
                stateMachine.ChangeState(character.atk_4);
            }
        }

        if (exitAtk)
        {
            if (character.b_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95 && !character.b_Animator.IsInTransition(0))
            {
                character.b_Animator.SetTrigger("ExitCombo");
                stateMachine.ChangeState(character.standing);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
