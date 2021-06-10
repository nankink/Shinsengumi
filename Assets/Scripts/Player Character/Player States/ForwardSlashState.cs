using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardSlashState : State
{
    float currentTime;
    float maxTimeInState = 0.917f;
    public float iaiDistance;

    public ForwardSlashState(Player_Brain character, StateMachine stateMachine) : base(character, stateMachine){}

    public override void Enter()
    {
        base.Enter();
        currentTime = 0;
        character.b_Animator.SetTrigger("iaiSlash");
        character.Movement.MoveForward(0.1f, iaiDistance);

        character.weapon.gameObject.SetActive(true);

        character.b_BodyCollider.isTrigger = true;
        character.b_HeadCollider.isTrigger = true;

        character.Attack.EquipMeleeWeapon(true);
        character.Attack.MeleeAttackStart();

        Debug.Log("damage: "+ character.Attack.damage + " /// move: "+ iaiDistance);

        character.Health.SetTrueInvunerabilityByTime(0, 0.85f);    
    }

    public override void Exit()
    {
        base.Exit();
        currentTime = 0;
        character.b_Animator.ResetTrigger("iaiSlash");

        character.weapon.gameObject.SetActive(false);

        character.b_BodyCollider.isTrigger = false;
        character.b_HeadCollider.isTrigger = false;

        character.Attack.EquipMeleeWeapon(false);
        character.Attack.MeleeAttackEnd();

    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        currentTime += Time.deltaTime;

        if(character.Attack.isSheathed)
        {
            if(currentTime >= maxTimeInState + 0.1f)
            {
                character.cooldownSystem.PutOnCooldown(character.iaiPrepping);
                stateMachine.ChangeState(character.standing);
            }
        }
        else 
        {
            if(character.b_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !character.b_Animator.IsInTransition(0))
            {
                character.cooldownSystem.PutOnCooldown(character.iaiPrepping);
                stateMachine.ChangeState(character.imposing);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
