using UnityEngine;
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
        ImgCd.gameObject.SetActive(true);
        ImgCd.fillAmount = 1f;
        //cast skill
        GameManager.Instance().MainPlayer.FireSkill(2);
    }


}
