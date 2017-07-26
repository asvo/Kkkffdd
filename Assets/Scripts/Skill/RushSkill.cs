using UnityEngine;
using System.Collections;

public class RushSkill : MonoBehaviour {
    
    private float RushSpeed = 2f;
    private float DamageTime = 0.5f;
    private float RushDamageRange = 3f;
    private int RushDamage = 2;
    private float RushSkillCd = 3.0f;
    //move time
    private float MaxRushMoveTime = 1.0f;

    private float mCurSkillTime;
    private bool mIsInRush = false;

    private BaseEntity mEntity;
    const string SpineAnimName = "skill1";
	
    public void Cast()
    {
        Util.LogAsvo("cast skill 1--- rush!");
        LoadSkillCfgData();
        mEntity = gameObject.GetComponent<BaseEntity>();
        //clear
        StopCoroutine("WaitToEndMove");
        StopCoroutine("CalculateRushDamage");
        //clear spine
        mEntity.SkeletonAnim.skeleton.SetToSetupPose();
        mEntity.SkeletonAnim.state.ClearTracks();

        mEntity.MoveCtrl.MoveForward(RushSpeed);
        StartCoroutine("WaitToEndMove");
        //start damage-calculate corutine
        StartCoroutine("CalculateRushDamage");
        //play anim
        mEntity.PlayAnim(SpineAnimName);
        SkillDataMgr.Instance().SetOnActionTime(SkillConst.PlayerSkill01SlotId);
    }

    private void LoadSkillCfgData()
    {
        SkillCfgUnit cfgUnit = SkillCfgMgr.Instance().GetSkillCfgBySlotId(SkillConst.PlayerSkill01SlotId);
        if (null != cfgUnit)
        {
            RushDamage = cfgUnit.Damge;
            RushDamageRange = cfgUnit.DamageRange;
            DamageTime = cfgUnit.DamageTime;
            MaxRushMoveTime = cfgUnit.SkillMoveTime;
            RushSpeed = cfgUnit.SkillMoveSpeed;
            RushSkillCd = cfgUnit.SkillCd;
        }
    }

    private IEnumerator WaitToEndMove()
    {
        yield return new WaitForSeconds(MaxRushMoveTime);
        Util.LogAsvo("End Rush Move.");
        //just for check
        StopCoroutine("CalculateRushDamage");
        mEntity.EndMove();
        EndSkill();
    }

    private IEnumerator CalculateRushDamage()
    {
        yield return new WaitForSeconds(DamageTime);
        BaseEntity target = Util.FindNereastTargetMonsterByDist(mEntity, RushDamageRange);
        Util.LogAsvo("Attack Rush !");
        if (null != target)
        {
            Util.LogAsvo("Attack Rush : " + target.name);
            DamagerHandler.Instance().CalculateDamage(mEntity, target, RushDamage);
        }
    }

    private void EndSkill()
    {
             
    }
}
