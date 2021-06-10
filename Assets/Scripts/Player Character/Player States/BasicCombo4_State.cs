using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCombo4_State : Combo_State
{
    public BasicCombo4_State(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }

    void InitializeVariables()
    {
        // Cooldown
        character.Attack.CooldownDuration = 0.5f;

        // State
        maxTimeInState = 0.917f;

        // Input
        minInputWindowInPercent = 0;

        // iFrame
        iframeTimeMin = 0.15f;
        iframeTime = 0.4f;
        // the sum of both shouldnt exceed the maxTimeInState
        // 

        // Movement
        delay = 0.23f;
        moveDist = 15f;
    }

    public override void Enter()
    {
        base.Enter();
        InitializeVariables();

        AttackEnter();

    }

    public override void Exit()
    {
        base.Exit();
        AttackExit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        currentTimeInState += Time.deltaTime;

        StateCheckNoTrigger();

        character.Helpers.DisplayText(TextFieldUI.CurrentTimeInState, currentTimeInState.ToString());
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
