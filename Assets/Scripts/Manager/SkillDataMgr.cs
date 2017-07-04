using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillDataMgr : Single<SkillDataMgr>
{
    public SkillCdData SkillCdData01 = new SkillCdData();
    

    public void SetOnSkillCd01(float cdTime, float curTime, bool isInToCd)
    {
        SkillCdData01.SkillCdTime = cdTime;
        SkillCdData01.mEndCdTime = curTime + cdTime;
        SkillCdData01.mIsInCd = isInToCd;
    }
}

public class SkillCdData
{
    public float SkillCdTime = 3.0f;
    public float mEndCdTime = 0f;
    public bool mIsInCd = false;
}