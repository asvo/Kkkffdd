using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillBtn : MonoBehaviour {

    public int SkillSlotId;
    public Image ImgCd;

    public float TestSkillCd = 5f;

    void Start()
    {
        ImgCd.gameObject.SetActive(false);
        ImgCd.fillClockwise = false;
        OnStart();
    }

    public virtual void OnStart(){}

    public void OnClickCastSkill()
    {
        OnCastSkill();
    }

    protected virtual void OnCastSkill()
    {
        if (SkillDataMgr.Instance().SkillCdData01.mIsInCd)
        {
            Debug.Log("技能cd中");
            return;
        }
        //set cd        
        SkillDataMgr.Instance().SetOnSkillCd01(TestSkillCd, Time.realtimeSinceStartup, true);
        ImgCd.gameObject.SetActive(true);
        ImgCd.fillAmount = 1f;
        //cast skill
        GameManager.Instance().MainPlayer.FireSkill(SkillSlotId);
    }

    void Update()
    {
        OnUpdateBeforeCd(Time.deltaTime);
        SkillCdData cdData = SkillDataMgr.Instance().SkillCdData01;
        if (null == cdData || !cdData.mIsInCd)
            return;
        float leftCdTime = cdData.mEndCdTime - Time.realtimeSinceStartup;
        if (leftCdTime < 0)
        {            
            OnCdOver();
        }
        else
        {
            float f = leftCdTime / cdData.SkillCdTime;
            ImgCd.fillAmount = f;
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
        SkillDataMgr.Instance().SkillCdData01.mIsInCd = false;
        ImgCd.fillAmount = 0f;
        ImgCd.gameObject.SetActive(false);
    }
}
