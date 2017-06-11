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

    public enum PlayerStatus
    {
        None = 0,
        Idle,
        Attack,
        Hit,
        CastSkill,
        Die
    }
    public PlayerStatus Status;

#region cfg-data
    public float NormalAttackCd = 1.5f;
    public float NormalAttackRange = 2.5f;
    public float NormalAttackDamgePoint = 0.3f;
    public int NormalAttackDamge = 1;

#endregion cfg-data

    public void InitPlayer()
    {
        Health = 1;

        isDead = false;
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
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

        Status = PlayerStatus.Idle;
        mAnimator.Play("八神Idle");
    }

    public override void Move(MoveDir moveDir)
    {
        CancelNormalAttack();
        if (TooNearToMonster(moveDir))
            EndMove();
        else
            base.Move(moveDir);
    }

    public override void EndMove()
    {
        base.EndMove();
    }
    
    private bool mIsNormalAttackInCd = false;

    public void FireSkill(int slot)
    {
        //能不能放技能(cd检查)
        if (mIsNormalAttackInCd)
            return;
        mIsNormalAttackInCd = true;
        StartCoroutine(ResetNormalAttackCd());

 //       SkillCaster.Instance().CastSkill(mSkillList[slot], this);
        StartCoroutine("NormalAttackPre");
    }

    private void CancelNormalAttack()
    {
        mAnimator.ResetTrigger("Attack");
        mAnimator.Play("八神Idle");
        StopCoroutine("NormalAttackPre");
    }

    private IEnumerator ResetNormalAttackCd()
    {
        yield return new WaitForSeconds(NormalAttackCd);
        mIsNormalAttackInCd = false;
    }

    private IEnumerator NormalAttackPre()
    {        
        if (mAnimator != null)
        {
            //mAnimator.SetTrigger("Attack");
            mAnimator.Play("LegAttack");
        }
        yield return new WaitForSeconds(NormalAttackDamgePoint);
        CastNormalAttack();
    }

    private void CastNormalAttack()
    {
   //     Debug.LogError("cast noarml attack");
        // cur-weight = 1.14f;
//        float normalAtackDist = 1.14f * 0.5f + 1.14f * 0.5f + 1.14f;
        BaseEntity target = Util.FindNereastTargetMonsterByDist(this, NormalAttackRange);
        if (null != target)
        {
            DamagerHandler.Instance().CalculateDamage(this, target, NormalAttackDamge);
        }
    }
    
    public override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);
    }

    public override void Die()
    {
        base.Die();
        Status = PlayerStatus.Die;
        //play die anim.
        mAnimator.Play("八神_Hit");

        StartCoroutine(WaitForDieAnimOver());

        //game over
        GameManager.Instance().PauseGame();
        GameOverUI.Instance.ShowUi();
    }

    IEnumerator WaitForDieAnimOver()
    {
        yield return new WaitForSeconds(1.0f);
        if (Status == PlayerStatus.Die)
        {
            this.gameObject.SetActive(false);
            StopAllCoroutines();
        }
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

