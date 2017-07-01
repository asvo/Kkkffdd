using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/*
*  功能需求 ： 
*  编写者     ： 林鸿伟
*  version  ：1.0
*/


public class IdleAIState : IAIState {

    bool m_bSetAttackPosition = false;//是否设置了攻击目标

    public float ConfusedTime = 0;

    public IdleAIState()
    { }

    public override void Update(List<BaseEntity> Targets)
    {
        //
        if (Targets == null || Targets.Count == 0)
        {
            if (m_bSetAttackPosition)
                m_CharacterAI.ChangeAIState(new MoveAIState());

            return;
        }

        //
        BaseEntity player = GameManager.Instance().MainPlayer;
        if (player == null || player.isDead)
        {
            return;
        }

        if (m_CharacterAI.TargetInAttackRange(player))
        {
            m_CharacterAI.ChangeAIState(new AttackAIState(player));
        }
        else
        {
            m_CharacterAI.ChangeAIState(new ChaseAIState(player));
        }
    }
}

