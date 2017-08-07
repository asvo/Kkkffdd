using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
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

        public IBaseValue SimpleClone()
        {
            return this.MemberwiseClone() as ICharacterValue;
        }

        public IBaseValue DeepClone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream) as ICharacterValue;
        }
    }
}
