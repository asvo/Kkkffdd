using UnityEngine;
using System.Collections;
using Spine.Unity;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class Monster : BaseEntity {

    public EnemyAI enemyAi = null;
    public float RestatsTime = 0.3f;

    public void LoadSettingData()
    {
        JsDataBaseValue jsdata = ValueManager.Instance().MonsterValueSettings;
        if (jsdata != null)
        {
            NormalAttackCd = jsdata.dic_BaseValues[e_BaseValue.NormalAttackCd.ToString()];
            NormalAttackRange = jsdata.dic_BaseValues[e_BaseValue.NormalAttackRange.ToString()];
            NormalAttackDamgePoint = jsdata.dic_BaseValues[e_BaseValue.NormalAttackDamgePoint.ToString()];
            NormalAttackDamge = (int)jsdata.dic_BaseValues[e_BaseValue.NormalAttackDamge.ToString()];
            RestatsTime = jsdata.dic_BaseValues[e_BaseValue.RestatsTime.ToString()];
            InitMoveSpeed = (int)jsdata.dic_BaseValues[e_BaseValue.MoveSpeed.ToString()];
        }
    }

    public void Spawn()
    {
        LoadSettingData();
        Health = 3;
        isDead = false;
        enemyAi = Util.TryAddComponent<EnemyAI>(gameObject);
        enemyAi.InitEnemyAI();

        if (SkeletonAnim != null)
        {
            SkeletonAnim.skeleton.SetToSetupPose();
            SkeletonAnim.state.ClearTracks();
        }
    }

    public void Attack()
    {
        if (GameManager.Instance().MainPlayer.isDead)
            return;        
        PlayAnim("run", false);
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
        float delaydestroy = 0;
        var trackEntry = SkeletonAnim.state.GetCurrent(0);
        if (trackEntry != null)
        {
            delaydestroy = trackEntry.endTime;
        }
        yield return new WaitForSeconds(delaydestroy);
        MonsterManager.Instance().MonsterDie(this);
    }

    public void MoveToPlayer()
    {
        MoveDir moveDir = MonsterManager.Instance().DirToPlayer(this);
        Move(moveDir);
    }
    public override void Move(MoveDir moveDir)
    {
        //     Debug.LogError("moveDir"+ moveDir);
        if (MoveCtrl != null)
        {
            if (moveDir == MoveDir.Left)
            {
                SkeletonAnim.Skeleton.FlipX = false;
            }
            else if (moveDir == MoveDir.Right)
            {
                SkeletonAnim.Skeleton.FlipX = true;
            }
            PlayAnim("run", true);
            MoveCtrl.Move(moveDir, InitMoveSpeed);
        }
    }


    public override void EndMove()
    {
        base.EndMove();
    }


    public void Restats()
    {
        SkeletonAnim.skeleton.SetToSetupPose();
        SkeletonAnim.state.ClearTracks();
        PlayAnim("hit2", false);
    }

    public void EndRestats()
    {
        SkeletonAnim.skeleton.SetToSetupPose();
        SkeletonAnim.state.ClearTracks();
        PlayAnim("run", true);
    }

    public void HitFly()
    {
        SkeletonAnim.skeleton.SetToSetupPose();
        SkeletonAnim.state.ClearTracks();
        PlayAnim("hit", true);
    }
}

