using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillBtn : MonoBehaviour {

    public int SkillSlotId;
    public Image ImgCd;
    public Text TxtCd;

    public float TestSkillCd = 5f;

    protected SkillCdData mCdData;

    void Start()
    {        
        ImgCd.gameObject.SetActive(false);
        ImgCd.fillClockwise = false;
        OnStart();
    }

    public virtual void OnStart(){}

    public void OnClickCastSkill()
    {
        mCdData = SkillDataMgr.Instance().GetSkillCdDataBySlotId(SkillSlotId);        
        OnCastSkill();
    }

    protected virtual void OnCastSkill()
    {        
        if (mCdData.mIsInCd)
        {
            Debug.Log("技能cd中");
            return;
        }
        //set cd
        SkillDataMgr.Instance().SetOnSkillCd(SkillSlotId, TestSkillCd, Time.realtimeSinceStartup, true);
        ImgCd.gameObject.SetActive(true);
        ImgCd.fillAmount = 1f;
        TxtCd.text = TestSkillCd.ToString("0.0");
        //cast skill
        GameManager.Instance().MainPlayer.FireSkill(SkillSlotId);
    }

    void Update()
    {
        OnUpdateBeforeCd(Time.deltaTime);
        if (null == mCdData || !mCdData.mIsInCd)
            return;
        float leftCdTime = mCdData.mEndCdTime - Time.realtimeSinceStartup;
        if (leftCdTime < 0)
        {            
            OnCdOver();
        }
        else
        {
            float f = leftCdTime / mCdData.SkillCdTime;
            ImgCd.fillAmount = f;
            TxtCd.text = leftCdTime.ToString("0.0");
            if (f <= 0.001f)
            {
                OnCdOver();
            }
        }
    }

    protected virtual void OnUpdateBeforeCd(float deltaTime)
    {
    }

    protected virtual void OnCdOver()
    {
        mCdData.mIsInCd = false;
        ResetShowCd();
    }

    protected void ResetShowCd()
    {
        ImgCd.fillAmount = 0f;
        TxtCd.text = string.Empty;
        ImgCd.gameObject.SetActive(false);
    }
}
