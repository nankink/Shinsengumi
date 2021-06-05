using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class GroundState : State
{
    public float xInput;

    bool p_wasCrouching;

    public GroundState(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine) { }

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

        xInput = Input.GetAxisRaw("Horizontal");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
  
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.Movement.CheckForGround();

        character.Movement.MovePlayer(xInput * Time.fixedDeltaTime);
    }


}
