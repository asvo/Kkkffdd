using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SkillConst
{
    public const int NormalAttackSkillSlotId = 0;
    public const int PlayerSkill01SlotId = 1;
    public const int PlayerSkill01BuffSlotId = 11;
    public const int PlayerSkill02SlotId = 2;
    public const int PlayerSkill02AttackSlotId = 21;
}

public class SkillCfgMgr : Single<SkillCfgMgr>
{
    private SkillCfgJsonData mCfgUnits;
    public List<SkillCfgUnit> CfgUnitData { get { return mCfgUnits.mCfgUnits; } }

    public void Load()
    {
        if (!Util.CheckHaveJsonFile(ValueManager.PLYER_SKILLS))
        {
            SetDefaultAndSave();
        }
        mCfgUnits = Util.LoadJson<SkillCfgJsonData>(ValueManager.PLYER_SKILLS);
    }

    public void Save()
    {
        SaveToPlayer();
        SaveToFile();
    }
	
    private void SaveToPlayer()
    {
        GameManager.Instance().MainPlayer.LoadSkillSettingData();
    }

    private void SetDefaultAndSave()
    {
        SetDefaultValues();
        SaveToFile();
    }

    private void SaveToFile()
    {
        Util.SaveJson(mCfgUnits, ValueManager.PLYER_SKILLS);
    }

    private void SetDefaultValues()
    {
        mCfgUnits = new SkillCfgJsonData();
        SkillCfgUnit cfgUnit = new SkillCfgUnit()
        {
            SlotId = SkillConst.NormalAttackSkillSlotId,        
            Name = "普通攻击",
            Damge = 1,
            DamageRange = 1.5f,
            DamageTime = 0.3f,
            SkillCd = 3f,
        };
        CfgUnitData.Add(cfgUnit);
        cfgUnit = new SkillCfgUnit()
        {
            SlotId = SkillConst.PlayerSkill01SlotId,
            Name = "技能1-跳斩",
            Damge = 2,
            DamageRange = 3f,
            DamageTime = 0.5f,
            SkillCd = 3f,
            SkillMoveTime = 1.0f,
            SkillMoveSpeed = 2f
        };
        CfgUnitData.Add(cfgUnit);
        cfgUnit = new SkillCfgUnit()
        {
            SlotId = SkillConst.PlayerSkill01BuffSlotId,
            Name = "技能1-跳斩buff",
            SkillCd = 1f,   //作为buff持续时间
        };
        CfgUnitData.Add(cfgUnit);
        cfgUnit = new SkillCfgUnit()
        {
            SlotId = SkillConst.PlayerSkill02SlotId,
            Name = "技能2-后跳",
            Damge = 2,
            DamageRange = 3f,
            DamageTime = 0.5f,
            SkillCd = 3f,
            SkillMoveTime = 1.0f,
            SkillMoveSpeed = 2f
        };
        CfgUnitData.Add(cfgUnit);
        cfgUnit = new SkillCfgUnit()
        {
            SlotId = SkillConst.PlayerSkill02AttackSlotId,
            Name = "技能2-后跳攻击",
            Damge = 2,
            DamageRange = 3f,
            DamageTime = 0.5f,
            SkillCd = 8f,
            SkillMoveTime = 1.0f,
            SkillMoveSpeed = 2f
        };
        CfgUnitData.Add(cfgUnit);
    }

    public SkillCfgUnit GetSkillCfgBySlotId(int slotId)
    {
        for (int i = 0; i < CfgUnitData.Count; ++i)
        {
            if (CfgUnitData[i].SlotId == slotId)
            {
                return CfgUnitData[i];
            }
        }
        return null;
    }
}

public class SkillCfgJsonData
{
    public List<SkillCfgUnit> mCfgUnits = new List<SkillCfgUnit>();
}

public class SkillCfgUnit
{
    public int SlotId;
    public string Name;

    public int Damge;
    public float DamageRange;
    public float DamageTime;
    public float SkillCd;

    //extra. TODO
    public float SkillMoveTime;
    public float SkillMoveSpeed;
}