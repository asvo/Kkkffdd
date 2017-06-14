using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class JsDataBaseValue
{
    public Dictionary<e_BaseValue, float> dic_BaseValues = new Dictionary<e_BaseValue, float>();
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

    public JsDataBaseValue MonsterValueSettings = null;
    public JsDataBaseValue PlayerValueSettings = null;

    public void Save(bool isPlayer)
    {
        SaveSettingValues(isPlayer);
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
        Dictionary<e_BaseValue, float> dic_BaseValues = defaultValue.dic_BaseValues;

        if (isPlayer)
        {
            dic_BaseValues.Add(e_BaseValue.MoveSpeed, 3);
            dic_BaseValues.Add(e_BaseValue.NormalAttackCd, 0.3f);
            dic_BaseValues.Add(e_BaseValue.NormalAttackDamge, 1);
            dic_BaseValues.Add(e_BaseValue.NormalAttackDamgePoint, 0.3f);
            dic_BaseValues.Add(e_BaseValue.NormalAttackRange, 1);

            PlayerValueSettings = defaultValue;
        }
        else
        {
            dic_BaseValues.Add(e_BaseValue.MoveSpeed, 4);
            dic_BaseValues.Add(e_BaseValue.NormalAttackCd, 0.3f);
            dic_BaseValues.Add(e_BaseValue.NormalAttackDamge, 1);
            dic_BaseValues.Add(e_BaseValue.NormalAttackDamgePoint, 0.3f);
            dic_BaseValues.Add(e_BaseValue.NormalAttackRange, 0.2f);
            dic_BaseValues.Add(e_BaseValue.RestatsTime, 1);

            MonsterValueSettings = defaultValue;
        }

        Save(isPlayer);
    }
}
