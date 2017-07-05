using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

    public Button BtnNormalAttack;

    public Text TxtSkill01Buff;    

    void Awake()
    {
        BtnNormalAttack.onClick.AddListener(OnClickNormalAttack);
        TxtSkill01Buff.text = string.Empty;
        SkillDataMgr.Instance().IsSkill01BuffActive = false;
        SkillDataMgr.Instance().SetOnSkill01BuffCdData(1f, 0f, false);
    }

    private void OnClickNormalAttack()
    {
        GameManager.Instance().MainPlayer.FireSkill(0);
    }

    void Update()
    {
        UpdateBuffs();
    }

    private void UpdateBuffs()
    {
        if (SkillDataMgr.Instance().IsSkill01BuffActive && SkillDataMgr.Instance().Skill01BuffCdData.mIsInCd)
        {
            float leftTime = SkillDataMgr.Instance().Skill01BuffCdData.mEndCdTime - Time.realtimeSinceStartup;
            if (leftTime <= 0)
            {
                SkillDataMgr.Instance().Skill01BuffCdData.mIsInCd = false;
                SkillDataMgr.Instance().IsSkill01BuffActive = false;
                TxtSkill01Buff.text = "";
            }
            else
            {
                TxtSkill01Buff.text = string.Format("技能01Buff效果剩余时间: {0:F} ", leftTime);
            }
        }
    }
}
