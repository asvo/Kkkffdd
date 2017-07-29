using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */

public class EnemyAI : ICharacterAI
{
    public EnemyType m_EnemyType = EnemyType.Monster_Normal;
    protected float JumpAttckRange = 0;
    public EnemyAI(BaseEntity entity,EnemyType enemyType) : base(entity)
    {
        base.m_Entity = entity;
        m_EnemyType = enemyType;

        m_AttackRange = entity.NormalAttackRange;
        m_ChaseRange = (entity as Monster).ChaseRange;
        JumpAttckRange = (entity as Monster).JumpAttckRange;        
    }

    public override void Idle()
    {
        StopMove();
        (m_Entity as Monster).Idle();
    }

    public override bool ObstacleOnWay()
    {
        Monster monster = m_Entity as Monster;
        if (monster != null)
            return monster.FrontMonster != null && !monster.FrontMonster.isDead;
        return false;
    }

    public override void Attack(BaseEntity Target)
    {
        //
        m_CoolDown -= Time.deltaTime;
        if (m_CoolDown > 0)
            return;
        m_CoolDown = ATTACK_COOLD_DOWN;
        //攻击目标
        (m_Entity as Monster).Attack();
    }

    /// <summary>
    /// 跳跃攻击
    /// </summary>
    /// <param name="Target"></param>
    /// <param name="CallBack"></param>
    public override void JumpAttack(BaseEntity Target,System.Action CallBack)
    {
        Monster monster = m_Entity as Monster;
        if (monster == null)
        {
            if (CallBack != null)
            {
                CallBack();
            }
            return;
        }
        JumpAttackAction jumpAction = Util.TryAddComponent<JumpAttackAction>(monster.gameObject);
        jumpAction.Attack(Target, monster.JumpHeight, monster.JumpTime, monster.PrepareJumpTime(Target), CallBack);
    }

    /// <summary>
    /// 翻滚闪避
    /// </summary>
    /// <param name="Attacker"></param>
    /// <param name="CallBack"></param>
    public virtual void RollBack(BaseEntity Attacker, System.Action CallBack)
    {
        Monster monster = m_Entity as Monster;
        if (monster == null)
        {
            if (CallBack != null)
            {
                CallBack();
            }
            return;
        }

        RollBackAction rollBackAction = Util.TryAddComponent<RollBackAction>(m_Entity.gameObject);
        rollBackAction.FireRollBack(Attacker, CallBack);
    }

    public override bool TargetInJumpAttckRange(BaseEntity Target)
    {
        return (distanceToTarget(Target) <= JumpAttckRange);
    }

    public override void Restats()
    {
        (m_Entity as Monster).Restats();
    }
}

/// <summary>
/// 怪物类型
/*  怪物技能
       1 跳跃攻击 当怪物处于攻击状态但因为被阻挡无法攻击到人物时，会触发该技能，使用该技能，会取消
           碰撞效果。跳跃攻击过程中被角色击中会被击飞，击飞距离视跳跃距离而定。释放该技能需要读条。读条
          时间视距离而定。
        
       2 后翻 如果该次攻击会使怪物死亡，则会触发该技能，效果为躲闪该次攻击并向后翻滚一小段距离。该技能
          只能使用一次。

    精英怪例1 同时具有技能1和2。     
    精英怪例2 只具有技能1，但获得强化，效果为：释放完一次技能1之后，无论是没有击中角色，还是
           被人物击飞，都会继续对人物使用技能1，且不需要读条无视距离（不管离人物多远）。         
    精英怪例3 同时具有技能1和2 但获得强化，效果为：技能2不再限定使用次数，但具有5scd。且释放
                 技能2后会立即使用技能1，且不需要读条无视距离。
 */
/// </summary>
public enum EnemyType
{
    Monster_Normal = 0,
    Monster_TwoSkill = 1,//同时具有技能1和2。
    Monster_OnlyJumpSkill = 2,
    Monster_TwoSkillStrength =3,

    Max = 10
}

