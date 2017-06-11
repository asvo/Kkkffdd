using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class Monster : BaseEntity {

    public EnemyAI enemyAi = null;
    /// <summary>
    /// 攻击范围
    /// </summary>
    public float AttackRange = 0.1f;
    /// <summary>
    /// 攻击频率
    /// </summary>
    public float AttckRate = 1;
    public float RestatsTime = 0.3f;
    public Animator mAnimator = null;

    public void Spawn()
    {

        Health = 3;
        isDead = false;
        enemyAi = Util.TryAddComponent<EnemyAI>(gameObject);
        enemyAi.InitEnemyAI();

        mAnimator = GetComponentInChildren<Animator>();
    }

    public void Attack()
    {
        Debug.LogError(gameObject.name + " is Attack!" +Time.realtimeSinceStartup);
        if (mAnimator != null)
        {
            mAnimator.SetBool("Attack", true);
        }
        //范围判断? if need

        DamagerHandler.Instance().CalculateDamage(this, GameManager.
            Instance().MainPlayer, 10);
    }

    public override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);
        Restats();
        Damage();
    }

    public void Damage()
    {
        if (enemyAi == null)
            return;
        enemyAi.Damage();       
    }

    public override void Die()
    {
        base.Die();
        HitFly();
        StartCoroutine(TimeForFly());
    }

    IEnumerator TimeForFly()
    {
        yield return new WaitForSeconds(4f);
        MonsterManager.Instance().MonsterDie(this);
    }

    public void MoveToPlayer()
    {
        //if (GameManager.Instance().player != null)
        //{
        //    float distanceToPlayer = Vector2.Distance(GameManager.Instance().player.transform.position, transform.position);
        //    if (distanceToPlayer < GameManager.NearestDistance)
        //    {
        //        //Debug.Log(gameObject.name + " distance to player " + distanceToPlayer);
        //        base.EndMove();
        //        return;
        //    }

            MoveDir moveDir = MonsterManager.Instance().DirToPlayer(this);
            Debug.LogError(moveDir);

            base.Move(moveDir);
        //}
    }

    public void Restats()
    {
        if (mAnimator != null)
        {
            mAnimator.Play("八神_HitBack");
        }
    }

    public void EndRestats()
    {
        if (mAnimator != null)
        {
            mAnimator.Play("八神Idle");
        }
    }

    public void HitFly()
    {
        if (mAnimator != null)
        {
            mAnimator.Play("八神_Hit");
        }
    }

    IEnumerator ResetApplyRootMotion()
    {
        yield return new WaitForSeconds(2f);
        if (mAnimator != null)
        {
            mAnimator.applyRootMotion = false;
        }
    }
}

