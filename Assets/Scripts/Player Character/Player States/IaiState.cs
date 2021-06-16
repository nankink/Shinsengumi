using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IaiState : State, IHasCooldown
{
    float currentTime;
    float maxIaiTime = 2f;
    bool iaiHeld;

    float damage;

    float iaiMinDistance = 35f;
    float iaiDistance;

    public int Id => 3;
    public float CooldownDuration => 2f;

    public IaiState(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine){}

    public override void Enter()
    {
        base.Enter();
        iaiHeld = true;
        currentTime = 0;
        
        character.b_Animator.SetBool("iaiPrep", true);
    }

    public override void Exit()
    {
        base.Exit();
        character.b_Animator.SetBool("iaiPrep", false);
    }

    public override void HandleInput()
    {
        base.HandleInput();
        while(character.PlayerInput.IaiInput)
        {
            currentTime += Time.deltaTime;
            
            iaiDistance = Mathf.Pow(currentTime, 3) + iaiMinDistance;
            damage = Mathf.Pow(currentTime, 3) + character.Attack.baseDamage;
            
            if(currentTime >= maxIaiTime) currentTime = maxIaiTime;
            
            return;
        }
        iaiHeld = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(!iaiHeld || currentTime >= maxIaiTime)
        {
            character.Attack.damage = damage;
            character.iaiSlashing.iaiDistance = iaiDistance;
            stateMachine.ChangeState(character.iaiSlashing);
        }
   
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
