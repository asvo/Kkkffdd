using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/*
*  功能需求 ： 僵直状态
*  编写者     ： 林鸿伟
*  version  ：1.0
*/

namespace AIState
{
    public class RestatsAIState : IAIState
    {

        //僵直时间
        private float m_RestatsTime = 0;
        private bool m_bOnRestats = false;

        public RestatsAIState(float Restatstime)
        {
            m_RestatsTime = Restatstime;
        }

        public override void Update(List<BaseEntity> Targets)
        {
            if (m_bOnRestats)
            {
                m_RestatsTime -= Time.deltaTime;
                if (m_RestatsTime <= 0)
                {
                    m_CharacterAI.ChangeAIState(new IdleAIState());
                }
                return;
            }

            m_bOnRestats = true;
            m_CharacterAI.Restats(Targets[0]);
        }
    }
}

