using UnityEngine;
using System.Collections;

public class RushSkill : MonoBehaviour {
    
    public float RushSpeed = 2f;
    public float DamageTime = 0.5f;
    public float RushDamageRange = 3f;
    public int RushDamage = 2;
    public float RushSkillCd = 3.0f;
    //move time
    public float MaxRushMoveTime = 1.0f;

    private float mCurSkillTime;
    private bool mIsInRush = false;

    private BaseEntity mEntity;
    const string SpineAnimName = "skill1";
	
    public void Cast()
    {
        //check cd
        if (mIsInRush)
            return;

        mIsInRush = true;
        StartCoroutine("WaitToResetSkillCd");
        Util.LogAsvo("cast skill 1--- rush!");
        mEntity = gameObject.GetComponent<BaseEntity>();
        mEntity.MoveCtrl.MoveForward(RushSpeed);
        StartCoroutine("WaitToEndMove");
        //start damage-calculate corutine
        StartCoroutine("CalculateRushDamage");
        //play anim
        mEntity.PlayAnim(SpineAnimName);
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

    private IEnumerator WaitToResetSkillCd()
    {
        yield return new WaitForSeconds(RushSkillCd);
        mIsInRush = false;
    }
}
