using UnityEngine;
using System.Collections;

public class Skill02Btn : SkillBtn {
        

    protected override void OnCastSkill()
    {
        if (mCdData.mIsInCd)
        {
            Debug.Log("技能cd中");
            return;
        }
        SkillDataMgr.Instance().SetOnSkillCd(SkillSlotId, TestSkillCd, Time.realtimeSinceStartup, true);
        ImgCd.gameObject.SetActive(true);
        ImgCd.fillAmount = 1f;
        //cast skill
        GameManager.Instance().MainPlayer.FireSkill(SkillSlotId);        
    }
}
