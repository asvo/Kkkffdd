using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class JsDataBaseValue
{
    public Dictionary<string, float> dic_BaseValues = new Dictionary<string, float>();
}

public class BaseValUnit
{
    public e_BaseValue BaseValType;
    public float Val;
}

public enum e_BaseValue
{
    None,
    NormalAttackCd,
    NormalAttackRange,
    NormalAttackDamgePoint,
    NormalAttackDamge,
    MoveSpeed,
    RestatsTime,
}
public class ValueManager : Single<ValueManager>
{
    public const string PLYER_VALUE = "PLYER_VALUE";
    public const string MONSTER_VALUE = "MONSTER_VALUE";
    //player skills
    public const string PLYER_SKILLS = "PLYER_SKILLS";

    public JsDataBaseValue MonsterValueSettings = null;
    public JsDataBaseValue PlayerValueSettings = null;

    public void Save(bool isPlayer)
    {
        SaveSettingValues(isPlayer);
        if (isPlayer)
        {
            GameManager.Instance().MainPlayer.LoadSettingData();
        }
        else
        {
            foreach (Monster monster in MonsterManager.Instance().ActiveMonsters)
            {
                monster.LoadSettingData();
            }
        }
    }

    public void Load(bool isPlayer)
    {
        if (isPlayer)
        {
            if (!Util.CheckHaveJsonFile(PLYER_VALUE))
            {
                SetDefaultValues(isPlayer);
            }

            PlayerValueSettings = Util.LoadJson<JsDataBaseValue>(PLYER_VALUE);
        }
        else
        {
            if (!Util.CheckHaveJsonFile(MONSTER_VALUE))
            {
                SetDefaultValues(isPlayer);
            }
            MonsterValueSettings = Util.LoadJson<JsDataBaseValue>(MONSTER_VALUE);
        }
    }

    private void SaveSettingValues(bool isPlayer)
    {
        JsDataBaseValue data = null;
        if (isPlayer)
        {
            data = PlayerValueSettings;
            Util.SaveJson(data, PLYER_VALUE);
        }
        else
        {
            data = MonsterValueSettings;
            Util.SaveJson(data, MONSTER_VALUE);
        }
    }

    private void SetDefaultValues(bool isPlayer)
    {
        JsDataBaseValue defaultValue = new JsDataBaseValue();
        Dictionary<string, float> dic_BaseValues = defaultValue.dic_BaseValues;

        if (isPlayer)
        {
            dic_BaseValues.Add(e_BaseValue.MoveSpeed.ToString(), 3);
            dic_BaseValues.Add(e_BaseValue.NormalAttackCd.ToString(), 0.3f);
            dic_BaseValues.Add(e_BaseValue.NormalAttackDamge.ToString(), 1);
            dic_BaseValues.Add(e_BaseValue.NormalAttackDamgePoint.ToString(), 0.3f);
            dic_BaseValues.Add(e_BaseValue.NormalAttackRange.ToString(), 1);

            PlayerValueSettings = defaultValue;
        }
        else
        {
            dic_BaseValues.Add(e_BaseValue.MoveSpeed.ToString(), 4);
            dic_BaseValues.Add(e_BaseValue.NormalAttackCd.ToString(), 0.3f);
            dic_BaseValues.Add(e_BaseValue.NormalAttackDamge.ToString(), 1);
            dic_BaseValues.Add(e_BaseValue.NormalAttackDamgePoint.ToString(), 0.3f);
            dic_BaseValues.Add(e_BaseValue.NormalAttackRange.ToString(), 0.2f);
            dic_BaseValues.Add(e_BaseValue.RestatsTime.ToString(), 1);

            MonsterValueSettings = defaultValue;
        }

        Save(isPlayer);
    }
}
