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
        mSkillCdData.Clear();
        mSkillCdData.Add(1, new SkillCdData());
        mSkillCdData.Add(11, new SkillCdData()); //skill 1的buff效果cd
        mSkillCdData.Add(2, new SkillCdData());     //skill 2 cd slot 1
        mSkillCdData.Add(21, new SkillCdData());    //skill 2 cd slot 2

        IsSkill01BuffActive = false;
        SetOnSkillCd(11, 1f, 0f, false);
    }

    public void ClearSkillCds()
    {
        IsSkill01BuffActive = false;
        IsSkill02Active = false;
        foreach(var pair in mSkillCdData)
        {
            pair.Value.ClearCd();
        }
    }

    public void SetOnSkillCd(int slotId, float cdTime, float curTime, bool isInToCd)
    {
        SkillCdData cddata = GetSkillCdDataBySlotId(slotId);
        if (null == cddata)
            return;
        SkillCfgUnit cfgUnit = SkillCfgMgr.Instance().GetSkillCfgBySlotId(slotId);
        if (null != cfgUnit)
            cdTime = cfgUnit.SkillCd;
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

#region skill2-special

    int[] skill2ReflectArry = new int[]{
        2, 21
    };

    private int mCurrentSkill2ReflectSlotId = 2;

    public void SetOnCurSkill2Cd(float cdTime, float curTime, bool isInToCd)
    {
        SkillCdData cddata = GetCurSkill2CdData();
        cddata.SetOnCd(cdTime, curTime, isInToCd);
    }

    public SkillCdData GetCurSkill2CdData()
    {
        return GetSkillCdDataBySlotId(mCurrentSkill2ReflectSlotId);
    }
    
    public SkillCdData GetLeastCdSkill2CdData()
    {
        float endCdTime = float.MaxValue;
        SkillCdData targetCdata = null;
        for(int i = 0; i < skill2ReflectArry.Length; ++i)
        {
            SkillCdData cdData = GetSkillCdDataBySlotId(skill2ReflectArry[i]);
            if (cdData.mEndCdTime < endCdTime)
            {
                targetCdata = cdData;
                endCdTime = cdData.mEndCdTime;
                //
                mCurrentSkill2ReflectSlotId = skill2ReflectArry[i];
            }
        }
        return targetCdata;
    }

#endregion
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

    public void ClearCd()
    {
        mIsInCd = false;
        mEndCdTime = 0f;
    }
}