using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */

public class Player : BaseEntity {

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
        JsDataBaseValue jsdata = ValueManager.Instance().PlayerValueSettings;
        if (jsdata != null)
        {
            NormalAttackCd = jsdata.dic_BaseValues[e_BaseValue.NormalAttackCd.ToString()];
            NormalAttackRange = jsdata.dic_BaseValues[e_BaseValue.NormalAttackRange.ToString()];
            NormalAttackDamgePoint = jsdata.dic_BaseValues[e_BaseValue.NormalAttackDamgePoint.ToString()];
            NormalAttackDamge = (int)jsdata.dic_BaseValues[e_BaseValue.NormalAttackDamge.ToString()];
            InitMoveSpeed = jsdata.dic_BaseValues[e_BaseValue.MoveSpeed.ToString()];
        }
    }

    public void InitPlayer()
    {
        LoadSettingData();
        Health = 1;

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
    }    

    public override void Move(MoveDir moveDir)
    {
        curMoveDir = moveDir;
        if (mIsNormalAttackInCd)
        {
            MoveDir dir = MoveCtrl.GetCurrentFaceDir();
            if (dir == moveDir)
                return;     //目前（2017/06/20）攻击时只处理向后移动的行为
            CancelNormalAttack();
        }        

        if (TooNearToMonster(moveDir))
            EndMove();
        else
            base.Move(moveDir);
    }

    public override void EndMove()
    {
        base.EndMove();
        PlayAnim("idle");
    }
    
    private bool mIsNormalAttackInCd = false;

    public void FireSkill(int slot)
    {
 //       SkillCaster.Instance().CastSkill(mSkillList[slot], this);
        if (0 == slot)
        {
            FireNormalAttack();
        }
        else if (1 == slot)
        {
            RushSkill rushSkill = Util.TryAddComponent<RushSkill>(gameObject);
            rushSkill.Cast();
        }
        else if (2 == slot)
        {
            JumpBackSkill jumpSkill = Util.TryAddComponent<JumpBackSkill>(gameObject);
            jumpSkill.Cast();
        }
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
    }

    private IEnumerator ResetNormalAttackCd()
    {
        yield return new WaitForSeconds(NormalAttackCd);
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
        Spine.Animation anim = animState.GetAnimation(0,"attack");
  //      SkeletonAnim.timeScale = anim.duration / NormalAttackCd;
        PlayAnim("attack");
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
            //normal attack hit on target.
            if (SkillDataMgr.Instance().IsSkill01BuffActive)
            {
                //buff效果。使所有技能cd-1
                SkillDataMgr.Instance().ReducePlayerAllSkillCd(1, 1f);
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
                if (Vector2.Distance(monster.transform.position, transform.position) <= GameManager.NearestDistance + monster.NormalAttackRange)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

