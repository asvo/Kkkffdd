﻿using UnityEngine;
using System.Collections;

public class Skill02Btn : SkillBtn {
        


    protected override void OnCastSkill()
    {
        mCdData = SkillDataMgr.Instance().GetLeastCdSkill2CdData();
        if (mCdData.mIsInCd)
        {
            Debug.Log("技能cd中");
            return;
        }
        SkillDataMgr.Instance().SetOnCurSkill2Cd(TestSkillCd, Time.realtimeSinceStartup, true);
        mCdData = SkillDataMgr.Instance().GetLeastCdSkill2CdData();
        if (mCdData.mIsInCd)
        {
            ImgCd.gameObject.SetActive(true);
            float leftCdTime = mCdData.mEndCdTime - Time.realtimeSinceStartup;
            if (leftCdTime < 0f)
                leftCdTime = 0f;
            ImgCd.fillAmount = leftCdTime / mCdData.SkillCdTime;
        }
        //cast skill
        GameManager.Instance().MainPlayer.FireSkill(2);
    }

    protected override void OnUpdateBeforeCd(float deltaTime)
    {
        mCdData = SkillDataMgr.Instance().GetLeastCdSkill2CdData();
    }
}
