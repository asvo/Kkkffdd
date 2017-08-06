using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ValueModule;


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

    JumpPrepareTimeRate,//跳跃准备时间比率，距离*比率
    JumpTime,//跳跃时间
    JumpHitBackSpeed,//跳跃时被击退速度

}
public class ValueManager : Single<ValueManager>
{
    public const string PLYER_VALUE = "PLYER_VALUE";
    public const string MONSTER_VALUE = "MONSTER_VALUE";
    //player skills
    public const string PLYER_SKILLS = "PLYER_SKILLS";

    public MonsterValue MonsterValueSettings = null;
    public PlayerValue PlayerValueSettings = null;

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
            PlayerValueSettings = Util.LoadJson<PlayerValue>(PLYER_VALUE);
        }
        else
        {
            if (!Util.CheckHaveJsonFile(MONSTER_VALUE))
            {
                SetDefaultValues(isPlayer);
            }
            MonsterValueSettings = Util.LoadJson<MonsterValue>(MONSTER_VALUE);
        }
    }

    private void SaveSettingValues(bool isPlayer)
    {
        if (isPlayer)
        {
            Util.SaveJson(PlayerValueSettings, PLYER_VALUE);
        }
        else
        {
            Util.SaveJson(MonsterValueSettings, MONSTER_VALUE);
        }
    }

    private void SetDefaultValues(bool isPlayer)
    {
        if (isPlayer)
        {
            PlayerValueSettings = new PlayerValue();
        }
        else
        {
            MonsterValueSettings = new MonsterValue();
        }
        Save(isPlayer);
    }
}
