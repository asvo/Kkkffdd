using UnityEngine;
using System.Collections;
using Spine.Unity;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class Monster : BaseEntity {

    public EnemyAI m_EnemyAI = null;
    public float ChaseRange = 5;
    public float RestatsTime = 0.3f;
    public float MaxConfusedTime = 20;

    #region Jump parameters
    public float JumpPrepareTime = 0.5f;
    public float JumpAttckRange = 6;
    public float JumpTime = 1;
    public float JumpHeight = 3;
    public bool HaveSpecialSkill = false;
    #endregion

    public float RollBackRange = 3;

    public Monster FrontMonster = null;

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

    public virtual void Spawn(EnemyAI AIBrain =null)
    {
        Health = 3;
        isDead = false;

        m_EnemyAI = AIBrain;       

        if (SkeletonAnim != null)
        {
            SkeletonAnim.skeleton.SetToSetupPose();
            SkeletonAnim.state.ClearTracks();
        }
    }

    void Start()
    {
        MoveCtrl.CC2D.onControllerCollidedEvent += onTriggerEnterEvent;
    }

    void onTriggerEnterEvent(RaycastHit2D hit)
    {
        if (hit.collider == null)
        {
            FrontMonster = null;
            return;
        }
        Monster monster = hit.collider.GetComponent<Monster>();
        if (monster != null)
        {
            if (monster._CurrentAIState.Equals(typeof(JumpAttackAIState).ToString()))
                return;

            FrontMonster = monster;
            if (m_EnemyAI != null)
            {
                m_EnemyAI.ChangeAIState(new IdleAIState());
            }
        }
    }

    void Update()
    {
        if (isDead)
            return;
        if (m_EnemyAI != null && m_EnemyAI.GetCurrentState() != null)
        {
            _CurrentAIState = m_EnemyAI.GetCurrentState();
            m_EnemyAI.Update(new System.Collections.Generic.List<BaseEntity>() { GameManager.Instance().MainPlayer });
        }
        else
        {
            _CurrentAIState = "None";
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
        ResetPoseAndPlayAnim("attack1", false);
        //范围判断? if need
        StartCoroutine(CalculateDamage());

    }

    IEnumerator CalculateDamage()
    {
        yield return new WaitForSeconds(0.5f);
        if (!GameManager.Instance().bInvincible)
        {
            DamagerHandler.Instance().CalculateDamage(this, GameManager.Instance().MainPlayer, 10);
        }
    }

    public override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);

        if (m_EnemyAI != null)
        {
            if (m_EnemyAI.GetCurrentState().Equals(typeof(JumpAttackAIState).ToString()))
            {
                JumpAttackAction jumpAction = GetComponent<JumpAttackAction>();
                if (jumpAction != null && jumpAction.enabled)
                {
                    jumpAction.ForceStop();
                }
                StartCoroutine(HitOnSpace());
            }
            else
            {
                m_EnemyAI.ChangeAIState(new RestatsAIState(RestatsTime));
            }
        }
    }

    public override void Die()
    {
        base.Die();
        StartCoroutine(DieOnGround());
    }

    IEnumerator DieOnGround()
    {
        //地面死亡
        ResetPoseAndPlayAnim("hitfly1", false);
        float delaydestroy = 0;
        var trackEntry = SkeletonAnim.state.GetCurrent(0);
        if (trackEntry != null)
        {
            delaydestroy = trackEntry.endTime;
        }
        yield return new WaitForSeconds(delaydestroy);

        ResetPoseAndPlayAnim("hitfly2", false);
        trackEntry = SkeletonAnim.state.GetCurrent(0);
        if (trackEntry != null)
        {
            delaydestroy = trackEntry.endTime;
        }
        yield return new WaitForSeconds(delaydestroy);
        MonsterManager.Instance().MonsterDie(this);
    }

    IEnumerator HitOnSpace()
    {
        //地面死亡
        ResetPoseAndPlayAnim("hitfly2", false);
        float delaydestroy = 0;
        var trackEntry = SkeletonAnim.state.GetCurrent(0);
        if (trackEntry != null)
        {
            delaydestroy = trackEntry.endTime;
        }
        yield return new WaitForSeconds(delaydestroy);

        ResetPoseAndPlayAnim("hitfly3", false);
        trackEntry = SkeletonAnim.state.GetCurrent(0);
        if (trackEntry != null)
        {
            delaydestroy = trackEntry.endTime;
        }
        yield return new WaitForSeconds(delaydestroy);
        if (!isDead)
        {
            m_EnemyAI.ChangeAIState(new IdleAIState());
        }
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
    [Header("状态")]
    public string _CurrentAIState;

    public virtual  float PrepareJumpTime(BaseEntity to)
    {
        float prepareTime = Mathf.Abs(Vector2.Distance(this.transform.position, to.transform.position)) * 0.1f;
        return prepareTime;
    }

    public void ChangeCollider(bool open)
    {
        MoveCtrl.CC2D.platformMask = 1 << Util.GroundLayer | 1 << Util.WallLayer;
    }
}

