using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/*
*  功能需求 ： 
*  编写者     ： 林鸿伟
*  version  ：1.0
*/


public class ConfuseAIState : IAIState {

    private float confuseTime = 0;

    public ConfuseAIState(float confuseTime)
    {
        this.confuseTime = confuseTime;
    }

    public override void Update(List<BaseEntity> Targets)
    {
        if (confuseTime <= 0)
        {
            m_CharacterAI.ChangeAIState(new IdleAIState());
            return;
        }

        confuseTime -= Time.deltaTime;
    }
}

