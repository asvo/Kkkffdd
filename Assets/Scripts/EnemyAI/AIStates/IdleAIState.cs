using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/*
*  功能需求 ： 
*  编写者     ： 林鸿伟
*  version  ：1.0
*/

namespace AIState
{
    public class IdleAIState : IAIState
    {

        public float ConfusedTime = 0;
        bool m_bOnIdle = false;

        public IdleAIState(BaseEntity Target = null)
        {
            m_bOnIdle = false;
        }

        public override void Update(List<BaseEntity> Targets)
        {
            if (!m_bOnIdle)
            {
                m_CharacterAI.Idle();
                m_bOnIdle = true;
            }

            //
            if (Targets.Count == 0 || Targets[0].isDead)
            {
                return;
            }
            if (m_CharacterAI.ObstacleOnWay())
            {
                if (m_CharacterAI.TargetInJumpAttckRange(Targets[0]))
                {
                    if (m_CharacterAI.CheckCanJump(Targets[0]))
                    {
                        m_CharacterAI.StopMove();
                        m_CharacterAI.ChangeAIState(new JumpAttackAIState(Targets[0]));
                        return;
                    }
                }
                return;
            }
            //有目标进入攻击范围
            if (m_CharacterAI.TargetInAttackRange(Targets[0]))
            {
                m_CharacterAI.ChangeAIState(new AttackAIState(Targets[0]));
            }
            else
            {
                m_CharacterAI.ChangeAIState(new ChaseAIState(Targets[0]));
            }
        }
    }
}
