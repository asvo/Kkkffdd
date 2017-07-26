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
    }

    private void OnClickNormalAttack()
    {
        if (!SkillFirer.Instance.CheckCanFireSkill(SkillConst.NormalAttackSkillSlotId))
            return;
        if (SkillDataMgr.Instance().IsSkill02Active)
        {
            EventMgr.Instance().TriggerEvent(EventsType.FireNoramlAttack, null);
            return;
        }
        GameManager.Instance().MainPlayer.FireSkill(SkillConst.NormalAttackSkillSlotId);
    }

    void Update()
    {
        UpdateBuffs();
    }

    private void UpdateBuffs()
    {        
        if (SkillDataMgr.Instance().IsSkill01BuffActive)
        {
            SkillCdData mSkill01BuffCd = SkillDataMgr.Instance().GetSkillCdDataBySlotId(11);
            if (mSkill01BuffCd != null && mSkill01BuffCd.mIsInCd)
            {
                float leftTime = mSkill01BuffCd.mEndCdTime - Time.realtimeSinceStartup;
                if (leftTime <= 0)
                {
                    mSkill01BuffCd.mIsInCd = false;
                    SkillDataMgr.Instance().IsSkill01BuffActive = false;
                    TxtSkill01Buff.text = "";
                }
                else
                {
                    TxtSkill01Buff.text = string.Format("普攻双倍伤害.技能01Buff效果剩余时间: {0:F} ", leftTime);
                }
            }
        }
    }
}
