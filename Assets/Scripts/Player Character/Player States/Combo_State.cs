using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo_State : State
{
    public float currentTimeInState;
    public float maxTimeInState;

    public float minInputWindow;
    public float maxInputWindow;

    public float iframeTimeMinInPercent;
    public float iframeTimeMin;
    public float iframeTimeInPercent;
    public float iframeTime;

    public float delay;
    public float moveDist;

    public bool triggerAtk;
    public bool exitAtk;

    public Combo_State(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }

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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void AttackEnter()
    {
        currentTimeInState = 0;

        character.b_Animator.SetTrigger("Attack");
        character.Movement.MoveForward(delay, moveDist);
        triggerAtk = exitAtk = false;

        minInputWindow *= maxTimeInState;
        maxInputWindow = maxTimeInState;

        iframeTime = maxTimeInState * iframeTimeInPercent;
        iframeTimeMin = maxTimeInState * iframeTimeMinInPercent;

        character.Health.SetTrueInvunerabilityByTime(iframeTimeMin, iframeTime);

        character.Attack.EquipMeleeWeapon(true);
        character.Attack.MeleeAttackStart();
    }

    public void AttackExit()
    {
        character.b_Animator.ResetTrigger("Attack");
        currentTimeInState = 0;
        triggerAtk = exitAtk = false;

        character.Attack.MeleeAttackEnd();
        character.Attack.EquipMeleeWeapon(false);
    }

    public void InputCheck()
    {
        if (character.PlayerInput.AttackInput)
        {
            if (currentTimeInState >= minInputWindow && currentTimeInState < maxInputWindow)
            {
                triggerAtk = true;
            }
        }
        if (currentTimeInState >= maxInputWindow && !character.PlayerInput.AttackInput)
        {
            exitAtk = true;
        }
    }

    public void StateCheck(State obj)
    {
                if (triggerAtk)
        {
            if (character.b_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !character.b_Animator.IsInTransition(0))
            {
                exitAtk = false;
                triggerAtk = false;
                stateMachine.ChangeState(obj);
            }
        }

        if (exitAtk)
        {
            if (character.b_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !character.b_Animator.IsInTransition(0))
            {
                character.b_Animator.SetTrigger("ExitCombo");
                character.cooldownSystem.PutOnCooldown(character.Attack);
                stateMachine.ChangeState(character.imposing);
            }
        }
    }


}
