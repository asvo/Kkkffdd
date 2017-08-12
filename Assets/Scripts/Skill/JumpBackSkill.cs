using UnityEngine;
using System.Collections;

public class JumpBackSkill : MonoBehaviour {

    private float DamageTime = 0.5f;
    private float DamageRange = 3f;
    private int Damage = 2;
    //specail parameters
    private float BackMoveSpeed = 3f;
    private float BackMoveTime = 1.2f;

    const string ActionJumpName = "skill2_1";
    const string ActionAttackName = "skill2_2";
    const string ActionAttackOverName = "skill2_3";

    private BaseEntity mEntity;
    private bool mIsFiringSkill = false;
    private bool mHasAttack = false;
    private float mWaitTime = 0f;
    private bool mHasPlayFallAction = false;
    private Vector3 mStartSkillPos;

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
        LoadSkillCfgData();
        mEntity = GetComponent<BaseEntity>();

        mIsFiringSkill = true;
        SkillDataMgr.Instance().IsSkill02Active = true;
        mHasAttack = false;
        mWaitTime = 0f;
        mHasPlayFallAction = false;
        mStartSkillPos = mEntity.transform.position;
        //play anim
        mEntity.PlayAnim(ActionJumpName);
        SkillDataMgr.Instance().SetOnActionTime(SkillConst.PlayerSkill02SlotId);
        //move back
        mEntity.MoveCtrl.MoveForward(-BackMoveSpeed);
    }

    private void LoadSkillCfgData()
    {
        SkillCfgUnit cfgUnit = SkillCfgMgr.Instance().GetSkillCfgBySlotId(SkillConst.PlayerSkill02SlotId);
        if (null != cfgUnit)
        {
            Damage = cfgUnit.Damge;
            DamageRange = cfgUnit.DamageRange;
            DamageTime = cfgUnit.DamageTime;
            BackMoveTime = cfgUnit.SkillMoveTime;
            BackMoveSpeed = cfgUnit.SkillMoveSpeed;
        }
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
        Util.LogAsvo("Skill02 attack !");
        //CastDamage();

        PlaceSkillZone();
        EndSkill();
    }

    private void PlaceSkillZone()
    {
        float halfRange = DamageRange * 0.5f;
        Vector2 placePos = transform.position + Vector3.right * halfRange;
        Vector2 placeSize = new Vector2(DamageRange, mEntity.EntityCollider.size.y);
        StayInDamgeZoneCtrler.Instance().PlaceZone(SkillConst.PlayerSkill02SlotId,
            mEntity, placePos, placeSize, BackMoveTime - DamageTime, Damage);
    }

    private void CastDamage()
    {
        float moveDist = Vector3.Distance(mStartSkillPos, mEntity.transform.position);
        BaseEntity target = Util.FindNereastTargetMonsterByDist(mEntity, DamageRange + moveDist);
        if (null != target)
        {
            Util.LogAsvo("Skill02 attack, targetname = " + target.name);
            DamagerHandler.Instance().CalculateDamage(mEntity, target, Damage);
        }
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
