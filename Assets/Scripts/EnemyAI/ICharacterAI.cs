using UnityEngine;
using System.Collections.Generic;
/*
 *  功能需求 ： 怪物AI接口类
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public abstract class ICharacterAI
{
    protected BaseEntity m_Entity = null;
    protected float m_AttackRange = 0;
    protected IAIState m_AIState = null;//AI状态

    protected const float CONFUSE_TIME = 20;
    protected float m_ConfuseTime = CONFUSE_TIME;

    protected const float ATTACK_COOLD_DOWN = 1f;//攻击CD
    protected float m_CoolDown = ATTACK_COOLD_DOWN;

    public ICharacterAI(BaseEntity entity)
    {
        m_Entity = entity;
        m_AttackRange = entity.NormalAttackDamge;
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
            Debug.LogError("ChangeAIState from: none to:" + NewAIState.ToString());
        else
            Debug.LogError("ChangeAIState from: " + m_AIState.ToString() + " to: " + NewAIState.ToString());
        m_AIState = NewAIState;
        m_AIState.SetCharacterAI(this);
    }

    //攻击目标
    public virtual void Attack(BaseEntity entity)
    {
        //
        m_CoolDown -= Time.deltaTime;
        if (m_CoolDown > 0)
            return;
        m_CoolDown = ATTACK_COOLD_DOWN;

        //攻击目标
        (m_Entity as Monster).Attack();
    }

    //是否在攻击范围内
    public bool TargetInAttackRange(BaseEntity entity)
    {
        float dist = Vector3.Distance(m_Entity.transform.position, entity.transform.position);
        return (dist <= m_AttackRange);
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

    //停止移动
    public void StopMove()
    {
        m_Entity.EndMove();
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
}

