using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/*
*  功能需求 ： 
*  编写者     ： 林鸿伟
*  version  ：1.0
*/


public class AttackAIState : IAIState {

    private BaseEntity m_AttackTarget = null;//攻击对象

    public AttackAIState(BaseEntity AttackTarget)
    {
        m_AttackTarget = AttackTarget;
    }

    public override void Update(List<BaseEntity> Targets)
    {
        //没有目标，或目标死亡改为Idle
        if (m_AttackTarget == null || m_AttackTarget.isDead)
        {
            m_CharacterAI.ChangeAIState(new IdleAIState());
            return;
        }

        //不在攻击范围内，改为追击
        if (m_CharacterAI.TargetInAttackRange(m_AttackTarget) == false)
        {
            m_CharacterAI.ChangeAIState(new ChaseAIState(m_AttackTarget));
            return;
        }

        //攻击
        m_CharacterAI.Attack(m_AttackTarget);
    }
}

