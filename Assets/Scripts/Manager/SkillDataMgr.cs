using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillDataMgr : Single<SkillDataMgr>
{
    private Dictionary<int, SkillCdData> mSkillCdData = new Dictionary<int, SkillCdData>();

    public bool IsSkill01BuffActive = false;
    public bool IsSkill02Active = false;

    public void InitSkillCdData()
    {
        mSkillCdData.Add(1, new SkillCdData());
        mSkillCdData.Add(11, new SkillCdData()); //skill 1的buff效果cd
        mSkillCdData.Add(2, new SkillCdData());

        IsSkill01BuffActive = false;
        SetOnSkillCd(11, 1f, 0f, false);
    }

    public void SetOnSkillCd(int slotId, float cdTime, float curTime, bool isInToCd)
    {
        SkillCdData cddata = GetSkillCdDataBySlotId(slotId);
        if (null == cddata)
            return;
        cddata.SetOnCd(cdTime, curTime, isInToCd);
    }

    public SkillCdData GetSkillCdDataBySlotId(int slotId)
    {
        SkillCdData skilcd = null;
        if (!mSkillCdData.TryGetValue(slotId, out skilcd))
        {
            Debug.Log("不存在的技能slotId : " + slotId);
        }
        return skilcd;
    }

    public void ReducePlayerAllSkillCd(int slotId, float reduceAmount)
    {
        SkillCdData cddata = GetSkillCdDataBySlotId(slotId);
        if (null == cddata)
            return;
        cddata.AddCd(-reduceAmount);
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

    public void SetOnCd(float cdTime, float curTime, bool isInToCd)
    {
        SkillCdTime = cdTime;
        mEndCdTime = curTime + cdTime;
        mIsInCd = isInToCd;
    }
}