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
    public float ChaseRange = 5;
    public float RestatsTime = 0.3f;
    public float MaxConfusedTime = 20;

    #region Jump parameters
    public float JumpPrepareTime = 0.5f;
    public float JumpAttckRange = 6;
    public float JumpTime = 1;
    public float JumpHeight = 3;
    #endregion

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

        enemyAi = new EnemyAI(this);
        enemyAi.ChangeAIState(new ConfuseAIState(MaxConfusedTime));

        if (SkeletonAnim != null)
        {
            SkeletonAnim.skeleton.SetToSetupPose();
            SkeletonAnim.state.ClearTracks();
        }
    }

    void Update()
    {
        if (isDead)
            return;
        if (enemyAi != null && enemyAi.GetCurrentState() != null)
        {
            enemyAi.Update(new System.Collections.Generic.List<BaseEntity>() { GameManager.Instance().MainPlayer });
        }
    }

    public void Idle()
    {
        ResetPoseAndPlayAnim("run", true);
    }

    public void Attack()
    {
        if (GameManager.Instance().MainPlayer.isDead)
            return;
        ResetPoseAndPlayAnim("run", false);
        //范围判断? if need

        DamagerHandler.Instance().CalculateDamage(this, GameManager.
            Instance().MainPlayer, 10);
    }

    public void JumpAttck(BaseEntity Target)
    {
        JumpAttackAcition jumpAction = Util.TryAddComponent<JumpAttackAcition>(this.gameObject);
        jumpAction.Attack(Target, JumpHeight, JumpTime, EndJumpPose);
    }   

    private void PlayJumpPose()
    {
        ResetPoseAndPlayAnim("attack2_End", false);
    }

    private void EndJumpPose()
    {
        Util.LogHW("Jump over!");
    }


    public override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);
        if (enemyAi != null)
        {
            enemyAi.ChangeAIState(new RestatsAIState(RestatsTime));
        }
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
        ResetPoseAndPlayAnim("hit2", false);
    }

    public void EndRestats()
    {
        ResetPoseAndPlayAnim("run", true);
    }

    public void HitFly()
    {
        ResetPoseAndPlayAnim("hit1", true);
    }

    public void ResetPoseAndPlayAnim(string Anim, bool isLoop)
    {
        SkeletonAnim.skeleton.SetToSetupPose();
        SkeletonAnim.state.ClearTracks();
        PlayAnim(Anim, isLoop);
    }

    public MoveDir DirToTarget(BaseEntity Target)
    {
        return MonsterManager.Instance().DirToTarget(this.transform, Target.transform);
    }

    public string _CurrentAIState
    {
        get
        {
            if (enemyAi != null)
            {
                return enemyAi.GetCurrentState();
            }
            else
            {
                return "None";
            }
        }
    }
}

