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
        if (SkillDataMgr.Instance().IsSkill02Active)
        {
            SkillCdData cdData = SkillDataMgr.Instance().GetSkillCdDataBySlotId(SkillConst.PlayerSkill02AttackSlotId);
            if (null != cdData && !cdData.mIsInCd)
            {
                SkillDataMgr.Instance().SetOnSkillCd(SkillConst.PlayerSkill02AttackSlotId, cdData.SkillCdTime, Time.realtimeSinceStartup, true);
                EventMgr.Instance().TriggerEvent(EventsType.FireNoramlAttack, null);
            }
            return;
        }

        if (SkillDataMgr.Instance().CheckNormalSkillIsInSkill())        //普通攻击不能在其它技能中释放.
            return;
        if (!SkillFirer.Instance.CheckCanFireSkill(SkillConst.NormalAttackSkillSlotId))
            return;
        GameManager.Instance().MainPlayer.FireSkill(SkillConst.NormalAttackSkillSlotId);
    }    

    void Update()
    {
        UpdateBuffs();
        UpdateSkill2AttackCd();
    }

    private SkillCdData mSkill02AttackCd = null;

    private void UpdateSkill2AttackCd()
    {
        if (null == mSkill02AttackCd)
            mSkill02AttackCd = SkillDataMgr.Instance().GetSkillCdDataBySlotId(SkillConst.PlayerSkill02AttackSlotId);
        if (null != mSkill02AttackCd && mSkill02AttackCd.mIsInCd)
        {
            float leftTime = mSkill02AttackCd.mEndCdTime - Time.realtimeSinceStartup;
            if (leftTime <= 0)
            {
                mSkill02AttackCd.mIsInCd = false;
            }
        }
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

	void OnGUI()
	{
		if (GUILayout.Button ("Test-Spine-Mix")) {
			TestSpineMix ();
		}
	}

	private void TestSpineMix()
	{
		Player p = GameManager.Instance ().MainPlayer;
		p.SkeletonAnim.state.SetAnimation (0, "Interruption attacks", false);
		p.SkeletonAnim.state.SetAnimation (1, "machine", false);

	}
}
