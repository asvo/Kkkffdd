using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */

public class Player : BaseEntity {

    public Animator mAnimator;

    private List<SkillData> mSkillList;

    public void InitPlayer()
    {
        Health = 1;

        gameObject.transform.position = Vector3.zero;
        gameObject.transform.localScale = Vector3.one;
        gameObject.layer = Util.PlayerLayer;
        MoveCtrl.CC2D.platformMask = 1 << Util.MonsterLayer;

        //mSkillList = new List<SkillData>();
        //SkillData sk = new SkillData
        //{
        //    SkillId = 1001,
        //    SlotId = 0,
        //    DamagePoint = 0.3f,
        //    Damage = 3f,
        //    AttackRange = 5f,
        //    AnimationName = "Attack",
        //    Priority = ActPriority.NormalAttack
        //};
    }

    public override void Move(MoveDir moveDir)
    {
        if (TooNearToMonster(moveDir))
            EndMove();
        else
            base.Move(moveDir);
    }

    public override void EndMove()
    {
        base.EndMove();
    }

    public void FireSkill(int slot)
    {
 //       SkillCaster.Instance().CastSkill(mSkillList[slot], this);
        StartCoroutine(NormalAttackPre());
    }

    private IEnumerator NormalAttackPre()
    {        
        if (mAnimator != null)
        {
            mAnimator.SetTrigger("Attack");
        }
        yield return new WaitForSeconds(0.3f);
        CastNormalAttack();
    }

    private void CastNormalAttack()
    {
        BaseEntity target = Util.FindNereastTargetMonsterByDist(this, 50f);
        if (null != target)
        {
            DamagerHandler.Instance().CalculateDamage(this, target, 1);
        }
    }
    
    public override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);
    }

    public override void Die()
    {
        //play die anim.

        //game over
        Debug.Log("Player Died, Game Over~");
    }

    private bool TooNearToMonster(MoveDir moveDir)
    {
        foreach (Monster monster in MonsterManager.Instance().ActiveMonsters)
        {
            if (MonsterManager.Instance().DirToTarget(transform, monster.transform) == moveDir)
            {
                if (Vector2.Distance(monster.transform.position, transform.position) <= GameManager.NearestDistance + monster.AttackRange)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

