using UnityEngine;
using System.Collections;

public class JumpBackSkill : MonoBehaviour {

    public float DamageTime = 0.5f;
    public float DamageRange = 3f;
    public int Damage = 2;

    const string ActionJumpName = "skill2_1";
    const string ActionAttackName = "skill2_2";
    const string ActionAttackOverName = "skill2_3";

    private BaseEntity mEntity;
    private bool mIsFiringSkill = false;
    private bool mHasAttack = false;
    private float mWaitTime = 0f;

    void OnEnable()
    {
        EventMgr.Instance().AttachEvent(EventsType.FireNoramlAttack, OnFireNormalAttack);
    }

    void OnDisable()
    {
        EventMgr.Instance().DettachEvent(EventsType.FireNoramlAttack, OnFireNormalAttack);
    }

    private void OnFireNormalAttack(object param)
    {
        if (!mIsFiringSkill || mHasAttack)  //只允许攻击一次
            return;
        Util.LogAsvo("Recv fire noraml attack Event..");
        mHasAttack = true;
        //play attack anim
        mEntity.PlayAnim(ActionAttackName);
        //start damage-calculate corutine
        StartCoroutine("CalculateSkillDamage");
    }

    public void Cast()
    {
        if (!this.enabled)
            enabled = true;
        Util.LogAsvo("cast skill 2--- jump back!");
        mEntity = GetComponent<BaseEntity>();

        mIsFiringSkill = true;
        mHasAttack = false;
        mWaitTime = 0f;
        //play anim
        mEntity.PlayAnim(ActionJumpName);        
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
            if (mWaitTime >= 1.0f)
            {
                EndSkill();
            }
        }
    }

    private void EndSkill()
    {
        mEntity.PlayAnim(ActionAttackOverName);
        mIsFiringSkill = false;
        enabled = false;
    }
}
