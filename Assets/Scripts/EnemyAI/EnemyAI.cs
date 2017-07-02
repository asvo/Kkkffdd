using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */

public class EnemyAI : ICharacterAI
{
    public EnemyAI(BaseEntity entity) : base(entity)
    {
        base.m_Entity = entity;
        m_AttackRange = entity.NormalAttackRange;
    }

    public override void Idle()
    {
        (m_Entity as Monster).Idle();
    }

    public override void Attack(BaseEntity entity)
    {
        //
        m_CoolDown -= Time.deltaTime;
        if (m_CoolDown > 0)
            return;
        m_CoolDown = ATTACK_COOLD_DOWN;

        //攻击目标
        (m_Entity as Monster).Attack();
    }

    public override void Restats()
    {
        (m_Entity as Monster).Restats();
    }
}

