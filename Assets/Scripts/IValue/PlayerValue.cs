using UnityEngine;
using System;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */

namespace ValueModule
{
    [Serializable]
    public class PlayerValue : ICharacterValue
    {

        public PlayerValue()
        {
            NormalAttackCd = 0.15f;
            NormalAttackRange = 2f;
            NormalAttackDamgePoint = 0.3f;
            NormalAttackDamge = 1;

            InitMoveSpeed = 2f;
            MaxMoveSpeed = 10f;

            Health = 3;
        }
    }
}

