using UnityEngine;
using System.Collections;

public class SkillBase : MonoBehaviour {

    public float DamagePoint = 0.5f;
    public float DamageRange = 3.0f;
    public int Damage = 1;
    public float SkillCd = 3.0f;
    protected bool mIsInSkill = false;

    public void Cast() 
    {
        if (mIsInSkill)
            return;

        mIsInSkill = true;
        StartCoroutine("WaitToResetSkillCd");
        StartCoroutine("CalculateSkillDamage");
        OnCast();
    }

    protected IEnumerator WaitToResetSkillCd()
    {
        yield return new WaitForSeconds(SkillCd);
        mIsInSkill = false;
    }

    protected IEnumerator CalculateSkillDamage()
    {
        yield return new WaitForSeconds(DamagePoint);
        OnCalculateDamage();
    }

    protected virtual void OnCalculateDamage() { }

    protected virtual void OnCast() { }
}
