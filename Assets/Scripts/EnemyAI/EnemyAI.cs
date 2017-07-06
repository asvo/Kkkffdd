using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */

public class EnemyAI : ICharacterAI
{

    protected float JumpAttckRange = 0;
    public EnemyAI(BaseEntity entity) : base(entity)
    {
        base.m_Entity = entity;
        m_AttackRange = entity.NormalAttackRange;
        m_ChaseRange = (entity as Monster).ChaseRange;
        JumpAttckRange = (entity as Monster).JumpAttckRange;
    }

    public override void Idle()
    {
        (m_Entity as Monster).Idle();
    }

    public override void Attack(BaseEntity Target)
    {
        //
        m_CoolDown -= Time.deltaTime;
        if (m_CoolDown > 0)
            return;
        m_CoolDown = ATTACK_COOLD_DOWN;

        //攻击目标
        (m_Entity as Monster).Attack();
    }

    public override void JumpAttack(BaseEntity Target)
    {
        (m_Entity as Monster).JumpAttck(Target);
    }

    public override bool TargetInJumpAttckRange(BaseEntity Target)
    {
        return (distanceToTarget(Target) <= JumpAttckRange);
    }

    public override void Restats()
    {
        (m_Entity as Monster).Restats();
    }
}

