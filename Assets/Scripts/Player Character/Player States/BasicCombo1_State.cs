using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCombo1_State : Combo_State
{
    public BasicCombo1_State(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }

    void InitializeVariables()
    {
        // Cooldown
        character.Attack.CooldownDuration = 2f;
 
        // State
        maxTimeInState = 0.367f;

        // Input
        minInputWindowInPercent = 0.4f;

        // iFrame
        iframeTimeMin = 0.15f;
        iframeTime = 0.1f;
        // the sum of both shouldnt exceed the maxTimeInState
        // 

        // Movement
        delay = 0.05f;
        moveDist = 8f;
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

        InputCheck();
        StateCheck(character.atk_2);

        character.Helpers.DisplayText(TextFieldUI.CurrentTimeInState, currentTimeInState.ToString());
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }
}
