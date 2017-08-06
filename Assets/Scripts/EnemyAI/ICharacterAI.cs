using UnityEngine;
using System.Collections.Generic;
/*
 *  功能需求 ： 怪物AI接口类
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */

namespace AIState
{
    public abstract class ICharacterAI
    {
        protected BaseEntity m_Entity = null;
        protected float m_AttackRange = 0;
        protected float m_ChaseRange = 0;

        protected IAIState m_AIState = null;//AI状态

        protected const float CONFUSE_TIME = 20;
        protected float m_ConfuseTime = CONFUSE_TIME;

        protected const float ATTACK_COOLD_DOWN = 1f;//攻击CD
        protected float m_CoolDown = ATTACK_COOLD_DOWN;

        public ICharacterAI(BaseEntity entity)
        {
            m_Entity = entity;
            m_AttackRange = entity.m_AttrValue.NormalAttackDamge;
        }

        public EnemyType GetEnemyType()
        {
            if (m_Entity is Monster)
            {
                return (m_Entity as Monster).m_EnemyAI.m_EnemyType;
            }
            else
            {
                return EnemyType.Max;
            }
        }

        public string GetCurrentState()
        {
            if (m_AIState == null)
                return null;
            return m_AIState.ToString();
        }

        //更换AI状态
        public virtual void ChangeAIState(IAIState NewAIState)
        {
            if (m_AIState == null)
                WriteLog(string.Format(m_Entity.gameObject.name + " State: {0}===============>{1} ", "None", NewAIState.ToString().Replace("AIState", "")));
            else
                WriteLog(string.Format(m_Entity.gameObject.name + " State: {0}===============>{1} ", m_AIState.ToString().Replace("AIState", ""), NewAIState.ToString().Replace("AIState", "")));

            if (m_AIState != null)
            {
                m_AIState.ForceStop();
            }
            m_AIState = NewAIState;
            m_AIState.SetCharacterAI(this);
        }

        //计算跳跃攻击的时间
        public float CalculateJumpTimeToTarget(BaseEntity Target)
        {
            return (m_Entity as Monster).GetMonsterValue().JumpTime;
            //float dist = Vector2.Distance(m_Entity.transform.position, Target.transform.position);
            //return dist * 0.33f;
        }

        public virtual void Idle()
        {
            StopMove();
        }

        public virtual bool ObstacleOnWay()
        {
            return false;
        }

        //攻击目标
        public virtual void Attack(BaseEntity Target)
        { }

        public virtual void JumpAttack(BaseEntity Target, System.Action CallBack)
        { }

        //是否在攻击范围内
        public bool TargetInAttackRange(BaseEntity Target)
        {
            return (distanceToTarget(Target) <= m_AttackRange);
        }

        public virtual bool TargetInJumpAttckRange(BaseEntity Target)
        {
            return false;
        }

        //是否在追击范围内
        public bool TargetInChaseRange(BaseEntity Target)
        {
            return (distanceToTarget(Target) <= m_ChaseRange);
        }

        protected float distanceToTarget(BaseEntity Target)
        {
            return Vector2.Distance(m_Entity.transform.position, Target.transform.position);
        }

        public bool CheckCanJump(BaseEntity Target)
        {
            List<Monster> samedirectmonsters = MonsterManager.Instance().GetMonsterInfrontToTarget(m_Entity as Monster, Target);
            if (samedirectmonsters.Count != 1)
                return false;

            if (samedirectmonsters[0]._CurrentAIState.Contains("JumpAttackAIState"))
                return false;

            return true;
        }

        //当前位置
        public Vector3 GetPosition()
        {
            return m_Entity.transform.position;
        }

        //移动
        public void MoveTo(Vector3 Postition)
        {
            m_Entity.Move(Postition.x - GetPosition().x >= 0 ? MoveDir.Right : MoveDir.Left);
        }

        public void ChangeBoxCollider()
        {
            m_Entity.MoveCtrl.CC2D.platformMask = 1 << Util.GroundLayer | 1 << Util.WallLayer | 1 << Util.PlayerLayer | 1 << Util.MonsterLayer;
        }

        //停止移动
        public void StopMove()
        {
            m_Entity.EndMove();
        }

        //僵直状态
        public virtual void Restats(BaseEntity HitFrom)
        {

        }

        //设置死亡
        public void Killed()
        {
            m_Entity.Die();
        }

        //是否死亡
        public bool IsDead()
        {
            return m_Entity.isDead;
        }

        //目标删除
        public void RemoveAITarget(BaseEntity Target)
        {
            m_AIState.RemoveTarget(Target);
        }

        //更新AI
        public void Update(List<BaseEntity> Targets)
        {
            m_AIState.Update(Targets);
        }

        public static void WriteLog(object log)
        {
            Util.LogHW(log);
        }
    }
}

