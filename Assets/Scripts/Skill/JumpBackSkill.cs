using UnityEngine;
using System.Collections;

public class JumpBackSkill : MonoBehaviour {

    public float DamageTime = 0.5f;
    public float DamageRange = 3f;
    public int Damage = 2;
    //specail parameters
    public float BackMoveSpeed = 3f;
    public float BackMoveTime = 1.2f;

    const string ActionJumpName = "skill2_1";
    const string ActionAttackName = "skill2_2";
    const string ActionAttackOverName = "skill2_3";

    private BaseEntity mEntity;
    private bool mIsFiringSkill = false;
    private bool mHasAttack = false;
    private float mWaitTime = 0f;
    private bool mHasPlayFallAction = false;

    void OnEnable()
    {
        EventMgr.Instance().AttachEvent(EventsType.FireNoramlAttack, OnFireNormalAttack);
    }

    void OnDisable()
    {
        EventMgr.Instance().DettachEvent(EventsType.FireNoramlAttack, OnFireNormalAttack);
    }
    
    public void Cast()
    {
        if (!this.enabled)
            enabled = true;
        Util.LogAsvo("cast skill 2--- jump back!");
        mEntity = GetComponent<BaseEntity>();

        mIsFiringSkill = true;
        SkillDataMgr.Instance().IsSkill02Active = true;
        mHasAttack = false;
        mWaitTime = 0f;
        mHasPlayFallAction = false;
        //play anim
        mEntity.PlayAnim(ActionJumpName);        
        //move back
        mEntity.MoveCtrl.MoveForward(-BackMoveSpeed);
    }

    private void OnFireNormalAttack(object param)
    {
        if (!mIsFiringSkill || mHasAttack)  //只允许攻击一次
            return;
        Util.LogAsvo("Recv fire noraml attack Event..");
        mHasAttack = true;
        //play attack anim
        mEntity.SkeletonAnim.skeleton.SetToSetupPose();
        mEntity.PlayAnim(ActionAttackName);
        //start damage-calculate corutine
        StartCoroutine("CalculateSkillDamage");
    }

    IEnumerator CalculateSkillDamage()
    {
        yield return new WaitForSeconds(DamageTime);
        BaseEntity target = Util.FindNereastTargetMonsterByDist(mEntity, DamageRange);
        Util.LogAsvo("Skill02 attack !");
        if (null != target)
        {
            Util.LogAsvo("Skill02 attack, targetname = " + target.name);
            DamagerHandler.Instance().CalculateDamage(mEntity, target, Damage);
        }

        EndSkill();
    }

    void Update()
    {
        if (!mIsFiringSkill)
        {
            return;
        }
        if (!mHasAttack)
        {            
            mWaitTime += Time.deltaTime;
            if (mWaitTime >= 1.0f && !mHasPlayFallAction)
            {
                mHasPlayFallAction = true;
                Debug.LogError("play fall");
                mEntity.PlayAnim(ActionAttackOverName);
            }
            else if (mWaitTime >= BackMoveTime)
            {
                EndSkill();
            }
        }
    }

    private void EndSkill()
    {
        mEntity.MoveCtrl.EndMove();
        mIsFiringSkill = false;
        enabled = false;
        SkillDataMgr.Instance().IsSkill02Active = false;
    }
}
