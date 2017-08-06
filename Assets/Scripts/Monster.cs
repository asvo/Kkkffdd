using UnityEngine;
using System.Collections;
using Spine.Unity;
using AIState;
using ValueModule;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class Monster : BaseEntity {

    public EnemyAI m_EnemyAI = null;
    public Monster FrontMonster = null;

    private MonsterValue mMonsterValue = null;
    public void LoadSettingData()
    {
        m_AttrValue = ValueManager.Instance().MonsterValueSettings;
        mMonsterValue = m_AttrValue as MonsterValue;
    }
    public MonsterValue GetMonsterValue()
    {
        return mMonsterValue;
    }

    public virtual void Spawn(EnemyAI AIBrain =null)
    {
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
        ForceDamege();
    }

    public void ForceDamege()
    {
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
                m_EnemyAI.ChangeAIState(new RestatsAIState(GetMonsterValue().RestatsTime));
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
            MoveCtrl.Move(moveDir, m_AttrValue.InitMoveSpeed);
        }
    }

    public override void EndMove()
    {
        base.EndMove();
    }

    public void Restats(MoveDir HitBackDir)
    {
        MoveCtrl.Move(HitBackDir, m_AttrValue.InitMoveSpeed * 5);
        ResetPoseAndPlayAnim("hit2", false);

        Invoke("EndMove", 0.1f);
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
        float prepareTime = Mathf.Abs(Vector2.Distance(this.transform.position, to.transform.position)) * GetMonsterValue().JumpPrepareTimeRate;
        return prepareTime;
    }

    public void ChangeCollider(bool open)
    {
        MoveCtrl.CC2D.platformMask = 1 << Util.GroundLayer | 1 << Util.WallLayer;
    }
}

