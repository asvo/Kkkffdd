using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SkillSettingItem : MonoBehaviour {

    public Text TxtSkillName;
    public Transform PropertyItemRoot;
    public GameObject TemplateInputGobj;

    private bool mHasInit = false;
    private int mCurSlotId = 0;

    private List<InputField> mCfgTxt = new List<InputField>();

    public void ShowSetting(int skillSlot)
    {
        mCurSlotId = skillSlot;
        SkillCfgUnit cfgUnit = SkillCfgMgr.Instance().GetSkillCfgBySlotId(mCurSlotId);
        TxtSkillName.text = cfgUnit.Name + " :";

        if (!mHasInit)
        {
            mCfgTxt.Clear();
            CreateOnePropertyField(cfgUnit.Damge.ToString(), "伤害");
            CreateOnePropertyField(cfgUnit.DamageRange.ToString(), "伤害范围");
            CreateOnePropertyField(cfgUnit.DamageTime.ToString(), "伤害时间点");
            CreateOnePropertyField(cfgUnit.SkillCd.ToString(), "cd时间");

            CreateOnePropertyField(cfgUnit.SkillMoveTime.ToString(), "位移时间");
            CreateOnePropertyField(cfgUnit.SkillMoveSpeed.ToString(), "位移速度");

            mHasInit = true;
        }
        else
        {
            SetFieldVal(mCfgTxt[0], cfgUnit.Damge.ToString(), "伤害");
            SetFieldVal(mCfgTxt[1], cfgUnit.DamageRange.ToString(), "伤害范围");
            SetFieldVal(mCfgTxt[2], cfgUnit.DamageTime.ToString(), "伤害时间点");
            SetFieldVal(mCfgTxt[3], cfgUnit.SkillCd.ToString(), "cd时间");
            SetFieldVal(mCfgTxt[4], cfgUnit.SkillMoveTime.ToString(), "位移时间");
            SetFieldVal(mCfgTxt[5], cfgUnit.SkillMoveSpeed.ToString(), "位移速度");
        }
    }

    private void CreateOnePropertyField(string valueStr, string desc)
    {
        GameObject gobj = Util.AddChildToTarget(TemplateInputGobj, PropertyItemRoot.gameObject);
        InputField _input = gobj.GetComponent<InputField>();
        SetFieldVal(_input, valueStr, desc);
        mCfgTxt.Add(_input);
    }

    private void SetFieldVal(InputField _input, string valStr, string desc)
    {
        _input.text = valStr;
        _input.transform.FindChild("Desc").GetComponent<Text>().text = desc;
        _input.gameObject.SetActive(true);
    }

    public void SaveToCorrespondSkill()
    {
        SkillCfgUnit cfgUnit = SkillCfgMgr.Instance().GetSkillCfgBySlotId(mCurSlotId);
        int val = 1;
        if (int.TryParse(mCfgTxt[0].text, out val))
        {
            cfgUnit.Damge = val;
        }
        float valF = 1f;
        if (float.TryParse(mCfgTxt[1].text, out valF))
        {
            cfgUnit.DamageRange = valF;
        }
        if (float.TryParse(mCfgTxt[2].text, out valF))
        {
            cfgUnit.DamageTime = valF;
        }
        if (float.TryParse(mCfgTxt[3].text, out valF))
        {
            cfgUnit.SkillCd = valF;
        }
        if (float.TryParse(mCfgTxt[4].text, out valF))
        {
            cfgUnit.SkillMoveTime = valF;
        }
        if (float.TryParse(mCfgTxt[5].text, out valF))
        {
            cfgUnit.SkillMoveSpeed = valF;
        }
        SkillCfgMgr.Instance().Save();
    }
}
