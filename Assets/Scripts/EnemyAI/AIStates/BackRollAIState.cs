using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/*
*  功能需求 ： 怪物技能：后翻
*  编写者     ： 林鸿伟
*  version  ：1.0
*/


public class BackRollAIState : IAIState {

    public float RollRange = 2;
    public float rollSpeed = 0.5f;

    protected bool m_bOnRoll = false;

    public BackRollAIState(float range)
    {
        RollRange = range;
        m_bOnRoll = true;
    }

    public override void Update(List<BaseEntity> Targets)
    {
        if (m_bOnRoll)
        {
            (m_CharacterAI as EnemyAI).RollBack(Targets[0], OnFinish);
            m_bOnRoll = false;
        }
    }

    protected virtual void OnFinish()
    {

    }
}

