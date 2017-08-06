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
    public class MonsterValue : ICharacterValue
    {
        [Header("-----怪物特有部分-----")]
        public float ChaseRange = 5;
        public float RestatsTime = 0.3f;
        public float MaxConfusedTime = 20;


        [Header("-----跳跃技能部分-----")]
        public float JumpPrepareTime = 0.5f;
        public float JumpAttckRange = 6;
        public float JumpTime = 1;
        public float JumpHeight = 3;
        public float JumpHitBackSpeed = 3;
        public float JumpPrepareTimeRate = 0.1f;

        [Header("-----闪避技能-----")]
        public float RollBackRange = 3;

        /// <summary>
        /// 初始化
        /// </summary>
        public MonsterValue()
        {
            ChaseRange = 5;
            RestatsTime = 0.3f;
            MaxConfusedTime = 20;

            JumpPrepareTime = 0.5f;
            JumpAttckRange = 6;
            JumpTime = 1;
            JumpHeight = 3;
            JumpHitBackSpeed = 3;
            JumpPrepareTimeRate = 0.1f;

            RollBackRange = 3;
        }
    }
}

