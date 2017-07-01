using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/*
*  功能需求 ： 
*  编写者     ： 林鸿伟
*  version  ：1.0
*/


public class ChaseAIState : IAIState {

    private BaseEntity m_ChaseTarget = null;//

    private const float CHASE_CHECK_DIST = 0.2f;
    private Vector3 m_ChasePosition = Vector3.zero;
    private bool m_bOnChase = false;

    public ChaseAIState(BaseEntity ChaseTarget)
    {
        m_ChaseTarget = ChaseTarget;
    }
    public override void Update(List<BaseEntity> Targets)
    {
        if (m_ChaseTarget == null || m_ChaseTarget.isDead)
        {
            m_CharacterAI.ChangeAIState(new IdleAIState());
            return;
        }

        //在攻击目标内，改为攻击
        if (m_CharacterAI.TargetInAttackRange(m_ChaseTarget))
        {
            m_CharacterAI.StopMove();
            m_CharacterAI.ChangeAIState(new AttackAIState(m_ChaseTarget));
            return;
        }

        //已经在追击
        if (m_bOnChase)
        {
            //
            float dist = Vector3.Distance(m_ChasePosition, m_CharacterAI.GetPosition());
            if (dist < CHASE_CHECK_DIST)
            {
                m_CharacterAI.ChangeAIState(new IdleAIState());
            }
            return;
        }

        m_bOnChase = true;
        m_ChasePosition = m_ChaseTarget.transform.position;
        m_CharacterAI.MoveTo(m_ChasePosition);
    }
}

