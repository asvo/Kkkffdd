using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */

public class Player : BaseEntity
{

    //    public Animator mAnimator;    
    private List<SkillData> mSkillList;
    public float leftBorder = -5;
    public float rightBorder = 5;

    public static MoveDir curMoveDir = MoveDir.None;

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

    public void LoadSettingData()
    {
        m_AttrValue = ValueManager.Instance().PlayerValueSettings;
        //if (jsdata != null)
        //{
        //    //NormalAttackCd = jsdata.dic_BaseValues[e_BaseValue.NormalAttackCd.ToString()];
        //    //NormalAttackRange = jsdata.dic_BaseValues[e_BaseValue.NormalAttackRange.ToString()];
        //    //NormalAttackDamgePoint = jsdata.dic_BaseValues[e_BaseValue.NormalAttackDamgePoint.ToString()];
        //    //NormalAttackDamge = (int)jsdata.dic_BaseValues[e_BaseValue.NormalAttackDamge.ToString()];
        //    InitMoveSpeed = jsdata.dic_BaseValues[e_BaseValue.MoveSpeed.ToString()];
        //}
    }

    public void LoadSkillSettingData()
    {
        SkillCfgUnit cfgUnit = SkillCfgMgr.Instance().GetSkillCfgBySlotId(SkillConst.NormalAttackSkillSlotId);
        if (null != cfgUnit)
        {
            m_AttrValue.NormalAttackCd = cfgUnit.SkillCd;
            m_AttrValue.NormalAttackRange = cfgUnit.DamageRange;
            m_AttrValue.NormalAttackDamgePoint = cfgUnit.DamageTime;
            m_AttrValue.NormalAttackDamge = cfgUnit.Damge;
        }
    }

    public void InitPlayer()
    {
        LoadSettingData();
        m_AttrValue.Health = 1;

        isDead = false;
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.localScale = Vector3.one;
        gameObject.layer = Util.PlayerLayer;
        MoveCtrl.CC2D.platformMask = 1 << Util.GroundLayer | 1 << Util.WallLayer | 1 << Util.MonsterLayer;

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
        PlayAnim("idle");
        IsPLayerMoing = false;
    }

    public static bool IsPLayerMoing = false;

    public override void Move(MoveDir moveDir)
    {
        curMoveDir = moveDir;
        bool isSameDir = MoveCtrl.GetCurrentFaceDir() == moveDir;
        if (mIsNormalAttackInCd)
        {
            if (isSameDir)
            {
                Util.LogAsvo("same dir : " + moveDir);
                return;     //目前（2017/06/20）攻击时只处理向后移动的行为
            }            
        }
        if (!isSameDir)
        {
            CancelNormalAttack();
            CancelSkill01();
        }
        SkillDataMgr.Instance().ClearActionTimeBySlot(SkillConst.NormalAttackSkillSlotId);
        //修正朝向
        base.RotateToDir(moveDir);
        if (TooNearToMonster(moveDir))
            EndMove();
        else
        {
            if (isSameDir)
            {
                if (!IsPLayerMoing)
                {
                    IsPLayerMoing = true;
                    base.Move(moveDir);
                }
            }
            else
            {
                IsPLayerMoing = true;
                base.Move(moveDir);
            }
        }
    }

    public override void EndMove()
    {
        base.EndMove();
        PlayAnim("idle");
        IsPLayerMoing = false;
    }

    private bool mIsNormalAttackInCd = false;

    public void FireSkill(int slot)
    {
        //       SkillCaster.Instance().CastSkill(mSkillList[slot], this);
        if (SkillConst.NormalAttackSkillSlotId == slot)
        {
            FireNormalAttack();
        }
        else if (SkillConst.PlayerSkill01SlotId == slot)
        {
            RushSkill rushSkill = Util.TryAddComponent<RushSkill>(gameObject);
            rushSkill.Cast();
        }
        else if (SkillConst.PlayerSkill02SlotId == slot)
        {
            JumpBackSkill jumpSkill = Util.TryAddComponent<JumpBackSkill>(gameObject);
            jumpSkill.Cast();
        }
    }

    private void CancelSkill01()
    {
        RushSkill rushSkill = Util.TryAddComponent<RushSkill>(gameObject);
        rushSkill.BreakSkill();
    }

    private void FireNormalAttack()
    {
        //能不能放技能(cd检查)
        if (mIsNormalAttackInCd)
            return;
        mIsNormalAttackInCd = true;
        StartCoroutine(ResetNormalAttackCd());

        StartCoroutine("NormalAttackPre");
    }

    private void CancelNormalAttack()
    {
        StopAnim("attack");
        PlayAnim("idle");
        StopCoroutine("NormalAttackPre");
        mIsNormalAttackInCd = false;
        //cancel normal action cd
        Util.LogAsvo("cancel normal attack");
        StayInDamgeZoneCtrler.Instance().CancelPlaceZone(SkillConst.NormalAttackSkillSlotId);
    }

    private IEnumerator ResetNormalAttackCd()
    {
        yield return new WaitForSeconds(m_AttrValue.NormalAttackCd);
        mIsNormalAttackInCd = false;
    }

    private void StopMove()
    {
        base.EndMove();
        StopAnim("run");
    }

    private IEnumerator NormalAttackPre()
    {
        StopMove();

        if (SkeletonAnim == null)
        {
            yield return null;
        }
        Spine.AnimationState animState = SkeletonAnim.state;
        Spine.Animation anim = animState.GetAnimation(0, "attack");
        //      SkeletonAnim.timeScale = anim.duration / NormalAttackCd;
        PlayAnim("attack");
        SkeletonAnim.state.AddAnimation(0, "idle", false, 0.8f);
        SkillDataMgr.Instance().SetOnActionTime(SkillConst.NormalAttackSkillSlotId);
        yield return new WaitForSeconds(m_AttrValue.NormalAttackDamgePoint);
        PlaceNormalAttackZone();
     //   CastNormalAttack();
    }    

    private void PlaceNormalAttackZone()
    {
        Spine.AnimationState animState = SkeletonAnim.state;
        Spine.Animation anim = animState.GetAnimation(0, "attack");
        float placeTime = anim.Duration - m_AttrValue.NormalAttackDamgePoint;
        if (placeTime < 0f)
            placeTime = 0f;
        float halfRange = m_AttrValue.NormalAttackRange * 0.5f;
        Vector2 placePos = transform.position + Vector3.right * halfRange;
        Vector2 placeSize = new Vector2(m_AttrValue.NormalAttackRange, EntityCollider.size.y);
        StayInDamgeZoneCtrler.Instance().PlaceZone(SkillConst.NormalAttackSkillSlotId,
            this, placePos, placeSize, placeTime, m_AttrValue.NormalAttackDamge);
    }

    private void CastNormalAttack()
    {
        //     Debug.LogError("cast noarml attack");
        // cur-weight = 1.14f;
        //        float normalAtackDist = 1.14f * 0.5f + 1.14f * 0.5f + 1.14f;
        BaseEntity target = Util.FindNereastTargetMonsterByDist(this, m_AttrValue.NormalAttackRange);
        if (null != target)
            Util.LogAsvo("find target: " + target.name);
        if (null != target)
        {
            //int damage = SkillDataMgr.Instance().IsSkill01BuffActive ? NormalAttackDamge : 2*NormalAttackDamge;
            DamagerHandler.Instance().CalculateDamage(this, target, m_AttrValue.NormalAttackDamge);
            if (SkillDataMgr.Instance().IsSkill01BuffActive)
            {
                SkillDataMgr.Instance().ReducePlayerAllSkillCd(1.0f);
            }
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
        PlayAnim("idle");

        if (Status == PlayerStatus.Die && this.gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            this.gameObject.SetActive(false);
        }

        //game over
        GameManager.Instance().PauseGame();
        GameOverUI.Instance.ShowUi();
    }

    private bool TooNearToMonster(MoveDir moveDir)
    {
        foreach (Monster monster in MonsterManager.Instance().ActiveMonsters)
        {
            if (monster.gameObject.activeSelf == false)
                continue;
            if (MonsterManager.Instance().DirToTarget(transform, monster.transform) == moveDir)
            {
                if (Vector2.Distance(monster.transform.position, transform.position) <= GameManager.NearestDistance)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

