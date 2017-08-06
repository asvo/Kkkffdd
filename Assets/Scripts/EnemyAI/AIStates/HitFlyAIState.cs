using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/*
*  功能需求 ： 击飞
*  编写者     ： 林鸿伟
*  version  ：1.0
*/
namespace AIState
{

    public class HitFlyAIState : IAIState
    {

        private bool m_bHitFly = false;

        public override void Update(List<BaseEntity> Targets)
        {
            if (m_bHitFly)
            {
                return;
            }
            m_bHitFly = true;
            Debug.LogError("HitFly");
        }
    }
}

