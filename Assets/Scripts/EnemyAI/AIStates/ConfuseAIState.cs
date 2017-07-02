using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/*
*  功能需求 ： 迷茫状态，时长20s
*  编写者     ： 林鸿伟
*  version  ：1.0
*/


public class ConfuseAIState : IAIState {

    private float m_ConfuseTime = 0;
    private bool m_bOnConfuse = false;

    public ConfuseAIState(float confuseTime)
    {
        this.m_ConfuseTime = confuseTime;
    }

    public override void Update(List<BaseEntity> Targets)
    {
        if (Targets != null && Targets.Count > 0)
        {
            if (m_CharacterAI.TargetInAttackRange(Targets[0]))
            {
                m_CharacterAI.ChangeAIState(new AttackAIState(Targets[0]));
                return;
            }
        }

        if (m_bOnConfuse)
        {
            m_ConfuseTime -= Time.deltaTime;
            if (m_ConfuseTime <= 0)
            {
                m_CharacterAI.ChangeAIState(new IdleAIState());
                return;
            }
            return;
        }

        m_bOnConfuse = true;
        m_CharacterAI.Idle();
    }
}

