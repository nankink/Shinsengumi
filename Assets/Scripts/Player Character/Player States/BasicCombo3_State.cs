using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCombo3_State : Combo_State
{
    // State
    float currentTimeInState;
    float maxTimeInState = 0.4f;

    // Input
    float minInputWindow = 0.9f;
    float maxInputWindow;

    // iFrame
    float iframeTimeMinInPercent = 0.1f;
    float iframeTimeMin;
    float iframeTimeInPercent = 0.9f;
    float iframeTime;

    // Movement
    float delay = 0.15f;
    float moveDist = 12f;

    bool triggerAtk;
    bool exitAtk;

    public BasicCombo3_State(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        // Animation
        character.b_Animator.SetTrigger("Attack");
        character.Movement.MoveForward(delay, moveDist);

        triggerAtk = exitAtk = false;

        // Timing
        currentTimeInState = 0;
        Debug.Log("maxTimeInState 3: " + maxTimeInState);

        // Input
        minInputWindow *= maxTimeInState;
        maxInputWindow = maxTimeInState;

        // iFrame
        iframeTime = maxTimeInState * iframeTimeInPercent;
        iframeTimeMin = maxTimeInState * iframeTimeMinInPercent;

        character.Health.SetTrueInvunerabilityByTime(iframeTimeMin, iframeTime);
        // Attack
        character.Attack.EquipMeleeWeapon(true);
        character.Attack.MeleeAttackStart();
    }

    public override void Exit()
    {
        base.Exit();
        character.b_Animator.ResetTrigger("Attack");
        currentTimeInState = 0;

        character.Attack.MeleeAttackEnd();
        character.Attack.EquipMeleeWeapon(false);
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
        if (currentTimeInState >= maxInputWindow && !character.PlayerInput.AttackInput)
        {
            exitAtk = true;
        }

        if (triggerAtk)
        {
            if (character.b_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !character.b_Animator.IsInTransition(0))
            {
                exitAtk = false;
                triggerAtk = false;
                stateMachine.ChangeState(character.atk_4);
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

        character.Helpers.DisplayText(TextFieldUI.CurrentTimeInState, currentTimeInState.ToString());
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
