using UnityEngine;
using System.Collections;

public class Skill01Btn : SkillBtn {

    public float CdInterval = 2f;
    public int MaxCounter = 2;

    public int CurCounter = 0;
    public float mCurrentCdIntervalTime = 0f;
    public bool CanFireSecondTime = false;

    protected override void OnCastSkill()
    {
        bool canFireSkill = CurCounter < MaxCounter;
        if (!canFireSkill)
        {
            if (SkillDataMgr.Instance().SkillCdData01.mIsInCd)
            {
                Debug.Log("技能cd中");
                return;
            }
        }
        //set cd
        if (0 == CurCounter)
        {
            SkillDataMgr.Instance().SetOnSkillCd01(TestSkillCd, Time.realtimeSinceStartup, true);
            CanFireSecondTime = CurCounter + 1 == MaxCounter;
            //技能1特殊逻辑
            if (CanFireSecondTime)
            {
                ImgCd.gameObject.SetActive(false);
                ImgCd.fillAmount = 1f;                
                mCurrentCdIntervalTime = 0;
            }
            else
            {
                ImgCd.gameObject.SetActive(true);
                ImgCd.fillAmount = 1f;
            }
        }
        else
        {
            ImgCd.gameObject.SetActive(true);
        }
        ++CurCounter;
        //cast skill
        GameManager.Instance().MainPlayer.FireSkill(SkillSlotId);
    }

    protected override void OnUpdateBeforeCd(float deltaTime)
    {
        if (CanFireSecondTime)
        {
            mCurrentCdIntervalTime += deltaTime;
            if (mCurrentCdIntervalTime >= CdInterval)
            {
                CanFireSecondTime = false;
                CurCounter = MaxCounter;
                ImgCd.gameObject.SetActive(true);
            }
        }
    }

    protected override void OnCdOver()
    {
        base.OnCdOver();
        CurCounter = 0;
    }
}
