using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/*
*  功能需求 ： 
*  编写者     ： 林鸿伟
*  version  ：1.0
*/


public class MoveAIState : IAIState {

    private const float MOVE_CHECK_DIST = 1.5f;
    bool m_bOnMove = false;
    Vector3 m_AttackPosition = Vector3.zero;

    public MoveAIState()
    { }

    //设置要攻击的目标
    public override void SetAttckPosition(Vector3 AttackPosition)
    {
        m_AttackPosition = AttackPosition;
    }

    public override void Update(List<BaseEntity> Targets)
    {
        //有目标时，改为待机状态
        if (Targets != null && Targets.Count != 0)
        {
            m_CharacterAI.ChangeAIState(new IdleAIState());
            return;
        }

        //向目标移动中
        if (m_bOnMove)
        {
            //是否到达目标
            float dist = Vector3.Distance(m_AttackPosition, m_CharacterAI.GetPosition());

            if (dist < MOVE_CHECK_DIST)
            {
                m_CharacterAI.ChangeAIState(new IdleAIState());
                if (m_CharacterAI.IsDead() == false)
                {
                }
            }

            return;
        }

        //往目标移动
        m_bOnMove = true;
        m_CharacterAI.MoveTo(m_AttackPosition);
    }
}

