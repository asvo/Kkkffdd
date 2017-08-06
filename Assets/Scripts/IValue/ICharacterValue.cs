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
    public class ICharacterValue : IBaseValue
    {

        [Header("-----角色基础数据-----")]
        public int Health = 3;

        public float NormalAttackCd = 1.5f;
        public float NormalAttackRange = 2.5f;
        public float NormalAttackDamgePoint = 0.3f;
        public int NormalAttackDamge = 1;

        public float InitMoveSpeed = 2f;
        public float MaxMoveSpeed = 10f;
    }

}
