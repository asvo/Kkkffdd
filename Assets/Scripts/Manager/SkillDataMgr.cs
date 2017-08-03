using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillDataMgr : Single<SkillDataMgr>
{
    private Dictionary<int, SkillCdData> mSkillCdData = new Dictionary<int, SkillCdData>();

    public bool IsSkill01BuffActive = false;
    public bool IsSkill02Active = false;

    public Dictionary<int, bool> mIsSkillOver = new Dictionary<int,bool>();

    public void InitSkillCdData()
    {
        mSkillCdData.Clear();
        mSkillCdData.Add(0, new SkillCdData());
        mSkillCdData.Add(1, new SkillCdData());
        mSkillCdData.Add(11, new SkillCdData()); //skill 1的buff效果cd
        mSkillCdData.Add(2, new SkillCdData());     //skill 2 cd slot 1
        mSkillCdData.Add(21, new SkillCdData());    //skill 2 cd slot 2

        ResetSkillOver();

        IsSkill01BuffActive = false;
        SetOnSkillCd(11, 1f, 0f, false);
    }

    private void ResetSkillOver()
    {
        mIsSkillOver.Clear();
        mIsSkillOver.Add(0, true);
        mIsSkillOver.Add(1, true);
        mIsSkillOver.Add(2, true);
        mIsSkillOver.Add(21, true);
    }

    public bool CheckNormalSkillIsInSkill()
    {
        foreach (var pair in mIsSkillOver)
        {
            if (pair.Key == SkillConst.NormalAttackSkillSlotId)
                continue;
            if (!pair.Value)
                return true;
        }
        return false;
    }

    public void SetSkillIfOver(int slotId, bool isOver)
    {
        if (mIsSkillOver.ContainsKey(slotId))
        {
            mIsSkillOver[slotId] = isOver;
        }
    }

    public bool GetSkillIfOver(int slotId)
    {
        if (mIsSkillOver.ContainsKey(slotId))
        {
            return mIsSkillOver[slotId];
        }
        return false;
    }

    public void ClearSkillCds()
    {
        IsSkill01BuffActive = false;
        IsSkill02Active = false;
        foreach(var pair in mSkillCdData)
        {
            pair.Value.ClearCd();
            pair.Value.ClearActionTime();
        }

        ResetSkillOver();
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

    private void ReducePlayerSkillCd(int slotId, float reduceAmount)
    {
        SkillCdData cddata = GetSkillCdDataBySlotId(slotId);
        if (null == cddata)
            return;
        cddata.AddCd(-reduceAmount);
    }

    private static List<int> s_ReduceCdSkillList = new List<int>
    {
        1,2,21
    };
    public void ReducePlayerAllSkillCd(float reduceAmount)
    {
        for(int i = 0; i < s_ReduceCdSkillList.Count; ++i)
        {
            ReducePlayerSkillCd(s_ReduceCdSkillList[i], reduceAmount);
        }
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

    public void UpdateAllActionTime()
    {
        if (null == mSkillCdData)
            return;
        foreach(var skillCd in mSkillCdData)
        {
            skillCd.Value.UpdateActionTime();
        }
    }

    public void SetOnActionTime(int slotId)
    {
        SkillCdData cdData = GetSkillCdDataBySlotId(slotId);
        if (null != cdData)
            cdData.SetActionTime(Time.realtimeSinceStartup);
    }

    public void ClearActionTimeBySlot(int slotId)
    {
        SkillCdData cdData = GetSkillCdDataBySlotId(slotId);
        if (null != cdData)
            cdData.ClearActionTime();
    }
}

public class SkillCdData
{
    public float SkillCdTime = 3.0f;
    public float mEndCdTime = 0f;
    public bool mIsInCd = false;

    private bool mIsInAction = false;
    public bool IsInAction { get { return mIsInAction; } }
    public float ActionCfgTime = 1.0f;
    private float mActionEndTime = 1.0f;

    public void AddCd(float reduceAmount)
    {
        mEndCdTime += reduceAmount;
        if (mEndCdTime < 0f)
            mEndCdTime = 0f;
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

    public void SetActionTime(float curTime)
    {
        mIsInAction = true;
        mEndCdTime = curTime + ActionCfgTime;
    }

    public void UpdateActionTime()
    {
        if (mIsInAction)
        {
            if (Time.realtimeSinceStartup >= mEndCdTime)
            {
                mIsInAction = false;
            }
        }
    }

    public void ClearActionTime()
    {
        mIsInAction = false;
        mActionEndTime = 0f;
    }
}