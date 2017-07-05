using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillDataMgr : Single<SkillDataMgr>
{
    public SkillCdData SkillCdData01 = new SkillCdData();
    public SkillCdData Skill01BuffCdData = new SkillCdData();
    public bool IsSkill01BuffActive = false;

    public void SetOnSkill01BuffCdData(float cdTime, float curTime, bool isInToCd)
    {
        Skill01BuffCdData.SkillCdTime = cdTime;
        Skill01BuffCdData.mEndCdTime = curTime + cdTime;
        Skill01BuffCdData.mIsInCd = isInToCd;
    }

    public void SetOnSkillCd01(float cdTime, float curTime, bool isInToCd)
    {
        SkillCdData01.SkillCdTime = cdTime;
        SkillCdData01.mEndCdTime = curTime + cdTime;
        SkillCdData01.mIsInCd = isInToCd;
    }

    public void ReducePlayerAllSkillCd(float reduceAmount)
    {
        SkillCdData01.AddCd(-reduceAmount);
    }
}

public class SkillCdData
{
    public float SkillCdTime = 3.0f;
    public float mEndCdTime = 0f;
    public bool mIsInCd = false;

    public void AddCd(float reduceAmount)
    {
        mEndCdTime += reduceAmount;
    }
}