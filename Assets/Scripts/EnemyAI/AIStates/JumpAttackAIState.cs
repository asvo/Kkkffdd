using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/*
*  功能需求 ： 跳起攻击
*  编写者     ： 林鸿伟
*  version  ：1.0
*/


public class JumpAttackAIState : IAIState {

    private float m_JumpTime = 0;
    private bool m_bOnJump = false;
    private BaseEntity Target;

    public JumpAttackAIState(BaseEntity Target)
    {
        this.Target = Target;
        m_bOnJump = false;
    } 

    public override void Update(List<BaseEntity> Targets)
    {
        if (!m_bOnJump)
        {
            if (Target == null || Target.isDead)
            {
                m_CharacterAI.ChangeAIState(new IdleAIState());
                return;
            }
        }
        else
        {
            return;
        }

        m_bOnJump = true;
        m_JumpTime = m_CharacterAI.CalculateJumpTimeToTarget(Target);
        m_CharacterAI.JumpAttack(Target, JumpOver);

    }

    private void JumpOver()
    {
        m_bOnJump = false;
        if (m_CharacterAI.TargetInAttackRange(Target))
        {
            //玩家在攻击范围内，转换为攻击状态，执行攻击
            m_CharacterAI.ChangeAIState(new AttackAIState(Target));
            return;
        }
        else //超出范围，转为跟踪状态
        {
            m_CharacterAI.ChangeAIState(new ChaseAIState(Target));
            return;
        }
    }
}

