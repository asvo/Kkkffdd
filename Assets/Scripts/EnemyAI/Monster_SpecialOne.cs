﻿using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class Monster_SpecialOne : Monster {

    private bool m_bHadCastDodge = false;
    public override void Spawn(EnemyAI AIBrain = null)
    {
        base.Spawn(AIBrain);
        m_bHadCastDodge = false;
    }

    public override void OnDamaged(int damage)
    {
        if (m_bHadCastDodge == false && Health - damage < 0)
        {
            if (m_EnemyAI != null)
            {
                m_bHadCastDodge = true;
                m_EnemyAI.ChangeAIState(new BackRollAIState(RollBackRange));
                return;
            }
        }
        base.OnDamaged(damage);
    }
}
